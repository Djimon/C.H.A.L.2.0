# global.EDebugLevel

_Automatically generated/updated from `Assets/src/utils/DebugManager.cs`._

# Purpose
- Provides a centralized logging system with configurable debug levels and tag management.

# Public API
- Namespace: None
- Types
  - public static class DebugManager
    - Public fields/properties:
      - EDebugLevel CurrentDebugLevel: Current debug level setting.
      - bool ProductiveMode: Indicates if the application is in productive mode.
    - Public methods:
      - void Init(DebugConfig config): Initializes the debug configuration settings.
      - void Log(string msg, EDebugLevel level = EDebugLevel.Debug, string tag = "System", LogType logType = LogType.Log, Color? customColor = null): Logs a message with specified parameters.
      - void Info(string msg, string tag = "Info"): Logs an informational message.
      - void DebugLog(string msg, string tag = "Debug"): Logs a debug message.
      - void DevLog(string msg, string tag = "Dev"): Logs a development message.
      - void Warning(string msg, string tag = "Warning"): Logs a warning message.
      - void Error(string msg, string tag = "Error"): Logs an error message.
      - void SetTagActive(string tag, bool active): Activates or deactivates a tag.
      - void SetDebugLevel(EDebugLevel level): Sets the debug level.
      - void SetProductiveMode(bool productive): Sets the productive mode.
      - IEnumerable<string> GetExcludedTags(): Gets the collection of excluded tags.
  - public static class DebugManagerAutoInit
    - Public methods:
      - void AutoInit(): Automatically initializes DebugManager with DebugConfig if available.

# Key Behavior & Side Effects
- Initializes debug settings from a `DebugConfig` asset if available.
- Logs messages based on the current debug level and active tags.
- Automatically adds unknown tags to the `DebugConfig` asset if configured to do so.

# Constraints & Failure Modes
- Initialization can only occur once; subsequent calls to `Init` are ignored.
- Messages are not logged if the application is in productive mode and the log level is not set to production.
- Unknown tags are added to the excluded list if not active.

# Example
```csharp
DebugConfig config = Resources.Load<DebugConfig>("config/DebugConfig");
DebugManager.Init(config);
DebugManager.Log("This is a test message.");
```

# Unknowns
- The structure and fields of `DebugConfig` are not defined in this file.

