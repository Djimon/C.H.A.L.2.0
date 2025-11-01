# BayatGames.SaveGameFree.Serializers.SaveGameBinarySerializer

_Automatically generated/updated from `Assets/src/xTernal/SaveGameFree/Scripts/Serializers/SaveGameBinarySerializer.cs`._

```text
1) Purpose
- Defines SaveGameBinarySerializer class in BayatGames.SaveGameFree.Serializers.
- Implements ISaveGameSerializer; provides generic binary serialization to/from a Stream using BinaryFormatter.
- Platform-gated: uses BinaryFormatter only when not targeting Windows Store/UWP; otherwise logs an error.

```

```text
2) Public API
- Namespace/module: BayatGames.SaveGameFree.Serializers

- Type: public class SaveGameBinarySerializer : ISaveGameSerializer
  - Public fields/properties: none
  - Public methods:
    - public void Serialize<T> ( T obj, Stream stream, Encoding encoding )
      - Serializes the specified object to the provided stream using BinaryFormatter.
      - Side effects: writes to stream; on exception logs via Debug.LogException; encoding parameter is unused.
    - public T Deserialize<T> ( Stream stream, Encoding encoding )
      - Deserializes an object of type T from the provided stream using BinaryFormatter.
      - Side effects: returns deserialized object; on exception logs via Debug.LogException; encoding parameter is unused; returns default(T) if deserialization fails.

```

```text
3) Key Behavior & Side Effects
- Compile-time behavior:
  - If not (UNITY_WSA) or not (UNITY_WINRT): use BinaryFormatter to serialize/deserialize.
  - Else: log error "SaveGameFree: The Binary Serialization isn't supported in Windows Store and UWP."
- Serialize flow (when active):
  - Create BinaryFormatter
  - formatter.Serialize(stream, obj)
  - Catch Exception -> Debug.LogException
- Deserialize flow (when active):
  - Create BinaryFormatter
  - result = (T)formatter.Deserialize(stream)
  - Catch Exception -> Debug.LogException
  - Return result (default(T) if failed)
- Encoding parameter is accepted but not used by this implementation.

```

```text
4) Constraints & Failure Modes
- Platform constraint:
  - On Windows Store/UWP builds, BinaryFormatter path is excluded; operation fails with a runtime error log.
- Null/invalid inputs:
  - No explicit null checks for stream or obj; potential NullReferenceException if stream is null or obj is used in serialization.
- Type requirements:
  - BinaryFormatter typically requires serializable types; runtime failures possible if obj/stream types are not serializable.
- Error handling:
  - Exceptions are caught and logged; methods do not rethrow.
- Performance/allocation:
  - Uses BinaryFormatter; no custom pooling or streaming optimizations evident.
- Encoding parameter:
  - Present for signature compatibility; not utilized by implementation.

```

```text
5) Example
```csharp
// Example usage
using System.IO;
using System.Text;

public class ExampleUsage
{
    public void Run()
    {
        var serializer = new BayatGames.SaveGameFree.Serializers.SaveGameBinarySerializer();
        var obj = new MySerializableType(); // ensure type is [Serializable] as needed by BinaryFormatter

        using (var ms = new MemoryStream())
        {
            serializer.Serialize<MySerializableType>(obj, ms, Encoding.UTF8);
            ms.Position = 0;
            var deserialized = serializer.Deserialize<MySerializableType>(ms, Encoding.UTF8);
        }
    }
}
```

```

```text
6) Unknowns
- Details of ISaveGameSerializer interface (beyond the two generic methods shown) are not present in this file.
- Whether there are additional platform-specific alternatives or fallbacks elsewhere in the project.
- Any project-wide serialization policies, e.g., expected [Serializable] requirements, are not specified here.
```
