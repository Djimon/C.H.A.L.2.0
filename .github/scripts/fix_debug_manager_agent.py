import os
import pathlib
import re
import time
import subprocess
from dataclasses import dataclass
from typing import List, Dict, Optional, Tuple

import requests

# ---------------- CONFIG ----------------

ROOT = pathlib.Path(".")
AGENT_LABEL = "Agent"
DEBUG_MANAGER_LABEL = "Agent/DebugManager"

RE_WRONG_DEBUG = re.compile(
    r'\bDebug\.(LogWarning|LogError|Log)\s*\(',
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


def list_debug_manager_issues(session: requests.Session, owner: str, repo: str) -> List[dict]:
    issues: List[dict] = []
    page = 1
    per_page = 50

    while True:
        url = f"https://api.github.com/repos/{owner}/{repo}/issues"
        params = {
            "state": "open",
            "labels": f"{AGENT_LABEL},{DEBUG_MANAGER_LABEL}",
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
        print(f"FixDebugManagerBot: Failed to close issue #{issue_number}: {resp.status_code} {resp.text}")


# ---------------- Issue Body Parsing ----------------

def parse_findings_from_issue(issue: dict) -> List[FindingItem]:
    """
    Erwartet Body im Format vom ReviewAgent, z.B.:

    Automatic finding by ReviewAgent.

    **Kind:** Agent/DebugManager
    **File:** `Assets/.../Foo.cs`
    **Line:** 42
    **Symbol:** `Debug.Log`
    **Message:** Use DebugManager instead of UnityEngine.Debug.*.
    """
    number = issue["number"]
    body = issue.get("body") or ""
    lines = body.splitlines()

    findings: List[FindingItem] = []

    file_path: Optional[str] = None
    line_no: Optional[int] = None
    symbol: Optional[str] = None

    for line in lines:
        l = line.strip()
        if not l:
            continue
        lower = l.lower()
        if "file:" in lower:
            idx = lower.find("file:")
            rest = l[idx + len("file:"):]
            rest = rest.replace("*", "").strip()
            file_path = rest.strip("`").strip()
        elif "line:" in lower:
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

    if file_path and line_no is not None and symbol:
        findings.append(
            FindingItem(
                issue_number=number,
                file=file_path,
                line=line_no,
                symbol=symbol,
                message="Wrong logger usage",
            )
        )
        print(f"parse_findings_from_issue: issue #{number} -> 1 finding ({file_path}:{line_no} {symbol})")
    else:
        print(f"parse_findings_from_issue: issue #{number} -> no finding (file={file_path}, line={line_no}, symbol={symbol})")

    return findings


# ---------------- File Patching ----------------

def replace_wrong_debug_in_line(line: str) -> str:
    """
    Ersetzt Debug.Log*, Debug.LogWarning, Debug.LogError durch DebugManager.*.
    Mappings:
      Debug.Log        -> DebugManager.DebugLog
      Debug.LogWarning -> DebugManager.Warning
      Debug.LogError   -> DebugManager.Error
    """

    def repl(match: re.Match) -> str:
        method = match.group(1)  # Log | LogWarning | LogError
        if method == "Log":
            new = "DebugManager.DebugLog"
        elif method == "LogWarning":
            new = "DebugManager.Warning"
        else:  # LogError
            new = "DebugManager.Error"
        return new + "("

    return RE_WRONG_DEBUG.sub(repl, line)


def fix_debug_calls_in_file(path: pathlib.Path) -> bool:
    """
    Geht die Datei Zeile für Zeile durch und ersetzt falsche Debug-Aufrufe.
    Kommentare, die mit // beginnen, werden übersprungen.
    """
    text = path.read_text(encoding="utf-8", errors="ignore")
    lines = text.splitlines()
    changed = False

    for i, line in enumerate(lines):
        stripped = line.lstrip()
        if stripped.startswith("//"):
            continue  # Kommentarzeile ignorieren

        new_line = replace_wrong_debug_in_line(line)
        if new_line != line:
            lines[i] = new_line
            changed = True
            # Debug-Ausgabe optional:
            # print(f"FixDebugManagerBot: Replaced in {path}:{i+1}")

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
    git_run(["push", "origin", branch])


def create_pull_request(session: requests.Session, owner: str, repo: str,
                        branch: str, base: str) -> Optional[str]:
    url = f"https://api.github.com/repos/{owner}/{repo}/pulls"
    payload = {
        "title": "Replace UnityEngine.Debug calls with DebugManager (Agent/DebugManager)",
        "head": branch,
        "base": base,
        "body": "This PR was created by FixAgentDebugManager bot to replace direct Debug.Log* calls with DebugManager equivalents.",
    }
    resp = session.post(url, json=payload)
    if resp.status_code not in (200, 201):
        print(f"FixDebugManagerBot: Failed to create PR ({resp.status_code}).")
        print(f"Response: {resp.text}")
        print(f"FixDebugManagerBot: Please create a PR manually from branch '{branch}' into '{base}'.")
        return None

    pr = resp.json()
    print(f"Created PR #{pr.get('number')} -> {pr.get('html_url')}")
    return pr.get("html_url")


# ---------------- Main ----------------

def main():
    owner, repo = get_repo_from_env()
    session = get_github_session()

    issues = list_debug_manager_issues(session, owner, repo)
    print(f"FixDebugManagerBot: Found {len(issues)} open issues with labels {AGENT_LABEL} + {DEBUG_MANAGER_LABEL}.")
    if not issues:
        print("FixDebugManagerBot: No issues to process.")
        return

    file_to_items: Dict[str, List[FindingItem]] = {}
    issue_numbers: set[int] = set()

    for issue in issues:
        print(f"FixDebugManagerBot: Parsing issue #{issue['number']} - {issue.get('title')!r}")
        items = parse_findings_from_issue(issue)
        print(f"FixDebugManagerBot:   -> parsed {len(items)} finding(s) from issue #{issue['number']}")
        if not items:
            body = (issue.get("body") or "").splitlines()
            preview = "\n".join(body[:8])
            print(f"FixDebugManagerBot:   body preview:\n{preview}\n---")
            continue
        for item in items:
            file_to_items.setdefault(item.file, []).append(item)
            issue_numbers.add(item.issue_number)

    if not file_to_items:
        print("FixDebugManagerBot: No parsable findings in issues.")
        return

    branch_name = f"autofix/debug-manager-{int(time.time())}"
    print(f"FixDebugManagerBot: Creating branch {branch_name}")
    ensure_branch(branch_name)

    any_file_changed = False
    for file_path, items in file_to_items.items():
        p = ROOT / file_path
        if not p.exists():
            print(f"FixDebugManagerBot: File not found (skipping): {file_path}")
            continue
        print(f"FixDebugManagerBot: Updating {file_path} (Debug.Log -> DebugManager.*)")
        changed = fix_debug_calls_in_file(p)
        any_file_changed |= changed

    if not any_file_changed or not any_changes():
        print("FixDebugManagerBot: No changes were made; aborting commit/PR.")
        return

    commit_all("chore: replace Debug.Log* with DebugManager (auto)")
    push_branch(branch_name)

    base_branch = os.getenv("BASE_BRANCH", "master")
    pr_url = create_pull_request(session, owner, repo, branch_name, base_branch)

    if pr_url is None:
        print("FixDebugManagerBot: No PR URL (PR not created). Leaving issues open.")
        print(f"FixDebugManagerBot: You can now create a PR manually from branch '{branch_name}'.")
        return

    # Issues schließen
    for issue_number in issue_numbers:
        close_issue(session, owner, repo, issue_number, pr_url)

    print("FixDebugManagerBot: Done.")


if __name__ == "__main__":
    main()
