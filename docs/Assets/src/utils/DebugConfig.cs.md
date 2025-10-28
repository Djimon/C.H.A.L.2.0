# Assets/src/utils/DebugConfig.cs

_Automatic generated/updated._

```markdown
# Purpose
- Defines a `DebugConfig` ScriptableObject for managing debug settings in Unity.
- Provides configuration options for debug levels, tag management, and color settings.

# Public API
- Namespace/module: None
- Types
  - public class `DebugConfig` [extends ScriptableObject]
    - Public fields/properties:
      - `public DebugManager.EDebugLevel level`: Sets the debug level.
      - `public bool productiveMode`: Indicates if productive mode is active.
      - `public bool autoAddUnknownTagsToAsset`: Automatically adds unknown tags to the asset.
      - `public bool includeGameTimestamps`: Includes timestamps in game logs.
      - `public bool colorWholeLine`: Determines if the whole line is colored.
      - `public List<TagEntry> tags`: List of tag entries for logging.
  - [System.Serializable] public class `TagEntry`
    - Public fields/properties:
      - `public string name`: Name of the tag.
      - `public bool active`: Indicates if the tag is active.
      - `public Color color`: Color associated with the tag.

# Key Behavior & Side Effects
- `OnValidate()`: Ensures default tags are added when the asset is created or edited.
- `EnsureTag(string name, Color color, bool active)`: Adds a tag if it does not already exist; skips if the name is null or empty.

# Constraints & Failure Modes
- Guards against null or empty tag names in `EnsureTag`.
- Tags are only seeded once; if the list is empty or null, defaults are added.
- Uses UnityEditor methods to mark the asset as dirty and save changes.

# Example
```csharp
DebugConfig debugConfig = ScriptableObject.CreateInstance<DebugConfig>();
debugConfig.level = DebugManager.EDebugLevel.Debug;
debugConfig.tags.Add(new DebugConfig.TagEntry { name = "CustomTag", color = Color.green, active = true });
```

# Unknowns
- No information on the `DebugManager.EDebugLevel` type or its values.
- No details on how `DebugConfig` is utilized within the broader application context.
```
