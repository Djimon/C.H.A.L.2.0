# BayatGames.SaveGameFree.SaveGamePath

_Automatically generated/updated from `Assets/src/xTernal/SaveGameFree/Scripts/SaveGame.cs`._

Purpose
- Defines SaveGamePath enum with two options: PersistentDataPath and DataPath.
- Provides a public static SaveGame API for saving and loading data with optional encoding, serialization, and encoding pipelines.
- Exposes events OnSaved/OnLoaded and callbacks SaveCallback/LoadCallback for save/load lifecycle hooks.

Public API
- Namespace/Module: BayatGames.SaveGameFree

- Types
  - public enum SaveGamePath
    - PersistentDataPath
    - DataPath

  - public static class SaveGame
    - public delegate void SaveHandler(object obj, string identifier, bool encode, string password, ISaveGameSerializer serializer, ISaveGameEncoder encoder, Encoding encoding, SaveGamePath path)
    - public delegate void LoadHandler(object loadedObj, string identifier, bool encode, string password, ISaveGameSerializer serializer, ISaveGameEncoder encoder, Encoding encoding, SaveGamePath path)

    - public static event SaveHandler OnSaved
    - public static event LoadHandler OnLoaded

    - public static SaveHandler SaveCallback
    - public static LoadHandler LoadCallback

    - public static ISaveGameSerializer Serializer { get; set; }
    - public static ISaveGameEncoder Encoder { get; set; }
    - public static Encoding DefaultEncoding { get; set; }
    - public static bool Encode { get; set; }
    - public static SaveGamePath SavePath { get; set; }
    - public static string EncodePassword { get; set; }
    - public static bool LogError { get; set; }

    - public static void Save<T>(string identifier, T obj)
    - public static void Save<T>(string identifier, T obj, bool encode)
    - public static void Save<T>(string identifier, T obj, string encodePassword)
    - public static void Save<T>(string identifier, T obj, ISaveGameSerializer serializer)
    - public static void Save<T>(string identifier, T obj, ISaveGameEncoder encoder)
    - public static void Save<T>(string identifier, T obj, Encoding encoding)
    - public static void Save<T>(string identifier, T obj, SaveGamePath savePath)
    - public static void Save<T>(string identifier, T obj, bool encode, string password, ISaveGameSerializer serializer, ISaveGameEncoder encoder, Encoding encoding, SaveGamePath path)

    - public static T Load<T>(string identifier)
    - public static T Load<T>(string identifier, T defaultValue)
    - public static T Load<T>(string identifier, bool encode, string encodePassword)
    - public static T Load<T>(string identifier, ISaveGameSerializer serializer)
    - public static T Load<T>(string identifier, ISaveGameEncoder encoder)
    - public static T Load<T>(string identifier, Encoding encoding)
    - public static T Load<T>(string identifier, SaveGamePath savePath)
    - public static T Load<T>(string identifier, T defaultValue, bool encode)
    - public static T Load<T>(string identifier, T defaultValue, string encodePassword)
    - public static T Load<T>(string identifier, T defaultValue, ISaveGameSerializer serializer)
    - public static T Load<T>(string identifier, T defaultValue, ISaveGameEncoder encoder)
    - public static T Load<T>(string identifier, T defaultValue, Encoding encoding)
    - public static T Load<T>(string identifier, T defaultValue, SaveGamePath savePath)
    - public static T Load<T>(string identifier, T defaultValue, bool encode, string password, ISaveGameSerializer serializer, ISaveGameEncoder encoder, Encoding encoding, SaveGamePath path)

    - public static bool Exists(string identifier)
    - public static bool Exists(string identifier, SaveGamePath path)

    - public static void Delete(string identifier)
    - public static void Delete(string identifier, SaveGamePath path)

    - public static void Clear()
    - public static void Clear(SaveGamePath path)

    - public static void DeleteAll()
    - public static void DeleteAll(SaveGamePath path)

    - public static FileInfo[] GetFiles()
    - public static FileInfo[] GetFiles(string identifier)
    - public static FileInfo[] GetFiles(string identifier, SaveGamePath path)

    - public static DirectoryInfo[] GetDirectories()
    - public static DirectoryInfo[] GetDirectories(string identifier)
    - public static DirectoryInfo[] GetDirectories(string identifier, SaveGamePath path)

    - public static bool IOSupported()
    - public static bool IsFilePath(string str)

