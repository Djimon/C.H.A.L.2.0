# global.review_agent

_Automatically generated/updated from `.github/scripts/review_agent.py`._

# Purpose
- Defines a script for reviewing code in a GitHub repository, checking for missing XML documentation and non-English debug messages, and creating issues for findings.

# Public API
- Namespace/module: None

- Types
  - Finding
    - `kind`: Type of finding (e.g., SUMMARY_FINDING, DEBUG_FINDING).
    - `file`: Relative path of the file where the finding occurred.
    - `line`: 1-based line number of the finding.
    - `symbol`: Name of the class/method or "Debug call".
    - `message`: Description of the finding.

- Public methods
  - `main() -> None`
    - Entry point for running the review process and creating issues.
  - `run_review() -> List[Finding]`
    - Executes the review process and returns a list of findings.
  - `create_issue_for_group(session: requests.Session, owner: str, repo: str, kind: str, file: str, group: List[Finding], fingerprint: str) -> None`
    - Creates a GitHub issue for a group of findings.
  - `create_issue_for_finding(session: requests.Session, owner: str, repo: str, finding: Finding, fingerprint: str) -> None`
    - Creates a GitHub issue for a single finding.
  - `load_existing_fingerprints(session: requests.Session, owner: str, repo: str) -> set[str]`
    - Loads existing issue fingerprints from the GitHub repository.
  - `files_to_process() -> List[str]`
    - Determines which files to process based on changes since the last commit or a full scan.

# Key Behavior & Side Effects
- Scans for public types, methods, and properties without XML documentation.
- Checks for TODO comments and non-English debug messages.
- Creates GitHub issues for findings, grouping them by file and type.
- Skips files in "xTernal" or "ThirdParty" directories.

# Constraints & Failure Modes
- Requires a valid GITHUB_TOKEN environment variable for GitHub API access.
- Limits issue creation to a specified batch size to avoid rate limiting.
- Handles file reading errors gracefully, skipping unreadable files.

# Example
```python
if __name__ == "__main__":
    main()
```

# Unknowns
- None.

