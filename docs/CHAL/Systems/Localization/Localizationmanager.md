# CHAL.Systems.Localization.Localizationmanager

_Automatically generated/updated from `Assets/src/Systems/Localization/Localizationmanager.cs`._

1) Purpose
- Defines a static localization helper in CHAL.Systems.Localization.
- Loads translations from a JSON asset at Resources/Localization/{languageCode} via Load.
- Translates keys using Translate, with a fallback to the input key if not found.

```

```csharp
2) Public API
- Namespace/module
  - CHAL.Systems.Localization
- Types
  - public static class LocalizationManager
    - Private fields
      - private static Dictionary<string, string> _dict
    - Public methods
      - public static void Load(string languageCode)
      - public static string Translate(string key)

```

```text
3) Key Behavior & Side Effects
- Load(string languageCode)
  - Reads a TextAsset from Resources.Load<TextAsset>($"Localization/{languageCode}").
  - Deserializes JSON into LocalizationDict via JsonUtility.FromJson<LocalizationDict>(json.text).
  - Converts to Dictionary<string, string> with ToDictionary() and assigns to _dict.
  - Mutates internal static state (_dict) each call.
- Translate(string key)
  - If _dict is non-null and contains the key, returns the mapped value.
  - Otherwise returns the input key (fallback).

```

```text
4) Constraints & Failure Modes
- No null checks for the loaded asset:
  - If the asset is missing, json will be null and json.text would throw.
- No explicit error handling for JSON parsing failures.
- _dict is a static field; no synchronization, so potential race conditions if Load is called concurrently.
- Fallback behavior in Translate is limited to a missing dictionary or missing key; no other error signaling.
- Path convention: asset must be at Resources/Localization/{languageCode} as a TextAsset.

```

```csharp
5) Example
```csharp
LocalizationManager.Load("en");
string name = LocalizationManager.Translate("Enemy_InsectSwarm_Name");
// name = translated value if present; otherwise "Enemy_InsectSwarm_Name"
```

```text
6) Unknowns
- LocalizationDict type and its ToDictionary() implementation are not defined in this file.
- Exact JSON structure expected by LocalizationDict is not shown.
- Details of the dictionary content (e.g., all supported keys) are not known from this file.
- Behavior when the language asset is malformed or missing is not explicitly defined beyond the lack of guards in this file.

