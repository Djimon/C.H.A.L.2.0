# global.TagEntry

_Automatically generated/updated from `Assets/src/utils/DebugConfig.cs`._

1) Purpose
- Defines a ScriptableObject DebugConfig for configuring debug/logging behavior in Unity.
- Exposes a serializable TagEntry list to describe tag metadata (name, color, active).
- Provides global settings: debug level, productive mode, auto-add unknown tags flag, timestamps, and color mode (tag vs. whole line). Includes editor-created asset menu.

```

```text
2) Public API
- Namespace/module: none

- Types
  - public class DebugConfig : ScriptableObject
    - public DebugManager.EDebugLevel level
      - Default: Debug
    - public bool productiveMode
      - Default: false
    - public bool autoAddUnknownTagsToAsset
      - Default: true
    - public bool includeGameTimestamps
      - Default: false
    - [Header("Color Mode (tag only vs. whole line")] public bool colorWholeLine
      - Default: false
    - public List<TagEntry> tags
      - Default: empty list

  - public class TagEntry [System.Serializable]
    - public string name
    - public bool active
      - Default: true
    - public Color color
      - Default: Color.white

- Public API details
  - DebugConfig
    - level: DebugManager.EDebugLevel
    - productiveMode: bool
    - autoAddUnknownTagsToAsset: bool
    - includeGameTimestamps: bool
    - colorWholeLine: bool
    - tags: List<TagEntry>
  - TagEntry
    - name: string
    - active: bool
    - color: Color

- Editor-only helpers (not public)
  - private bool _seeded
  - private void OnValidate()
  - private void EnsureTag(string name, Color color, bool active)
  - Note: OnValidate/EnsureTag are compiled only under UNITY_EDITOR

- Asset creation
  - [CreateAssetMenu(fileName = "DebugConfig", menuName = "Config/DebugConfig")]
  - Allows creating a DebugConfig asset via the Unity Editor menu

```

```text
3) Key Behavior & Side Effects
- OnValidate (editor only)
  - If _seeded is false or tags is null/empty:
    - Calls EnsureTag for default tags: "System" (yellow, active), "Info" (soft blue), "Debug" (white, active), "Warning" (orange), "Error" (red)
    - Sets _seeded = true
    - Marks the asset dirty and saves assets via UnityEditor.AssetDatabase
- EnsureTag (editor only)
  - If name is null/empty, returns
  - If a tag with the same name already exists in tags, returns
  - Otherwise adds a new TagEntry { name, color, active } to tags
- Runtime implications
  - autoAddUnknownTagsToAsset is documented as a behavior, but this file does not implement runtime behavior for that flag
  - The data is stored in the ScriptableObject asset (fields and TagEntry list)
- Editor-only considerations
  - Code within #if UNITY_EDITOR blocks is excluded from builds
  - Asset creation via CreateAssetMenu is editor-facing

```

```text
4) Constraints & Failure Modes
- UNITY_EDITOR guards
  - OnValidate and EnsureTag compile only in the editor
- Tag initialization
  - OnValidate seeds defaults only when _seeded is false or tags is null/empty
  - Prevents duplicate default tags by checking existing names
- Serialization
  - TagEntry is [System.Serializable]; fields are public for Unity serialization
- External references
  - level uses DebugManager.EDebugLevel; depends on that enum being defined elsewhere
- Colors
  - Defaults rely on UnityEngine.Color values (e.g., Color.yellow, Color.white)
- Potential side effects
  - OnValidate calls AssetDatabase.SaveAssets; may trigger asset re-imports when edited in the UI
- Runtime behavior
  - No runtime methods to load or apply this config are defined in this file
- Null handling
  - EnsureTag guards against empty/null names
- Asset location
  - Comment suggests saving under Resources/config/DebugConfig.asset, but creation is via CreateAssetMenu; the actual path is not enforced by code

```

```text
5) Example
- Not derivable from this file: no runtime loading helper is defined; asset creation is editor-driven

```

```text
6) Unknowns
- How the project loads and consumes DebugConfig at runtime (no load/usage APIs in this file)
- Exact runtime behavior of autoAddUnknownTagsToAsset
- Interaction with DebugManager.EDebugLevel beyond the field name
- Any other serialization or migration considerations when upgrading Unity versions

