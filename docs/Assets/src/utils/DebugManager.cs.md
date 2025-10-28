# Assets/src/utils/DebugManager.cs

_Automatic generated/updated._

```markdown
# Purpose
- Defines a static `DebugManager` for logging with configurable debug levels and tags.
- Provides methods for initializing the debug system and logging messages with various severity levels.

# Public API
- Namespace/module: None
- Types
  - `public static class DebugManager`
    - Public fields/properties: None
    - Public methods:
      - `public static void Init(DebugConfig config)`
      - `public static void Log(string msg, EDebugLevel level = EDebugLevel.Debug, string tag = "System", LogType logType = LogType.Log, Color? customColor = null)`
      - `public static void Info(string msg, string tag = "Info")`
      - `public static void DebugLog(string msg, string tag = "Debug")`
      - `public static void DevLog(string msg, string tag = "Dev")`
      - `public static void Warning(string msg, string tag = "Warning")`
      - `public static void Error(string msg, string tag = "Error")`
      - `public static void SetTagActive(string tag, bool active)`
      - `public static void SetDebugLevel(EDebugLevel level)`
      - `public static void SetProductiveMode(bool productive)`
      - `public static IEnumerable<string> GetExcludedTags()`
  - `public static class DebugManagerAutoInit`
    - Public fields/properties: None
    - Public methods:
      - `private static void AutoInit()`

# Key Behavior & Side Effects
- `Init(DebugConfig config)`: Initializes the debug manager with a configuration, setting up active tags and colors.
- `LogInternal(...)`: Handles the actual logging, filtering messages based on the current debug level and productive mode.
- Automatically loads `DebugConfig` from resources on application start if available.

# Constraints & Failure Modes
- Initialization can only occur once; subsequent calls to `Init` are ignored.
- Logs are filtered based on the current debug level and productive mode.
- Unknown tags are added to the excluded list if not active.

# Example
```csharp
DebugManager.Init(myDebugConfig);
DebugManager.Log("This is a debug message.");
```

# Unknowns
- The structure and fields of `DebugConfig` are not defined in this file.
```
