# global.DebugConfig

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
      - bool productiveMode: Indicates if the productive mode is enabled.
      - bool autoAddUnknownTagsToAsset: Automatically adds unknown tags to the asset.
      - bool includeGameTimestamps: Includes timestamps in game logs.
      - bool colorWholeLine: Determines if the whole line should be colored.
      - List<TagEntry> tags: List of custom tags for logging.
    - Public methods: None

  - [System.Serializable] public class TagEntry
    - Public fields/properties:
      - string name: The name of the tag.
      - bool active: Indicates if the tag is active.
      - Color color: The color associated with the tag.

3) Key Behavior & Side Effects
- OnValidate: Ensures default tags are added when the asset is created or edited, if not already seeded.
- EnsureTag: Adds a new tag if it does not already exist in the tags list.

4) Constraints & Failure Modes
- OnValidate will not add tags if the name is null or empty.
- Tags are only added if the list is empty or the asset has not been seeded.

5) Example
```csharp
// Creating a DebugConfig asset
DebugConfig debugConfig = ScriptableObject.CreateInstance<DebugConfig>();
debugConfig.level = DebugManager.EDebugLevel.Debug;
debugConfig.productiveMode = true;
```

6) Unknowns
- None.
