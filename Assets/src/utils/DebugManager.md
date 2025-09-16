# DebugManager & DebugConfig

A lightweight and configurable logging system for Unity projects.  
It provides colored log messages, tag filtering, log levels, and auto-initialization via `Resources/DebugConfig.asset`.

---

## tl;dr
- Clear, color-coded logs.
- Flexible tag-based filtering.
- Adjustable log verbosity.
- Auto-initialization (zero boilerplate).
- Optional auto-registration of new tags.

## Setup

1. Copy both files into your Unity project:
   - `DebugManager.cs` → place in a `Scripts/` folder.
   - `DebugConfig.cs` → place in a `Scripts/Config/` folder (optional, just for organization).

2. Create a `Resources/config/` folder in your project:
    - your folder structure should look like this: Assets/Resources/config/

3. Create a new DebugConfig asset:
- Right-click in the Project window → `Create → Config → DebugConfig`
- Save it as:
  ```
  Assets/Resources/config/DebugConfig.asset
  ```

4. Done! The `DebugManager` auto-initializes on game start via `DebugManagerAutoInit`.

---

## DebugConfig Options

| Setting                  | Description                                                                 |
|---------------------------|-----------------------------------------------------------------------------|
| **level**                 | Global log level (`Production`, `Test`, `Dev`, `Debug`). Controls verbosity.|
| **productiveMode**        | If enabled, only `Production` level logs are shown.                        |
| **autoAddUnknownTagsToAsset** | If enabled, new tags that appear in code will be automatically added to the asset.|
| **colorWholeLine**        | If enabled, the entire log message is colored instead of just the tag.     |
| **tags**                  | List of tags (name, active toggle, and color). Active tags are shown; inactive tags are filtered out. |

---

## Usage Examples

### Standard logging
```csharp
DebugManager.Info("Game started");       // Light Blue [Info] tag
DebugManager.Debugging("Rolling loot");  // White [Debug] tag
DebugManager.Warning("Something odd");   // Orange [Warning] tag
DebugManager.Error("Critical failure");  // Red [Error] tag
```

### Custom tags
```csharp
DebugManager.Log("Enemy AI decision", DebugManager.EDebugLevel.Debug, "AI");
DebugManager.Log("Loot drop rolled", DebugManager.EDebugLevel.Debug, "Loot", LogType.Log);
```
- If the tag "AI" or "Loot" is not defined in the DebugConfig, logs still appear (default white, no toggle).
- If you add "AI" or "Loot" to the DebugConfig.tags, you can set their colors and enable/disable them via checkboxes.
- or enable autoAddUnknownTagsToAsset in the DebugConfig 

### Complete Log() Signature
```csharp
public static void Log(
    string msg,
    DebugManager.EDebugLevel level = DebugManager.EDebugLevel.Debug,
    string tag = "System",
    LogType logType = LogType.Log,
    Color? customColor = null
)
```
#### Parameters
| Parameter       | Type                            | Default      | Description                                                                     |
| --------------- | ------------------------------- | ------------ | ------------------------------------------------------------------------------- |
| **msg**         | `string`                        | *(required)* | The actual log message you want to display.                                     |
| **level**       | `DebugManager.EDebugLevel`      | `Debug`      | The severity/verbosity level of the log (`Production`, `Test`, `Dev`, `Debug`). |
| **tag**         | `string`                        | `"System"`   | A category label for the log (e.g. `"AI"`, `"Loot"`, `"Combat"`).               |
| **logType**     | `UnityEngine.LogType`           | `Log`        | Unity’s log type: `Log`, `Warning`, or `Error`. Determines Console icon.        |
| **customColor** | `UnityEngine.Color?` (nullable) | `null`       | Optional override color. If set, overrides the color defined in `DebugConfig`.  |

#### Examples
```csharp
// Minimal
DebugManager.Log("Simple system message");

// With custom level & tag
DebugManager.Log("Enemy spawned", DebugManager.EDebugLevel.Dev, "AI");

// As a warning
DebugManager.Log("Low health", DebugManager.EDebugLevel.Test, "Combat", LogType.Warning);

// With custom color (blue)
DebugManager.Log("Special loot drop!", DebugManager.EDebugLevel.Debug, "Loot", LogType.Log, Color.cyan);
```

---

## Notes
- Unity always prepends its own timestamp to logs in the Console. The DebugManager adds its own timestamp (game time in seconds by default).
- All configuration is handled through the DebugConfig.asset – no code changes are required.
- Defaults (System, Info, Debug, Warning, Error) are automatically seeded into the asset.


