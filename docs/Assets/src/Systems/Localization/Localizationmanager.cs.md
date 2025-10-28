# Assets/src/Systems/Localization/Localizationmanager.cs

_Automatic generated/updated._

```markdown
# Purpose
- Defines a static `LocalizationManager` for loading and translating localized strings.

# Public API
- Namespace: `CHAL.Systems.Localization`
- Types
  - `public static class LocalizationManager`
    - Public methods:
      - `public static void Load(string languageCode);`
        - Loads localization data from a JSON file based on the provided language code.
      - `public static string Translate(string key);`
        - Translates the given key to its corresponding localized string; returns the key if not found.

# Key Behavior & Side Effects
- `Load`: Loads a JSON file from the `Resources/Localization` directory and populates the internal dictionary.
- `Translate`: Returns the localized string for a key or the key itself if not found.

# Constraints & Failure Modes
- Assumes the JSON file exists and is correctly formatted; no error handling for file loading or JSON parsing is present.
- `_dict` is initialized only after `Load` is called; calling `Translate` before `Load` will result in a fallback to the key.

# Example
```csharp
LocalizationManager.Load("en");
string enemyName = LocalizationManager.Translate("Enemy_InsectSwarm_Name");
```

# Unknowns
- The structure of the JSON file and the `LocalizationDict` class used for deserialization are not defined in this file.
```
