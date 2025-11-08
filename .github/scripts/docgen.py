import os, pathlib
import re
from datetime import datetime
from git import Repo
from openai import OpenAI
from difflib import SequenceMatcher

# ----- Einstellungen -----
DOC_EXTS = {".cs", ".py", ".ts", ".tsx", ".js", ".java", ".go"}
EXCLUDE_DIRS = set(d.strip() for d in os.getenv("DOCGEN_EXCLUDE_DIRS", "BayatGames,MatthewAssets,ThirdParty,Packages").split(","))
ALLOWED_NAMESPACE_PREFIXES = tuple(s.strip() for s in os.getenv("DOCGEN_ALLOWED_NS", "CHAL,global").split(","))

OUT_DIR = pathlib.Path("docs")
OUT_DIR.mkdir(parents=True, exist_ok=True)
DOC_ROOT = OUT_DIR

# Konfiguration (Env oder Default)
CHANGE_RATE_THRESHOLD = float(os.getenv("DOCGEN_CHANGE_RATE", "0.10"))  # 10% default

INDEX_MAX_LEVELS = int(os.getenv("DOCGEN_INDEX_MAX_LEVELS", "3"))

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
    try:
        # Wenn es keinen HEAD~1 gibt (z.B. erster Commit), fallback auf ls-files
        diff = repo.git.diff("--name-only", "HEAD~1..HEAD")
        if not diff.strip():
            diff = repo.git.diff("--name-only", "--cached") or repo.git.ls_files()
    except Exception:
        diff = repo.git.ls_files()

    files = [f.strip() for f in diff.splitlines() if f.strip()]
    files = [
        f for f in files
        if pathlib.Path(f).suffix in DOC_EXTS
        and pathlib.Path(f).exists()
        and not is_excluded_path(pathlib.Path(f))
    ]
    return files

def is_excluded_path(p: pathlib.Path) -> bool:
    # irgendein Pfadteil gehört zu EXCLUDE_DIRS?
    return any(part in EXCLUDE_DIRS for part in p.parts)

def extract_namespace_and_public_types(code: str):
    ns_match = NS_RX.search(code)
    namespace = ns_match.group(1) if ns_match else "global"
    types = [(m.group(1), m.group(2)) for m in TYPE_RX.finditer(code)]
    return namespace, types

def strip_outer_markdown_fence(text: str) -> str:
    # Entfernt genau einen äußeren ```markdown oder ```md Block, lässt innere Fences in Ruhe
    m = re.match(r'^\s*```(?:markdown|md|text)\s*\n([\s\S]*?)\n```\s*$', text)
    if m:
        return m.group(1).rstrip() + "\n"
    return text

def load_existing_doc_body(path: pathlib.Path) -> str | None:
    """
    Liest die bestehende .md, entfernt den Auto-Header (# FQN / _Automatically generated..._),
    gibt nur den eigentlichen fachlichen Body zurück.
    Wenn Datei nicht existiert -> None.
    """
    if not path.exists():
        return None

    raw = path.read_text(encoding="utf-8", errors="ignore").strip()

    # Wir haben aktuell Header in der Form:
    #   # <fq>
    #
    #   _Automatically generated/updated from `...`._
    #
    #   <rest...>

    lines = raw.splitlines()
    # Heuristik: schmeiß die ersten leerzeilen / headerzeilen weg bis wir beim "echten" Content sind.
    cleaned = []
    skipping = True
    for line in lines:
        if skipping:
            # Wir skippen solange Zeilen anfangen mit "# " oder "_" oder leer sind.
            if line.startswith("# "):
                continue
            if line.strip().startswith("_Automatically"):
                continue
            if line.strip() == "":
                continue
            # ab hier erster "echter" Inhalt
            skipping = False
        cleaned.append(line)
    body = "\n".join(cleaned).strip()
    return body if body else None

def read_text(path):
    return pathlib.Path(path).read_text(encoding="utf-8", errors="ignore")

