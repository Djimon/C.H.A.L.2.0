# Assets/src/xTernal/SaveGameFree/Scripts/SaveGame.cs

_Automatic generated/updated._

```markdown
## Purpose
- Defines a static class `SaveGame` for saving and loading game data.
- Provides an enumeration `SaveGamePath` for specifying save locations.

## Public API
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
      - `static void Save<T>(string identifier, T obj)`: Saves data using the identifier.
      - `static void Save<T>(string identifier, T obj, bool encode)`: Saves data with optional encoding.
      - `static void Save<T>(string identifier, T obj, string encodePassword)`: Saves data with an encoding password.
      - `static void Save<T>(string identifier, T obj, ISaveGameSerializer serializer)`: Saves data with a specified serializer.
      - `static void Save<T>(string identifier, T obj, ISaveGameEncoder encoder)`: Saves data with a specified encoder.
      - `static void Save<T>(string identifier, T obj, Encoding encoding)`: Saves data with a specified encoding.
      - `static void Save<T>(string identifier, T obj, SaveGamePath savePath)`: Saves data to a specified path.
      - `static T Load<T>(string identifier)`: Loads data using the identifier.
      - `static T Load<T>(string identifier, T defaultValue)`: Loads data with a default value.
      - `static T Load<T>(string identifier, bool encode, string encodePassword)`: Loads data with optional encoding.
      - `static T Load<T>(string identifier, ISaveGameSerializer serializer)`: Loads data with a specified serializer.
      - `static T Load<T>(string identifier, ISaveGameEncoder encoder)`: Loads data with a specified encoder.
      - `static T Load<T>(string identifier, Encoding encoding)`: Loads data with a specified encoding.
      - `static T Load<T>(string identifier, SaveGamePath savePath)`: Loads data from a specified path.
      - `static bool Exists(string identifier)`: Checks if the specified identifier exists.
      - `static bool Exists(string identifier, SaveGamePath path)`: Checks if the specified identifier exists at a given path.
      - `static void Delete(string identifier)`: Deletes the specified identifier.
      - `static void Delete(string identifier, SaveGamePath path)`: Deletes the specified identifier at a given path.
      - `static void Clear()`: Clears all saved data.
      - `static void Clear(SaveGamePath path)`: Clears all saved data at a specified path.
      - `static FileInfo[] GetFiles()`: Retrieves files from the save path.
      - `static FileInfo[] GetFiles(string identifier)`: Retrieves files from a specified directory.
      - `static DirectoryInfo[] GetDirectories()`: Retrieves directories from the save path.
      - `static DirectoryInfo[] GetDirectories(string identifier)`: Retrieves directories from a specified directory.
      - `static bool IOSupported()`: Checks if IO is supported on the current platform.
      - `static bool IsFilePath(string str)`: Determines if the string is a file path.

## Key Behavior & Side Effects
- `Save<T>` methods save data to a specified path, optionally encoding it.
- `Load<T>` methods load data from a specified path, returning a default value if not found.
- Events `OnSaved` and `OnLoaded` are triggered after saving and loading operations, respectively.
- `Exists` checks if a save file exists and logs a warning if it does not.

## Constraints & Failure Modes
- Throws `ArgumentNullException` if the identifier is null or empty in `Save`, `Load`, and `Exists` methods.
- Uses `PlayerPrefs` for saving data on platforms where file IO is not supported.
- May log warnings if attempting to load non-existent files.

## Example
```csharp
// Saving an object
SaveGame.Save("playerData", playerObject);

// Loading an object
PlayerData loadedData = SaveGame.Load<PlayerData>("playerData");
```

## Unknowns
- Specific behavior of `ISaveGameSerializer` and `ISaveGameEncoder` is not defined in this file.
- The exact implementation details of `PlayerPrefs` and its limitations are not covered.
```
