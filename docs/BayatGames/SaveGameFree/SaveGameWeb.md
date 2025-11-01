# BayatGames.SaveGameFree.SaveGameWeb

_Automatically generated/updated from `Assets/src/xTernal/SaveGameFree/Scripts/SaveGameWeb.cs`._

```text
1) Purpose
- Defines SaveGameWeb class to save/load data to/from a web URL using HTTP POST.
- Provides configurable defaults (credentials, URL, encoding, serializer, encoder).
- Exposes instance properties to customize per-request behavior and supports Save/Load workflows via coroutines.

```

```csharp
2) Public API
- Namespace/module
  - BayatGames.SaveGameFree

- Types
  - public class SaveGameWeb
    - Static members
      - public static string DefaultUsername
        - getter/setter for default username
      - public static string DefaultPassword
        - getter/setter for default password
      - public static string DefaultURL
        - getter/setter for default URL
      - public static bool DefaultEncode
        - getter/setter for default encode flag
      - public static string DefaultEncodePassword
        - getter/setter for default encode password
      - public static ISaveGameSerializer DefaultSerializer
        - getter/setter for default serializer; lazily initializes to SaveGameJsonSerializer
      - public static ISaveGameEncoder DefaultEncoder
        - getter/setter for default encoder; lazily initializes to SaveGameSimpleEncoder
      - public static Encoding DefaultEncoding
        - getter/setter for default text encoding; lazily initializes to UTF8

    - Instance members
      - protected string m_Username
      - protected string m_Password
      - protected string m_URL
      - protected bool m_Encode
      - protected string m_EncodePassword
      - protected ISaveGameSerializer m_Serializer
      - protected ISaveGameEncoder m_Encoder
      - protected Encoding m_Encoding
      - protected UnityWebRequest m_Request
      - protected bool m_IsError
      - protected string m_Error

      - public virtual string Username { get; set; }
        - Username used for authentication in requests

      - public virtual string Password { get; set; }
        - Password used for authentication in requests

      - public virtual string URL { get; set; }
        - URL to post to

      - public virtual bool Encode { get; set; }
        - If true, data is encoded before sending

      - public virtual string EncodePassword { get; set; }
        - Password used for encoding/decoding data

      - public virtual ISaveGameSerializer Serializer { get; set; }
        - Serializer used for (de)serialization; lazily initializes to SaveGameJsonSerializer

      - public virtual ISaveGameEncoder Encoder { get; set; }
        - Encoder used for (de)coding data; lazily initializes to SaveGameSimpleEncoder

      - public virtual Encoding Encoding { get; set; }
        - Text encoding for (de)serialization; lazily initializes to UTF8

      - public virtual UnityWebRequest Request { get; }
        - Access to the last UnityWebRequest

      - public virtual bool IsError { get; }
        - Indicates if the last operation produced an error

      - public virtual string Error { get; }
        - Error message from the last operation

      - Constructors (overload chain)
        - public SaveGameWeb() : this(DefaultUsername)
        - public SaveGameWeb(string username)
        - public SaveGameWeb(string username, string password)
        - public SaveGameWeb(string username, string password, string url)
        - public SaveGameWeb(string username, string password, string url, bool encode)
        - public SaveGameWeb(string username, string password, string url, bool encode, string encodePassword)
        - public SaveGameWeb(string username, string password, string url, bool encode, string encodePassword, ISaveGameSerializer serializer)
        - public SaveGameWeb(string username, string password, string url, bool encode, string encodePassword, ISaveGameSerializer serializer, ISaveGameEncoder encoder)
        - public SaveGameWeb(string username, string password, string url, bool encode, string encodePassword, ISaveGameSerializer serializer, ISaveGameEncoder encoder, Encoding encoding)
        - Each constructor assigns the corresponding protected fields (username, password, url, encode, encodePassword, serializer, encoder, encoding)

      - public virtual IEnumerator Save<T>(string identifier, T obj)
        - Serialize obj to memory via Serializer using Encoding
        - Optionally encode via Encoder using EncodePassword
        - yield return Send(identifier, data, "save")
        - Logs error or success

      - public virtual IEnumerator Download(string identifier)
        - yield return Send(identifier, null, "load")
        - Logs error or success

      - public virtual T Load<T>(string identifier)
        - return Load<T>(identifier, default(T))

      - public virtual T Load<T>(string identifier, T defaultValue)
        - If not error and response text present
        - Decode if Encode is true
        - Deserialize via Serializer from a MemoryStream using Encoding
        - Return result or defaultValue on error

      - public virtual IEnumerator Send(string identifier, string data, string action)
        - Build form with fields: identifier, action, username
        - Optionally include data and password
        - Post to URL via UnityWebRequest.Post
        - Await SendWebRequest (Unity version dependent)
        - Set IsError/Error based on UnityWebRequest status
        - If response text starts with "Error", mark as error

```

```csharp
3) Key Behavior & Side Effects
- Default values are centralized in static fields; instance values can override per-call behavior.
- Save<T> serializes an object to a memory stream using the configured Serializer and Encoding.
- If Encode is true, Save<T> data is encoded with EncodePassword before sending.
- Send builds an HTTP POST form with identifier, action, username, and optional data/password; stores the last UnityWebRequest in Request.
- After a request, IsError/Error reflect success or failure; non-empty response text starting with "Error" is treated as an error.
- Load<T> uses the last response (downloadHandler.text) as the source of data to deserialize; if decoding is needed, Decode is applied first.
- Lazy initialization: Serializer, Encoder, Encoding default instances are created if not set by the user.

```

```csharp
4) Constraints & Failure Modes
- Unity version branches in Send:
  - UNITY_2019_1_OR_NEWER: uses m_Request.result
  - UNITY_2017_1_OR_NEWER: uses isNetworkError or isHttpError
  - Older: uses isError
- NULL/empty handling:
  - If data is null/empty, it is not added to form fields.
  - Password is added only if not null/empty.
  - Encoding/Serializer/Encoder default lazily if not provided.
- Error handling:
  - If download text starts with "Error", treated as error.
  - Network/HTTP errors surfaced via m_Error.
- Performance/memory:
  - MemoryStream used for (de)serialization; disposed after use in Load.
  - Data is kept in memory between Save/Load calls; no persistent storage here.

```

```csharp
5) Example
- Minimal usage in a Unity coroutine:

```csharp
using System.Collections;
using UnityEngine;
using BayatGames.SaveGameFree;

public class SaveExample : MonoBehaviour
{
    IEnumerator Start()
    {
        var saver = new SaveGameWeb(); // uses defaults or configured values
        var obj = new { level = 5, score = 12345 };

        // Save example
        yield return saver.Save("player1", obj);
        if (saver.IsError)
        {
            Debug.LogError("Save failed: " + saver.Error);
        }

        // Load example
        var loaded = saver.Load<MyData>("player1", new MyData());
        // Process loaded data...
    }

    // Example type matching the deserialized shape
    private class MyData { public int level; public int score; }
}
```

```

```text
6) Unknowns
- Server-side API behavior and validation are outside this file.
- Network reliability, retries, and timeouts are not implemented here.
- Exact formats of serialized data (beyond using Serializer/Encoder) and how the server interprets fields are not defined in this file.
- Threading guarantees beyond Unity coroutines are not specified.
- Any side effects on external systems (e.g., authentication server) are not detailed.
