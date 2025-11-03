# global.fix_debug_language_agent

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
- Parses issue bodies to extract file paths, line numbers, symbols, and messages.
- Translates debug messages using OpenAI's API and updates the corresponding files.
- Creates a new branch, commits changes, and opens a pull request if modifications are made.
- Closes related issues after a successful pull request creation.

# Constraints & Failure Modes
- Requires environment variables: GITHUB_REPOSITORY, GITHUB_TOKEN, OPENAI_API_KEY.
- Handles issues with missing or invalid data gracefully by logging warnings.
- Assumes the presence of a valid Git repository and appropriate permissions for API access.

# Example
```python
# Example usage of the main function
if __name__ == "__main__":
    main()
```

# Unknowns
- The behavior of the OpenAI API and its response format cannot be determined from this file.

