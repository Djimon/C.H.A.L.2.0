# global.fix_debug_manager_agent

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
  - get_repo_from_env() -> Tuple[str, str]
  - get_github_session() -> requests.Session
  - list_debug_manager_issues(session: requests.Session, owner: str, repo: str) -> List[dict]
  - close_issue(session: requests.Session, owner: str, repo: str, issue_number: int, pr_url: str) -> None
  - parse_findings_from_issue(issue: dict) -> List[FindingItem]
  - replace_wrong_debug_in_line(line: str) -> str
  - fix_debug_calls_in_file(path: pathlib.Path) -> bool
  - git_run(args: List[str]) -> None
  - ensure_branch(branch: str) -> None
  - any_changes() -> bool
  - commit_all(message: str) -> None
  - push_branch(branch: str) -> None
  - create_pull_request(session: requests.Session, owner: str, repo: str, branch: str, base: str) -> Optional[str]
  - main() -> None

# Key Behavior & Side Effects
- Retrieves open issues labeled with "Agent" and "Agent/DebugManager" from a GitHub repository.
- Parses issue bodies to identify incorrect Debug calls and generates a list of findings.
- Replaces incorrect Debug calls in specified files and commits the changes to a new branch.
- Creates a pull request to merge changes and closes the related issues.

# Constraints & Failure Modes
- Requires GITHUB_REPOSITORY and GITHUB_TOKEN environment variables to be set.
- Skips files located in "ThirdParty" or containing "xTernal" in their path.
- Handles Git operations and may fail if the repository state is not clean or if network issues occur.

# Example
```python
# Example usage of the main function
if __name__ == "__main__":
    main()
```

# Unknowns
- The specific format of the issue body expected by `parse_findings_from_issue` is not documented outside the code.

