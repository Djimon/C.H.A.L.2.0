# BayatGames.SaveGameFree.Serializers.SaveGameBinarySerializer

_Automatically generated/updated from `Assets/src/xTernal/SaveGameFree/Scripts/Serializers/SaveGameBinarySerializer.cs`._

# Purpose
- Defines a binary serializer for saving and loading game data.

# Public API
- Namespace: `BayatGames.SaveGameFree.Serializers`
- Types
  - public class `SaveGameBinarySerializer` implements `ISaveGameSerializer`
    - Public methods:
      - `void Serialize<T>(T obj, Stream stream, Encoding encoding)`
      - `T Deserialize<T>(Stream stream, Encoding encoding)`

# Key Behavior & Side Effects
- `Serialize` method serializes an object to a stream using binary formatting and logs exceptions.
- `Deserialize` method deserializes an object from a stream using binary formatting and logs exceptions.
- Both methods log an error if called on Windows Store or UWP platforms, as binary serialization is not supported.

# Constraints & Failure Modes
- Serialization and deserialization are wrapped in try-catch blocks to handle exceptions.
- On Windows Store and UWP, serialization and deserialization are not supported, leading to error logs.

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
