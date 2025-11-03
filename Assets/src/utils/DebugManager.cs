using System.Collections.Generic;
using UnityEngine;

    public static class DebugManager
    {
        //Create asset of Type "DebugConfig" and save it under "Resources/config/DebugConfig.asset"
        public enum EDebugLevel
        {
            Production = 1,
            Test = 2,
            Dev = 3,
            Debug = 4
        }

        private static EDebugLevel CurrentDebugLevel = EDebugLevel.Debug;
        private static bool ProductiveMode = false;

        private static readonly HashSet<string> ActiveTags = new();
        private static readonly HashSet<string> ExcludedTags = new();
        private static readonly Dictionary<string, Color> TagColors = new();

        private static DebugConfig _config;
        private static bool isInitialized = false;

        // ---------------- INIT ----------------
/// <summary>
/// Initializes the debug configuration settings.
/// </summary>
/// <param name="config">The debug configuration to apply.</param>
        public static void Init(DebugConfig config)
        {
            if (isInitialized) return;
            _config = config;

            if (_config != null)
            {
                CurrentDebugLevel = _config.level;
                ProductiveMode = _config.productiveMode;

                ActiveTags.Clear();
                ExcludedTags.Clear();
                TagColors.Clear();

                foreach (var entry in _config.tags)
                {
                    if (string.IsNullOrEmpty(entry.name)) continue;
                    if (entry.active) ActiveTags.Add(entry.name);
                    TagColors[entry.name] = entry.color;
                }
            }

            // Immer in-memory sicherstellen (für den Fall, dass Asset alt/leer ist)
            EnsureDefault("System", Color.yellow);
            EnsureDefault("Info", Color.green);
            EnsureDefault("Debug", Color.white);
            EnsureDefault("Warning", new Color(1f, 0.65f, 0f));
            EnsureDefault("Error", Color.red);

#if UNITY_EDITOR
            // …und auch ins Asset schreiben, falls noch nicht vorhanden
            if (_config != null)
            {
                EnsureTagInAsset("System", Color.yellow, true);
                EnsureTagInAsset("Info", Color.green, true);
                EnsureTagInAsset("Debug", Color.white, true);
                EnsureTagInAsset("Warning", new Color(1f, 0.64f, 0f), true);
                EnsureTagInAsset("Error", Color.red, true);

                UnityEditor.EditorUtility.SetDirty(_config);
                UnityEditor.AssetDatabase.SaveAssets();
            }
#endif

            isInitialized = true;
            Info("DebugManager Initialized: see DebugConfig");

        }

        private static void EnsureDefault(string name, Color color)
        {
            if (!TagColors.ContainsKey(name)) TagColors[name] = color;
            if (!ActiveTags.Contains(name)) ActiveTags.Add(name);
        }

#if UNITY_EDITOR
        private static void EnsureTagInAsset(string name, Color color, bool active)
        {
            if (_config == null) return;
            bool exists = false;
            foreach (var t in _config.tags) { if (t.name == name) { exists = true; break; } }
            if (!exists)
                _config.tags.Add(new DebugConfig.TagEntry { name = name, color = color, active = active });
        }
#endif

        // ---------------- PUBLIC API ----------------
/// <summary>
/// Logs a message with specified debug level, tag, and optional parameters.
/// </summary>
/// <param name="msg">The message to log.</param>
/// <param name="level">The debug level of the log (default is EDebugLevel.Debug).</param>
/// <param name="tag">An optional tag for the message (default is "System").</param>
/// <param name="logType">The type of log (default is LogType.Log).</param>
/// <param name="customColor">An optional custom color for the log.</param>
        public static void Log(string msg, EDebugLevel level = EDebugLevel.Debug, string tag = "System",
                               LogType logType = LogType.Log, Color? customColor = null)
        {
            LogInternal(msg, level, tag, logType, customColor);
        }

/// <summary>
/// Logs an informational message with an optional tag.
/// </summary>
/// <param name="msg">The informational message to log.</param>
/// <param name="tag">An optional tag for the message (default is "Info").</param>
        public static void Info(string msg, string tag = "Info") =>
            LogInternal(msg, EDebugLevel.Test, tag, LogType.Log);

/// <summary>
/// Logs a debug message with an optional tag.
/// </summary>
/// <param name="msg">The debug message to log.</param>
/// <param name="tag">An optional tag for the message (default is "Debug").</param>
        public static void DebugLog(string msg, string tag = "Debug") =>
            LogInternal(msg, EDebugLevel.Debug, tag, LogType.Log);

/// <summary>
/// Logs a development message with an optional tag.
/// </summary>
/// <param name="msg">The development message to log.</param>
/// <param name="tag">An optional tag for the message (default is "Dev").</param>
        public static void DevLog(string msg, string tag = "Dev") =>
            LogInternal(msg, EDebugLevel.Dev, tag, LogType.Log);

/// <summary>
/// Logs a warning message with an optional tag.
/// </summary>
/// <param name="msg">The warning message to log.</param>
/// <param name="tag">An optional tag for the warning (default is "Warning").</param>
        public static void Warning(string msg, string tag = "Warning") =>
            LogInternal(msg, EDebugLevel.Test, tag, LogType.Warning);

/// <summary>
/// Logs an error message with an optional tag.
/// </summary>
/// <param name="msg">The error message to log.</param>
/// <param name="tag">An optional tag for the error (default is "Error").</param>
        public static void Error(string msg, string tag = "Error") =>
            LogInternal(msg, EDebugLevel.Production, tag, LogType.Error);

/// <summary>
/// Activates or deactivates a tag in the application.
/// </summary>
/// <param name="tag">The tag to activate or deactivate.</param>
/// <param name="active">True to activate the tag; otherwise, false.</param>
        public static void SetTagActive(string tag, bool active)
        {
            if (active) ActiveTags.Add(tag);
            else ActiveTags.Remove(tag);
        }

/// <summary>
/// Sets the debug level for the application.
/// </summary>
/// <param name="level">The debug level to set.</param>
        public static void SetDebugLevel(EDebugLevel level) => CurrentDebugLevel = level;
/// <summary>
/// Sets the productive mode for the application.
/// </summary>
/// <param name="productive">True to enable productive mode; otherwise, false.</param>
        public static void SetProductiveMode(bool productive) => ProductiveMode = productive;
/// <summary>
/// Gets the collection of excluded tags.
/// </summary>
/// <returns>An enumerable list of excluded tag strings.</returns>
        public static IEnumerable<string> GetExcludedTags() => ExcludedTags;

        // ---------------- INTERNAL ----------------
        private static void LogInternal(string message, EDebugLevel level, string tag, LogType logType, Color? customColor = null)
        {
            if (ProductiveMode && level != EDebugLevel.Production) return;
            if (level > CurrentDebugLevel) return;

            bool isKnown = TagColors.ContainsKey(tag) || ActiveTags.Contains(tag);

            // Deaktivierte Tags blocken
            if (isKnown && !ActiveTags.Contains(tag))
            {
                ExcludedTags.Add(tag);
                return;
            }

            // Unbekannte dürfen loggen, landen aber in Excluded-Liste
            if (!isKnown)
            {
                ExcludedTags.Add(tag);
                // Optional: return hier, wenn unbekannte auch blockiert sein sollen
            }

            // Farbe bestimmen
            if (!TagColors.TryGetValue(tag, out var tagColor))
            {
                tagColor = customColor ?? Color.white;
                TagColors[tag] = tagColor;

#if UNITY_EDITOR
                // Optional: unbekannten Tag automatisch ins Asset übernehmen
                if (_config != null && _config.autoAddUnknownTagsToAsset)
                {
                    bool exists = false;
                    foreach (var t in _config.tags) { if (t.name == tag) { exists = true; break; } }
                    if (!exists)
                    {
                        _config.tags.Add(new DebugConfig.TagEntry { name = tag, color = tagColor, active = true });
                        ActiveTags.Add(tag);
                        UnityEditor.EditorUtility.SetDirty(_config);
                        UnityEditor.AssetDatabase.SaveAssets();
                    }
                }
#endif
            }
            else if (customColor.HasValue)
            {
                tagColor = customColor.Value;
            }

            bool wholeLine = _config != null && _config.colorWholeLine;
            bool addTime = _config != null && _config.includeGameTimestamps;
            string timeStamp = addTime ? "[" + Time.time.ToString("F3") + "]" : ""; // Spielzeit seit Start
            string levelName = level.ToString();
            string formatted = $"{timeStamp}[{levelName}][{tag}]: {message}";

            if (wholeLine)
            {
                // Alles färben
                formatted = $"<color=#{ColorUtility.ToHtmlStringRGB(tagColor)}>{timeStamp}[{levelName}][{tag}]: {message}</color>";
            }
            else
            {
                // Nur Tag färben
                string coloredTag = $"<color=#{ColorUtility.ToHtmlStringRGB(tagColor)}>{tag} @ {timeStamp}</color>";
                formatted = $"{timeStamp}[{levelName}][{coloredTag}]: {message}";
            }

            switch (logType)
            {
                case LogType.Error: Debug.LogError(formatted); break;
                case LogType.Warning: Debug.LogWarning(formatted); break;
                default: Debug.Log(formatted); break;
            }
        }

    }

    public static class DebugManagerAutoInit
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void AutoInit()
        {
            // Lädt Resources/DebugConfig.asset, wenn vorhanden
            DebugConfig cfg = Resources.Load<DebugConfig>("config/DebugConfig");
            if (cfg != null) DebugManager.Init(cfg);
            else Debug.LogWarning("[DebugManager] No DebugConfig found in Resources/. Using runtime defaults.");
        }
    }

