# .github/scripts/fix_debug_manager_agent.py

_Automatically generated/updated from `.github/scripts/fix_debug_manager_agent.py`._

# Purpose
- Automates the process of replacing UnityEngine.Debug calls with DebugManager equivalents in code files.
- Interacts with GitHub to manage issues and pull requests related to the changes made.

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

- Public methods:
  - main() -> None
    - Executes the main logic of the script.

# Key Behavior & Side Effects
- Retrieves open issues from GitHub with specific labels.
- Parses issue bodies to identify incorrect Debug calls.
- Modifies files to replace Debug calls with DebugManager calls.
- Creates a new branch, commits changes, and opens a pull request on GitHub.
- Closes related issues after a successful pull request creation.

# Constraints & Failure Modes
- Requires GITHUB_REPOSITORY and GITHUB_TOKEN environment variables to be set.
- Skips files located in "ThirdParty" or containing "xTernal" in their path.
- Handles Git operations and may fail if the repository state is not clean or if network issues occur.

# Example
```python
# Example usage of the script would be to run it in an environment where the necessary
# GitHub tokens and repository information are set in the environment variables.
# The script will automatically handle the rest.
```

# Unknowns
- The specific format of the issue body expected by the `parse_findings_from_issue` function is not documented outside the code.

