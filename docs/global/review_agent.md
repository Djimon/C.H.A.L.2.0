# global.review_agent

_Automatically generated/updated from `.github/scripts/review_agent.py`._

# Purpose
- Defines a review agent that checks for missing XML documentation summaries and non-English debug messages in C# files.

# Public API
- Namespace/module: None
- Types
  - `Finding`
    - `kind`: Type of finding (e.g., SUMMARY_FINDING or DEBUG_FINDING).
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
- `run_review()`: Executes the review process and collects findings.
- `main()`: Entry point that runs the review and prints findings to the console.
- `create_issue_for_finding(session: requests.Session, owner: str, repo: str, finding: Finding, fingerprint: str)`: Creates a GitHub issue for a finding, including fingerprint and labels.

# Constraints & Failure Modes
- Handles missing files and read errors gracefully by printing error messages and continuing execution.
- Only processes files with the `.cs` extension.
- Assumes UTF-8 encoding for reading files.
- Requires `GITHUB_TOKEN` environment variable for GitHub API interactions.

# Example
```python
if __name__ == "__main__":
    main()
```

# Unknowns
- None.
