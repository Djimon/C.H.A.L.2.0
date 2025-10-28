# global.EDebugLevel

_Automatically generated/updated from `Assets/src/utils/DebugManager.cs`._

# Purpose
- Defines a static `DebugManager` for logging with different debug levels and tag management.
- Provides initialization and configuration loading from a `DebugConfig` asset.

# Public API
- Namespace/module: None
- Types
  - **public static class** `DebugManager`
    - **public static void** `Init(DebugConfig config)`
    - **public static void** `Log(string msg, EDebugLevel level = EDebugLevel.Debug, string tag = "System", LogType logType = LogType.Log, Color? customColor = null)`
    - **public static void** `Info(string msg, string tag = "Info")`
    - **public static void** `DebugLog(string msg, string tag = "Debug")`
    - **public static void** `DevLog(string msg, string tag = "Dev")`
    - **public static void** `Warning(string msg, string tag = "Warning")`
    - **public static void** `Error(string msg, string tag = "Error")`
    - **public static void** `SetTagActive(string tag, bool active)`
    - **public static void** `SetDebugLevel(EDebugLevel level)`
    - **public static void** `SetProductiveMode(bool productive)`
    - **public static IEnumerable<string>** `GetExcludedTags()`
  - **public static class** `DebugManagerAutoInit`
    - **private static void** `AutoInit()`

# Key Behavior & Side Effects
- Initializes the `DebugManager` with a `DebugConfig` asset if available.
- Logs messages based on the current debug level and active tags.
- Automatically adds unknown tags to the `DebugConfig` asset if configured to do so.
- Excludes tags that are not active from logging.

# Constraints & Failure Modes
- Initialization can only occur once; subsequent calls to `Init` are ignored.
- Logging is suppressed in productive mode for non-production messages.
- Unknown tags are logged but added to the excluded list if not active.

# Example
```csharp
DebugManager.Init(myDebugConfig);
DebugManager.Log("This is a debug message.");
DebugManager.SetTagActive("CustomTag", true);
```

# Unknowns
- The structure and fields of `DebugConfig` are not defined in this file.
- The behavior of the `DebugConfig` asset when it is missing or improperly configured is not detailed.

