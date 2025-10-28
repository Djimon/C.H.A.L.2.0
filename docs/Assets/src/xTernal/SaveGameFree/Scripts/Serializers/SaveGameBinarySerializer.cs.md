# Assets/src/xTernal/SaveGameFree/Scripts/Serializers/SaveGameBinarySerializer.cs

_Automatic generated/updated._

```markdown
# Purpose
- Defines a binary serializer for saving and loading game data.

# Public API
- Namespace: `BayatGames.SaveGameFree.Serializers`
- Types
  - `public class SaveGameBinarySerializer : ISaveGameSerializer`
    - Public methods:
      - `public void Serialize<T>(T obj, Stream stream, Encoding encoding)`
        - Serializes the specified object to the provided stream with the specified encoding.
      - `public T Deserialize<T>(Stream stream, Encoding encoding)`
        - Deserializes an object from the provided stream using the specified encoding; returns default(T) if unsuccessful.

# Key Behavior & Side Effects
- `Serialize` method:
  - Uses `BinaryFormatter` to serialize the object.
  - Catches exceptions and logs them using `Debug.LogException`.
  - Logs an error if called on Windows Store or UWP platforms.
- `Deserialize` method:
  - Uses `BinaryFormatter` to deserialize the object.
  - Catches exceptions and logs them using `Debug.LogException`.
  - Logs an error if called on Windows Store or UWP platforms.
  - Returns default value of type T if deserialization fails.

# Constraints & Failure Modes
- Serialization is not supported on Windows Store and UWP platforms, leading to error logging.
- Exception handling is implemented for both serialization and deserialization processes.

# Example
```csharp
var serializer = new SaveGameBinarySerializer();
using (var stream = new MemoryStream())
{
    serializer.Serialize(myObject, stream, Encoding.UTF8);
    stream.Position = 0; // Reset stream position for reading
    var deserializedObject = serializer.Deserialize<MyType>(stream, Encoding.UTF8);
}
```

# Unknowns
- None.
```
