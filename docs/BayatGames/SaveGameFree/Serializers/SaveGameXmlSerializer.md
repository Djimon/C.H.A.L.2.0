# BayatGames.SaveGameFree.Serializers.SaveGameXmlSerializer

_Automatically generated/updated from `Assets/src/xTernal/SaveGameFree/Scripts/Serializers/SaveGameXmlSerializer.cs`._

# Purpose
- Defines a serializer for saving and loading game data in XML format.

# Public API
- Namespace: `BayatGames.SaveGameFree.Serializers`
- Types
  - `public class SaveGameXmlSerializer : ISaveGameSerializer`
    - Public methods:
      - `void Serialize<T>(T obj, Stream stream, Encoding encoding)`
        - Serializes the specified object to the provided stream using the specified encoding.
      - `T Deserialize<T>(Stream stream, Encoding encoding)`
        - Deserializes an object from the provided stream using the specified encoding and returns it.

# Key Behavior & Side Effects
- Both `Serialize` and `Deserialize` methods catch exceptions and log them using `Debug.LogException`.

# Constraints & Failure Modes
- The `Serialize` method requires a valid `Stream` and `Encoding`.
- The `Deserialize` method returns the default value of type `T` if an exception occurs during deserialization.
