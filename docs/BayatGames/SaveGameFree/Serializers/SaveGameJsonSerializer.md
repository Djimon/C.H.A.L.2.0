# BayatGames.SaveGameFree.Serializers.SaveGameJsonSerializer

_Automatically generated/updated from `Assets/src/xTernal/SaveGameFree/Scripts/Serializers/SaveGameJsonSerializer.cs`._

# Purpose
- Defines a JSON serializer for saving and loading game data.

# Public API
- Namespace: `BayatGames.SaveGameFree.Serializers`
- Types
  - public class `SaveGameJsonSerializer` implements `ISaveGameSerializer`
    - Public methods:
      - `void Serialize<T>(T obj, Stream stream, Encoding encoding)`
        - Serializes the specified object to the provided stream using the specified encoding.
      - `T Deserialize<T>(Stream stream, Encoding encoding)`
        - Deserializes an object from the provided stream using the specified encoding and returns it.

# Key Behavior & Side Effects
- `Serialize` method:
  - Uses `fsSerializer` for serialization unless in a Unity WSA/WinRT environment, where it falls back to `JsonUtility`.
  - Catches and logs exceptions during serialization.
- `Deserialize` method:
  - Uses `fsSerializer` for deserialization unless in a Unity WSA/WinRT environment, where it falls back to `JsonUtility`.
  - Catches and logs exceptions during deserialization.
  - Returns default value if deserialization fails.

# Constraints & Failure Modes
- Handles exceptions during serialization and deserialization by logging them.
- In WSA/WinRT environments, uses `JsonUtility` for serialization/deserialization, which may have different limitations compared to `fsSerializer`.

# Example
```csharp
var serializer = new SaveGameJsonSerializer();
using (var stream = new MemoryStream())
{
    serializer.Serialize(myObject, stream, Encoding.UTF8);
    stream.Position = 0; // Reset stream position for reading
    var deserializedObject = serializer.Deserialize<MyObjectType>(stream, Encoding.UTF8);
}
```

# Unknowns
- None.

