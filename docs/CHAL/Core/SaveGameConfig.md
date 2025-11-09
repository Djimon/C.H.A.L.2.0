# Assets/src/Core/SaveGameConfig.cs

_Automatically generated/updated from `Assets/src/Core/SaveGameConfig.cs`._

# Purpose
- Defines a configuration asset for saving game data, including file formats and paths.

# Public API
- Namespace: `CHAL.Core`
- Types
  - `public sealed class GameSaveConfig : ScriptableObject`
    - Public fields/properties:
      - `public bool useJsonInEditor` - Indicates if JSON format should be used in the editor.
      - `public bool encodeInPlayer` - Indicates if data should be encoded in the player.
      - `public string encodePassword` - Password for encoding (default is "changeme").
      - `public string baseFolder` - Base folder for saving profiles (default is "profiles").
      - `public string singleProfileFolder` - Folder for a single profile (default is "main").
      - `public string fileStem` - Base name for the profile file (default is "profile").
      - `public string extensionJson` - File extension for JSON files (default is "json").
      - `public string extensionDat` - File extension for encoded files (default is "dat").
    - Public methods:
      - `public string ResolveFileIdRuntime()`
        - Returns the resolved file ID as a string based on current settings.
      - `public bool ShouldEncodeRuntime()`
        - Returns true if encoding is needed; otherwise, false.

# Key Behavior & Side Effects
- `ResolveFileIdRuntime()` constructs a file path based on the current settings, differentiating between JSON and encoded formats depending on the environment (editor or player).
- `ShouldEncodeRuntime()` determines if encoding should be applied based on the environment.

# Constraints & Failure Modes
- The `encodePassword` should not be hardcoded; it should be set via Bootstrap/BuildConfig.
- The behavior of methods varies between the Unity Editor and the player build.

# Example
```csharp
GameSaveConfig config = ScriptableObject.CreateInstance<GameSaveConfig>();
string fileId = config.ResolveFileIdRuntime();
bool shouldEncode = config.ShouldEncodeRuntime();
```

# Unknowns
- None.
