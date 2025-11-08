# CHAL.Systems.Localization.LocalizationEntry

_Automatically generated/updated from `Assets/src/Systems/Localization/LocalizationDict.cs`._

# Purpose
- Defines localization entry and dictionary classes for managing localization data.

# Public API
- Namespace: `CHAL.Systems.Localization`
- Types
  - `LocalizationEntry`
    - Public fields/properties:
      - `string key`: The key for the localization entry.
      - `string value`: The value for the localization entry.
  - `LocalizationDict`
    - Public fields/properties:
      - `List<LocalizationEntry> entries`: A list of localization entries.
    - Public methods:
      - `Dictionary<string, string> ToDictionary()`: Converts entries to a dictionary of key-value pairs.

# Key Behavior & Side Effects
- The `ToDictionary` method creates a dictionary from the `entries` list, excluding any entries with null or empty keys.

# Constraints & Failure Modes
- The `ToDictionary` method ignores entries with null or empty keys, ensuring only valid entries are included in the resulting dictionary. 

# Example
```csharp
var localizationDict = new LocalizationDict();
localizationDict.entries.Add(new LocalizationEntry { key = "greeting", value = "Hello" });
var dict = localizationDict.ToDictionary(); // dict will contain { "greeting": "Hello" }
```

# Unknowns
- None.
