import os
import pathlib
import re
import json
import requests
from dataclasses import dataclass
from typing import List, Optional
from git import Repo

# -------- Config --------
ROOT = pathlib.Path(".")
DOC_EXTS = {".cs"}  # aktuell nur C#, kann erweitert werden
SUMMARY_FINDING = "Agent/Summary"
DEBUG_FINDING = "Agent/DebugLanguage"
DEBUG_MANAGER_FINDING = "Agent/DebugManager"

UNITY_METHOD_EXCLUSIONS = {
    "Awake",
    "OnEnable",
    "Start",
    "Update",
    "FixedUpdate",
    "LateUpdate",
    "OnDisable",
    "OnDestroy",
    "Reset",
    "OnValidate",
    "OnDrawGizmos",
    "OnDrawGizmosSelected",
    "OnTriggerEnter",
    "OnTriggerEnter2D",
    "OnTriggerStay",
    "OnTriggerStay2D",
    "OnTriggerExit",
    "OnTriggerExit2D",
    "OnCollisionEnter",
    "OnCollisionEnter2D",
    "OnCollisionStay",
    "OnCollisionStay2D",
    "OnCollisionExit",
    "OnCollisionExit2D",
}


@dataclass
class Finding:
    kind: str          # SUMMARY_FINDING oder DEBUG_FINDING
    file: str          # relativer Pfad
    line: int          # 1-based
    symbol: str        # Klassen-/Methodenname oder "Debug call"
    message: str       # kurze Beschreibung

# -------- Helper --------

def make_fingerprint(f: Finding) -> str:
    # Eindeutige ID pro Finding: <Kind>|<File>|<Symbol>
    return f"{f.kind}|{f.file}|{f.symbol}"

def get_repo_from_env() -> Optional[tuple[str, str]]:
    repo = os.getenv("GITHUB_REPOSITORY")
    if not repo or "/" not in repo:
        return None
    owner, name = repo.split("/", 1)
    return owner, name


def get_github_session() -> Optional[requests.Session]:
    token = os.getenv("GITHUB_TOKEN")
    if not token:
        print("ReviewAgent: No GITHUB_TOKEN set, skipping issue creation.")
        return None
    s = requests.Session()
    s.headers.update({
        "Authorization": f"Bearer {token}",
        "Accept": "application/vnd.github+json",
    })
    return s


def load_existing_fingerprints(session: requests.Session, owner: str, repo: str) -> set[str]:
    """
    Lädt alle offenen Issues mit Label 'Agent' und sammelt Fingerprints aus der Body-Zeile:
    'Fingerprint: <...>'
    """
    fingerprints: set[str] = set()
    page = 1
    per_page = 50

    while True:
        url = f"https://api.github.com/repos/{owner}/{repo}/issues"
        params = {
            "state": "open",
            "labels": "Agent",
            "page": page,
            "per_page": per_page,
        }
        resp = session.get(url, params=params)
        if resp.status_code != 200:
            print(f"ReviewAgent: Failed to load existing issues: {resp.status_code} {resp.text}")
            break

        issues = resp.json()
        if not issues:
            break

        for issue in issues:
            body = issue.get("body") or ""
            for line in body.splitlines():
                line = line.strip()
                if line.startswith("Fingerprint: "):
                    fp = line[len("Fingerprint: "):].strip()
                    if fp:
                        fingerprints.add(fp)
        page += 1

    return fingerprints


# -------- Git / File discovery --------

def changed_files_since_last_commit() -> List[str]:
    """
    Default: nur geänderte Dateien (HEAD~1..HEAD).
    Fallback: alle getrackten Dateien.
    """
    repo = Repo(".")
    try:
        diff = repo.git.diff("--name-only", "HEAD~1..HEAD")
    except Exception:
        diff = repo.git.ls_files()
    files = [f.strip() for f in diff.splitlines() if f.strip()]
    files = [
        f for f in files
        if pathlib.Path(f).suffix in DOC_EXTS and pathlib.Path(f).exists()
    ]
    return files


def all_repo_files() -> List[str]:
    paths: List[str] = []
    for p in ROOT.rglob("*"):
        if p.is_file() and p.suffix in DOC_EXTS and ".git" not in p.parts:
            paths.append(str(p.as_posix()))
    return paths


def files_to_process() -> List[str]:
    full = (os.getenv("FULL_SCAN", "").lower() == "true")
    return all_repo_files() if full else changed_files_since_last_commit()


