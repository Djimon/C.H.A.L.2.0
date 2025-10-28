# CHAL.Systems.Localization.LocalizationEntry

_Automatically generated/updated from `Assets/src/Systems/Localization/LocalizationDict.cs`._

# Purpose
- Defines a localization system with entries for keys and values.
- Provides a method to convert a list of localization entries into a dictionary.

# Public API
- Namespace: CHAL.Systems.Localization
- Types
  - [Serializable] class LocalizationEntry
    - Public fields:
      - string key: The localization key.
      - string value: The localization value.
  - [Serializable] class LocalizationDict
    - Public fields:
      - List<LocalizationEntry> entries: List of localization entries.
    - Public methods:
      - Dictionary<string, string> ToDictionary(): Converts entries to a dictionary, ignoring null or empty keys.

# Key Behavior & Side Effects
- The `ToDictionary` method creates a new dictionary from the entries, excluding any entries with null or empty keys.

# Constraints & Failure Modes
- The `ToDictionary` method does not handle duplicate keys; later entries will overwrite earlier ones in the resulting dictionary.
- Assumes that the `entries` list is initialized (default to an empty list).

# Example
```csharp
var localizationDict = new LocalizationDict();
localizationDict.entries.Add(new LocalizationEntry { key = "hello", value = "Hello" });
localizationDict.entries.Add(new LocalizationEntry { key = "world", value = "World" });
var dict = localizationDict.ToDictionary();
// dict now contains { "hello": "Hello", "world": "World" }
```

# Unknowns
- No information on how this localization system integrates with other systems or components.

