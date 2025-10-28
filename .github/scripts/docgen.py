import os, pathlib
from datetime import datetime
from git import Repo
from openai import OpenAI

# ----- Einstellungen (kannst du so lassen) -----
DOC_EXTS = {".cs", ".py", ".ts", ".tsx", ".js", ".java", ".go"}
OUT_DIR = pathlib.Path("docs")
OUT_DIR.mkdir(parents=True, exist_ok=True)

SYSTEM_PROMPT = """Du bist Technischer Redakteur.
Erzeuge aus Code knappe, sachliche Doku (Deutsch):
- Zweck der Datei
- Öffentliche API (Klassen/Funktionen, Parameter, Rückgabewerte)
- Wichtige Abläufe / Nebenwirkungen
- Randbedingungen/Fehlerfälle
- Kurzes Beispiel (falls sinnvoll)
Stabil, stichpunktartig, diff-freundlich. Wenn Datei trivial/auto-generiert -> Ein Satz: 'Übersprungen, da trivial/auto-generiert'.
"""

# ----- Hilfsfunktionen -----
def changed_files_since_last_commit():
    repo = Repo(".")
    # Nimm einfach den letzten Commit-Vergleich. Für PRs reicht das ebenfalls.
    try:
        diff = repo.git.diff("--name-only", "HEAD~1..HEAD")
    except Exception:
        diff = repo.git.ls_files()
    files = [f.strip() for f in diff.splitlines() if f.strip()]
    files = [f for f in files if pathlib.Path(f).suffix in DOC_EXTS and pathlib.Path(f).exists()]
    return files

def read_text(path):
    return pathlib.Path(path).read_text(encoding="utf-8", errors="ignore")

def write_if_changed(path: pathlib.Path, content: str) -> bool:
    old = path.read_text(encoding="utf-8") if path.exists() else ""
    if old.strip() != content.strip():
        path.parent.mkdir(parents=True, exist_ok=True)
        path.write_text(content, encoding="utf-8")
        return True
    return False

def doc_path_for(src_path: str) -> pathlib.Path:
    # docs/<src_path>.md
    return OUT_DIR / f"{src_path}.md"

# ----- LLM-Aufruf -----
def llm_markdown_for(path: str, code: str) -> str:
    client = OpenAI(api_key=os.getenv("OPENAI_API_KEY"))
    message = f"Datei: {path}\n\n```\n{code[:120000]}\n```"
    resp = client.chat.completions.create(
        model="gpt-4o-mini",
        messages=[
            {"role":"system","content": SYSTEM_PROMPT},
            {"role":"user","content": message},
        ],
        temperature=0.1,
    )
    return resp.choices[0].message.content.strip()

def main():
    files = changed_files_since_last_commit()
    if not files:
        print("Keine relevanten Änderungen erkannt.")
        return

    index_lines = [
        "# Automatische Doku",
        "",
        f"_Stand: {datetime.utcnow().isoformat()}Z_",
        "",
        "Geänderte Dateien in diesem Lauf:",
    ]

    any_change = False
    for f in files:
        code = read_text(f)
        md = llm_markdown_for(f, code)
        header = f"# {f}\n\n_Automatisch generiert/aktualisiert._\n\n"
        out = header + md + "\n"
        out_path = doc_path_for(f)
        changed = write_if_changed(out_path, out)
        any_change |= changed
        index_lines.append(f"- [{f}]({out_path.relative_to(OUT_DIR).as_posix()}){' (neu)' if changed else ''}")

    write_if_changed(OUT_DIR / "INDEX.md", "\n".join(index_lines) + "\n")
    print("Fertig. Änderungen:", any_change)

if __name__ == "__main__":
    main()
