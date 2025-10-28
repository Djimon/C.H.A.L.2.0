# Assets/src/xTernal/SaveGameFree/Scripts/SaveGameWeb.cs

_Automatic generated/updated._

```markdown
## Purpose
- Defines the `SaveGameWeb` class for saving and loading game data from a web server.

## Public API
- Namespace: `BayatGames.SaveGameFree`
- Types:
  - `public class SaveGameWeb`
    - Public fields/properties:
      - `static string DefaultUsername`: Gets/sets the default username.
      - `static string DefaultPassword`: Gets/sets the default password.
      - `static string DefaultURL`: Gets/sets the default URL.
      - `static bool DefaultEncode`: Gets/sets whether to use default encoding.
      - `static string DefaultEncodePassword`: Gets/sets the default encoding password.
      - `static ISaveGameSerializer DefaultSerializer`: Gets/sets the default serializer.
      - `static ISaveGameEncoder DefaultEncoder`: Gets/sets the default encoder.
      - `static Encoding DefaultEncoding`: Gets/sets the default encoding.
      - `virtual string Username`: Gets/sets the username.
      - `virtual string Password`: Gets/sets the password.
      - `virtual string URL`: Gets/sets the URL.
      - `virtual bool Encode`: Gets/sets whether to encode.
      - `virtual string EncodePassword`: Gets/sets the encode password.
      - `virtual ISaveGameSerializer Serializer`: Gets/sets the serializer.
      - `virtual ISaveGameEncoder Encoder`: Gets/sets the encoder.
      - `virtual Encoding Encoding`: Gets/sets the encoding.
      - `virtual UnityWebRequest Request`: Gets the current web request.
      - `virtual bool IsError`: Indicates if there was an error.
      - `virtual string Error`: Gets the error message.
    - Public methods:
      - `IEnumerator Save<T>(string identifier, T obj)`: Saves the object to the web.
      - `IEnumerator Download(string identifier)`: Downloads data from the web.
      - `T Load<T>(string identifier)`: Loads data from the web.
      - `T Load<T>(string identifier, T defaultValue)`: Loads data with a default value.
      - `IEnumerator Send(string identifier, string data, string action)`: Sends data to the web.

## Key Behavior & Side Effects
- `Save<T>`: Serializes and sends data to the server; logs success or error.
- `Download`: Sends a request to load data; logs success or error.
- `Load<T>`: Retrieves and deserializes data from the server; returns default value if an error occurs.
- `Send`: Handles network requests and checks for errors based on Unity version.

## Constraints & Failure Modes
- Handles null or empty values for identifiers and data.
- Uses Unity's `UnityWebRequest` for network operations; error handling varies by Unity version.
- Memory streams are disposed after use in `Load<T>`.

## Example
```csharp
SaveGameWeb saveGame = new SaveGameWeb("username", "password", "http://www.example.com");
yield return saveGame.Save("gameData", myGameObject);
```

## Unknowns
- Specific behavior of the server-side implementation is not defined.
- The exact structure of the data being saved/loaded is not specified.
```
