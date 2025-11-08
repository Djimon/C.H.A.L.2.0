# global.fix_summary_agent

_Automatically generated/updated from `.github/scripts/fix_summary_agent.py`._

# Purpose
- Automates the addition of XML documentation comments to C# code based on GitHub issues.

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
  - get_repo_from_env() -> Tuple[str, str]
  - get_github_session() -> requests.Session
  - list_summary_issues(session: requests.Session, owner: str, repo: str) -> List[dict]
  - close_issue(session: requests.Session, owner: str, repo: str, issue_number: int, pr_url: str) -> None
  - parse_findings_from_issue(issue: dict) -> List[FindingItem]
  - get_openai_client() -> OpenAI
  - generate_summary_for_declaration(client: OpenAI, declaration: str, context: str, is_method: bool) -> str
  - insert_summaries_in_file(path: pathlib.Path, items: List[FindingItem], client: OpenAI) -> bool
  - git_run(args: List[str]) -> None
  - ensure_branch(branch: str) -> None
  - any_changes() -> bool
  - commit_all(message: str) -> None
  - push_branch(branch: str) -> None
  - create_pull_request(session: requests.Session, owner: str, repo: str, branch: str, base: str) -> str

# Key Behavior & Side Effects
- Retrieves open GitHub issues labeled with "Agent/Summary".
- Parses issues to extract findings related to missing XML documentation.
- Generates XML documentation comments using OpenAI's API.
- Inserts generated comments into the corresponding C# files.
- Creates a new Git branch, commits changes, and opens a pull request.
- Closes the original issues after the pull request is created.

# Constraints & Failure Modes
- Requires environment variables: GITHUB_REPOSITORY, GITHUB_TOKEN, OPENAI_API_KEY.
- Handles only issues with specific formatting for parsing.
- Assumes the presence of public/protected/internal declarations for accurate line indexing.
- May fail to close issues or create pull requests if GitHub API responses are not successful.

# Example
```python
# Example usage of the main function
if __name__ == "__main__":
    main()
```

# Unknowns
- The specific format of the C# declarations that will be processed.
- The behavior of the OpenAI API in terms of response consistency and quality.

