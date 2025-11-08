# BayatGames.SaveGameFree.SaveGamePath

_Automatically generated/updated from `Assets/src/xTernal/SaveGameFree/Scripts/SaveGame.cs`._

# Purpose
- Defines a static class `SaveGame` for saving and loading game data.
- Provides an enum `SaveGamePath` for specifying save locations.

# Public API
- Namespace: `BayatGames.SaveGameFree`
- Types
  - **public static class** `SaveGame`
    - **public delegate** `SaveHandler`
    - **public delegate** `LoadHandler`
    - **public event** `OnSaved`
    - **public event** `OnLoaded`
    - **public static SaveHandler** `SaveCallback`
    - **public static LoadHandler** `LoadCallback`
    - **public static ISaveGameSerializer** `Serializer`
    - **public static ISaveGameEncoder** `Encoder`
    - **public static Encoding** `DefaultEncoding`
    - **public static bool** `Encode`
    - **public static SaveGamePath** `SavePath`
    - **public static string** `EncodePassword`
    - **public static bool** `LogError`
    - **public static void** `Save<T>(string identifier, T obj)`
    - **public static void** `Save<T>(string identifier, T obj, bool encode)`
    - **public static void** `Save<T>(string identifier, T obj, string encodePassword)`
    - **public static void** `Save<T>(string identifier, T obj, ISaveGameSerializer serializer)`
    - **public static void** `Save<T>(string identifier, T obj, ISaveGameEncoder encoder)`
    - **public static void** `Save<T>(string identifier, T obj, Encoding encoding)`
    - **public static void** `Save<T>(string identifier, T obj, SaveGamePath savePath)`
    - **public static T** `Load<T>(string identifier)`
    - **public static T** `Load<T>(string identifier, T defaultValue)`
    - **public static T** `Load<T>(string identifier, bool encode, string encodePassword)`
    - **public static T** `Load<T>(string identifier, ISaveGameSerializer serializer)`
    - **public static T** `Load<T>(string identifier, ISaveGameEncoder encoder)`
    - **public static T** `Load<T>(string identifier, Encoding encoding)`
    - **public static T** `Load<T>(string identifier, SaveGamePath savePath)`
    - **public static bool** `Exists(string identifier)`
    - **public static bool** `Exists(string identifier, SaveGamePath path)`
    - **public static void** `Delete(string identifier)`
    - **public static void** `Delete(string identifier, SaveGamePath path)`
    - **public static void** `Clear()`
    - **public static void** `Clear(SaveGamePath path)`
    - **public static void** `DeleteAll()`
    - **public static void** `DeleteAll(SaveGamePath path)`
    - **public static FileInfo[]** `GetFiles()`
    - **public static FileInfo[]** `GetFiles(string identifier)`
    - **public static FileInfo[]** `GetFiles(string identifier, SaveGamePath path)`
    - **public static DirectoryInfo[]** `GetDirectories()`
    - **public static DirectoryInfo[]** `GetDirectories(string identifier)`
    - **public static DirectoryInfo[]** `GetDirectories(string identifier, SaveGamePath path)`
    - **public static bool** `IOSupported()`
    - **public static bool** `IsFilePath(string str)`

# Key Behavior & Side Effects
- `Save<T>` methods save data to a specified path, optionally encoding it.
- `Load<T>` methods load data from a specified path, returning a default value if not found.
- Events `OnSaved` and `OnLoaded` are triggered after save/load operations.
- `Exists` checks if a save file exists, throwing an exception if the identifier is null or empty.
- `Delete` removes a save file or directory if it exists.

# Constraints & Failure Modes
- Throws `ArgumentNullException` if the identifier is null or empty in `Save`, `Load`, `Exists`, and `Delete` methods.
- Handles encoding and file path checks based on platform support.
- Uses `PlayerPrefs` for saving data when file operations are not supported.

# Example
```csharp
// Saving an object
SaveGame.Save("playerData", playerObject);

// Loading an object
PlayerData loadedData = SaveGame.Load<PlayerData>("playerData");
```

# Unknowns
- None.

