# .github/scripts/docgen.py

_Automatic generated/updated._

1) Purpose
- Defines a script for generating documentation from source files in a repository.
- Supports various programming languages based on file extensions.

2) Public API
- Namespace/module: None
- Types
  - None defined.

3) Key Behavior & Side Effects
- Scans for changed files since the last commit or all files if FULL_SCAN is true.
- Generates documentation using OpenAI's API based on the content of the source files.
- Writes generated documentation to a specified output directory.

4) Constraints & Failure Modes
- Only processes files with specific extensions: .cs, .py, .ts, .tsx, .js, .java, .go.
- Handles errors when reading files or accessing the git repository.
- Uses environment variable `OPENAI_API_KEY` for API access.

5) Example
- No clear example derivable from the file.

6) Unknowns
- Specific behavior of the OpenAI API response and its impact on documentation quality.

