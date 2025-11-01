# BayatGames.SaveGameFree.Encoders.SaveGameSimpleEncoder

_Automatically generated/updated from `Assets/src/xTernal/SaveGameFree/Scripts/Encoders/SaveGameSimpleEncoder.cs`._

1) Purpose
- Defines a SaveGameSimpleEncoder class that provides password-based string encoding/decoding.
- Uses Rijndael (AES-compatible) CBC with PKCS7 padding to encrypt/decrypt data when not targeting UNITY_WSA/UNITY_WINRT.
- Falls back to a simple base64 round-trip when Unity WSA/WinRT symbols are defined.

2) Public API
- Namespace/module: BayatGames.SaveGameFree.Encoders
- Types
  - public class SaveGameSimpleEncoder : ISaveGameEncoder
    - Public methods
      - public string Encode(string input, string password)
        - Encrypts the input using a password; returns a Base64 string containing salt, IV, and ciphertext.
        - On non-UNITY_WSA/UNITY_WINRT builds: performs cryptographic encryption.
        - On UNITY_WSA/UNITY_WINRT builds: returns Base64(input).
      - public string Decode(string input, string password)
        - Decrypts the input using a password; expects a Base64 string containing salt, IV, and ciphertext.
        - On non-UNITY_WSA/UNITY_WINRT builds: performs cryptographic decryption.
        - On UNITY_WSA/UNITY_WINRT builds: returns Base64(input) decoded to UTF-8 string.

3) Key Behavior & Side Effects
- Encode (non-WSA/WinRT):
  - Generates 32-byte random salt and 32-byte IV via Generate256BitsOfRandomEntropy.
  - Derives a 256-bit key using Rfc2898DeriveBytes with 1000 iterations.
  - Configures RijndaelManaged:
    - BlockSize: 256
    - Mode: CBC
    - Padding: PKCS7
  - Encrypts UTF-8 bytes of input with the derived key and IV.
  - Produces output: salt || iv || ciphertext, then Base64-encodes and returns it.
- Decode (non-WSA/WinRT):
  - Base64-decodes input to get salt, IV, and ciphertext.
  - Derives the same 256-bit key using the provided password and extracted salt.
  - Decrypts the ciphertext using CBC with the extracted IV.
  - Returns the resulting UTF-8 string.
- Encoding/decoding paths are split by platform via preprocessor directives.
- Resource handling:
  - Uses using blocks for streams to ensure disposal.
  - Explicit Close calls inside using blocks (redundant but present).
- Salt/IV/Key sizes:
  - Salt: 32 bytes
  - IV: 32 bytes
  - Key: 32 bytes (256 bits)

4) Constraints & Failure Modes
- Platform guards:
  - Crypto path enabled when not UNITY_WSA or UNITY_WINRT; otherwise non-crypto Base64 path.
- Potential errors:
  - No explicit null checks for input or password; may throw ArgumentNullException or CryptographicException.
  - Malformed or tampered Base64 input may throw during decoding or decryption.
  - Incorrect password leads to decryption failures.
- Security notes:
  - 1000 derivation iterations; standard but not unusually high by modern recommendations.
  - Uses 256-bit block size; relies on standard CBC mode.
- Performance:
  - Random entropy generation uses RNGCryptoServiceProvider; deterministic performance depends on platform.
- Memory:
  - Concatenates salt, IV, and ciphertext into a byte array before Base64 encoding; increases memory usage by a few multiples of input size.

5) Example
```csharp
using BayatGames.SaveGameFree.Encoders;

var encoder = new SaveGameSimpleEncoder();
string password = "strong-password";

string original = "Sensitive save data";
string encrypted = encoder.Encode(original, password);
string decrypted = encoder.Decode(encrypted, password);

// decrypted should equal original
```

6) Unknowns
- Details of the ISaveGameEncoder interface contract beyond this file.
- Behavior on platforms other than those targeted by the #if conditions (beyond the provided fallback behavior).
- Any security considerations beyond the implemented algorithm (e.g., salt/IV reuse policies, nonce handling across sessions).
- Exact thread-safety guarantees of SaveGameSimpleEncoder instances.
