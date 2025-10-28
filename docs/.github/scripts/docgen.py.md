# .github/scripts/docgen.py

_Automatic generated/updated._

```markdown
# Purpose
- Defines a script for generating documentation from source files in a Git repository.
- Supports specific file extensions for documentation generation.

# Public API
- No explicit namespace/module defined.
- Types
  - `def changed_files_since_last_commit()`
    - Returns a list of changed files since the last commit with supported extensions.
  - `def read_text(path)`
    - Reads text from a specified file path.
  - `def write_if_changed(path: pathlib.Path, content: str) -> bool`
    - Writes content to a file if it has changed; returns a boolean indicating if a change occurred.
  - `def doc_path_for(src_path: str) -> pathlib.Path`
    - Generates the documentation path for a given source file path.
  - `def all_repo_files()`
    - Returns a list of all tracked files in the repository with supported extensions.
  - `def files_to_process()`
    - Determines which files to process based on environment variable settings.
  - `def llm_markdown_for(path: str, code: str) -> str`
    - Calls an LLM to generate markdown documentation for the provided code.

# Key Behavior & Side Effects
- Generates documentation for changed files or all files based on the `FULL_SCAN` environment variable.
- Writes generated documentation to the `docs` directory.
- Updates an index file listing all processed files.

# Constraints & Failure Modes
- Only processes files with specific extensions defined in `DOC_EXTS`.
- Handles exceptions when reading files or accessing the Git repository.
- Uses environment variable `OPENAI_API_KEY` for LLM access; failure to set this will cause errors.

# Example
```python
# Example usage of the main function to generate documentation
if __name__ == "__main__":
    main()
```

# Unknowns
- Specific behavior of the OpenAI API and its response structure cannot be determined from this file.
```
