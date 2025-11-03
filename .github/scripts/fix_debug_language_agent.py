import os
import pathlib
import re
import time
import subprocess
from dataclasses import dataclass
from typing import List, Dict, Optional, Tuple

import requests
from openai import OpenAI

# ---------------- CONFIG ----------------

ROOT = pathlib.Path(".")
AGENT_LABEL = "Agent"
DEBUG_LANGUAGE_LABEL = "Agent/DebugLanguage"

OPENAI_MODEL = os.getenv("DEBUG_LANG_MODEL", "gpt-4o-mini")

DEBUG_SYSTEM_PROMPT = """You are a localization assistant for C# debug log messages.

Task:
Given a single debug log message text (possibly in German), rewrite it as a short, clear English message suitable for a log.

Rules:
- Preserve the *meaning* but use natural, concise English.
- Do NOT invent new information.
- Keep it short (ideally one short clause).
- Output ONLY a single C# string literal, including surrounding double quotes, properly escaped.
- No code fences, no explanations, no comments.
"""

# Wir erwarten weiterhin DebugManager.*("...")-Issues
# Beispiel-Body aus ReviewAgent:
# Automatic finding by ReviewAgent.
# **Kind:** Agent/DebugLanguage
# **File:** `path`
# **Line:** 123
# **Symbol:** `DebugManager.Info`
# **Message:** Non-English debug message detected: "Forschung fehlgeschlagen"

RE_FINDING_LINE = re.compile(
    r"^[-*]\s*Line\s+(\d+),\s*`([^`]+)`: (.+)$"
)

# Für das eigentliche Ersetzen: DebugManager.X("...") auf der Zeile
RE_DEBUG_MANAGER_CALL = re.compile(
    r'(DebugManager\.\w+)\s*\(\s*([@$]*"[^"]*")',
    re.MULTILINE,
)


@dataclass
class FindingItem:
    issue_number: int
    file: str
    line: int
    symbol: str
    message: str


# ---------------- GitHub Helpers ----------------

def get_repo_from_env() -> Tuple[str, str]:
    repo = os.getenv("GITHUB_REPOSITORY")
    if not repo or "/" not in repo:
        raise RuntimeError("GITHUB_REPOSITORY not set or invalid")
    owner, name = repo.split("/", 1)
    return owner, name


def get_github_session() -> requests.Session:
    token = os.getenv("GITHUB_TOKEN")
    if not token:
        raise RuntimeError("GITHUB_TOKEN not set")
    s = requests.Session()
    s.headers.update({
        "Authorization": f"Bearer {token}",
        "Accept": "application/vnd.github+json",
    })
    return s


def list_debug_language_issues(session: requests.Session, owner: str, repo: str) -> List[dict]:
    issues: List[dict] = []
    page = 1
    per_page = 50

    while True:
        url = f"https://api.github.com/repos/{owner}/{repo}/issues"
        params = {
            "state": "open",
            "labels": f"{AGENT_LABEL},{DEBUG_LANGUAGE_LABEL}",
            "page": page,
            "per_page": per_page,
        }
        resp = session.get(url, params=params)
        if resp.status_code != 200:
            raise RuntimeError(f"Failed to list issues: {resp.status_code} {resp.text}")

        batch = resp.json()
        if not batch:
            break

        issues.extend(batch)
        page += 1

    return issues


def close_issue(session: requests.Session, owner: str, repo: str, issue_number: int, pr_url: str) -> None:
    # Kommentar mit PR-Link
    comment_url = f"https://api.github.com/repos/{owner}/{repo}/issues/{issue_number}/comments"
    comment_body = {
        "body": f"Fixed by {pr_url}"
    }
    session.post(comment_url, json=comment_body)

    # Issue schließen
    url = f"https://api.github.com/repos/{owner}/{repo}/issues/{issue_number}"
    payload = {"state": "closed"}
    resp = session.patch(url, json=payload)
    if resp.status_code not in (200, 201):
        print(f"FixDebugLanguageBot: Failed to close issue #{issue_number}: {resp.status_code} {resp.text}")


