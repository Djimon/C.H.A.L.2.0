# BayatGames.SaveGameFree.Serializers.ISaveGameSerializer

_Automatically generated/updated from `Assets/src/xTernal/SaveGameFree/Scripts/Serializers/ISaveGameSerializer.cs`._

# Purpose
- Defines an interface for Save Game Serializers.

# Public API
- Namespace: `BayatGames.SaveGameFree.Serializers`
- Types
  - **public interface ISaveGameSerializer**
    - Public methods:
      - `void Serialize<T>(T obj, Stream stream, Encoding encoding);`
      - `T Deserialize<T>(Stream stream, Encoding encoding);`

# Key Behavior & Side Effects
- `Serialize` method converts an object to a stream using specified encoding.
- `Deserialize` method reconstructs an object from a stream using specified encoding.

# Constraints & Failure Modes
- No explicit guards or error handling are defined in the interface.

# Example
```csharp
ISaveGameSerializer serializer = ...; // Implementation of ISaveGameSerializer
using (var stream = new MemoryStream())
{
    serializer.Serialize(myObject, stream, Encoding.UTF8);
    stream.Position = 0; // Reset stream position for reading
    var deserializedObject = serializer.Deserialize<MyObjectType>(stream, Encoding.UTF8);
}
```

# Unknowns
- Specific implementations of the `ISaveGameSerializer` interface are not provided in this file.
