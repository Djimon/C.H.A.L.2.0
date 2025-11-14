import os, pathlib
import re
import time
import subprocess
import tempfile
import shutil
from datetime import datetime
from git import Repo
from openai import OpenAI
from difflib import SequenceMatcher
import requests


# ----- Einstellungen -----
BASE_BRANCH = os.getenv("BASE_BRANCH", "master")
DOCGEN_DRY_RUN = os.getenv("DOCGEN_DRY_RUN", "false").lower() == "true"
DOC_EXTS = {".cs", ".py", ".ts", ".tsx", ".js", ".java", ".go"}

EXCLUDE_DIRS = set(d.strip().lower() for d in os.getenv(
    "DOCGEN_EXCLUDE_DIRS", "BayatGames,MatthewAssets,ThirdParty,Packages"
).split(","))
EXCLUDE_NAMESPACE_PREFIXES = tuple(s.strip() for s in os.getenv("DOCGEN_EXCLUDE_NS", "BayatGames,MatthewAssets").split(","))
ALLOWED_NAMESPACE_PREFIXES = tuple(s.strip() for s in os.getenv("DOCGEN_ALLOWED_NS", "CHAL,global").split(","))

# Worktree/OUT_DIR NICHT beim Import anlegen (kein Seiteneffekt) – passiert in main()
OUT_DIR: pathlib.Path | None = None
DOC_ROOT: pathlib.Path | None = None

# Konfiguration (Env oder Default)
CHANGE_RATE_THRESHOLD = float(os.getenv("DOCGEN_CHANGE_RATE", "0.1"))  # 10% default
CODE_CHANGE_THRESHOLD = float(os.getenv("DOCGEN_CODE_CHANGE_RATE", "0.10"))  # 10% default

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

# ---------- GitHub Helpers (analog fix_summary_agent) ----------
def _docgen_branch_name() -> str:
    return f"autodoc/docs-{int(time.time())}"

def create_worktree(branch: str) -> str:
    """
    Legt einen separaten Git-Worktree an, der von origin/<BASE_BRANCH> ausgecheckt ist.
    Liefert den absolute Pfad dieses Worktrees zurück.
    """
    wt_root = tempfile.mkdtemp(prefix="docgen_wt_")
    # Sicherstellen: aktueller Repo hat origin/BASE_BRANCH
    git_run(["fetch", "origin", BASE_BRANCH])
    # Neue Branch (falls nicht vorhanden) auf origin/BASE_BRANCH abzweigen & als Worktree anlegen
    # Variante: direkt Worktree mit neuer Branch
    git_run(["worktree", "add", "-B", branch, wt_root, f"origin/{BASE_BRANCH}"])
    return wt_root

def cleanup_worktree(path: str) -> None:
    try:
        # Worktree entfernen, Branch bleibt im Repo (ist gewollt, da gepusht)
        git_run(["worktree", "remove", path, "--force"])
    except Exception:
        pass
    try:
        shutil.rmtree(path, ignore_errors=True)
    except Exception:
        pass

def push_branch(branch: str, cwd: str) -> None:
    # Push aus dem Worktree heraus
    git_run(["push", "origin", branch], cwd=cwd)

def _git_show_file(ref: str, path: str) -> str | None:
    """Liest Dateiinhalt aus Git für ref:path, None wenn nicht vorhanden."""
    try:
        r = subprocess.run(
            ["git", "show", f"{ref}:{path}"],
            capture_output=True, text=True, check=True
        )
        return r.stdout
    except Exception:
        return None

def code_change_rate(src_path: str, compare_ref: str = "HEAD~1") -> float:
    """
    Änderungsrate (0..1) des *Quellcodes* zwischen compare_ref und aktuellem Workspace.
    0.0 = identisch. 1.0 = komplett verschieden. Fehlende Altversion -> 1.0.
    """
    current = pathlib.Path(src_path).read_text(encoding="utf-8")
    previous = _git_show_file(compare_ref, src_path.replace("\\", "/"))
    if previous is None:
        return 1.0
    return 1.0 - SequenceMatcher(None, previous.strip(), current.strip()).ratio()

def get_repo_from_env():
    repo = os.getenv("GITHUB_REPOSITORY")
    if not repo or "/" not in repo:
        raise RuntimeError("GITHUB_REPOSITORY not set")
    owner, name = repo.split("/", 1)
    return owner, name

def get_github_session():
    token = os.getenv("GITHUB_TOKEN")
    if not token:
        raise RuntimeError("GITHUB_TOKEN not set")
    s = requests.Session()
    s.headers.update({
        "Authorization": f"Bearer {token}",
        "Accept": "application/vnd.github+json",
    })
    return s

def git_run(args: list[str], cwd: str | None = None) -> None:
    subprocess.run(["git"] + args, check=True, cwd=cwd)

