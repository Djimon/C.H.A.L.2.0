# Assets/src/utils/DebugConfig.cs

_Automatically generated/updated from `Assets/src/utils/DebugConfig.cs`._

1) Purpose
- Defines a configuration asset for debugging settings in the game.
- Allows customization of debug levels, logging behavior, and tag management.

2) Public API
- Namespace/module: None
- Types
  - public class DebugConfig : ScriptableObject
    - Public fields/properties:
      - DebugManager.EDebugLevel level: Sets the debug level.
      - bool productiveMode: Indicates if productive mode is enabled.
      - bool autoAddUnknownTagsToAsset: Automatically adds unknown tags to the asset.
      - bool includeGameTimestamps: Includes timestamps in game logs.
      - bool colorWholeLine: Determines if the whole line is colored.
      - List<TagEntry> tags: List of tag entries for logging.
  - [System.Serializable] public class TagEntry
    - Public fields/properties:
      - string name: Name of the tag.
      - bool active: Indicates if the tag is active.
      - Color color: Color associated with the tag.

3) Key Behavior & Side Effects
- OnValidate: Automatically populates default tags if the asset is newly created or edited.
- EnsureTag: Adds a new tag entry if it does not already exist.

4) Constraints & Failure Modes
- OnValidate checks if the tags list is null or empty before seeding defaults.
- EnsureTag prevents adding tags with null or empty names.

5) Example
```csharp
DebugConfig debugConfig = ScriptableObject.CreateInstance<DebugConfig>();
debugConfig.level = DebugManager.EDebugLevel.Debug;
debugConfig.productiveMode = true;
```

6) Unknowns
- None.
