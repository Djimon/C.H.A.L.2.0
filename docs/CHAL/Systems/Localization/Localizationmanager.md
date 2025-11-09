# Assets/src/Systems/Localization/Localizationmanager.cs

_Automatically generated/updated from `Assets/src/Systems/Localization/Localizationmanager.cs`._

# Purpose
- Defines a static `LocalizationManager` for loading and translating localization data.

# Public API
- Namespace: `CHAL.Systems.Localization`
- Types
  - **static class** `LocalizationManager`
    - **Public methods**
      - `Load(string languageCode)`: Loads localization data for the specified language code.
      - `Translate(string key)`: Translates the given key into its corresponding value; returns the key itself if not found.

# Key Behavior & Side Effects
- `Load`: Loads a JSON file from the Resources folder based on the provided language code and populates the internal dictionary.
- `Translate`: Returns the translated value for a key or the key itself if the key is not found in the dictionary.

# Constraints & Failure Modes
- `Load`: Assumes the JSON file exists and is correctly formatted; no error handling for missing files or invalid JSON.
- `Translate`: Returns the original key if the dictionary is null or the key is not found.

# Example
```csharp
LocalizationManager.Load("en");
string translatedValue = LocalizationManager.Translate("Enemy_InsectSwarm_Name");
```

# Unknowns
- The structure of the JSON file and the `LocalizationDict` class used for deserialization are not defined in this file.
