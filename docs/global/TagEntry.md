# global.TagEntry

_Automatically generated/updated from `Assets/src/utils/DebugConfig.cs`._

# Purpose
- Defines a `DebugConfig` ScriptableObject for managing debug settings in Unity.
- Provides configuration options for debug levels, tag management, and color settings.

# Public API
- Namespace/module: None
- Types
  - public class DebugConfig : ScriptableObject
    - Public fields/properties:
      - DebugManager.EDebugLevel level: Current debug level.
      - bool productiveMode: Indicates if productive mode is active.
      - bool autoAddUnknownTagsToAsset: Automatically add unknown tags to the asset.
      - bool includeGameTimestamps: Include timestamps in game logs.
      - bool colorWholeLine: Color the whole line instead of just the tag.
      - List<TagEntry> tags: List of tag entries for logging.
    - Public methods:
      - void OnValidate(): Ensures default tags are added when the asset is validated.
      - void EnsureTag(string name, Color color, bool active): Adds a tag if it does not already exist.

# Key Behavior & Side Effects
- On validation, if the asset is not seeded or has no tags, default tags are added.
- Marks the asset as dirty and saves changes to the asset database when defaults are written.

# Constraints & Failure Modes
- `EnsureTag` method guards against adding tags with null or empty names.
- Tags are only added if they do not already exist in the list.

# Example
```csharp
DebugConfig debugConfig = ScriptableObject.CreateInstance<DebugConfig>();
debugConfig.level = DebugManager.EDebugLevel.Debug;
debugConfig.autoAddUnknownTagsToAsset = true;
```

# Unknowns
- No information on the `DebugManager.EDebugLevel` type or its values.
- No details on how the `DebugConfig` is utilized within the broader application context.