def docs_changed(cwd: str) -> bool:
    res = subprocess.run(["git", "status", "--porcelain", "docs"], capture_output=True, text=True, cwd=cwd)
    return bool(res.stdout.strip())

def commit_docs(cwd: str | pathlib.Path, message: str) -> bool:
    git_run(["add", "docs"], cwd=str(cwd))
    if not docs_changed(cwd):
        return False
    # Git-Identity für CI falls nötig
    try:
        git_run(["config", "user.email"], cwd=str(cwd))
    except Exception:
        git_run(["config", "user.email", os.getenv("GIT_EMAIL", "docgen-bot@example.com")], cwd=str(cwd))
        git_run(["config", "user.name", os.getenv("GIT_USER", "docgen-bot")], cwd=str(cwd))
    git_run(["commit", "-m", message], cwd=str(cwd))
    return True

def create_pull_request(session: requests.Session, owner: str, repo: str,
                        branch: str, base: str) -> str:
    url = f"https://api.github.com/repos/{owner}/{repo}/pulls"
    payload = {
        "title": "docgen: auto-update docs",
        "head": branch,
        "base": base,
        "body": "This PR was created by docgen to update generated documentation.",
    }
    resp = session.post(url, json=payload)
    if resp.status_code not in (200, 201):
        raise RuntimeError(f"Failed to create PR: {resp.status_code} {resp.text}")
    pr = resp.json()
    print(f"Created PR #{pr.get('number')} -> {pr.get('html_url')}")
    return pr.get("html_url")



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
    parts_lower = [part.lower() for part in p.parts]
    return any(ex in parts_lower for ex in EXCLUDE_DIRS)

def is_allowed_namespace(ns: str) -> bool:
    # erst harte Excludes
    if any(ns.startswith(prefix) for prefix in EXCLUDE_NAMESPACE_PREFIXES if prefix):
        return False
    # dann nur erlaubte Präfixe
    return any(ns.startswith(prefix) for prefix in ALLOWED_NAMESPACE_PREFIXES if prefix)

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

def _last_commit_message() -> str:
    try:
        r = subprocess.run(
            ["git", "log", "-1", "--pretty=%B"],
            capture_output=True, text=True, check=True
        )
        return (r.stdout or "").strip()
    except Exception:
        return ""

def _parse_docgen_flags(msg: str) -> dict:
    """
    Erlaubte Steuer-Tags in der Commit-Message:
      [docgen:force]  -> erzwingt Regeneration & Write (übergeht beide Thresholds)
    """
    m = msg.lower()
    return {
        "force": "[docgen:force]" in m or "docgen:force" in m,
    }

def write_if_changed(path: pathlib.Path, content: str, force: bool = False) -> bool:
    """
    Schreibt nur, wenn die Änderungsrate >= Schwelle ist (oder Datei neu).
    Bei force=True wird immer geschrieben (wenn Inhalt überhaupt anders ist).
    """
    if not path.exists():
        path.parent.mkdir(parents=True, exist_ok=True)
        path.write_text(content, encoding="utf-8")
        return True

    old = path.read_text(encoding="utf-8")
    if force:
        if old.strip() != content.strip():
            path.write_text(content, encoding="utf-8")
            return True
        return False
    
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

def last_commit_date_str(path: pathlib.Path, cwd: str) -> str:
    # Versucht Git-Log im Worktree; fallback mtime
    try:
        rel = os.path.relpath(str(path), cwd)
        out = subprocess.run(
            ["git", "log", "-1", "--format=%ad", "--date=short", "--", rel],
            capture_output=True, text=True, cwd=cwd
        )
        dt = (out.stdout or "").strip()
        if dt:
            return dt
    except Exception:
        pass
    ts = datetime.fromtimestamp(path.stat().st_mtime)
    return ts.strftime("%Y-%m-%d")

def should_list_namespace(ns: str) -> bool:
    if any(ns.startswith(prefix) for prefix in EXCLUDE_NAMESPACE_PREFIXES if prefix):
        return False
    return any(ns.startswith(prefix) for prefix in ALLOWED_NAMESPACE_PREFIXES if prefix)

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

def build_header_for(src_path: str) -> str:
    # Einzeiliger, stabiler Header-Block; keine Timestamps.
    rel = src_path.replace("\\", "/")
    return f"# {rel}\n\n_Automatically generated/updated from `{rel}`._\n\n"

def out_markdown_path_for(namespace: str, pub_types: list[tuple[str, str]], src_path: str) -> pathlib.Path:
    base = DOC_ROOT / namespace.replace('.', '/')
    if pub_types:
        # erster öffentlicher Typ
        _, tname = pub_types[0]
        return base / f"{tname}.md"
    # Fallback: Name aus Quelldatei
    stem = pathlib.Path(src_path).stem
    return base / f"{stem}.md"

