# global.EDebugLevel

_Automatically generated/updated from `Assets/src/utils/DebugManager.cs`._

```text
Purpose
- Defines a centralized, config-driven debug/log system for Unity.
- Provides level-based filtering, tag-based coloring/activation, and in-editor asset syncing.
- Auto-initializes at runtime by loading a DebugConfig from Resources (config/DebugConfig).

```

```csharp
// Public API surface (from this file)
```

1) Public API

- Namespace/module: Global (no namespace)

- Types
  - public enum EDebugLevel
    - Production = 1
    - Test = 2
    - Dev = 3
    - Debug = 4
  - public static class DebugManager
    - Init(DebugConfig config)
    - Log(string msg, EDebugLevel level = EDebugLevel.Debug, string tag = "System", LogType logType = LogType.Log, Color? customColor = null)
    - Info(string msg, string tag = "Info")
    - DebugLog(string msg, string tag = "Debug")
    - DevLog(string msg, string tag = "Dev")
    - Warning(string msg, string tag = "Warning")
    - Error(string msg, string tag = "Error")
    - SetTagActive(string tag, bool active)
    - SetDebugLevel(EDebugLevel level)
    - SetProductiveMode(bool productive)
    - GetExcludedTags() -> IEnumerable<string>

Notes
- All surface is defined in this file; DebugConfig is referenced as a type but defined elsewhere.
- Unity types used: LogType, Color

```

3) Key Behavior & Side Effects

- Init(DebugConfig config)
  - Guards against re-initialization (no-op if already initialized).
  - Stores config and, if present:
    - Sets CurrentDebugLevel from config.level.
    - Sets ProductiveMode from config.productiveMode.
    - Clears ActiveTags, ExcludedTags, TagColors and repopulates from config.tags (for each non-empty name: if entry.active, add to ActiveTags; store color per tag).
  - Ensures default/system tags exist in memory: System, Info, Debug, Warning, Error with associated colors.
  - Editor-only: ensures tags exist in the DebugConfig asset, marks dirty, and saves assets.
  - Marks initialized and logs initialization message.

- EnsureDefault(string name, Color color)
  - Adds color to TagColors if missing; ensures name is in ActiveTags.

- EnsureTagInAsset (UNITY_EDITOR)
  - Adds a tag entry to _config.tags if the tag is not already present.

- Public logging API (Log, Info, DebugLog, DevLog, Warning, Error)
  - LogInternal handles core behavior:
    - If ProductiveMode is enabled and level is not Production, drop the log.
    - If level > CurrentDebugLevel, drop the log.
    - Determine if tag is known (TagColors contains tag) or active (ActiveTags contains tag).
    - If known but not ActiveTags, add to ExcludedTags and drop.
    - If unknown, add tag to ExcludedTags; may log but not block.
    - Determine tagColor:
      - If tag not in TagColors, use provided customColor or white; store in TagColors.
      - In UNITY_EDITOR, optionally auto-add unknown tags to asset if config.autoAddUnknownTagsToAsset is true (adds to config.tags, activates tag, marks dirty, saves assets).
    - Colorization:
      - If config.colorWholeLine: color entire line with tagColor.
      - Else: color only the tag portion; prepend time if configured.
    - Time stamping:
      - If config.includeGameTimestamps: prepend time in seconds since startup.
    - Formatting:
      - Whole line: "<color=#RRGGBB>time][LEVEL][tag]: message</color>"
      - Partial: "[time][LEVEL][<color=tagColor>tag @ time</color>]: message"
    - Output:
      - LogType.Error -> Debug.LogError(formatted)
      - LogType.Warning -> Debug.LogWarning(formatted)
      - Default -> Debug.Log(formatted)

- SetTagActive(string tag, bool active)
  - Adds/removes tag from ActiveTags based on flag.

- SetDebugLevel(EDebugLevel level)
  - Sets CurrentDebugLevel to level.

- SetProductiveMode(bool productive)
  - Sets ProductiveMode flag.

- GetExcludedTags() -> IEnumerable<string>
  - Returns the current ExcludedTags collection.

- Internal state/behavior notes
  - Unknown/disabled tags are recorded in ExcludedTags.
  - If _config is present and certain flags are set, new tags may be auto-added to the asset (Editor-only path).

- Runtime initialization helper (DebugManagerAutoInit)
  - OnBeforeSceneLoad, attempts to load Resources/config/DebugConfig (DebugConfig asset).
  - If found, calls DebugManager.Init(cfg); otherwise logs a warning.

```

4) Constraints & Failure Modes

- Init guards against multiple initializations; subsequent calls are no-ops.
- Null config: initialized with runtime defaults (no crash, defaults applied).
- Tag handling:
  - Known but not active tags are blocked (added to ExcludedTags).
  - Unknown tags are logged but added to ExcludedTags; may auto-add to asset in editor if enabled.
- Editor-only code paths (UnityEditor) are guarded with #if UNITY_EDITOR; runtime builds omit these sections safely.
- Color/formatting depends on _config fields:
  - colorWholeLine and includeGameTimestamps affect formatting.
  - autoAddUnknownTagsToAsset affects in-editor asset updates.
- Time-based formatting relies on Time.time for timestamp.
- GetExcludedTags returns an IEnumerable<string> view; internal collection is a HashSet<string>.

```

5) Example

```csharp
// Minimal example: initialize then log a few messages
DebugConfig cfg = Resources.Load<DebugConfig>("config/DebugConfig");
DebugManager.Init(cfg);

DebugManager.Log("System startup complete"); // default level/tag
DebugManager.Info("Info message", "Info");
DebugManager.Warning("Low memory warning", "Warning");
DebugManager.Error("Failed to load asset", "Error");

// Unknown tag example (will be recorded as excluded and may be auto-added in editor)
DebugManager.Log("This tag is new", EDebugLevel.Debug, "NewTag");
```

```

6) Unknowns

- The DebugConfig type definition is not present in this file; its fields (e.g., level, productiveMode, tags, autoAddUnknownTagsToAsset, colorWholeLine, includeGameTimestamps) are not defined here.
- DebugConfig.TagEntry type fields (name, color, active) are not defined here.
- Exact behavior of Resources path ("config/DebugConfig") and asset contents are not defined here beyond usage.
- Any behavior of the AutoInit path in builds without a DebugConfig asset is limited to the warning log.
- Any side effects outside this file (e.g., other systems consuming ExcludedTags or ActiveTags) are not represented here.
