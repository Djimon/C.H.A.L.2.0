# BayatGames.SaveGameFree.Encoders.SaveGameSimpleEncoder

_Automatically generated/updated from `Assets/src/xTernal/SaveGameFree/Scripts/Encoders/SaveGameSimpleEncoder.cs`._

# Purpose
- Defines a simple encoder for saving game data with encryption and decryption capabilities.

# Public API
- Namespace: `BayatGames.SaveGameFree.Encoders`
- Types
  - `public class SaveGameSimpleEncoder : ISaveGameEncoder`
    - Public methods:
      - `public string Encode(string input, string password)`
        - Encodes the input string using the provided password.
      - `public string Decode(string input, string password)`
        - Decodes the input string using the provided password.

# Key Behavior & Side Effects
- `Encode` method:
  - Generates random salt and IV for encryption.
  - Uses AES encryption (RijndaelManaged) in CBC mode with PKCS7 padding.
  - Returns a Base64-encoded string of the concatenated salt, IV, and encrypted data.
- `Decode` method:
  - Extracts salt and IV from the input string.
  - Uses AES decryption to return the original string.

# Constraints & Failure Modes
- The encoder is not available for `UNITY_WSA` or `UNITY_WINRT` platforms; falls back to simple Base64 encoding.
- Assumes valid input for encoding and decoding; no explicit error handling for invalid formats.

# Example
```csharp
var encoder = new SaveGameSimpleEncoder();
string encoded = encoder.Encode("myData", "myPassword");
string decoded = encoder.Decode(encoded, "myPassword");
```

# Unknowns
- No information on the behavior of `ISaveGameEncoder` interface or its other implementations.

