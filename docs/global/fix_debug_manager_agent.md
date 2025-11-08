# global.fix_debug_manager_agent

_Automatically generated/updated from `.github/scripts/fix_debug_manager_agent.py`._

# Purpose
- Automates the process of replacing UnityEngine.Debug calls with DebugManager equivalents in code files.
- Interacts with GitHub issues to identify and close issues related to incorrect Debug usage.

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

# Key Behavior & Side Effects
- Retrieves open GitHub issues labeled with "Agent" and "Agent/DebugManager".
- Parses issue bodies to identify incorrect Debug calls and their locations.
- Creates a new branch, modifies files to replace Debug calls, commits changes, and pushes to the repository.
- Creates a pull request to merge changes and closes related issues.

# Constraints & Failure Modes
- Requires GITHUB_REPOSITORY and GITHUB_TOKEN environment variables to be set.
- Skips files located in "ThirdParty" or containing "xTernal" in their path.
- Handles Git operations and may raise errors if Git commands fail.

# Example
```python
if __name__ == "__main__":
    main()
```

# Unknowns
- None