# ---------------- Issue Body Parsing ----------------
def extract_literal_from_issue_message(message: str) -> Optional[str]:
    """
    Erwartet z.B.:
      'Non-English debug message detected: "Forschung fehlgeschlagen"'
    Gibt das Stringliteral inkl. Quotes zurück: "\"Forschung fehlgeschlagen\""
    """
    if '"' not in message:
        return None
    first = message.find('"')
    last = message.rfind('"')
    if last <= first:
        return None
    return message[first:last+1]


def parse_findings_from_issue(issue: dict) -> List[FindingItem]:
    number = issue["number"]
    body = issue.get("body") or ""
    lines = body.splitlines()

    findings: List[FindingItem] = []

    file_path: Optional[str] = None

    # 1) File: Zeile finden (Markdown: **File:** `path`)
    for line in lines:
        l = line.strip()
        lower = l.lower()
        if "file:" in lower:
            idx = lower.find("file:")
            rest = l[idx + len("file:"):]
            # Sterne + Backticks + Spaces weg
            rest = rest.replace("*", "").strip()
            file_path = rest.strip("`").strip()
            break

    if not file_path:
        print(f"parse_findings_from_issue: issue #{number} -> no File: line found")
        return findings

    # 2) Legacy-Format: **Line:**, **Symbol:**, **Message:**
    line_no: Optional[int] = None
    symbol: Optional[str] = None
    message_lines: List[str] = []
    collecting_message = False

    for line in lines:
        l = line.strip()
        if not l:
            if collecting_message:
                collecting_message = False
            continue

        lower = l.lower()

        if "line:" in lower:
            idx = lower.find("line:")
            rest = l[idx + len("line:"):]
            rest = rest.replace("*", "").strip()
            try:
                line_no = int(rest)
            except ValueError:
                line_no = None

        elif "symbol:" in lower:
            idx = lower.find("symbol:")
            rest = l[idx + len("symbol:"):]
            rest = rest.replace("*", "").strip()
            symbol = rest.strip("`").strip()

        elif "message:" in lower:
            collecting_message = True
            idx = lower.find("message:")
            rest = l[idx + len("message:"):]
            rest = rest.replace("*", "").strip()
            if rest:
                message_lines.append(rest)

        elif "fingerprint:" in lower:
            collecting_message = False

        elif collecting_message:
            message_lines.append(l)

    if file_path and line_no is not None and symbol:
        msg = " ".join(message_lines).strip() or "Non-English debug message."
        findings.append(
            FindingItem(
                issue_number=number,
                file=file_path,
                line=line_no,
                symbol=symbol,
                message=msg,
            )
        )
        print(f"parse_findings_from_issue: issue #{number} -> 1 finding ({file_path}:{line_no} {symbol})")
    else:
        print(f"parse_findings_from_issue: issue #{number} -> no finding (file={file_path}, line={line_no}, symbol={symbol})")

    return findings


# ---------------- OpenAI Helper ----------------

def get_openai_client() -> OpenAI:
    api_key = os.getenv("OPENAI_API_KEY")
    if not api_key:
        raise RuntimeError("OPENAI_API_KEY not set")
    return OpenAI(api_key=api_key)


