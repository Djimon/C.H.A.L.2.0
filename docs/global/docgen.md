# global.docgen

_Automatically generated/updated from `.github/scripts/docgen.py`._

# Purpose
- Defines a script for generating documentation from source code files.
- Processes changes in tracked files and creates markdown documentation based on public types.

# Public API
- Namespace/module: None
- Types: None

# Key Behavior & Side Effects
- Scans for changed files since the last commit or all files if `FULL_SCAN` is set.
- Extracts namespaces and public types from the source code.
- Generates markdown documentation for each public type or falls back to file-based documentation.
- Writes documentation to the `docs` directory, creating necessary directories.

# Constraints & Failure Modes
- Only processes files with specific extensions defined in `DOC_EXTS`.
- Handles errors in reading files and writing documentation gracefully.
- Uses OpenAI API for generating documentation; requires a valid API key.

# Example
```python
# Example usage of the script is not explicitly provided in the code.
```

# Unknowns
- Specific details about the structure of the source code files being processed.
- Behavior of the OpenAI API response beyond the basic usage shown.

