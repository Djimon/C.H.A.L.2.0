# BayatGames.SaveGameFree.Encoders.ISaveGameEncoder

_Automatically generated/updated from `Assets/src/xTernal/SaveGameFree/Scripts/Encoders/ISaveGameEncoder.cs`._

# Purpose
- Defines an interface for Save Game Encoders.

# Public API
- Namespace: `BayatGames.SaveGameFree.Encoders`
- Types
  - `public interface ISaveGameEncoder`
    - Public methods:
      - `string Encode(string input, string password);`
      - `string Decode(string input, string password);`

# Key Behavior & Side Effects
- `Encode` method: Encodes the input string using the provided password.
- `Decode` method: Decodes the input string using the provided password.

# Constraints & Failure Modes
- No explicit guards or error handling noted in the interface.

# Example
```csharp
public class MyEncoder : ISaveGameEncoder
{
    public string Encode(string input, string password)
    {
        // Implementation here
    }

    public string Decode(string input, string password)
    {
        // Implementation here
    }
}
```

# Unknowns
- Implementation details of the encoding and decoding processes are not provided.

