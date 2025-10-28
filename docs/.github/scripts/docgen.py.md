# .github/scripts/docgen.py

_Automatic generated/updated._

```md
# Purpose
- Defines a script for generating documentation from source files based on changes in a Git repository.

# Public API
- Namespace/module: None
- Types
  - None

# Key Behavior & Side Effects
- Scans for changed files since the last commit or all files if `FULL_SCAN` is set to true.
- Generates markdown documentation for each relevant file using OpenAI's API.
- Writes the generated documentation to the `docs` directory.
- Updates an index file listing all processed files.

# Constraints & Failure Modes
- Only processes files with specific extensions: `.cs`, `.py`, `.ts`, `.tsx`, `.js`, `.java`, `.go`.
- Handles errors in reading files gracefully by ignoring encoding issues.
- Ensures the output directory exists before writing files.

# Example
```python
# Example usage of the script is not explicitly provided in the code.
```

# Unknowns
- Specific behavior of the OpenAI API call and its response structure cannot be determined from this file.
```
