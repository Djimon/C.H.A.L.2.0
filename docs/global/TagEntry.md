# global.TagEntry

_Automatically generated/updated from `Assets/src/utils/DebugConfig.cs`._

```text
1) Purpose
- Defines a Unity ScriptableObject type DebugConfig with debug/logging configuration fields, including a nested serializable TagEntry type and a public list of TagEntry.
- Enables creation via Unity's asset menu (CreateAssetMenu) with fileName "DebugConfig" and menuName "Config/DebugConfig"; includes a comment noting an intended asset path.
- Includes editor-only seed logic (OnValidate/EnsureTag) to populate default tags when the asset is created or edited in the Unity Editor.

2) Public API
- Namespace/module
  - None defined at top level.

- Types
  - public class DebugConfig : ScriptableObject
    - public DebugManager.EDebugLevel level
      - Public field indicating the debug level.
    - public bool productiveMode
      - Public field; description inferred by name.
    - public bool autoAddUnknownTagsToAsset
      - Public field; description inferred by name.
    - public bool includeGameTimestamps
      - Public field; description inferred by name.
    - [Header("Color Mode (tag only vs. whole line")]
      - Public field in the grouped header:
      - public bool colorWholeLine
        - Public field; description inferred by name.
    - public List<TagEntry> tags = new();
      - Serialized list of TagEntry items.
    - public class TagEntry
      - [System.Serializable]
      - public string name
        - Tag name.
      - public bool active = true
        - Whether the tag is active.
      - public Color color = Color.white
        - Display/color for the tag.
    - Note: TagEntry is a public nested type within DebugConfig; all fields are public.

- Public methods
  - None declared publicly. Editor-only methods exist under UNITY_EDITOR:
    - private void OnValidate() [UNITY_EDITOR]
    - private void EnsureTag(string name, Color color, bool active) [UNITY_EDITOR]
  - (These editor-only members are not exposed in the public API surface.)

3) Key Behavior & Side Effects
- Asset creation/editor setup
  - A ScriptableObject asset of type DebugConfig is intended to be created via the editor (CreateAssetMenu) and can be saved under the implied path described in the comment.
- Editor seed workflow (UNITY_EDITOR only)
  - OnValidate runs when the asset is created or modified in the Unity Editor.
  - If _seeded is false or tags is null/empty, OnValidate calls EnsureTag to populate defaults:
    - "System" (Color.yellow, active)
    - "Info" (Color(0.729, 0.808, 1.0), active)
    - "Debug" (Color.white, active)
    - "Warning" (Color(1.0, 0.64, 0.0), active) // orange
    - "Error" (Color.red, active)
  - After seeding, _seeded is set to true, the asset is marked dirty, and assets are saved via UnityEditor APIs.
- TagEntry behavior
  - EnsureTag adds a new TagEntry only if:
    - name is non-empty, and
    - no existing tag in tags has the same name.
  - Added TagEntry uses provided color and active values.
- Serialization/Editor scope
  - Editor-only code is wrapped in #if UNITY_EDITOR, so these behaviors are not compiled into runtime builds.

4) Constraints & Failure Modes
- UNITY_EDITOR gating
  - OnValidate and EnsureTag are compiled only in the Unity Editor; they do not exist in runtime builds.
- Null/empty handling
  - EnsureTag ignores empty/null names and avoids duplicates.
  - OnValidate relies on _seeded and tags; if tags is null/empty, default tags will be added.
- Data persistence
  - Uses UnityEditor.EditorUtility.SetDirty(this) and AssetDatabase.SaveAssets() to persist changes during editor operations.
- Public surface stability
  - Public fields are straightforward data carriers; no public methods are exposed to runtime behavior.

5) Example
- Not applicable/derivable from code alone in a minimal example form.

6) Unknowns
- How DebugManager.EDebugLevel is defined or used at runtime (not in this file).
- Exact runtime loading/usage of DebugConfig (e.g., how/where the asset is consumed or accessed by code).
- Any runtime implications of autoAddUnknownTagsToAsset, includeGameTimestamps, or colorWholeLine beyond their declarations.
- The actual asset path resolution beyond the comment (the code does not enforce Resources/config/DebugConfig.asset; the CreateAssetMenu path is the source of creation in the editor).
```
