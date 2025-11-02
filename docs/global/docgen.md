# global.docgen

_Automatically generated/updated from `.github/scripts/docgen.py`._

1) Purpose
- Generates documentation for source files based on public types and namespaces.
- Outputs documentation in Markdown format to a specified directory.

2) Public API
- Namespace/module: None
- Types: 
  - None

3) Key Behavior & Side Effects
- Scans for changed files or all files based on the `FULL_SCAN` environment variable.
- Extracts namespaces and public types from source code.
- Generates documentation for each public type or falls back to file-based documentation if no public types are found.
- Writes documentation to the `docs` directory, creating necessary directories if they do not exist.
- Updates an index file with links to generated documentation.

4) Constraints & Failure Modes
- Only processes files with specific extensions: `.cs`, `.py`, `.ts`, `.tsx`, `.js`, `.java`, `.go`.
- Handles missing files gracefully by returning `None` when attempting to load existing documentation.
- Uses the OpenAI API for generating documentation; requires a valid API key in the environment.

5) Example
- No clear example derivable from the code.

6) Unknowns
- The specific behavior of the OpenAI API response and its impact on documentation quality cannot be determined from this file.

