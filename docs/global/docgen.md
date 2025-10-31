# global.docgen

_Automatically generated/updated from `.github/scripts/docgen.py`._

1) Purpose
- Script to generate diff-friendly, Markdown documentation from a single source file, using an LLM.
- Parses code to extract a namespace and public types, then creates per-type docs or a file based on namespace and fallback name.
- Writes an INDEX.md listing changed docs and updates docs/ directory structure.

```

```python
2) Public API
- Module: docgen

- Public constants/regexes
  - DOC_EXTS = {".cs", ".py", ".ts", ".tsx", ".js", ".java", ".go"}
    - Allowed source file extensions to consider for doc generation.
  - OUT_DIR = pathlib.Path("docs")
    - Output root directory for generated docs; created if missing.
  - DOC_ROOT = OUT_DIR
    - Root path used for computing per-namespace/type output paths.
  - NS_RX = re.compile(r'^\s*namespace\s+([A-Za-z0-9_.]+)', re.MULTILINE)
    - Regex to capture a C#/namespaced-like namespace from code.
  - TYPE_RX = re.compile(r'^\s*public\s+(class|struct|interface|enum)\s+([A-Za-z0-9_]+)', re.MULTILINE)
    - Regex to capture public types (kind and name) from code.
  - SYSTEM_PROMPT = """You are a technical writer. Generate concise, factual, diff-friendly documentation from the provided **single source file only** (no external assumptions). Write in clear English, bullet-first
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
...
"""
    - System prompt used by the LLM to generate documentation.

- Public function changed_files_since_last_commit() -> list[str]
  - No parameters.
  - Determines files changed since last commit; uses git diff, falls back to listing files on error.

- Public function extract_namespace_and_public_types(code: str) -> tuple[str, list[tuple[str, str]]]
  - Parameters:
    - code: str
  - Returns: (namespace: str, types: list of (kind, name))
    - namespace from NS_RX or "global" if not found
    - types derived from TYPE_RX as (kind, name)

- Public function strip_outer_markdown_fence(text: str) -> str
  - Parameters:
    - text: str
  - Returns text with exactly one outer ```markdown or ```md fence removed, if present; inner fences preserved.

- Public function read_text(path) -> str
  - Parameters:
    - path
  - Reads file as UTF-8, ignores decoding errors.

- Public function write_if_changed(path: pathlib.Path, content: str) -> bool
  - Parameters:
    - path
    - content
  - Writes content only if different (ignores leading/trailing whitespace differences); returns True if written.

- Public function doc_path_for(src_path: str) -> pathlib.Path
  - Parameters:
    - src_path
  - Returns OUT_DIR / "<src_path>.md"

- Public function paths_for(namespace: str, type_names: list[str], fallback_basename: str) -> list[pathlib.Path]
  - Parameters:
    - namespace
    - type_names
    - fallback_basename
  - Returns list of doc paths:
    - If type_names non-empty: docs/<namespace with '.' -> '/'>/<Type>.md for each Type
    - If type_names empty: docs/<namespace with '.' -> '/'>/<fallback_basename>.md

- Public function all_repo_files() -> list[str]
  - No parameters.
  - Returns all tracked files with extensions in DOC_EXTS, excluding any path containing ".git".

- Public function files_to_process() -> list[str]
  - No parameters.
  - If FULL_SCAN env var equals "true" (case-insensitive), returns all_repo_files(); otherwise returns changed_files_since_last_commit().

- Public function llm_markdown_for(path: str, code: str) -> str
  - Parameters:
    - path
    - code
  - Creates OpenAI client using OPENAI_API_KEY; sends the code chunk to a system prompt; uses model "gpt-5-nano"; returns the generated Markdown after stripping an outer fenced block if present.

- Public function main() -> None
  - Orchestrates:
    - Determines files to process
    - For each file, extracts namespace and public types
    - Builds target docs per type or per file fallback
    - Calls llm_markdown_for for content
    - Writes/updates docs and an INDEX.md with change markers
    - Prints completion status

```

```markdown
3) Key Behavior & Side Effects
- Reads repository state to identify files to document (full scan or changes since HEAD~1).
- For each relevant source file:
  - Extracts namespace (or "global") and public types (kind, name) via regexes.
  - Builds destination paths under docs/ based on namespace and type names; uses a fallback for files without public types.
  - Invokes an OpenAI call per type (or per file) to generate Markdown documentation from the source.
  - Prepends a header with the fully-qualified name and source reference, then writes/overwrites the docs if content changed.
  - Appends an entry to the INDEX.md with a link to the output; marks "(neu)" for new or "(neu)" or "(neu)" if changed; otherwise leaves as existing.
- Writes or updates docs/INDEX.md and prints a final status: "Complete. Changes: {True|False}".
- Uses a fixed system prompt to guide the LLM to produce concise, structured documentation.
- A single source file is used as input to the documentation generator (no cross-file aggregation beyond code extraction).

```

```markdown
4) Constraints & Failure Modes
- FULL_SCAN env var controls whether to document all repo files or only changed files.
- Requires git context; if git diff fails, falls back to git ls-files; may error outside a git repo.
- OpenAI interaction depends on OPENAI_API_KEY; failure or absence of key may raise runtime errors.
- Writes to docs/; creates directories as needed; only writes when content actually changes.
- Assumes code contains recognizable namespace and public type patterns; if not, defaults to "global" and may generate generic docs.
- Uses model "gpt-5-nano" (non-standard) as defined in code; actual availability depends on environment.
- read_text ignores encoding errors; may drop some characters if files aren’t UTF-8-compatible.
- strip_outer_markdown_fence only removes a single outer fenced block; nested or multi-block fences remain intact.
- INDEX.md content reflects changes; if none, the script reports no changes.
- Dependency on external libraries: git (GitPython) and openai; not handled with robust fallback here.

```

```markdown
5) Example
- Minimal run (assuming Python is available and repository is a git repo with OpenAI access):
```bash
python3 .github/scripts/docgen.py
```

- Example with full scan:
```bash
FULL_SCAN=true python3 .github/scripts/docgen.py
```

```

```markdown
6) Unknowns
- Exact structure/format of generated Markdown depends on the LLM output and may vary per invocation.
- Behavior if OpenAI API is unreachable or rate-limited is not defined beyond runtime errors.
- The script’s handling on non-C#/non-language-specific repos (where NS_RX/TY_RX patterns don’t match) relies on defaults and may produce documents with generic namespaces.
- The actual model availability, response length limits, and token usage behavior are not specified beyond the code.
- Cross-version compatibility: the code uses modern typing (list[str]) and assumes Python+dependencies availability; environments may differ.