def translate_debug_literal(client: OpenAI, original_literal: str, context: str) -> str:
    """
    original_literal: C#-Stringliteral, z. B. "Forschung fehlgeschlagen"
                      oder @"Forschung fehlgeschlagen"
    Rückgabe: C#-Stringliteral in Englisch, mit doppelten Anführungszeichen.
    """
    # inneren Text (ohne @ und äußere Quotes) als Info extrahieren
    s = original_literal.strip()
    is_verbatim = s.startswith("@\"")
    if is_verbatim and s.endswith("\""):
        inner = s[2:-1]
    elif s.startswith("\"") and s.endswith("\""):
        inner = s[1:-1]
    else:
        inner = s

    user_prompt = f"""Original debug message text:
{inner}

Context (may be partial log call line):
{context}

Rewrite this debug message in concise, natural English suitable for a log.
Return ONLY a single C# string literal, including quotes and properly escaped."""
    resp = client.chat.completions.create(
        model=OPENAI_MODEL,
        messages=[
            {"role": "system", "content": DEBUG_SYSTEM_PROMPT},
            {"role": "user", "content": user_prompt},
        ],
        temperature=0.1,
    )
    content = resp.choices[0].message.content or ""
    lines = [ln for ln in content.splitlines() if ln.strip()]
    candidate = lines[0].strip() if lines else ""

    # Safety: wenn das Modell keine Quotes liefert, packen wir sie drum
    if not (candidate.startswith('"') and candidate.endswith('"')):
        candidate = '"' + candidate.strip('"') + '"'

    return candidate


# ---------------- File Patching ----------------

def insert_translations_in_file(path: pathlib.Path, items: List[FindingItem], client: OpenAI) -> bool:
    """
    Ersetzt für jedes FindingItem die DebugManager-Stringliteral-Nachricht durch eine englische Version.
    Versucht zuerst, den Call in der gemeldeten Zeile zu finden.
    Falls dort nichts ist, sucht er im ganzen File nach dem Literal aus der Issue-Message.
    """
    text = path.read_text(encoding="utf-8", errors="ignore")
    lines = text.splitlines()

    # Items von unten nach oben, damit Zeilenindizes stabil bleiben
    items_sorted = sorted(items, key=lambda it: it.line, reverse=True)
    changed = False

    for item in items_sorted:
        idx = item.line - 1
        if idx < 0 or idx >= len(lines):
            print(f"FixDebugLanguageBot: Line {item.line} out of range in {item.file}, skipping.")
            continue

        line = lines[idx]

        # 1) Versuch: DebugManager-Call direkt in der angegebenen Zeile
        m = RE_DEBUG_MANAGER_CALL.search(line)

        # 2) Fallback: anhand des Literals aus der Issue-Message im ganzen File suchen
        if not m:
            literal_from_msg = extract_literal_from_issue_message(item.message)
            if literal_from_msg:
                print(f"FixDebugLanguageBot: No DebugManager call on line {item.line}, "
                      f"searching by literal {literal_from_msg} in whole file...")
                match2 = None
                for m2 in RE_DEBUG_MANAGER_CALL.finditer(text):
                    if literal_from_msg in m2.group(2):
                        match2 = m2
                        break
                if match2:
                    # Zeilennummer aus Text-Offset bestimmen
                    prefix = text[:match2.start()]
                    idx = prefix.count("\n")
                    line = lines[idx]
                    m = match2

        if not m:
            print(f"FixDebugLanguageBot: No DebugManager call found for issue literal "
                  f"in {item.file} (line {item.line}), skipping.")
            continue

        literal = m.group(2)  # @"..." oder "..."
        context = line.strip()

        new_literal = translate_debug_literal(client, literal, context)
        if not new_literal or new_literal == literal:
            print(f"FixDebugLanguageBot: No change produced for {item.file}:{idx+1}.")
            continue

        new_line = line[:m.start(2)] + new_literal + line[m.end(2):]
        lines[idx] = new_line
        changed = True
        print(f"FixDebugLanguageBot: Updated literal in {item.file}:{idx+1}")

        # Text aktualisieren, damit spätere Fallback-Suchen den neuen Stand kennen
        text = "\n".join(lines) + "\n"

    if changed:
        path.write_text("\n".join(lines) + "\n", encoding="utf-8")
    return changed


# ---------------- Git Helpers ----------------

def git_run(args: List[str]) -> None:
    subprocess.run(["git"] + args, check=True)


def ensure_branch(branch: str) -> None:
    git_run(["checkout", "-b", branch])


def any_changes() -> bool:
    result = subprocess.run(["git", "status", "--porcelain"], capture_output=True, text=True)
    return bool(result.stdout.strip())


