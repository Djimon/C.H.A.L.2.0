# BayatGames.SaveGameFree.SaveGamePath

_Automatically generated/updated from `Assets/src/xTernal/SaveGameFree/Scripts/SaveGame.cs`._

# Purpose
- Defines a static class `SaveGame` for saving and loading game data.
- Provides event handlers for save/load operations and customizable serialization and encoding.

# Public API
- Namespace: `BayatGames.SaveGameFree`
- Types
  - `enum SaveGamePath`
    - `PersistentDataPath`: Represents the persistent data path.
    - `DataPath`: Represents the data path.
  - `static class SaveGame`
    - Public fields/properties:
      - `static ISaveGameSerializer Serializer`: Gets or sets the serializer.
      - `static ISaveGameEncoder Encoder`: Gets or sets the encoder.
      - `static Encoding DefaultEncoding`: Gets or sets the encoding.
      - `static bool Encode`: Gets or sets whether to encrypt data by default.
      - `static SaveGamePath SavePath`: Gets or sets the save path.
      - `static string EncodePassword`: Gets or sets the encryption password.
      - `static bool LogError`: Gets or sets whether to log errors.
    - Public methods:
      - `static void Save<T>(string identifier, T obj)`
      - `static void Save<T>(string identifier, T obj, bool encode)`
      - `static void Save<T>(string identifier, T obj, string encodePassword)`
      - `static void Save<T>(string identifier, T obj, ISaveGameSerializer serializer)`
      - `static void Save<T>(string identifier, T obj, ISaveGameEncoder encoder)`
      - `static void Save<T>(string identifier, T obj, Encoding encoding)`
      - `static void Save<T>(string identifier, T obj, SaveGamePath savePath)`
      - `static T Load<T>(string identifier)`
      - `static T Load<T>(string identifier, T defaultValue)`
      - `static T Load<T>(string identifier, bool encode, string encodePassword)`
      - `static T Load<T>(string identifier, ISaveGameSerializer serializer)`
      - `static T Load<T>(string identifier, ISaveGameEncoder encoder)`
      - `static T Load<T>(string identifier, Encoding encoding)`
      - `static T Load<T>(string identifier, SaveGamePath savePath)`
      - `static bool Exists(string identifier)`
      - `static bool Exists(string identifier, SaveGamePath path)`
      - `static void Delete(string identifier)`
      - `static void Delete(string identifier, SaveGamePath path)`
      - `static void Clear()`
      - `static void Clear(SaveGamePath path)`
      - `static void DeleteAll()`
      - `static void DeleteAll(SaveGamePath path)`
      - `static FileInfo[] GetFiles()`
      - `static FileInfo[] GetFiles(string identifier)`
      - `static FileInfo[] GetFiles(string identifier, SaveGamePath path)`
      - `static DirectoryInfo[] GetDirectories()`
      - `static DirectoryInfo[] GetDirectories(string identifier)`
      - `static DirectoryInfo[] GetDirectories(string identifier, SaveGamePath path)`
      - `static bool IOSupported()`
      - `static bool IsFilePath(string str)`

# Key Behavior & Side Effects
- `Save<T>` methods save data to a specified path, optionally encoding it.
- `Load<T>` methods retrieve data, returning a default value if the identifier does not exist.
- `Exists` checks if a save file exists for a given identifier.
- `Delete` removes a save file or directory.
- `DeleteAll` clears all saved data in the specified path.
- Events `OnSaved` and `OnLoaded` are triggered after save/load operations.

# Constraints & Failure Modes
- Throws `ArgumentNullException` if the identifier is null or empty in `Save` and `Load` methods.
- If the specified file does not exist during load, a warning is logged, and the default value is returned.
- The methods handle different platforms and may use `PlayerPrefs` for saving data on unsupported platforms.

# Example
```csharp
// Saving an object
SaveGame.Save("playerData", playerObject);

// Loading an object
PlayerData loadedData = SaveGame.Load<PlayerData>("playerData");
```

# Unknowns
- The behavior of `ISaveGameSerializer` and `ISaveGameEncoder` is not defined in this file.
- The specific implementation details of `SaveGameJsonSerializer` and `SaveGameSimpleEncoder` are not provided.

