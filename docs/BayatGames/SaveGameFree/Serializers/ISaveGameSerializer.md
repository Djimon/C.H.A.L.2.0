# BayatGames.SaveGameFree.Serializers.ISaveGameSerializer

_Automatically generated/updated from `Assets/src/xTernal/SaveGameFree/Scripts/Serializers/ISaveGameSerializer.cs`._

1) Purpose
- Defines a contract for Save Game serializers.
- Declares generic Serialize and Deserialize methods using Stream and Encoding.
- Located in BayatGames.SaveGameFree.Serializers namespace.

2) Public API
- Namespace/module: BayatGames.SaveGameFree.Serializers
- Types
  - public interface ISaveGameSerializer
    - public methods
      - void Serialize<T> ( T obj, Stream stream, Encoding encoding )
        - Serialize the specified object to stream with encoding.
      - T Deserialize<T> ( Stream stream, Encoding encoding )
        - Deserialize the specified object from stream using encoding.

3) Key Behavior & Side Effects
- No implementation or behavior provided in this file.
- Implementations define serialization format and stream handling.

4) Constraints & Failure Modes
- No constraints, null handling, or error handling defined in this file.
- Parameters are Stream and Encoding; specific expectations not stated.

5) Example
- Not included (no derivable concrete example from this interface alone).

6) Unknowns
- Serialization format (binary, JSON, XML, etc.) is unspecified.
- Null inputs, exceptions, threading behavior, and performance characteristics are unspecified.

