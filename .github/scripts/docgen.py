import os, pathlib
import re
from datetime import datetime
from git import Repo
from openai import OpenAI

# ----- Einstellungen -----
DOC_EXTS = {".cs", ".py", ".ts", ".tsx", ".js", ".java", ".go"}
OUT_DIR = pathlib.Path("docs")
OUT_DIR.mkdir(parents=True, exist_ok=True)
DOC_ROOT = OUT_DIR

#regex einstellungen
NS_RX = re.compile(r'^\s*namespace\s+([A-Za-z0-9_.]+)', re.MULTILINE)
TYPE_RX = re.compile(
    r'^\s*public\s+(class|struct|interface|enum)\s+([A-Za-z0-9_]+)',
    re.MULTILINE
)

SYSTEM_PROMPT = """You are a technical writer. Generate concise, factual, diff-friendly documentation from the provided **single source file only** (no external assumptions). 
If previous documentation is provided, treat it as the baseline. Reuse bullets that are still accurate. Update or add/remove bullets only where the code changed. Keep section order and names stable. Do not duplicate sections.
If previous documentation is not provided, create full documentation fresh. Write in clear English, bullet-first
Output sections in this fixed order (omit a section if empty):
1) Purpose
- What this file defines/provides (1–3 bullets), strictly from code.
2) Public API
- Namespace/module (if any)
- Types
  - <visibility> <kind> <Name> [extends/implements ...]
    - Public fields/properties (brief role if obvious)
    - Public methods (signatures with parameters/returns; note explicit side effects)
3) Key Behavior & Side Effects
- Major flows/state changes/error handling that are explicit in this file.
4) Constraints & Failure Modes
- Guards, null/empty handling, threading/async notes, performance/allocation hints (only if evident).
5) Example (only if clearly derivable)
- minimal example 
6) Unknowns
- Facts that cannot be determined from this file.

overall rules:
- Source of truth = this project only. Prefer omission over guessing.
- Exhaustively list public surface; keep names/signatures exact.
- Use short bullets; avoid prose, timestamps, and authors.
- If file is trivial or auto-generated, output a single line: "Skipped: trivial/auto-generated."
- Use an appropriate code fence language tag.
- Never use ```markdown or ```md fences or any thing similar like ```text (generated files already has the sufficient .md suffix no need for fences!) ; only use code fences for code examples with the correct language (e.g., csharp, python).
Unity specifics (apply only if explicitly present in this file):
- MonoBehaviour: list lifecycle methods (Awake/Start/Update/OnEnable/OnDisable/OnDestroy) with one-line purpose.
- ScriptableObject: treat as data/config asset; summarize serialized fields; mention [CreateAssetMenu] if present.
- Editor-only: if under an Editor folder or using UnityEditor, mark as editor tooling.
- RequireComponent / physics callbacks: note required components and that OnTrigger*/OnCollision*/FixedUpdate are physics-related.
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

def extract_namespace_and_public_types(code: str):
    ns_match = NS_RX.search(code)
    namespace = ns_match.group(1) if ns_match else "global"
    types = [(m.group(1), m.group(2)) for m in TYPE_RX.finditer(code)]
    return namespace, types

def strip_outer_markdown_fence(text: str) -> str:
    # Entfernt genau einen äußeren ```markdown oder ```md Block, lässt innere Fences in Ruhe
    m = re.match(r'^\s*```(?:markdown|md)\s*\n([\s\S]*?)\n```\s*$', text)
    if m:
        return m.group(1).rstrip() + "\n"
    return text

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

def paths_for(namespace: str, type_names: list[str], fallback_basename: str):
    """
    Ablage nach Namespace: docs/<Namespace/als/Ordner>/<Type>.md
    Beispiel: docs/CHAL/Systems/Research/ResearchService.md
    """
    base = DOC_ROOT / namespace.replace('.', '/')
    if type_names:
        return [ base / f"{t}.md" for t in type_names ]
    return [ base / f"{fallback_basename}.md" ]

def all_repo_files():
    # alle getrackten Dateien mit passenden Endungen
    paths = []
    for p in pathlib.Path(".").rglob("*"):
        if p.is_file() and p.suffix in DOC_EXTS and ".git" not in p.parts:
            paths.append(str(p.as_posix()))
    return paths

def files_to_process():
    # FULL_SCAN=true (oder 'True') -> alles
    full = (os.getenv("FULL_SCAN","").lower() == "true")
    if full:
        return all_repo_files()
    # sonst nur Änderungen seit letztem Commit
    return changed_files_since_last_commit()

# ----- LLM-Aufruf -----
def llm_markdown_for(path: str, code: str) -> str:
    client = OpenAI(api_key=os.getenv("OPENAI_API_KEY"))
    message = f"File: {path}\n\n```\n{code[:120000]}\n```"
    resp = client.chat.completions.create(
        model="gpt-5-nano",
        messages=[
            {"role":"system","content": SYSTEM_PROMPT},
            {"role":"user","content": message},
        ],
        temperature=1,
    )
    md = resp.choices[0].message.content.strip()
    return strip_outer_markdown_fence(md)

def main():
    files = files_to_process()
    if not files:
        print("No relevant changes.")
        return

    index_lines = [
        "# Automatic Documentation",
        "",
        f"_Status: {datetime.utcnow().isoformat()}Z_",
        "",
        "Changed files:",
    ]

    any_change = False
    for f in files:
        code = read_text(f)

        # Namespace + öffentliche Typen aus dem Code ziehen
        namespace, pub_types = extract_namespace_and_public_types(code)
        type_names = [name for _, name in pub_types]

        # Zielpfade erzeugen (eine Datei je public Type, sonst Fallback auf Dateibasis)
        fallback_basename = pathlib.Path(f).with_suffix("").name
        targets = paths_for(namespace, type_names, fallback_basename)

        if type_names:
            for t, out_path in zip(type_names, targets):
                fq = f"{namespace}.{t}" if namespace else t
                md = llm_markdown_for(f, code)
                header = f"# {fq}\n\n_Automatically generated/updated from `{f}`._\n\n"
                out = header + md + "\n"
                changed = write_if_changed(out_path, out)
                any_change |= changed
                index_lines.append(f"- [{fq}]({out_path.relative_to(OUT_DIR).as_posix()}){' (neu)' if changed else ''}")
        else:
            # kein public type -> Datei unter docs/<Namespace>/<Dateiname>.md
            out_path = targets[0]
            fq = f"{namespace}.{fallback_basename}" if namespace else fallback_basename
            md = llm_markdown_for(f, code)
            header = f"# {fq}\n\n_Automatically generated/updated from `{f}`._\n\n"
            out = header + md + "\n"
            changed = write_if_changed(out_path, out)
            any_change |= changed
            index_lines.append(f"- [{fq}]({out_path.relative_to(OUT_DIR).as_posix()}){' (neu)' if changed else ''}")

    write_if_changed(OUT_DIR / "INDEX.md", "\n".join(index_lines) + "\n")
    print("Complete. Changes:", any_change)

if __name__ == "__main__":
    main()
