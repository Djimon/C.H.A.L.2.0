# Assets/src/patchCSfiles.py

_Automatically generated/updated from `Assets/src/patchCSfiles.py`._

# Purpose
- Converts all `.cs` files in the project directory from CP1252 encoding to UTF-8 encoding.
- Skips files located in "xTernal" or "ThirdParty" directories.

# Public API
- No explicit namespace/module defined.

- Types
  - None

# Key Behavior & Side Effects
- Reads each `.cs` file as bytes and attempts to decode using CP1252.
- If decoding fails, it prints a message and skips the file.
- Successfully decoded files are re-saved in UTF-8 format with newline normalization.
- Prints a confirmation message for each successfully converted file.

# Constraints & Failure Modes
- Files in "xTernal" or "ThirdParty" directories are excluded from processing.
- Handles `UnicodeDecodeError` by skipping files that cannot be decoded as CP1252.

# Example
```python
# Example usage is implicit; the script processes all .cs files in the current directory.
```

# Unknowns
- None.