def change_rate(a: str, b: str) -> float:
    """
    Liefert Anteil geänderter Zeichen (0.0..1.0).
    0.0 = identisch, 1.0 = komplett verschieden.
    """
    # SequenceMatcher ratio = Gleichheit; wir wollen Änderungsrate:
    ratio = SequenceMatcher(None, a.strip(), b.strip()).ratio()
    return 1.0 - ratio

def write_if_changed(path: pathlib.Path, content: str) -> bool:
    """
    Schreibt nur, wenn die Änderungsrate >= Schwelle ist (oder Datei neu).
    """
    if not path.exists():
        path.parent.mkdir(parents=True, exist_ok=True)
        path.write_text(content, encoding="utf-8")
        return True

    old = path.read_text(encoding="utf-8")
    delta = change_rate(old, content)

    if delta >= CHANGE_RATE_THRESHOLD:
        path.write_text(content, encoding="utf-8")
        return True

    return False

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
    paths = []
    for p in pathlib.Path(".").rglob("*"):
        if p.is_file() and p.suffix in DOC_EXTS and ".git" not in p.parts and not is_excluded_path(p):
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
def llm_markdown_for(path: str, code: str, old_body: str | None) -> str:
    client = OpenAI(api_key=os.getenv("OPENAI_API_KEY"))

    # Wir bauen die User-Message:
    # - file path
    # - previous doc (falls da)
    # - full source code
    parts = [f"File: {path}"]

    if old_body:
        parts.append("Previous documentation (to update, not to discard):")
        parts.append(old_body)

    parts.append("Source code:")
    parts.append("```")
    parts.append(code[:120000])
    parts.append("```")

    message = "\n\n".join(parts)

    resp = client.chat.completions.create(
        model="gpt-4o-mini",
        messages=[
            {"role":"system","content": SYSTEM_PROMPT},
            {"role":"user","content": message},
        ],
        temperature=0.1,
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
    # tracke für den Index, welche Dateien in DIESEM Lauf neu/aktualisiert wurden
    changed_map = {}  # rel_path (str) -> True/False
    for f in files:
        code = read_text(f)

        # Namespace + öffentliche Typen aus dem Code ziehen
        namespace, pub_types = extract_namespace_and_public_types(code)
        type_names = [name for _, name in pub_types]

        # Zielpfade erzeugen (eine Datei je public Type, sonst Fallback auf Dateibasis)
        fallback_basename = pathlib.Path(f).with_suffix("").name
        targets = paths_for(namespace, type_names, fallback_basename)

        if type_names:
            # eine Datei pro public type
            for t, out_path in zip(type_names, targets):
                fq = f"{namespace}.{t}" if namespace else t

                # Bestehende Body-Doku (ohne Header) laden
                old_body = load_existing_doc_body(out_path)

                # Neue/aktualisierte Doku vom Modell holen
                md_body = llm_markdown_for(f, code, old_body)

                header = (
                    f"# {fq}\n\n"
                    f"_Automatically generated/updated from `{f}`._\n\n"
                )
                out = header + md_body + "\n"

                changed = write_if_changed(out_path, out)
                rel_for_index = out_path.relative_to(OUT_DIR).as_posix()
                changed_map[rel_for_index] = changed
                any_change |= changed

                index_lines.append(
                    f"- [{fq}]({out_path.relative_to(OUT_DIR).as_posix()})"
                    f"{' (new)' if changed else ''}"
                )

        else:
            # kein public type -> Datei unter docs/<Namespace>/<Dateiname>.md
            out_path = targets[0]
            fq = f"{namespace}.{fallback_basename}" if namespace else fallback_basename

            old_body = load_existing_doc_body(out_path)
            md_body = llm_markdown_for(f, code, old_body)

            header = (
                f"# {fq}\n\n"
                f"_Automatically generated/updated from `{f}`._\n\n"
            )
            out = header + md_body + "\n"
            new_out = header + md_body + "\n"

            if out_path.exists():
                old_out = out_path.read_text(encoding="utf-8")
                delta = change_rate(old_out, new_out)
                if delta < CHANGE_RATE_THRESHOLD:
                    # Für den Index markieren wir NICHT als geändert
                    changed = False
                else:
                    changed = write_if_changed(out_path, new_out)
            else:
                changed = write_if_changed(out_path, new_out)

            rel_for_index = out_path.relative_to(OUT_DIR).as_posix()
            changed_map[rel_for_index] = changed
            any_change |= changed

            index_lines.append(
                f"- [{fq}]({out_path.relative_to(OUT_DIR).as_posix()})"
                f"{' (new)' if changed else ''}"
            )


    def last_commit_date_str(path: pathlib.Path) -> str:
        # Git-Commit-Datum statt mtime; fallback mtime
        try:
            repo = Repo(".")
            rel = str(path.as_posix())
            # --date=short (YYYY-MM-DD), %ad = author date
            dt = repo.git.log("-1", '--format=%ad', '--date=short', "--", rel).strip()
            if dt:
                return dt
        except Exception:
            pass
        # fallback: mtime (YYYY-MM-DD)
        ts = datetime.fromtimestamp(path.stat().st_mtime)
        return ts.strftime("%Y-%m-%d")

    def should_list_namespace(ns: str) -> bool:
        # Nur gewünschte Präfixe (z. B. "CHAL" und "global")
        return ns.startswith(ALLOWED_NAMESPACE_PREFIXES)

    def build_namespace_tree(namespace_map):
        tree = {}
        for ns, entries in namespace_map.items():
            if not should_list_namespace(ns):
                continue
            parts = ns.split(".")
            cursor = tree
            for depth, part in enumerate(parts):
                key = part
                if depth >= INDEX_MAX_LEVELS:
                    # Rest zusammenfassen
                    key = ".".join(parts[INDEX_MAX_LEVELS - 1:])
                    cursor = cursor.setdefault(key, {})
                    break
                cursor = cursor.setdefault(key, {})
            # Marker für Einträge
            cursor.setdefault("__entries__", []).extend(entries)
        return tree

    def render_tree(node: dict, level: int, index_lines: list):
        # level: 2 -> "##", 3 -> "###", 4 -> "####"
        for key, child in sorted(((k, v) for k, v in node.items() if k != "__entries__"), key=lambda x: x[0].lower()):
            hdr = "#" * min(2 + (level - 1), 4)  # max bis ####
            index_lines.append(f"{hdr} {key}")
            index_lines.append("")
            # Childknoten rendern
            render_tree(child, level + 1, index_lines)
            # Dann ggf. die Einträge unter dieser Überschrift
            entries = child.get("__entries__", [])
            for (type_name, rel, stamp, is_new) in sorted(entries, key=lambda x: x[0].lower()):
                index_lines.append(f"- [{type_name}]({rel}) ({stamp})")
            if entries:
                index_lines.append("")

    def build_index(namespace_map):
        index_lines = []
        index_lines.append("# Automatic Documentation")
        index_lines.append("")
        index_lines.append("All documented namespaces and types.")
        index_lines.append("")
        tree = build_namespace_tree(namespace_map)
        render_tree(tree, level=1, index_lines=index_lines)
        return "\n".join(index_lines) + "\n"

    # --- Index sammeln ---
    namespace_map = {}
    for p in OUT_DIR.rglob("*.md"):
        if p.name == "INDEX.md":
            continue
        rel = p.relative_to(OUT_DIR).as_posix()
        ns_key = rel.rsplit("/", 1)[0].replace("/", ".") if "/" in rel else "global"
        type_name = rel.rsplit("/", 1)[-1][:-3]
        is_new = changed_map.get(rel, False)
        stamp = "new" if is_new else last_commit_date_str(p)  # Git-basiert
        namespace_map.setdefault(ns_key, []).append((type_name, rel, stamp, is_new))

    index_text = build_index(namespace_map)
    write_if_changed(OUT_DIR / "INDEX.md", index_text)

    print("Complete. Changes:", any_change)

if __name__ == "__main__":
    main()
