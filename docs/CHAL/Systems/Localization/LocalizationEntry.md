# CHAL.Systems.Localization.LocalizationEntry

_Automatically generated/updated from `Assets/src/Systems/Localization/LocalizationDict.cs`._

1) Purpose
- Defines a serializable LocalizationEntry with key/value fields.
- Defines a serializable LocalizationDict containing a list of LocalizationEntry.
- Provides ToDictionary() to convert entries to a Dictionary<string, string>, skipping entries with empty keys.

2) Public API
- Namespace: CHAL.Systems.Localization

- Types
  - public class LocalizationEntry
    - public string key
    - public string value

  - public class LocalizationDict
    - public List<LocalizationEntry> entries = new();
    - public Dictionary<string, string> ToDictionary()

3) Key Behavior & Side Effects
- LocalizationDict.entries is a publicly accessible list, initialized to an empty list.
- LocalizationDict.ToDictionary():
  - Creates and returns a new Dictionary<string, string>.
  - Iterates over all entries; for each entry with a non-empty key, adds dict[e.key] = e.value.
  - If multiple entries share the same non-empty key, later entries overwrite earlier ones.
  - Entries with null or empty keys are ignored.
  - Values can be null; the dictionary will store the null value for that key.

4) Constraints & Failure Modes
- Null or empty keys are skipped; no exception is thrown for them.
- Duplicate keys in entries overwrite earlier values in the resulting dictionary.
- Not thread-safe by virtue of using a new Dictionary per call; no explicit synchronization.
- No explicit error handling; ToDictionary simply builds and returns the dictionary.

5) Example
```csharp
using CHAL.Systems.Localization;
using System.Collections.Generic;

var loc = new LocalizationDict();
loc.entries = new List<LocalizationEntry>
{
    new LocalizationEntry { key = "greeting", value = "Hello" },
    new LocalizationEntry { key = "farewell", value = "Goodbye" },
    // This entry will be ignored if key is empty
    new LocalizationEntry { key = "", value = "Ignored" }
};

Dictionary<string, string> map = loc.ToDictionary();
// map["greeting"] -> "Hello"
// map["farewell"] -> "Goodbye"
```

6) Unknowns
- How this integrates with other localization systems or UI components is not shown.
- Whether these classes are intended to be ScriptableObjects or editor assets is not specified.
- Specific usage contexts (e.g., scene data, asset pipelines) are not defined beyond the file.