def create_issue_for_finding(session: requests.Session, owner: str, repo: str,
                             finding: Finding, fingerprint: str) -> None:
    """
    Erstellt ein Issue für ein Finding, inkl. Fingerprint und Labels.
    """
    # Labels: "Agent", plus kind-spezifisch
    labels = ["Agent", finding.kind]

    title = f"[{finding.kind}] {finding.symbol} in {finding.file}"
    body_lines = [
        f"Automatic finding by ReviewAgent.",
        "",
        f"**Kind:** {finding.kind}",
        f"**File:** `{finding.file}`",
        f"**Line:** {finding.line}",
        f"**Symbol:** `{finding.symbol}`",
        "",
        f"**Message:** {finding.message}",
        "",
        f"Fingerprint: {fingerprint}",
    ]
    body = "\n".join(body_lines)

    url = f"https://api.github.com/repos/{owner}/{repo}/issues"
    payload = {
        "title": title,
        "body": body,
        "labels": labels,
    }
    resp = session.post(url, json=payload)
    if resp.status_code not in (200, 201):
        print(f"ReviewAgent: Failed to create issue for {fingerprint}: {resp.status_code} {resp.text}")
    else:
        issue = resp.json()
        print(f"ReviewAgent: Created issue #{issue.get('number')} for {fingerprint}")


# -------- Check A: fehlende <summary> --------

# grobe Patterns – bewusst pragmatisch gehalten
RE_PUBLIC_TYPE = re.compile(
    r'^\s*public\s+(class|interface)\s+([A-Za-z0-9_]+)', re.MULTILINE
)
RE_PUBLIC_METHOD = re.compile(
    r'^\s*public\s+[\w<>\[\],\s]+\s+([A-Za-z0-9_]+)\s*\(', re.MULTILINE
)
RE_PUBLIC_PROPERTY = re.compile(
    r'^\s*public\s+[\w<>\[\],\s]+\s+([A-Za-z0-9_]+)\s*\{\s*get', re.MULTILINE
)


def check_missing_summary(path: str, text: str) -> List[Finding]:
    """
    Simple Heuristik:
    - '///' Kommentarblöcke werden einem folgenden Declaration-Block zugeordnet.
    - Wenn der Block kein <summary> enthält, zählt er nicht.
    - public Types/Methods/Properties ohne vorherigen /// <summary> => Finding.
    - ABER: public Types nur, wenn der Typ-Block mehr als 10 Zeilen hat.
    """
    findings: List[Finding] = []
    lines = text.splitlines()

    def estimate_block_size(start_idx: int) -> int:
        depth = 0
        seen_open = False
        for j in range(start_idx, len(lines)):
            line = lines[j]
            # recht naive, aber meist ausreichende Klammerzählung
            opens = line.count("{")
            closes = line.count("}")
            if opens:
                seen_open = True
            depth += opens
            depth -= closes
            # sobald wir nach einem ersten '{' wieder bei <= 0 sind, ist der Block zu Ende
            if seen_open and depth <= 0:
                return j - start_idx + 1
        # Fallback: bis Dateiende
        return len(lines) - start_idx

    # Map: line_index -> has_summary_comment
    summary_for_next_decl = [False] * len(lines)

    i = 0
    while i < len(lines):
        line = lines[i]
        if line.strip().startswith("///"):
            # Sammle zusammenhängenden ///-Block
            start = i
            block = []
            while i < len(lines) and lines[i].strip().startswith("///"):
                block.append(lines[i])
                i += 1
            # Enthält dieser Block <summary>?
            block_text = "\n".join(block)
            has_summary = "<summary>" in block_text
            if has_summary and i < len(lines):
                # Markiere: auf der nächsten "deklarationsfähigen" Zeile liegt summary
                summary_for_next_decl[i] = True
            continue
        i += 1

    # Jetzt alle relevanten public-Decls durchgehen
    for idx, line in enumerate(lines):
        # Wir wollen nur echte Deklarationszeilen
        # und nur wenn dort KEIN summary-Flag gesetzt ist
        if summary_for_next_decl[idx]:
            continue

        # Type?
        m_type = RE_PUBLIC_TYPE.match(line)
        if m_type:
            kind, name = m_type.groups()
            # nur flaggen, wenn der Typ-Block "groß genug" ist (> 10 Zeilen)
            block_size = estimate_block_size(idx)
            if block_size <= 10:
                continue
            findings.append(
                Finding(
                    kind=SUMMARY_FINDING,
                    file=path,
                    line=idx + 1,
                    symbol=f"{kind} {name}",
                    message=f"Missing <summary> XML doc for public {kind} '{name}'.",
                )
            )
            continue

        # Method?
        m_method = RE_PUBLIC_METHOD.match(line)
        if m_method:
            name = m_method.group(1)
            # Unity-Standard-Lifecycle-Methoden ignorieren
            if name in UNITY_METHOD_EXCLUSIONS:
                continue
            findings.append(
                Finding(
                    kind=SUMMARY_FINDING,
                    file=path,
                    line=idx + 1,
                    symbol=f"method {name}()",
                    message=f"Missing <summary> XML doc for public method '{name}'.",
                )
            )
            continue

        # Property?
        #m_prop = RE_PUBLIC_PROPERTY.match(line)
        #if m_prop:
        #    name = m_prop.group(1)
        #    findings.append(
        #        Finding(
        #            kind=SUMMARY_FINDING,
        #            file=path,
        #            line=idx + 1,
        #            symbol=f"property {name}",
        #            message=f"Missing <summary> XML doc for public property '{name}'.",
        #        )
        #    )
        #    continue

    return findings


