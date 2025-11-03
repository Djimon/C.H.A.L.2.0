import os
import pathlib
import re
from dataclasses import dataclass
from typing import List, Optional
from git import Repo

# -------- Config --------
ROOT = pathlib.Path(".")
DOC_EXTS = {".cs"}  # aktuell nur C#, kann erweitert werden
SUMMARY_FINDING = "Agent/Summary"
DEBUG_FINDING = "Agent/DebugLanguage"


@dataclass
class Finding:
    kind: str          # SUMMARY_FINDING oder DEBUG_FINDING
    file: str          # relativer Pfad
    line: int          # 1-based
    symbol: str        # Klassen-/Methodenname oder "Debug call"
    message: str       # kurze Beschreibung


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
    """
    findings: List[Finding] = []
    lines = text.splitlines()

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
        m_prop = RE_PUBLIC_PROPERTY.match(line)
        if m_prop:
            name = m_prop.group(1)
            findings.append(
                Finding(
                    kind=SUMMARY_FINDING,
                    file=path,
                    line=idx + 1,
                    symbol=f"property {name}",
                    message=f"Missing <summary> XML doc for public property '{name}'.",
                )
            )
            continue

    return findings


# -------- Check B: Debug-Messages Englisch --------

# einfache Heuristik: Debug.*(...) oder irgendwasLog(...)
RE_DEBUG_CALL = re.compile(
    r'(Debug\w*\.Log\w*|DebugManager\.\w+|\w*Logger\.\w+)\s*\(\s*(@"[^"]*"|"[^"]*")',
    re.MULTILINE,
)

GERMAN_HINT_WORDS = [
    "nicht", "kein","erfolgreich","kein","laden", "mit","fehler", "fehlgeschlagen", "fertig",
    "forschung", "ausrüstung", "bereit", "spieler", "karte",
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
    letters = [ch for ch in s if ch.isalpha()]
    if letters:
        ascii_letters = [ch for ch in letters if "a" <= ch.lower() <= "z"]
        if len(ascii_letters) / len(letters) < 0.7:
            return True

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

    return findings


def main():
    findings = run_review()
    if not findings:
        print("ReviewAgent: No findings for Summary or DebugLanguage.")
        return

    # Aktuell: nur in der Konsole ausgeben.
    # Hier später: GitHub Issues erstellen pro Finding.
    print("ReviewAgent: Findings:")
    for f in findings:
        print(
            f"[{f.kind}] {f.file}:{f.line} — {f.symbol} :: {f.message}"
        )


if __name__ == "__main__":
    main()
