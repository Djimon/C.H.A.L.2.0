# .github/scripts/docgen.py

_Automatisch generiert/aktualisiert._

```markdown
# Purpose
- Defines a script for generating documentation from source files in a Git repository.
- Processes changed files or all files based on environment variable settings.

# Public API
- Namespace/module: None
- Types
  - None

# Key Behavior & Side Effects
- Detects changed files since the last commit or all files if FULL_SCAN is set.
- Reads source files and generates Markdown documentation using OpenAI's API.
- Writes generated documentation to the `docs` directory, creating files if they do not exist.

# Constraints & Failure Modes
- Handles file reading errors by ignoring them.
- Only processes files with specific extensions defined in `DOC_EXTS`.
- Uses environment variable `OPENAI_API_KEY` for API access; failure to provide this will result in an error.

# Example
- No clear example derivable from the code.

# Unknowns
- Specific behavior of the OpenAI API response handling.
- The exact structure of the generated documentation is not defined in the code.
```