# -------- Check B: Debug-Messages Englisch --------

# einfache Heuristik: Debug.*(...) oder irgendwasLog(...)
RE_DEBUG_CALL = re.compile(
    r'(DebugManager\.\w+)\s*\(\s*(@"[^"]*"|"[^"]*")',
    re.MULTILINE,
)

# Falsche Logger-Nutzung: UnityEngine.Debug.* direkt
RE_WRONG_DEBUG = re.compile(
    r'\bDebug\.(Log|LogWarning|LogError)\s*\(',
    re.MULTILINE,
)

GERMAN_HINT_WORDS = [
    "nicht", "kein","erfolgreich","kein","laden", "mit","fehler", "fehlgeschlagen", "fertig",
    "forschung", "ausrüstung", "bereit", "spieler", "karte","gefunden","konnte","starte",
    "speicher", "daten", "welle", "gegner", "held", "erfolg",
    "abbruch", "überschrieben", "geladen", "speichern",
]
GERMAN_SPECIAL_CHARS = set("äöüÄÖÜß")


def is_likely_non_english(text: str) -> bool:
    s = text.strip().strip('"').strip("'")
    if not s:
        return False

    # Sonderzeichen: deutlicher Hinweis
    if any(ch in GERMAN_SPECIAL_CHARS for ch in s):
        return True

    lower = s.lower()

    # offensichtliche deutsche Wörter?
    if any(w in lower for w in GERMAN_HINT_WORDS):
        return True

    # Wenn extrem wenig A-Z-Anteil -> ebenfalls verdächtig
    #letters = [ch for ch in s if ch.isalpha()]
    #if letters:
    #    ascii_letters = [ch for ch in letters if "a" <= ch.lower() <= "z"]
    #    if len(ascii_letters) / len(letters) < 0.7:
    #        return True

    return False


def check_debug_language(path: str, text: str) -> List[Finding]:
    findings: List[Finding] = []
    lines = text.splitlines()

    for match in RE_DEBUG_CALL.finditer(text):
        full_call = match.group(1)  # e.g. Debug.LogWarning
        literal = match.group(2)    # "..."
        # Zeilennummer bestimmen (einfach über text[:match.start()] zählen)
        prefix = text[: match.start()]
        line = prefix.count("\n") + 1

        if is_likely_non_english(literal):
            findings.append(
                Finding(
                    kind=DEBUG_FINDING,
                    file=path,
                    line=line,
                    symbol=full_call,
                    message=f"Non-English debug message detected: {literal}",
                )
            )

    return findings

def check_wrong_debug_logger(path: str, text: str) -> List[Finding]:
    findings: List[Finding] = []
    for match in RE_WRONG_DEBUG.finditer(text):
        which = match.group(1)  # Log | LogWarning | LogError
        prefix = text[: match.start()]
        line = prefix.count("\n") + 1

        findings.append(
            Finding(
                kind=DEBUG_MANAGER_FINDING,
                file=path,
                line=line,
                symbol=f"Debug.{which}",
                message="Use DebugManager instead of UnityEngine.Debug.*.",
            )
        )
    return findings
    
# -------- Runner / Output --------

def run_review() -> List[Finding]:
    findings: List[Finding] = []
    files = files_to_process()
    if not files:
        print("ReviewAgent: No relevant files.")
        return findings

    for f in files:
        p = pathlib.Path(f)
        try:
            text = p.read_text(encoding="utf-8", errors="ignore")
        except Exception as e:
            print(f"ReviewAgent: Failed to read {f}: {e}")
            continue

        findings.extend(check_missing_summary(f, text))
        findings.extend(check_debug_language(f, text))
        findings.extend(check_wrong_debug_logger(f, text))

    return findings


def main():
    findings = run_review()
    if not findings:
        print("ReviewAgent: No findings for Summary or DebugLanguage.")
        return

    print("ReviewAgent: Findings:")
    for f in findings:
        print(f"[{f.kind}] {f.file}:{f.line} — {f.symbol} :: {f.message}")

    # Versuche Issues zu erstellen
    repo_info = get_repo_from_env()
    session = get_github_session()
    if not repo_info or not session:
        print("ReviewAgent: Missing repo info or token, skipping GitHub issues.")
        return

    owner, repo = repo_info
    existing = load_existing_fingerprints(session, owner, repo)
    print(f"ReviewAgent: Loaded {len(existing)} existing fingerprints from open Agent issues.")

    created_count = 0
    for f in findings:
        fp = make_fingerprint(f)
        if fp in existing:
            # Issue gibt es schon
            continue
        create_issue_for_finding(session, owner, repo, f, fp)
        existing.add(fp)
        created_count += 1

    print(f"ReviewAgent: Issue creation done. New issues created: {created_count}")


if __name__ == "__main__":
    main()