Key Behavior & Side Effects
- Save<T> core path handling
  - Validates identifier; throws ArgumentNullException if null/empty.
  - Resolves filePath from identifier and SavePath unless identifier is already a path.
  - Creates directories as needed (platform-aware guards).
  - Chooses streaming strategy based on encode and platform:
    - encode: uses MemoryStream for serialization into a byte sequence, then Base64 and encoder.Encode, writes to file or PlayerPrefs.
    - non-encode: writes raw string (from memory) to file or PlayerPrefs.
  - Serializes object via serializer.Serialize(obj, stream, encoding).
  - On success, invokes SaveCallback and OnSaved with lifecycle details.

- Load<T> core path handling
  - Validates identifier; throws ArgumentNullException if null/empty.
  - Resolves filePath similarly to Save.
  - If file/path does not exist, returns defaultValue and logs warning.
  - Reads data according to encode flag and platform:
    - encode: reads encoded string, decodes, Base64-to-MemoryStream, then deserializes.
    - non-encode: reads from file or PlayerPrefs into a MemoryStream, then deserializes.
  - Applies defaultValue if deserialization yields null.
  - Invokes LoadCallback and OnLoaded with lifecycle details.

- Exists/Delete/DeleteAll/GetFiles/GetDirectories/IOSupported/IsFilePath
  - Exists checks filesystem (when IO is supported) or PlayerPrefs, per platform guards.
  - Delete removes file/directory or PlayerPrefs key; no-op if not present.
  - Clear/DeleteAll remove targeted path contents or all PlayerPrefs keys.
  - GetFiles/GetDirectories enumerate filesystem entries; return empty arrays if not present.
  - IOSupported returns true when platform allows filesystem-based IO; false for WebGL and certain others.
  - IsFilePath detects rooted paths via Path.IsPathRooted with guards for some platforms.

- Threading/async
  - All operations are synchronous; no explicit async API.

- Callbacks
  - SaveCallback/LoadCallback and OnSaved/OnLoaded receive full surface: object, identifier, encode, password, serializer, encoder, encoding, path.

- Defaults
  - Serializer: SaveGameJsonSerializer by default.
  - Encoder: SaveGameSimpleEncoder by default.
  - Encoding: UTF8 by default.
  - Encode: false by default.
  - SavePath: PersistentDataPath by default.
  - EncodePassword: "h@e#ll$o%^" by default.

- Platform specifics
  - On supported platforms, saves to filesystem; on unsupported, uses PlayerPrefs.
  - On Windows/WSA variants, uses Windows.File/Windows.Directory APIs when available.

Constraints & Failure Modes
- Identifier must be non-empty; otherwise throws ArgumentNullException("identifier").
- When loading, if the target does not exist, returns defaultValue and emits a warning via Debug.LogWarningFormat.
- Null serializer/encoding parameters are replaced with defaults; null encoding defaults to DefaultEncoding.
- Encoding/decoding paths require correct password when encoding is enabled; mismatches may cause decode errors at load.
- IO accessibility depends on platform; WebGL/PS4/SamsungTV/tVOS conditions restrict filesystem usage (may fall back to PlayerPrefs).
- Directory creation is guarded by preprocessor directives; may not run on all platforms.
- Resource cleanup: streams are disposed after use to release resources.
- GetFiles/GetDirectories/Exists do not throw for missing paths but return empty arrays or false accordingly.

Example
// Minimal usage example (pseudo-syntax, using default API)
var player = new { Name = "Alice", Level = 5 };

// Save
BayatGames.SaveGameFree.SaveGame.Save("playerData", player);

// Load
var loaded = BayatGames.SaveGameFree.SaveGame.Load<dynamic>("playerData", null);

// Optional: customize pipeline
BayatGames.SaveGameFree.SaveGame.Serializer = new SomeCustomSerializer();
BayatGames.SaveGameFree.SaveGame.Encoder = new SomeCustomEncoder();

Unknowns
- ISaveGameSerializer, ISaveGameEncoder interfaces and concrete implementations (e.g., SaveGameJsonSerializer, SaveGameSimpleEncoder) referenced but not defined in this file.
- Details of serialization formats and encoder behavior are defined elsewhere.
- Behavior for mixed/partial saves, versioning, and migration strategies are not specified here.
- No explicit event args beyond surface types; cannot deduce any extra event payloads beyond the delegates defined.

