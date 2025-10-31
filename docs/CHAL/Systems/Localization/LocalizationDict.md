# CHAL.Systems.Localization.LocalizationDict

_Automatically generated/updated from `Assets/src/Systems/Localization/LocalizationDict.cs`._

# Purpose
- Defines a localization system with entries for key-value pairs.
- Provides a method to convert localization entries into a dictionary.

# Public API
- Namespace: `CHAL.Systems.Localization`
- Types:
  - **public class LocalizationEntry**
    - Public fields:
      - `string key`: The localization key.
      - `string value`: The localization value.
  - **public class LocalizationDict**
    - Public fields:
      - `List<LocalizationEntry> entries`: List of localization entries.
    - Public methods:
      - `Dictionary<string, string> ToDictionary()`: Converts entries to a dictionary, ignoring entries with null or empty keys.

# Key Behavior & Side Effects
- The `ToDictionary` method creates a new dictionary from the `entries` list, filtering out any entries with null or empty keys.

# Constraints & Failure Modes
- The `ToDictionary` method does not handle duplicate keys; the last entry with the same key will overwrite previous ones.
- Assumes that `entries` is initialized; if not, it will default to an empty list.

# Example
```csharp
var localizationDict = new LocalizationDict();
localizationDict.entries.Add(new LocalizationEntry { key = "greeting", value = "Hello" });
var dict = localizationDict.ToDictionary();
// dict now contains: { "greeting": "Hello" }
```

# Unknowns
- None.
