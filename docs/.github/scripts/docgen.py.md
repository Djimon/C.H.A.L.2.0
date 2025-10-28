# .github/scripts/docgen.py

_Automatic generated/updated._

```markdown
# Purpose
- Defines a script for generating documentation from source files in a Git repository.
- Processes changed files or all files based on an environment variable.

# Public API
- Namespace/module: None
- Types
  - None

# Key Behavior & Side Effects
- Detects changed files since the last commit or all files if the environment variable `FULL_SCAN` is set to true.
- Reads the content of each relevant file and generates documentation using an OpenAI model.
- Writes the generated documentation to markdown files in the `docs` directory.
- Updates an index file listing all processed files.

# Constraints & Failure Modes
- Only processes files with specific extensions: `.cs`, `.py`, `.ts`, `.tsx`, `.js`, `.java`, `.go`.
- Handles exceptions when reading the Git diff, falling back to listing all tracked files.
- Ensures the output directory exists before writing files.

# Example
- No clear example derivable from the code.

# Unknowns
- Specific behavior of the OpenAI API call and its response structure.
- The exact format of the generated documentation.
```
