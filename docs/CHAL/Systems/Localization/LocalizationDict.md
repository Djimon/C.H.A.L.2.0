# CHAL.Systems.Localization.LocalizationDict

_Automatically generated/updated from `Assets/src/Systems/Localization/LocalizationDict.cs`._

1) Purpose
- Defines a structure for localization entries and a collection to manage them.

2) Public API
- Namespace: `CHAL.Systems.Localization`
- Types
  - public class `LocalizationEntry`
    - Public fields:
      - `string key`: The key for the localization entry.
      - `string value`: The localized value corresponding to the key.
  - public class `LocalizationDict`
    - Public fields:
      - `List<LocalizationEntry> entries`: A list of localization entries.
    - Public methods:
      - `Dictionary<string, string> ToDictionary()`: Converts the entries to a dictionary with string keys and values.

3) Key Behavior & Side Effects
- The `ToDictionary` method creates a dictionary from the entries, ignoring any entries with null or empty keys.

4) Constraints & Failure Modes
- The `ToDictionary` method does not handle duplicate keys; the last entry with a given key will overwrite any previous entry in the dictionary.

5) Example
```csharp
var localizationDict = new LocalizationDict();
localizationDict.entries.Add(new LocalizationEntry { key = "greeting", value = "Hello" });
localizationDict.entries.Add(new LocalizationEntry { key = "farewell", value = "Goodbye" });
var dict = localizationDict.ToDictionary();
// dict now contains: { "greeting": "Hello", "farewell": "Goodbye" }
```

6) Unknowns
- None.
