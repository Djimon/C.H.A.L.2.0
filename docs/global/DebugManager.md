# Assets/src/utils/DebugManager.cs

_Automatically generated/updated from `Assets/src/utils/DebugManager.cs`._

# Purpose
- Provides a centralized logging system with configurable debug levels and tag management.

# Public API
- Namespace: None
- Types
  - public static class DebugManager
    - Public methods:
      - `static void Init(DebugConfig config)` - Initializes the debug configuration settings.
      - `static void Log(string msg, EDebugLevel level = EDebugLevel.Debug, string tag = "System", LogType logType = LogType.Log, Color? customColor = null)` - Logs a message with specified debug level, tag, and optional parameters.
      - `static void Info(string msg, string tag = "Info")` - Logs an informational message with an optional tag.
      - `static void DebugLog(string msg, string tag = "Debug")` - Logs a debug message with an optional tag.
      - `static void DevLog(string msg, string tag = "Dev")` - Logs a development message with an optional tag.
      - `static void Warning(string msg, string tag = "Warning")` - Logs a warning message with an optional tag.
      - `static void Error(string msg, string tag = "Error")` - Logs an error message with an optional tag.
      - `static void SetTagActive(string tag, bool active)` - Activates or deactivates a tag in the application.
      - `static void SetDebugLevel(EDebugLevel level)` - Sets the debug level for the application.
      - `static void SetProductiveMode(bool productive)` - Sets the productive mode for the application.
      - `static IEnumerable<string> GetExcludedTags()` - Gets the collection of excluded tags.

  - public static class DebugManagerAutoInit
    - Public methods:
      - `static void AutoInit()` - Automatically initializes the DebugManager by loading the DebugConfig asset.

# Key Behavior & Side Effects
- Initializes the debug configuration only once; subsequent calls are ignored.
- Logs messages based on the current debug level and active tags.
- Automatically adds unknown tags to the DebugConfig asset if configured to do so in the editor.

# Constraints & Failure Modes
- If `ProductiveMode` is enabled, only messages with `EDebugLevel.Production` will be logged.
- If a tag is not active, it will be added to the excluded tags list and will not log messages.
- The `Init` method will not reinitialize if called multiple times.

# Example
```csharp
DebugConfig config = Resources.Load<DebugConfig>("config/DebugConfig");
DebugManager.Init(config);
DebugManager.Info("This is an info message.");
DebugManager.Warning("This is a warning message.");
```

# Unknowns
- The structure and properties of `DebugConfig` and its `tags` field cannot be determined from this file.

