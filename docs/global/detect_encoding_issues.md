# Assets/src/detect_encoding_issues.py

_Automatically generated/updated from `Assets/src/detect_encoding_issues.py`._

# Purpose
- Detects non-UTF-8 encoded `.cs` files in the current directory and its subdirectories.

# Public API
- No namespace/module defined.

- Types
  - None

# Key Behavior & Side Effects
- Recursively searches for `.cs` files starting from the current directory.
- Skips files located in `.git`, `xTernal`, or `ThirdParty` directories.
- Collects paths of files that raise a `UnicodeDecodeError` when attempting to read as UTF-8.
- Prints a list of non-UTF-8 files to the console.

# Constraints & Failure Modes
- Assumes the presence of a valid file system and permissions to read files.
- Only checks files with a `.cs` extension.
- Does not handle other encoding errors beyond `UnicodeDecodeError`.

# Example
```python
# Run the script to check for non-UTF-8 encoded .cs files
python detect_encoding_issues.py
```

# Unknowns
- None
