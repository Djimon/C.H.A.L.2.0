# BayatGames.SaveGameFree.Serializers.SaveGameXmlSerializer

_Automatically generated/updated from `Assets/src/xTernal/SaveGameFree/Scripts/Serializers/SaveGameXmlSerializer.cs`._

# Purpose
- Defines a serializer for saving and loading game data in XML format.

# Public API
- Namespace: BayatGames.SaveGameFree.Serializers
- Types
  - public class SaveGameXmlSerializer : ISaveGameSerializer
    - Public methods:
      - void Serialize<T>(T obj, Stream stream, Encoding encoding)
        - Serializes the specified object to the provided stream using the specified encoding.
      - T Deserialize<T>(Stream stream, Encoding encoding)
        - Deserializes an object from the provided stream using the specified encoding; returns default(T) if an error occurs.

# Key Behavior & Side Effects
- Serialize method logs exceptions to the Unity console if serialization fails.
- Deserialize method logs exceptions to the Unity console if deserialization fails.

# Constraints & Failure Modes
- Handles exceptions during serialization and deserialization by logging them.
- Returns default value for type T if deserialization fails.

# Unknowns
- None.

