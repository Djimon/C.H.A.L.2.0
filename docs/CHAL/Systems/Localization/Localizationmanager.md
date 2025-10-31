# CHAL.Systems.Localization.Localizationmanager

_Automatically generated/updated from `Assets/src/Systems/Localization/Localizationmanager.cs`._

# Purpose
- Defines a static class for managing localization in the application.

# Public API
- Namespace: `CHAL.Systems.Localization`
- Types
  - `public static class LocalizationManager`
    - Public methods:
      - `public static void Load(string languageCode);`
        - Loads localization data from a JSON file based on the provided language code.
      - `public static string Translate(string key);`
        - Translates a given key to the corresponding localized string; returns the key if not found.

# Key Behavior & Side Effects
- `Load`: Loads a JSON file from the Resources folder and populates the localization dictionary.
- `Translate`: Returns the localized string for a key or the key itself if not found.

# Constraints & Failure Modes
- Assumes the JSON file exists in the specified path; no error handling for missing files.
- `_dict` is initialized only after `Load` is called; `Translate` will return the key if `Load` has not been executed.

# Example
```csharp
LocalizationManager.Load("en");
string enemyName = LocalizationManager.Translate("Enemy_InsectSwarm_Name");
```

# Unknowns
- The structure of the JSON file and the `LocalizationDict` class used for deserialization are not defined in this file.

