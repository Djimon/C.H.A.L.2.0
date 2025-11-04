# global.review_agent

_Automatically generated/updated from `.github/scripts/review_agent.py`._

# Purpose
- Defines a review agent that checks for missing XML documentation summaries and non-English debug messages in C# files.
- Checks for incorrect usage of Unity's `Debug` logger.

# Public API
- Namespace/module: None
- Types
  - `Finding`
    - `kind`: Type of finding (e.g., SUMMARY_FINDING, DEBUG_FINDING, DEBUG_MANAGER_FINDING).
    - `file`: Relative path of the file.
    - `line`: 1-based line number of the finding.
    - `symbol`: Class/method name or "Debug call".
    - `message`: Brief description of the finding.

# Key Behavior & Side Effects
- `changed_files_since_last_commit()`: Returns modified C# files since the last commit or all tracked files if an error occurs.
- `all_repo_files()`: Returns all C# files in the repository excluding the `.git` directory.
- `files_to_process()`: Determines which files to process based on the `FULL_SCAN` environment variable.
- `check_missing_summary(path: str, text: str)`: Checks for public types, methods, and properties missing XML `<summary>` documentation.
- `check_debug_language(path: str, text: str)`: Checks for non-English debug messages in debug log calls.
- `check_wrong_debug_logger(path: str, text: str)`: Checks for incorrect usage of Unity's `Debug` logger.
- `run_review()`: Executes the review process and collects findings.
- `main()`: Entry point that runs the review and prints findings to the console.
- `create_issue_for_finding(session: requests.Session, owner: str, repo: str, finding: Finding, fingerprint: str)`: Creates a GitHub issue for a finding, including fingerprint and labels.
- `create_issue_for_group(session: requests.Session, owner: str, repo: str, kind: str, file: str, group: List[Finding], fingerprint: str)`: Creates a GitHub issue for a group of findings.

# Constraints & Failure Modes
- Handles missing files and read errors gracefully by printing error messages and continuing execution.
- Only processes files with the `.cs` extension.
- Assumes UTF-8 encoding for reading files.
- Requires `GITHUB_TOKEN` environment variable for GitHub API interactions.
- Limits issue creation to a batch size defined by the `ISSUE_BATCH_SIZE` environment variable.

# Example
```python
if __name__ == "__main__":
    main()
```

# Unknowns
- None.
