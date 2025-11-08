# global.docgen

_Automatically generated/updated from `.github/scripts/docgen.py`._

1) Purpose
- Defines a script for generating documentation from source code files in a Git repository.
- Supports automatic updates of documentation based on changes in the codebase.

2) Public API
- Namespace/module: None
- Types
  - None

3) Key Behavior & Side Effects
- Scans for changed files since the last commit or all files if FULL_SCAN is true.
- Generates documentation for public types found in the source code and writes it to markdown files.
- Updates existing documentation only if the change rate exceeds a defined threshold.

4) Constraints & Failure Modes
- Excludes specified directories and namespaces from documentation generation.
- Handles missing files gracefully by skipping them.
- Uses Git to determine the last commit date for documentation timestamps.

5) Example
- No clear example derivable from the code.

6) Unknowns
- None.

