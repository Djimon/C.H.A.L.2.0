# global.DebugConfig

_Automatically generated/updated from `Assets/src/utils/DebugConfig.cs`._

# Purpose
- Defines a `DebugConfig` ScriptableObject for managing debug settings in Unity.
- Provides configuration options for debug levels, tag management, and color settings.

# Public API
- Namespace: None
- Types
  - public class DebugConfig : ScriptableObject
    - Public fields/properties:
      - DebugManager.EDebugLevel level: Sets the debug level.
      - bool productiveMode: Indicates if productive mode is active.
      - bool autoAddUnknownTagsToAsset: Automatically adds unknown tags to the asset.
      - bool includeGameTimestamps: Includes timestamps in game logs.
      - bool colorWholeLine: Determines if the whole line is colored.
      - List<TagEntry> tags: List of tag entries for logging.
    - Public methods:
      - void OnValidate(): Ensures default tags are added when the asset is validated.
      - void EnsureTag(string name, Color color, bool active): Adds a tag if it does not already exist.

# Key Behavior & Side Effects
- On asset validation, default tags are added if the asset is newly created or if no tags exist.
- The `EnsureTag` method prevents duplicate tags from being added.

# Constraints & Failure Modes
- `EnsureTag` guards against null or empty tag names.
- The `OnValidate` method modifies the asset and marks it dirty for saving in the editor.

# Example
```csharp
DebugConfig debugConfig = ScriptableObject.CreateInstance<DebugConfig>();
debugConfig.level = DebugManager.EDebugLevel.Debug;
debugConfig.tags.Add(new DebugConfig.TagEntry { name = "CustomTag", color = Color.green, active = true });
```

# Unknowns
- Specific details about `DebugManager.EDebugLevel` are not defined in this file.

