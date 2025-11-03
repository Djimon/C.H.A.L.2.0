import os
import pathlib
import re
import time
import subprocess
from dataclasses import dataclass
from typing import List, Dict, Tuple
import requests
from openai import OpenAI

# ---------------- Config ----------------

ROOT = pathlib.Path(".")
SUMMARY_LABEL = "Agent/Summary"
AGENT_LABEL = "Agent"

OPENAI_MODEL = os.getenv("SUMMARY_MODEL", "gpt-4o-mini")

SUMMARY_SYSTEM_PROMPT = """You write concise C# XML documentation comments.

Given a single C# declaration (class or method) and a brief context, generate ONLY the XML doc comment lines that should be placed immediately above the declaration.

Rules:
- Output ONLY lines starting with '///'.
- Use clear, simple English.
- Keep the <summary> short (1–2 sentences).
- For methods: include <summary>, <param> tags for each parameter (if any), and <returns> if the return type is not void.
- For classes: usually only <summary>.
- Do NOT wrap the result in code fences.
- Do NOT include the declaration itself.
- No extra commentary.
"""

# Regex zum Parsen unserer Agent-Issues
RE_FINDING_LINE = re.compile(
    r"^- Line (\d+), `([^`]+)`: (.+)$"
)

# C# Typen/Methoden erkennen
RE_DECLARATION_LINE = re.compile(
    r'^\s*(public|internal|protected\s+internal|protected)\s+(.+)$'
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


def list_summary_issues(session: requests.Session, owner: str, repo: str) -> List[dict]:
    issues: List[dict] = []
    page = 1
    per_page = 50

    while True:
        url = f"https://api.github.com/repos/{owner}/{repo}/issues"
        params = {
            "state": "open",
            "labels": f"{AGENT_LABEL},{SUMMARY_LABEL}",
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
    # Optional: Kommentar mit Verweis auf PR
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
        print(f"Failed to close issue #{issue_number}: {resp.status_code} {resp.text}")


# ---------------- Parsing Issue Bodies ----------------

def parse_findings_from_issue(issue: dict) -> List[FindingItem]:
    """
    Unterstützt das aktuelle Body-Format, z.B.:

      Automatic finding by ReviewAgent.

      Kind: Agent/Summary
      File: Assets/src/Systems/Crafting/CraftingService.cs
      Line: 114
      Symbol: method CanCraft()

      Message: Missing
      XML doc for public method 'CanCraft'.

      Fingerprint: ...

    Und zusätzlich optional das spätere Format mit "### Findings" Zeilen,
    falls du den ReviewAgent später umstellst.
    """
    number = issue["number"]
    body = issue.get("body") or ""
    lines = body.splitlines()

    findings: List[FindingItem] = []

    file_path: Optional[str] = None

    # 1) File rausziehen
    for line in lines:
        l = line.strip()
        if l.lower().startswith("file:"):
            # nach dem ersten ':' alles nehmen und Backticks entfernen
            _, rest = l.split(":", 1)
            file_path = rest.strip().strip("`")
            break

    if not file_path:
        return findings

    # 2) Versuchen, moderne "Findings"-Zeilen zu parsen (falls später eingeführt)
    found_any_bullets = False
    for line in lines:
        l = line.strip()
        if l.startswith("- Line "):
            m = RE_FINDING_LINE.match(l)
            if not m:
                continue
            ln = int(m.group(1))
            symbol = m.group(2)
            msg = m.group(3)
            findings.append(
                FindingItem(
                    issue_number=number,
                    file=file_path,
                    line=ln,
                    symbol=symbol,
                    message=msg,
                )
            )
            found_any_bullets = True

    if found_any_bullets:
        return findings

    # 3) Legacy-Format: einzelne Line/Symbol/Message-Blöcke
    line_no: Optional[int] = None
    symbol: Optional[str] = None
    message_lines: List[str] = []
    collecting_message = False

    for line in lines:
        l = line.strip()
        if not l:
            if collecting_message:
                # leere Zeile beendet Message-Block
                collecting_message = False
            continue

        if l.lower().startswith("line:"):
            _, rest = l.split(":", 1)
            try:
                line_no = int(rest.strip())
            except ValueError:
                line_no = None
        elif l.lower().startswith("symbol:"):
            _, rest = l.split(":", 1)
            symbol = rest.strip().strip("`")
        elif l.lower().startswith("message:"):
            collecting_message = True
            # alles nach "Message:" auf derselben Zeile als Start
            _, rest = l.split(":", 1)
            rest = rest.strip()
            if rest:
                message_lines.append(rest)
        elif l.lower().startswith("fingerprint:"):
            # Fingerprint markiert Ende; keine Message mehr
            collecting_message = False
        elif collecting_message:
            # weitere Zeilen der Message
            message_lines.append(l)

    if file_path and line_no is not None and symbol:
        msg = " ".join(message_lines).strip() or "Missing XML doc."
        findings.append(
            FindingItem(
                issue_number=number,
                file=file_path,
                line=line_no,
                symbol=symbol,
                message=msg,
            )
        )

    return findings


# ---------------- OpenAI Helper ----------------

def get_openai_client() -> OpenAI:
    api_key = os.getenv("OPENAI_API_KEY")
    if not api_key:
        raise RuntimeError("OPENAI_API_KEY not set")
    return OpenAI(api_key=api_key)


def generate_summary_for_declaration(client: OpenAI, declaration: str, context: str, is_method: bool) -> str:
    """
    Ruft OpenAI auf und gibt die XML-Doc-Zeilen (/// ...) zurück.
    """
    user_prompt = f"""C# declaration:
{declaration}

Context (may be partial body or surrounding code):
{context}

Task:
Generate ONLY the C# XML doc comment lines (starting with '///') that should be placed immediately above this declaration.

Remember:
- Short, clear English.
- 1–2 sentences in <summary>.
- For methods: <param> for each parameter, and <returns> if return type is not void.
- For classes: usually only <summary>.
"""

    resp = client.chat.completions.create(
        model=OPENAI_MODEL,
        messages=[
            {"role": "system", "content": SUMMARY_SYSTEM_PROMPT},
            {"role": "user", "content": user_prompt},
        ],
        temperature=0.1,
    )
    content = resp.choices[0].message.content or ""
    # Kein Codefence erwartet, aber falls doch, einfach trimmen
    lines = [ln for ln in content.splitlines() if ln.strip()]
    # Sicherstellen, dass nur Zeilen mit /// durchkommen
    final = "\n".join(ln for ln in lines if ln.strip().startswith("///"))
    return final.strip()


# ---------------- File Patching ----------------

def insert_summaries_in_file(path: pathlib.Path, items: List[FindingItem], client: OpenAI) -> bool:
    """
    Fügt für jedes FindingItem eine XML-Summary ein.
    Bearbeitet die Datei in-place.
    Gibt True zurück, wenn Änderungen gemacht wurden.
    """
    text = path.read_text(encoding="utf-8", errors="ignore")
    lines = text.splitlines()

    # Items von unten nach oben, damit Zeilenindizes stabil bleiben
    items_sorted = sorted(items, key=lambda it: it.line, reverse=True)
    changed = False

    for item in items_sorted:
        idx = item.line - 1
        if idx < 0 or idx >= len(lines):
            continue

        # Skip, wenn bereits ein ///-Block direkt drüber hängt
        if idx - 1 >= 0 and lines[idx - 1].strip().startswith("///"):
            continue

        decl_line = lines[idx]
        # Wenn die Deklarationszeile kein public/protected/internal hat, ist evtl. Zeilennummer leicht off.
        # Wir versuchen, nach oben zu suchen, bis wir eine "Deklarationszeile" finden.
        search_idx = idx
        while search_idx >= 0 and not RE_DECLARATION_LINE.match(lines[search_idx]):
            search_idx -= 1
        if search_idx >= 0:
            decl_line = lines[search_idx]
            idx = search_idx  # wir setzen Insert-Position hierhin

        # Kleines Kontextfenster für den LLM (ein paar Zeilen darunter)
        context_slice = lines[idx: min(len(lines), idx + 15)]
        context = "\n".join(context_slice)

        is_method = "method" in item.symbol  # grobe Heuristik; aus dem ReviewAgent-Symbol
        xml = generate_summary_for_declaration(
            client=client,
            declaration=decl_line,
            context=context,
            is_method=is_method,
        )
        if not xml:
            continue

        xml_lines = xml.splitlines()
        # Einfügen direkt über der Deklaration
        lines[idx:idx] = xml_lines
        changed = True

    if changed:
        path.write_text("\n".join(lines) + "\n", encoding="utf-8")
    return changed


# ---------------- Git Helpers ----------------

def git_run(args: List[str]) -> None:
    subprocess.run(["git"] + args, check=True)


def ensure_branch(branch: str) -> None:
    # neue Branch vom aktuellen HEAD
    git_run(["checkout", "-b", branch])


def any_changes() -> bool:
    result = subprocess.run(["git", "status", "--porcelain"], capture_output=True, text=True)
    return bool(result.stdout.strip())


def commit_all(message: str) -> None:
    git_run(["add", "."])
    git_run(["commit", "-m", message])


def push_branch(branch: str) -> None:
    git_run(["push", "origin", branch])


def create_pull_request(session: requests.Session, owner: str, repo: str,
                        branch: str, base: str) -> str:
    url = f"https://api.github.com/repos/{owner}/{repo}/pulls"
    payload = {
        "title": "Auto-add XML summaries (Agent/Summary)",
        "head": branch,
        "base": base,
        "body": "This PR was created by FixAgentSummary bot to add missing XML documentation summaries.",
    }
    resp = session.post(url, json=payload)
    if resp.status_code not in (200, 201):
        raise RuntimeError(f"Failed to create PR: {resp.status_code} {resp.text}")
    pr = resp.json()
    print(f"Created PR #{pr.get('number')} -> {pr.get('html_url')}")
    return pr.get("html_url")


# ---------------- Main ----------------

def main():
    owner, repo = get_repo_from_env()
    session = get_github_session()
    client = get_openai_client()

    issues = list_summary_issues(session, owner, repo)
    if not issues:
        print("FixSummaryBot: No open Agent/Summary issues.")
        return

    # Findings je Datei sammeln
    file_to_items: Dict[str, List[FindingItem]] = {}
    issue_numbers: set[int] = set()

    for issue in issues:
        items = parse_findings_from_issue(issue)
        if not items:
            continue
        for item in items:
            file_to_items.setdefault(item.file, []).append(item)
            issue_numbers.add(item.issue_number)

    if not file_to_items:
        print("FixSummaryBot: No parsable findings in issues.")
        return

    # Neue Branch erzeugen
    branch_name = f"autofix/summary-{int(time.time())}"
    print(f"FixSummaryBot: Creating branch {branch_name}")
    ensure_branch(branch_name)

    # Dateien patchen
    any_file_changed = False
    for file_path, items in file_to_items.items():
        p = ROOT / file_path
        if not p.exists():
            print(f"FixSummaryBot: File not found (skipping): {file_path}")
            continue
        print(f"FixSummaryBot: Updating {file_path} ({len(items)} item(s))")
        changed = insert_summaries_in_file(p, items, client)
        any_file_changed |= changed

    if not any_file_changed or not any_changes():
        print("FixSummaryBot: No changes were made; aborting commit/PR.")
        return

    commit_all("chore: add XML summaries (auto)")
    push_branch(branch_name)

    base_branch = os.getenv("BASE_BRANCH", "main")
    pr_url = create_pull_request(session, owner, repo, branch_name, base_branch)

    # Issues schließen
    for issue_number in issue_numbers:
        close_issue(session, owner, repo, issue_number, pr_url)

    print("FixSummaryBot: Done.")


if __name__ == "__main__":
    main()
