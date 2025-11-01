# CHAL.Systems.Localization.LocalizationDict

_Automatically generated/updated from `Assets/src/Systems/Localization/LocalizationDict.cs`._

1) Purpose
- Defines a simple serializable localization model and a helper to convert it to a dictionary.
- Contains two public classes in namespace CHAL.Systems.Localization: LocalizationEntry and LocalizationDict.
- LocalizationDict provides a ToDictionary() method to produce a Dictionary<string, string> from its entries.

2) Public API
- Namespace/module: CHAL.Systems.Localization
- Types
  - public class LocalizationEntry
    - Fields
      - public string key
      - public string value
  - public class LocalizationDict
    - Fields
      - public List<LocalizationEntry> entries = new();
    - Methods
      - public Dictionary<string, string> ToDictionary()
        - Builds and returns a dictionary from entries, skipping entries with empty or null key.

3) Key Behavior & Side Effects
- ToDictionary flow:
  - Creates a new Dictionary<string, string>.
  - Iterates over entries.
  - For each entry with a non-empty key, assigns dict[e.key] = e.value (overwrites on duplicate keys).
  - Returns the populated dictionary.
- No other side effects beyond the returned dictionary; uses default entry list initialization.

4) Constraints & Failure Modes
- entries is initialized to an empty list, preventing null by default; external null assignment could cause runtime errors in ToDictionary.
- Entries with null or empty key are ignored.
- Duplicate keys in entries will result in the last value win (override previous entries with the same key).

5) Example
```csharp
using System.Collections.Generic;
using CHAL.Systems.Localization;

public class ExampleUsage
{
    void Demo()
    {
        var dictObj = new LocalizationDict
        {
            entries = new List<LocalizationEntry>
            {
                new LocalizationEntry { key = "greet", value = "Hello" },
                new LocalizationEntry { key = "farewell", value = "Goodbye" }
            }
        };

        Dictionary<string, string> map = dictObj.ToDictionary();
        // map["greet"] == "Hello", map["farewell"] == "Goodbye"
    }
}
```

6) Unknowns
- How this structure is populated in broader application flow is not specified.
- Thread-safety and mutation semantics beyond ToDictionary are not defined.
- Interaction with Unity inspector serialization or asset loading is not described.