def commit_all(message: str) -> None:
    git_run(["add", "."])
    git_run(["commit", "-m", message])


def push_branch(branch: str) -> None:
    # falls Org ein PAT verlangt, kann hier remote-URL angepasst werden
    git_run(["push", "origin", branch])


def create_pull_request(session: requests.Session, owner: str, repo: str,
                        branch: str, base: str) -> Optional[str]:
    url = f"https://api.github.com/repos/{owner}/{repo}/pulls"
    payload = {
        "title": "Auto-translate DebugManager messages (Agent/DebugLanguage)",
        "head": branch,
        "base": base,
        "body": "This PR was created by FixAgentDebugLanguage bot to translate non-English DebugManager messages to English.",
    }
    resp = session.post(url, json=payload)
    if resp.status_code not in (200, 201):
        print(f"FixDebugLanguageBot: Failed to create PR ({resp.status_code}).")
        print(f"Response: {resp.text}")
        print(f"FixDebugLanguageBot: Please create a PR manually from branch '{branch}' into '{base}'.")
        return None

    pr = resp.json()
    print(f"Created PR #{pr.get('number')} -> {pr.get('html_url')}")
    return pr.get("html_url")


# ---------------- Main ----------------

def main():
    owner, repo = get_repo_from_env()
    session = get_github_session()
    client = get_openai_client()

    issues = list_debug_language_issues(session, owner, repo)
    print(f"FixDebugLanguageBot: Found {len(issues)} open issues with labels {AGENT_LABEL} + {DEBUG_LANGUAGE_LABEL}.")
    if not issues:
        print("FixDebugLanguageBot: No issues to process.")
        return

    file_to_items: Dict[str, List[FindingItem]] = {}
    issue_numbers: set[int] = set()

    for issue in issues:
        print(f"FixDebugLanguageBot: Parsing issue #{issue['number']} - {issue.get('title')!r}")
        items = parse_findings_from_issue(issue)
        print(f"FixDebugLanguageBot:   -> parsed {len(items)} finding(s) from issue #{issue['number']}")
        if not items:
            body = (issue.get("body") or "").splitlines()
            preview = "\n".join(body[:8])
            print(f"FixDebugLanguageBot:   body preview:\n{preview}\n---")
            continue
        for item in items:
            print(f"FixDebugLanguageBot:   + {item.file}:{item.line} {item.symbol}")
            file_to_items.setdefault(item.file, []).append(item)
            issue_numbers.add(item.issue_number)

    if not file_to_items:
        print("FixDebugLanguageBot: No parsable findings in issues.")
        return

    branch_name = f"autofix/debug-lang-{int(time.time())}"
    print(f"FixDebugLanguageBot: Creating branch {branch_name}")
    ensure_branch(branch_name)

    any_file_changed = False
    for file_path, items in file_to_items.items():
        p = ROOT / file_path
        if not p.exists():
            print(f"FixDebugLanguageBot: File not found (skipping): {file_path}")
            continue
        print(f"FixDebugLanguageBot: Updating {file_path} ({len(items)} item(s))")
        changed = insert_translations_in_file(p, items, client)
        any_file_changed |= changed

    if not any_file_changed or not any_changes():
        print("FixDebugLanguageBot: No changes were made; aborting commit/PR.")
        return

    commit_all("chore: translate DebugManager messages to English (auto)")
    push_branch(branch_name)

    base_branch = os.getenv("BASE_BRANCH", "master")
    pr_url = create_pull_request(session, owner, repo, branch_name, base_branch)

    if pr_url is None:
        print("FixDebugLanguageBot: No PR URL (PR not created). Leaving issues open.")
        print(f"FixDebugLanguageBot: You can now create a PR manually from branch '{branch_name}'.")
        return

    # Issues schließen
    for issue_number in issue_numbers:
        close_issue(session, owner, repo, issue_number, pr_url)

    print("FixDebugLanguageBot: Done.")


if __name__ == "__main__":
    main()
