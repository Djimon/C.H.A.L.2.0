# Assets/src/Core/SaveGameConfig.cs

_Automatic generated/updated._

```markdown
## Purpose
- Defines a configuration asset for saving game data using Unity's ScriptableObject.
- Provides methods to resolve file identifiers and determine encoding behavior at runtime.

## Public API
- Namespace: `CHAL.Core`
- Types
  - `public sealed class GameSaveConfig : ScriptableObject`
    - Public fields/properties:
      - `public bool useJsonInEditor` - Indicates if JSON format is used in the editor.
      - `public bool encodeInPlayer` - Indicates if data should be encoded in the player.
      - `public string encodePassword` - Password for encoding (default is "changeme").
      - `public string baseFolder` - Base folder for saving profiles.
      - `public string singleProfileFolder` - Folder for the main profile.
      - `public string fileStem` - Base name for the profile file.
      - `public string extensionJson` - File extension for JSON files.
      - `public string extensionDat` - File extension for encoded files.
    - Public methods:
      - `public string ResolveFileIdRuntime()`: Returns the file path based on the current runtime context.
      - `public bool ShouldEncodeRuntime()`: Returns whether encoding should occur based on the current runtime context.

## Key Behavior & Side Effects
- `ResolveFileIdRuntime()` constructs a file path based on whether the application is running in the editor or player, and the encoding settings.
- `ShouldEncodeRuntime()` determines if encoding is applied based on the runtime environment.

## Constraints & Failure Modes
- The `encodePassword` should not be hardcoded; it should be set via Bootstrap/BuildConfig.
- The methods use preprocessor directives to differentiate behavior between the Unity Editor and the player build.

## Example
```csharp
GameSaveConfig config = ScriptableObject.CreateInstance<GameSaveConfig>();
string filePath = config.ResolveFileIdRuntime();
bool shouldEncode = config.ShouldEncodeRuntime();
```

## Unknowns
- None.
```
