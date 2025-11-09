# .github/scripts/fix_debug_language_agent.py

_Automatically generated/updated from `.github/scripts/fix_debug_language_agent.py`._

# Purpose
- Automates the translation of non-English C# debug log messages to English in GitHub issues.

# Public API
- Namespace/module: None
- Types
  - FindingItem
    - Public fields/properties:
      - issue_number: int
      - file: str
      - line: int
      - symbol: str
      - message: str
    - Public methods: None

# Key Behavior & Side Effects
- Retrieves open GitHub issues labeled with "Agent" and "Agent/DebugLanguage".
- Parses issue messages to extract file paths, line numbers, symbols, and messages.
- Translates debug messages using OpenAI's API and updates the corresponding files.
- Creates a new branch, commits changes, and opens a pull request if changes are made.
- Closes related issues after successful pull request creation.

# Constraints & Failure Modes
- Requires environment variables: GITHUB_REPOSITORY, GITHUB_TOKEN, OPENAI_API_KEY.
- Handles only specific formats of issue messages for parsing.
- May skip issues or files if expected formats are not found or if no changes are made.

# Example
```python
# Example usage of the main function
if __name__ == "__main__":
    main()
```

# Unknowns
- None

