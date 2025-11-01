# global.DebugConfig

_Automatically generated/updated from `Assets/src/utils/DebugConfig.cs`._

```text
1) Purpose
- Defines a ScriptableObject DebugConfig for centralizing debug-related settings.
- Declares a serializable TagEntry and a public List<TagEntry> tags to configure per-tag color and activation.
- Exposes CreateAssetMenu for editor asset creation and includes editor-only seed logic to populate defaults.

2) Public API
- Public class: DebugConfig : ScriptableObject
  - Public fields
    - DebugManager.EDebugLevel level = DebugManager.EDebugLevel.Debug
      - Current global debug level
    - bool productiveMode = false
      - Flag for production-oriented behavior
    - bool autoAddUnknownTagsToAsset = true
      - If true: unknown tags logged for the first time are automatically added to the asset (color white, active)
        - Note: described via comment; not implemented elsewhere in this file
    - bool includeGameTimestamps = false
      - Whether to include timestamps from the game context
    - bool colorWholeLine = false
      - Colorization scope: tag-only vs. whole line (under a header)
  - List<TagEntry> tags = new()
    - Collection of per-tag configurations (name, active, color)

- Public nested type: TagEntry [System.Serializable]
  - Public fields
    - string name
      - Tag name
    - bool active = true
      - Whether the tag is active
    - Color color = Color.white
      - Color associated with the tag

- Public surface summary (no public methods defined)
  - None beyond fields defined above (OnValidate/EnsureTag are private/editor-only)

3) Key Behavior & Side Effects
- Asset creation UX
  - The CreateAssetMenu attribute enables creating a DebugConfig asset via Unity editor menu (Assets -> Create -> Config -> DebugConfig).
- Editor-only seed logic
  - In UNITY_EDITOR, OnValidate runs when the asset is created or edited.
  - If not _seeded or if tags is null or empty:
    - EnsureTag is called to add default tags: System, Info, Debug, Warning, Error with predefined colors/active state.
    - _seeded is set to true.
    - Marks the asset dirty and saves assets via UnityEditor (SetDirty; SaveAssets).
  - EnsureTag(string name, Color color, bool active) adds a TagEntry if name is non-empty and not already present.
- Tag behavior hints
  - The field autoAddUnknownTagsToAsset has a documented effect in a comment: unknown tags logged for the first time are automatically added to the asset (white color, active).
  - The colorWholeLine flag controls colorization scope for tags/lines (actual usage beyond this file is not shown).

4) Constraints & Failure Modes
- Editor-only code
  - OnValidate and tag seeding execute only within the Unity Editor; runtime builds will not run this logic.
- Potential syntax issue in code
  - The Header attribute is written as [Header("Color Mode (tag only vs. whole line"] which appears to have an unmatched quote; this could cause a compile-time error depending on exact parsing.
- Asset-location note
  - The comment suggests saving to Resources/config/DebugConfig.asset, but the code omits any hard path logic; asset location is not enforced by the file.
- External type reference
  - DebugManager.EDebugLevel is referenced but not defined in this file; its definition lives elsewhere.

5) Example
- Minimal usage in Unity and code snippet:

- In Unity Editor:
  - Right-click in the Project window
  - Choose Create -> Config -> DebugConfig
  - Name the asset (default: DebugConfig)

- Example usage in C#:
```csharp
using UnityEngine;

public class DebugConfigUser : MonoBehaviour
{
    [SerializeField] private DebugConfig config;

    void Start()
    {
        if (config != null)
        {
            Debug.Log("Debug level: " + config.level);
        }
    }
}
```

6) Unknowns
- DebugManager.EDebugLevel type definition is not present in this file.
- Runtime behavior for autoAddUnknownTagsToAsset is not implemented here; documented only by a comment.
- The exact asset path for the created DebugConfig asset is not enforced by code (only described in a comment).
- The Header attribute appears to have a syntax issue which could affect compilation.
- Any runtime usage of includeGameTimestamps, colorWholeLine, or tags beyond data storage is not shown in this file.
```
