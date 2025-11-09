# Assets/src/Systems/Localization/LocalizationDict.cs

_Automatically generated/updated from `Assets/src/Systems/Localization/LocalizationDict.cs`._

1) Purpose
- Defines a localization system with entries for key-value pairs.
- Provides functionality to convert localization entries into a dictionary.

2) Public API
- Namespace: CHAL.Systems.Localization
- Types
  - [Serializable] class LocalizationEntry
    - Public fields:
      - string key: The key for the localization entry.
      - string value: The value for the localization entry.
  - [Serializable] class LocalizationDict
    - Public fields:
      - List<LocalizationEntry> entries: A list of localization entries.
    - Public methods:
      - Dictionary<string, string> ToDictionary(): Converts entries to a dictionary; returns a dictionary of key-value pairs.

3) Key Behavior & Side Effects
- The `ToDictionary` method creates a dictionary from the entries, ignoring any entries with null or empty keys.

4) Constraints & Failure Modes
- The `ToDictionary` method does not handle duplicate keys; later entries will overwrite earlier ones in the resulting dictionary.
- Assumes that the `entries` list is initialized before calling `ToDictionary`.

5) Example
```csharp
var localizationDict = new LocalizationDict();
localizationDict.entries.Add(new LocalizationEntry { key = "greeting", value = "Hello" });
var dict = localizationDict.ToDictionary();
// dict now contains: { "greeting": "Hello" }
```

6) Unknowns
- None.
