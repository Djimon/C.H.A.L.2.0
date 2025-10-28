# BayatGames.SaveGameFree.Serializers.SaveGameBinarySerializer

_Automatically generated/updated from `Assets/src/xTernal/SaveGameFree/Scripts/Serializers/SaveGameBinarySerializer.cs`._

# Purpose
- Defines a binary serializer for saving and loading game data.

# Public API
- Namespace: BayatGames.SaveGameFree.Serializers
- Types
  - public class SaveGameBinarySerializer : ISaveGameSerializer
    - Public methods:
      - void Serialize<T>(T obj, Stream stream, Encoding encoding)
        - Serializes the specified object to the provided stream with the given encoding.
      - T Deserialize<T>(Stream stream, Encoding encoding)
        - Deserializes an object from the provided stream using the given encoding; returns default(T) if unsuccessful.

# Key Behavior & Side Effects
- Serialize method logs exceptions if serialization fails.
- Deserialize method logs exceptions if deserialization fails and returns default value.

# Constraints & Failure Modes
- Binary serialization is not supported on Windows Store and UWP platforms; logs an error in such cases.
- Both methods handle exceptions and log them using Debug.LogException.

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

