# BayatGames.SaveGameFree.SaveGameWeb

_Automatically generated/updated from `Assets/src/xTernal/SaveGameFree/Scripts/SaveGameWeb.cs`._

# Purpose
- Defines the `SaveGameWeb` class for saving and loading game data from a web server.

# Public API
- Namespace: `BayatGames.SaveGameFree`
- Types:
  - **public class SaveGameWeb**
    - **Public Properties:**
      - `static string DefaultUsername` - Gets or sets the default username.
      - `static string DefaultPassword` - Gets or sets the default password.
      - `static string DefaultURL` - Gets or sets the default URL.
      - `static bool DefaultEncode` - Gets or sets the default encoding flag.
      - `static string DefaultEncodePassword` - Gets or sets the default encode password.
      - `static ISaveGameSerializer DefaultSerializer` - Gets or sets the default serializer.
      - `static ISaveGameEncoder DefaultEncoder` - Gets or sets the default encoder.
      - `static Encoding DefaultEncoding` - Gets or sets the default encoding.
      - `virtual string Username` - Gets or sets the username.
      - `virtual string Password` - Gets or sets the password.
      - `virtual string URL` - Gets or sets the URL.
      - `virtual bool Encode` - Gets or sets the encoding flag.
      - `virtual string EncodePassword` - Gets or sets the encode password.
      - `virtual ISaveGameSerializer Serializer` - Gets or sets the serializer.
      - `virtual ISaveGameEncoder Encoder` - Gets or sets the encoder.
      - `virtual Encoding Encoding` - Gets or sets the encoding.
      - `virtual UnityWebRequest Request` - Gets the current web request.
      - `virtual bool IsError` - Indicates if there was an error.
      - `virtual string Error` - Gets the error message.
    - **Public Methods:**
      - `SaveGameWeb()` - Initializes with default username.
      - `SaveGameWeb(string username)` - Initializes with specified username.
      - `SaveGameWeb(string username, string password)` - Initializes with specified username and password.
      - `SaveGameWeb(string username, string password, string url)` - Initializes with specified username, password, and URL.
      - `SaveGameWeb(string username, string password, string url, bool encode)` - Initializes with specified username, password, URL, and encoding flag.
      - `SaveGameWeb(string username, string password, string url, bool encode, string encodePassword)` - Initializes with specified parameters including encode password.
      - `SaveGameWeb(string username, string password, string url, bool encode, string encodePassword, ISaveGameSerializer serializer)` - Initializes with specified parameters including serializer.
      - `SaveGameWeb(string username, string password, string url, bool encode, string encodePassword, ISaveGameSerializer serializer, ISaveGameEncoder encoder)` - Initializes with specified parameters including encoder.
      - `SaveGameWeb(string username, string password, string url, bool encode, string encodePassword, ISaveGameSerializer serializer, ISaveGameEncoder encoder, Encoding encoding)` - Initializes with all parameters.
      - `IEnumerator Save<T>(string identifier, T obj)` - Saves the specified object.
      - `IEnumerator Download(string identifier)` - Downloads data for the specified identifier.
      - `T Load<T>(string identifier)` - Loads data for the specified identifier.
      - `T Load<T>(string identifier, T defaultValue)` - Loads data with a default value.
      - `IEnumerator Send(string identifier, string data, string action)` - Sends data to the server.

# Key Behavior & Side Effects
- `Save<T>` method serializes an object and sends it to the server.
- `Download` method retrieves data from the server.
- `Load<T>` methods deserialize data from the server response.
- `Send` method handles the web request and checks for errors.

# Constraints & Failure Modes
- Handles null or empty values for identifiers and data.
- Uses Unity's `UnityWebRequest` for network operations, which may fail due to network issues.
- Error handling is done through `IsError` and `Error` properties.

# Example
```csharp
SaveGameWeb saveGame = new SaveGameWeb("user", "pass", "http://www.example.com");
yield return saveGame.Save("gameData", myGameObject);
```

# Unknowns
- Specific behavior of the server-side implementation is not defined in this file.

