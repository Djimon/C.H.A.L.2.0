# global.docgen

_Automatically generated/updated from `.github/scripts/docgen.py`._

1) Purpose
- A Python script that generates Markdown documentation from source files by extracting namespaces and public types, then producing per-type docs under docs/ using an OpenAI-based generator.
- Supports incremental updates (Changed files since last commit) or a full scan via FULL_SCAN; builds an INDEX.md listing generated/updated files.
- Diff-friendly output: content is updated only when changes occur, with per-file headers indicating provenance and a timestamp in the index.

2) Public API
- Module: docgen

Constants
- public constant set[str] DOC_EXTS = {".cs", ".py", ".ts", ".tsx", ".js", ".java", ".go"}
- public constant pathlib.Path OUT_DIR = pathlib.Path("docs")
- public constant pathlib.Path DOC_ROOT = OUT_DIR
- public constant re.Pattern NS_RX
  - Regex to capture namespace from code: r'^\s*namespace\s+([A-Za-z0-9_.]+)', MULTILINE
- public constant re.Pattern TYPE_RX
  - Regex to capture public types: r'^\s*public\s+(class|struct|interface|enum)\s+([A-Za-z0-9_]+)', MULTILINE
- public constant str SYSTEM_PROMPT
  - System prompt text used to guide the OpenAI-based documentation generation
  - (large multi-line string defined in code)

Public functions
- public function changed_files_since_last_commit() -> list[str]
  - Returns list of changed source files since HEAD (or all tracked files on error), filtered to DOC_EXTS and existing files.
- public function extract_namespace_and_public_types(code: str) -> tuple[str, list[tuple[str, str]]]
  - Returns (namespace, list of (kind, name)) for public types found in code; namespace defaults to "global" if not found.
- public function strip_outer_markdown_fence(text: str) -> str
  - If text is a single outer ```markdown or ```md block, returns the inner content; otherwise returns text unchanged.
- public function read_text(path) -> str
  - Reads a file with utf-8 encoding, errors ignored.
- public function write_if_changed(path: pathlib.Path, content: str) -> bool
  - Writes content to path only if content differs (ignoring surrounding whitespace); ensures parent dirs exist; returns True if written.
- public function doc_path_for(src_path: str) -> pathlib.Path
  - Returns documentation path as docs/<src_path>.md
- public function paths_for(namespace: str, type_names: list[str], fallback_basename: str) -> list[pathlib.Path]
  - Builds target paths for docs:
    - If type_names non-empty: docs/<namespace as path>/<Type>.md for each type
    - If no types: docs/<namespace as path>/<fallback_basename>.md
- public function all_repo_files() -> list[str]
  - Recursively lists all tracked files in the repo that match DOC_EXTS (excluding .git); returns POSIX-style paths.
- public function files_to_process() -> list[str]
  - Returns all_repo_files() if FULL_SCAN is set to "true" (case-insensitive), otherwise returns changed_files_since_last_commit().
- public function llm_markdown_for(path: str, code: str) -> str
  - Calls OpenAI API (OPENAI_API_KEY) with the file path and code to generate Markdown content; uses model "gpt-5-nano" and returns the content with outer fences stripped.
- public function main() -> None
  - Orchestrates the scan, per-file and per-type doc generation, file writes, and INDEX.md creation; prints status messages.

3) Key Behavior & Side Effects
- File discovery and filtering
  - Determines candidate files via FULL_SCAN or changes since last commit; only processes files with extensions in DOC_EXTS and that exist.
- Namespace and type extraction
  - Uses NS_RX to determine namespace (or "global" if absent); uses TYPE_RX to identify public types (class/struct/interface/enum) and their names.
- Documentation generation
  - For each public type, computes a target path under docs/ based on namespace and type name.
  - Calls llm_markdown_for for a Markdown description of the code, using a system prompt to guide output.
  - Prepends a header with the fully qualified name (namespace.type or just type) and a note that it’s auto-generated from the source file.
  - Writes content to the target path only if content changes; tracks whether any changes occurred.
- Indexing and output layout
  - Builds an INDEX.md in docs/ including a status timestamp and a list of Changed files with links to their generated docs; marks new or updated entries with “(neu)” or “(neu)”.
  - If a file contains no public types, a single fallback doc is produced using the file’s base name.
- External dependencies and side effects
  - Requires Git (via gitpython) for diffs; requires OpenAI API access (OPENAI_API_KEY) for content generation.
  - Reads source files with utf-8 encoding, ignoring invalid bytes; writes docs to a docs/ directory created if needed.
- Execution flow
  - Entry point: main() guarded by if __name__ == "__main__":; prints "Complete. Changes: ..." with a boolean indicating any changes.

4) Constraints & Failure Modes
- Environment and API constraints
  - Requires OPENAI_API_KEY for llm_markdown_for(); OpenAI model "gpt-5-nano" is hard-coded and may be unavailable.
- File system and path guarantees
  - Docs are written under docs/; paths for per-type docs are nested by namespace (dots replaced by folders).
  - write_if_changed uses strip() to compare contents; whitespace differences cause no change.
- Git interaction and fallbacks
  - changed_files_since_last_commit uses HEAD~1..HEAD; on error falls back to ls-files; requires a Git repository context.
- Error handling
  - llm_markdown_for and OpenAI calls have no explicit error handling in this file; exceptions propagate and can terminate main().
  - read_text uses errors="ignore" for robustness against encoding issues.
- Performance and scalability
  - Scans and processes per-file per-type; number of docs grows with number of public types found.
  - Incremental updates rely on git diffs; full scans can be expensive for large repos.

5) Example
- Not provided: no explicit usage example derivable beyond the described behavior.

6) Unknowns
- Exact content of generated Markdown for each public type; driven by the OpenAI response to SYSTEM_PROMPT + file content.
- Behavior on non-Python/C-like languages beyond the supported regexes; namespaces not matching the regex default to "global".
- Any runtime errors not explicitly handled (e.g., API failures, network errors, invalid OPENAI_API_KEY formats) and their recoveries.
- If external tooling (Git, OpenAI) changes their APIs or behavior, this script’s surface may need adjustment.
