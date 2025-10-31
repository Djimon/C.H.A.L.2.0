# CHAL.Core.SaveGameConfig

_Automatically generated/updated from `Assets/src/Core/SaveGameConfig.cs`._

# Purpose
- Defines a configuration asset for saving game data in Unity.
- Provides methods to resolve file paths and determine encoding behavior based on the environment.

# Public API
- Namespace: CHAL.Core
- Types
  - public sealed class GameSaveConfig : ScriptableObject
    - Public fields/properties:
      - bool useJsonInEditor: Indicates if JSON format is used in the editor.
      - bool encodeInPlayer: Indicates if data should be encoded in the player.
      - string encodePassword: Password for encoding (default is "changeme").
      - string baseFolder: Base folder for saving profiles (default is "profiles").
      - string singleProfileFolder: Folder for the main profile (default is "main").
      - string fileStem: Base name for profile files (default is "profile").
      - string extensionJson: File extension for JSON files (default is "json").
      - string extensionDat: File extension for encoded files (default is "dat").
    - Public methods:
      - string ResolveFileIdRuntime(): Resolves the file ID based on the environment.
      - bool ShouldEncodeRuntime(): Determines if encoding should occur based on the environment.

# Key Behavior & Side Effects
- `ResolveFileIdRuntime()` constructs a file path based on whether the application is running in the editor or player.
- `ShouldEncodeRuntime()` returns false in the editor and the value of `encodeInPlayer` in the player.

# Constraints & Failure Modes
- The `encodePassword` should not be hardcoded; it should be set via Bootstrap/BuildConfig.
- The file path resolution depends on the environment (editor vs player).

# Example
```csharp
GameSaveConfig config = ScriptableObject.CreateInstance<GameSaveConfig>();
string filePath = config.ResolveFileIdRuntime();
bool shouldEncode = config.ShouldEncodeRuntime();
```

# Unknowns
- None.

