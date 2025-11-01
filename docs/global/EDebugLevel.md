# global.EDebugLevel

_Automatically generated/updated from `Assets/src/utils/DebugManager.cs`._

```text
1) Purpose
- Defines a centralized, static DebugManager for level-based, tagged logging with per-tag colorization and optional time stamps.
- Maintains in-memory state: CurrentDebugLevel, ProductiveMode, ActiveTags, ExcludedTags, TagColors; initializes from a DebugConfig asset when provided.
- Auto-initializes at runtime from Resources/DebugConfig (config asset path) via DebugManagerAutoInit; editor-only ensures persistence of tags to the asset.

```

```text
2) Public API
- Namespace/module: global namespace (no explicit namespace)

- Types
  - public enum EDebugLevel
    - Production
    - Test
    - Dev
    - Debug

- Public surface
  - public static void Init(DebugConfig config)
    - Initializes DebugManager from a DebugConfig; idempotent
  - public static void Log(string msg, EDebugLevel level = EDebugLevel.Debug, string tag = "System",
                               LogType logType = LogType.Log, Color? customColor = null)
    - Core logging entry point; applies level/tag/color rules and formats output
  - public static void Info(string msg, string tag = "Info")
  - public static void DebugLog(string msg, string tag = "Debug")
  - public static void DevLog(string msg, string tag = "Dev")
  - public static void Warning(string msg, string tag = "Warning")
  - public static void Error(string msg, string tag = "Error")
  - public static void SetTagActive(string tag, bool active)
    - Adds or removes a tag from the ActiveTags set
  - public static void SetDebugLevel(EDebugLevel level)
  - public static void SetProductiveMode(bool productive)
  - public static IEnumerable<string> GetExcludedTags()
    - Returns currently excluded tag names

```

```text
3) Key Behavior & Side Effects
- Init(config)
  - If already initialized, returns immediately.
  - Sets CurrentDebugLevel from config.level and ProductiveMode from config.productiveMode (if config is non-null).
  - Clears ActiveTags, ExcludedTags, TagColors; repopulates from config.tags:
    - Skips entries with empty name
    - If entry.active, adds to ActiveTags
    - TagColors[entry.name] = entry.color
  - Ensures default in-memory tags/colors: "System" (yellow), "Info" (green), "Debug" (white), "Warning" (orange), "Error" (red)
  - Editor-only: writes defaults to asset for System/Info/Debug/Warning/Error with EnsureTagInAsset, marks asset dirty, saves assets
  - Logs initialization info
- EnsureDefault(name, color)
  - Ensures TagColors has color for name; ensures ActiveTags contains name
- EnsureTagInAsset(name, color, active) [Editor-only]
  - Adds tag entry to _config.tags if missing
- LogInternal(message, level, tag, logType, customColor)
  - Blocking rules:
    - If ProductiveMode and level != Production => return
    - If level > CurrentDebugLevel => return
  - Tag knowledge:
    - isKnown = TagColors.ContainsKey(tag) || ActiveTags.Contains(tag)
    - If known but not active: add tag to ExcludedTags and return
    - If unknown: add tag to ExcludedTags
      - Editor-only: optional auto-add to asset if _config.autoAddUnknownTagsToAsset
  - Color resolution:
    - If no color for tag, use provided customColor or white; cache in TagColors
    - Editor-only: optionally auto-add unknown tag to asset as above
  - Formatting:
    - wholeLine = _config != null && _config.colorWholeLine
    - addTime = _config != null && _config.includeGameTimestamps
    - timeStamp = addTime ? "[" + Time.time.ToString("F3") + "]" : ""
    - levelName = level.ToString()
    - If wholeLine: wrap entire line in tagColor
    - Else: color only the tag portion of the line
    - color formatting uses ColorUtility.ToHtmlStringRGB(tagColor)
  - Output:
    - logType determines Debug.LogError/Debug.LogWarning/Debug.Log
- DebugManagerAutoInit.AutoInit
  - BeforeSceneLoad: Loads Resources/config/DebugConfig (DebugConfig asset)
  - If found: DebugManager.Init(cfg)
  - Else: Logs a warning that no DebugConfig was found and defaults are used

```

```text
4) Constraints & Failure Modes
- Initialization guards
  - Init is a one-shot operation guarded by isInitialized
- Editor-only behavior
  - EnsureTagInAsset, file writes, and AssetDatabase actions only execute with UNITY_EDITOR
  - Auto-add unknown tags to asset is gated behind _config.autoAddUnknownTagsToAsset
- Nullability and defaults
  - _config may be null; defaults are ensured via in-memory files only
- Logging controls
  - ProductiveMode suppresses all non-production levels
  - Level gating prevents logs when level > CurrentDebugLevel
- Data structures
  - ActiveTags, ExcludedTags, TagColors are in-memory HashSets/Dictionaries; no explicit threading safeguards
- Unknowns
  - Exact structure of DebugConfig and DebugConfig.TagEntry is not defined here
  - Asset creation/loading semantics depend on Unity project setup (Resources folder, asset path, scripting runtime)
  - Behavior for unknown tags in non-editor builds relies on code paths that may or may not execute depending on UNITY_EDITOR

```

```text
5) Example
```csharp
// Example usage (minimal)
DebugManager.Log("Startup complete", DebugManager.EDebugLevel.Dev, "System");
DebugManager.SetTagActive("System", true);
foreach (var t in DebugManager.GetExcludedTags()) { /* inspect excluded tags if needed */ }
```

```

```text
6) Unknowns
- The exact structure of DebugConfig and DebugConfig.TagEntry (fields like level, productiveMode, tags, color, active) are not defined in this file.
- How to create or modify the DebugConfig asset beyond in-editor writes is not shown here.
- Behavior of Resources.Load path and asset persistence may vary by Unity version; specifics are outside this file.
- Any runtime interactions with other subsystems (e.g., how colors render in the Unity console across platforms) are not specified here.
```