def build_index_for_outdir(out_dir: pathlib.Path, worktree_dir: str) -> str:
    namespace_map = {}
    for p in out_dir.rglob("*.md"):
        if p.name == "INDEX.md":
            continue
        rel = p.relative_to(out_dir).as_posix()
        ns_key = rel.rsplit("/", 1)[0].replace("/", ".") if "/" in rel else "global"
        type_name = rel.rsplit("/", 1)[-1][:-3]
        stamp = last_commit_date_str(p, cwd=worktree_dir)
        namespace_map.setdefault(ns_key, []).append((type_name, rel, stamp, False))
    return build_index(namespace_map)

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

def main():
    print(f"[docgen] BASE_BRANCH={BASE_BRANCH}")
    print(f"[docgen] ALLOWED_NS={ALLOWED_NAMESPACE_PREFIXES}")
    print(f"[docgen] EXCLUDE_NS={EXCLUDE_NAMESPACE_PREFIXES}")
    print(f"[docgen] EXCLUDE_DIRS={EXCLUDE_DIRS}")
    # Commit-Message lesen und docgen-Steuerflags erkennen
    cm = _last_commit_message()
    flags = _parse_docgen_flags(cm)
    DOCGEN_FORCE_ALL = flags.get("force", False)
    print(f"[docgen] flags: force={DOCGEN_FORCE_ALL}")

    # 1) EIN Worktree & EIN OUT_DIR
    doc_branch = _docgen_branch_name()
    worktree_dir = create_worktree(doc_branch)

    global OUT_DIR, DOC_ROOT
    OUT_DIR = pathlib.Path(worktree_dir) / "docs"
    OUT_DIR.mkdir(parents=True, exist_ok=True)
    DOC_ROOT = OUT_DIR

    any_change = False
    try:
        # 2) Generierung – ALLES schreibt nur in OUT_DIR (Worktree)
        files = files_to_process()  # dein bestehender Sammler
        for f in files:
            # Pfadfilter & Namespace-Filter weiterhin hier:
            if is_excluded_path(pathlib.Path(f)):
                continue
            code = pathlib.Path(f).read_text(encoding="utf-8")
            ns, pub_types = extract_namespace_and_public_types(code)
            if not is_allowed_namespace(ns):
                continue

            out_rel = out_markdown_path_for(ns, pub_types, f) 
            p = out_markdown_path_for(ns, pub_types, f)
            out_path = p if p.is_absolute() else (OUT_DIR / p)

            must_generate = not out_path.exists()
            if not must_generate and not DOCGEN_FORCE_ALL:
                delta_code = code_change_rate(f, compare_ref=os.getenv("DOCGEN_COMPARE_REF", "HEAD~1"))
                if delta_code < CODE_CHANGE_THRESHOLD:
                    # zu wenig Code-Änderung: LLM SKIPPEN
                    # (Optional: Touch/Index unverändert lassen)
                    continue

            old_body = load_existing_doc_body(out_path)  
            md_body = llm_markdown_for(f, code, old_body)
            header = build_header_for(f)  # falls vorhanden; sonst leer
            changed = write_if_changed(out_path, header + md_body + "\n",force=DOCGEN_FORCE_ALL)

            any_change |= changed

        # 3) Index über DENSELBEN OUT_DIR
        index_text = build_index_for_outdir(OUT_DIR, worktree_dir)  # nutze last_commit_date_str(..., cwd=worktree_dir)
        write_if_changed(OUT_DIR / "INDEX.md", index_text)

        # 4) Dry-Run? -> NIX commit/push/PR
        if DOCGEN_DRY_RUN:
            print(f"[docgen] dry-run: docs generated only in worktree: {worktree_dir}")
            return

        # 5) Commit/Push/PR – EINMAL, nur wenn docs/ wirklich Änderungen hat
        if docs_changed(worktree_dir):
            if commit_docs(worktree_dir, "chore(docs): update generated documentation (auto)"):
                push_branch(doc_branch, worktree_dir)
                owner, repo = get_repo_from_env()
                session = get_github_session()
                pr_url = create_pull_request(session, owner, repo, doc_branch, BASE_BRANCH)
                print(f"[docgen] PR opened -> {pr_url}")
            else:
                print("[docgen] nothing to commit under docs (worktree)")
        else:
            print("[docgen] no changes under docs (worktree)")

    finally:
        # 6) Worktree optional aufräumen
        if os.getenv("DOCGEN_KEEP_WORKTREE", "false").lower() != "true":
            cleanup_worktree(worktree_dir)

    print("Complete. Changes:", any_change)


if __name__ == "__main__":
    main()
