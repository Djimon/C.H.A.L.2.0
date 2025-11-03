# global.DebugConfig

_Automatically generated/updated from `Assets/src/utils/DebugConfig.cs`._

1) Purpose
- Defines a ScriptableObject type DebugConfig with CreateAssetMenu for Editor integration.
- Exposes public debug/config fields: level, productiveMode, autoAddUnknownTagsToAsset, includeGameTimestamps, colorWholeLine, and a serializable TagEntry plus a public List<TagEntry> tags.
- Includes an editor-only mechanism to seed default tag entries on asset creation/edit.

2) Public API
- Namespace/module
  - Global namespace (no explicit namespace)

- Types
  - public class DebugConfig : ScriptableObject
    - Public fields
      - public DebugManager.EDebugLevel level = DebugManager.EDebugLevel.Debug
        - Default level for debug logging
      - public bool productiveMode = false
        - Flag for productive/run-time mode
      - public bool autoAddUnknownTagsToAsset = true
        - If true, unknown tags logged for the first time are auto-added to the asset (as per comment)
      - public bool includeGameTimestamps = false
        - Include timestamps in game-related logs
      - [Header("Color Mode (tag only vs. whole line")]
        - public bool colorWholeLine = false
          - If true, color applies to the whole line; otherwise tag-only coloring
      - public List<TagEntry> tags = new()
        - Collection of tag entries used for logging/coloring

    - Nested types
      - public class TagEntry
        - public string name
          - Tag name
        - public bool active = true
          - Whether this tag is active
        - public Color color = Color.white
          - Color associated with this tag
    - Public methods
      - None declared (OnValidate is editor-only and private)

3) Key Behavior & Side Effects
- Editor-only seeding (UNITY_EDITOR)
  - OnValidate(): runs when the asset is created/edited in the Unity Editor if not seeded or if tags is null/empty
  - Seeds default tags via EnsureTag for:
    - System (yellow, active)
    - Info (custom light-blue, active)
    - Debug (white, active)
    - Warning (orange, active)
    - Error (red, active)
  - Sets _seeded = true; marks asset dirty; saves assets
  - EnsureTag(name, color, active):
    - No-op if name is null/empty
    - No-op if a tag with the same name already exists
    - Adds a new TagEntry { name, color, active } to tags
- Asset creation/serialization
  - AssetMenu integration enables creation via Editor menu
  - tags is serialized; TagEntry is public and serializable
- Runtime implications
  - The auto-add behavior is described in a comment; no explicit runtime code here beyond field definition
  - Color and active state of tags influence how logging is presented (per external usage of this config)

4) Constraints & Failure Modes
- Editor-only logic
  - OnValidate and EnsureTag are wrapped in UNITY_EDITOR; not present in builds
- Safety guards
  - EnsureTag avoids null/empty names and duplicates
  - OnValidate handles null tags by treating as empty
- Serialization considerations
  - _seeded is private and editor-only; persists via serialization but not part of public API
- Performance
  - Seed operation occurs at edit-time; frequent edits may trigger asset writes (per Unity editor behavior)

5) Example
- Runtime instantiation
```csharp
// Create an instance at runtime (not yet saved as asset)
var cfg = ScriptableObject.CreateInstance<DebugConfig>();
```

6) Unknowns
- Details of DebugManager.EDebugLevel values and behavior beyond default
- Exact effect of autoAddUnknownTagsToAsset at runtime (comment describes intent; implementation not shown here)
- Exact Unity asset path/location for the created asset beyond the comment
- Any additional editor tooling or editor script integrations not present in this file

