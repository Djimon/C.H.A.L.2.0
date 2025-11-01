# CHAL.Core.SaveGameConfig

_Automatically generated/updated from `Assets/src/Core/SaveGameConfig.cs`._

Purpose
- Defines a ScriptableObject (GameSaveConfig) to configure save-game format and paths.
- Centralizes runtime decisions for file naming and encoding mode.
- Exposes inspector-friendly fields (format, paths, and a password placeholder).

Public API
- Namespace/module: CHAL.Core
- Types
  - public sealed class GameSaveConfig : ScriptableObject
    - Public fields
      - bool useJsonInEditor // Format: in Editor use JSON
      - bool encodeInPlayer // Format: in Player use encoded DAT
      - string encodePassword // Default: "changeme"; Tooltip: not hard-coded; set via Bootstrap/BuildConfig
      - string baseFolder // Default: "profiles"
      - string singleProfileFolder // Default: "main"
      - string fileStem // Default: "profile"
      - string extensionJson // Default: "json"
      - string extensionDat // Default: "dat"
    - Public methods
      - string ResolveFileIdRuntime()
        - Returns runtime file path: baseFolder/singleProfileFolder/fileStem.{extension}
        - Determines extension via conditional compilation:
          - UNITY_EDITOR: json = useJsonInEditor
          - else: json = !encodeInPlayer ? true : false
        - ext = json ? extensionJson : extensionDat
      - bool ShouldEncodeRuntime()
        - UNITY_EDITOR: returns false
        - else: returns encodeInPlayer

Key Behavior & Side Effects
- Runtime resolution of file id
  - Uses UNITY_EDITOR to decide whether to honor useJsonInEditor or to infer JSON vs DAT from encodeInPlayer
  - Builds and returns a string path: baseFolder/singleProfileFolder/fileStem.{extension}
- Encoding decision at runtime
  - UNITY_EDITOR => encoding disabled (false)
  - Runtime => encoding enabled if encodeInPlayer is true
- Does not perform any file I/O; only string generation and boolean decisions
- encodePassword is stored but not used by any method (no encoding performed here)

Constraints & Failure Modes
- No null/empty validation implemented; relies on inspector/defaults
- Behavior depends on UNITY_EDITOR compile-time symbol
  - Editor vs runtime paths may differ due to conditional logic
- encodePassword is unused in current logic; potential discrepancy between field and actual encoding behavior
- No threading or asynchronous concerns present

Example
```csharp
// Example usage
using CHAL.Core;
using UnityEngine;

public class SaveConfigUsageExample
{
    public void Demo()
    {
        var cfg = ScriptableObject.CreateInstance<GameSaveConfig>();
        string path = cfg.ResolveFileIdRuntime();
        bool willEncode = cfg.ShouldEncodeRuntime();

        // Example outputs (defaults):
        // path -> "profiles/main/profile.json" (in editor since useJsonInEditor = true)
        // willEncode -> false (editor)
    }
}
```

Unknowns
- How encodePassword is applied during actual encoding (not implemented here)
- How this config integrates with Bootstrap/BuildConfig beyond the provided fields
- Any platform-specific path normalization beyond string concatenation
- Runtime lifecycle or persistence of this ScriptableObject instance beyond being an asset or created at runtime
