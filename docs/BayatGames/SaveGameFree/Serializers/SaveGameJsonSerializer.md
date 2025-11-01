# BayatGames.SaveGameFree.Serializers.SaveGameJsonSerializer

_Automatically generated/updated from `Assets/src/xTernal/SaveGameFree/Scripts/Serializers/SaveGameJsonSerializer.cs`._

```csharp
// documentation for: Assets/src/xTernal/SaveGameFree/Scripts/Serializers/SaveGameJsonSerializer.cs
```

1) Purpose
- Defines SaveGameJsonSerializer class implementing ISaveGameSerializer.
- Provides JSON-based serialization/deserialization for generic types.
- Uses FullSerializer paths on most platforms; falls back to JsonUtility on UNITY_WSA/UNITY_WINRT; logs errors via Debug.LogException.

2) Public API
- Namespace/module
  - BayatGames.SaveGameFree.Serializers
- Types
  - public class SaveGameJsonSerializer : ISaveGameSerializer
    - Public methods
      - public void Serialize<T> ( T obj, Stream stream, Encoding encoding )
        - Serializes obj to the provided stream using the specified encoding.
        - Non-UNITY_WSA/UNITY_WINRT path: uses fsSerializer to serialize to fsData, writes compressed JSON via fsJsonPrinter, and disposes the writer; errors are logged.
        - UNITY_WSA/UNITY_WINRT path: uses StreamWriter and JsonUtility.ToJson; writes to stream and disposes writer.
      - public T Deserialize<T> ( Stream stream, Encoding encoding )
        - Deserializes an object of type T from the provided stream using the specified encoding.
        - Non-UNITY_WSA/UNITY_WINRT path: reads all text, parses with fsJsonParser, deserializes via fsSerializer, assigns default(T) if result is null; logs on exception; disposes reader.
        - UNITY_WSA/UNITY_WINRT path: uses StreamReader and JsonUtility.FromJson<T> to obtain the result; disposes reader.

3) Key Behavior & Side Effects
- Serialization flows
  - Non-WSA/WinRT: 
    - Creates fsSerializer and fsData; serializer.TrySerialize(obj, out data); writes fsJsonPrinter.CompressedJson(data) to the stream via StreamWriter; disposes writer.
    - Exceptions are caught and logged with Debug.LogException.
  - WSA/WinRT:
    - Creates StreamWriter; writes JsonUtility.ToJson(obj) to stream; disposes writer.
- Deserialization flows
  - Non-WSA/WinRT:
    - Creates fsSerializer and StreamReader; reads full text; parses with fsJsonParser.Parse; serializer.TryDeserialize(data, ref result); if result is null, resets to default(T); disposes reader.
    - Exceptions are caught and logged with Debug.LogException.
  - WSA/WinRT:
    - Creates StreamReader; reads full text; result = JsonUtility.FromJson<T>(text); disposes reader.
- Resource management
  - StreamWriter/StreamReader are disposed in both code paths.
- Logging
  - Exceptions are logged via Debug.LogException; no exceptions are propagated.

4) Constraints & Failure Modes
- Error handling
  - Serialize: any exception is swallowed (logged); no exception is thrown.
  - Deserialize: exceptions are swallowed (logged); returns default(T) on failure; if parsed result is null, returns default(T).
- Platform behavior
  - Behavior depends on UNITY_WSA and UNITY_WINRT define symbols due to conditional compilation.
- Encoding
  - Uses the provided Encoding for both StreamWriter/StreamReader.
- Type compatibility
  - Serialization/deserialization relies on FullSerializer expectations (or JsonUtility for the WSA/WinRT path); unsupported types may fail to serialize/deserialize.

5) Example
```csharp
using System.IO;
using System.Text;
using BayatGames.SaveGameFree.Serializers;

public class ExampleUsage
{
    public void SaveObject<T>(T obj, string path)
    {
        var serializer = new SaveGameJsonSerializer();
        using (var stream = new FileStream(path, FileMode.Create, FileAccess.Write))
        {
            serializer.Serialize(obj, stream, Encoding.UTF8);
        }
    }

    public T LoadObject<T>(string path)
    {
        var serializer = new SaveGameJsonSerializer();
        using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
        {
            return serializer.Deserialize<T>(stream, Encoding.UTF8);
        }
    }
}
```

6) Unknowns
- Details of ISaveGameSerializer interface (beyond method signatures shown here).
- Exact behavior/limitations of FullSerializer in this context (e.g., handling of complex or non-serializable types).
- Any external side effects beyond Debug.LogException logging.
- Specific runtime implications on platforms not explicitly covered by the conditional compilation (beyond the UNITY_WSA/UNITY_WINRT branches).
