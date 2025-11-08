# BayatGames.SaveGameFree.Encoders.SaveGameSimpleEncoder

_Automatically generated/updated from `Assets/src/xTernal/SaveGameFree/Scripts/Encoders/SaveGameSimpleEncoder.cs`._

# Purpose
- Provides functionality to encode and decode strings using a password-based encryption scheme.

# Public API
- Namespace: `BayatGames.SaveGameFree.Encoders`
- Types
  - public class `SaveGameSimpleEncoder` implements `ISaveGameEncoder`
    - Public methods:
      - `string Encode(string input, string password)` - Encrypts the input string using the specified password.
      - `string Decode(string input, string password)` - Decrypts the input string using the specified password.

# Key Behavior & Side Effects
- The `Encode` method generates random salt and IV for encryption, returning a Base64-encoded string.
- The `Decode` method extracts salt and IV from the input, decrypts the string, and returns the original plaintext.

# Constraints & Failure Modes
- The encoding and decoding methods handle exceptions related to invalid input formats.
- The implementation uses `RijndaelManaged` for encryption, which requires specific key sizes and modes.
- The code is conditionally compiled to provide a simpler encoding/decoding mechanism for certain platforms (UNITY_WSA or UNITY_WINRT).

# Example
```csharp
var encoder = new SaveGameSimpleEncoder();
string encoded = encoder.Encode("Hello, World!", "myPassword");
string decoded = encoder.Decode(encoded, "myPassword");
```

# Unknowns
- No information on the behavior of `ISaveGameEncoder` as it is not defined in the provided code.

