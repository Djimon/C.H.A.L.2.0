# .github/scripts/review_agent.py

_Automatically generated/updated from `.github/scripts/review_agent.py`._

# Purpose
- Defines a script for reviewing code in a GitHub repository, checking for missing XML documentation and non-English debug messages, and creating issues for findings.

# Public API
- Namespace/module: None
- Types
  - `Finding`
    - `kind`: Type of finding (e.g., summary or debug finding).
    - `file`: Relative path of the file where the finding was detected.
    - `line`: 1-based line number of the finding.
    - `symbol`: Name of the class/method or "Debug call".
    - `message`: Description of the finding.

# Key Behavior & Side Effects
- Scans files for specific patterns related to missing summaries and debug messages.
- Creates GitHub issues for findings if a valid GitHub token is provided.
- Groups findings by file and type before creating issues.
- Skips files in "xTernal" or "ThirdParty" directories.

# Constraints & Failure Modes
- Requires a valid `GITHUB_TOKEN` to create issues on GitHub.
- Limits issue creation to a defined `BATCH_SIZE` to avoid rate limiting.
- Handles missing or unreadable files gracefully, logging errors without crashing.

# Example
```python
# Example usage of the script would be running it directly:
if __name__ == "__main__":
    main()
```

# Unknowns
- The behavior of the script when encountering files with unsupported extensions or formats is not explicitly defined.

