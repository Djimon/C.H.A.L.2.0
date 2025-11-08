# CHAL.Systems.Localization.Localizationmanager

_Automatically generated/updated from `Assets/src/Systems/Localization/Localizationmanager.cs`._

# Purpose
- Defines a static `LocalizationManager` for loading and translating localization data.

# Public API
- Namespace: `CHAL.Systems.Localization`
- Types
  - `public static class LocalizationManager`
    - Public methods:
      - `public static void Load(string languageCode)`
        - Loads localization data for the specified language code.
      - `public static string Translate(string key)`
        - Translates the given key into its corresponding value; returns the key itself if not found.

# Key Behavior & Side Effects
- `Load` method loads localization data from a JSON file located in the `Resources/Localization` directory.
- `Translate` method returns the translated value for a key or the key itself if the translation is not found.

# Constraints & Failure Modes
- The `Load` method assumes the JSON file exists and is correctly formatted; no error handling for missing files or invalid JSON is present.
- The `_dict` is only populated after `Load` is called; calling `Translate` before `Load` will return the key itself.

# Example
```csharp
LocalizationManager.Load("en");
string translatedValue = LocalizationManager.Translate("Enemy_InsectSwarm_Name");
```

# Unknowns
- The structure of the JSON file and the `LocalizationDict` class used for deserialization cannot be determined from this file.
