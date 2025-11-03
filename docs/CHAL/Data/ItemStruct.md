# CHAL.Data.ItemStruct

_Automatically generated/updated from `Assets/src/Data/Structs/ItemStruct.cs`._

1) Purpose
- Defines public readonly struct ItemKey in namespace CHAL.Data.
- Represents a key composed of Category and Id parts.
- Provides parsing via TryParse(string, out ItemKey) for "category:id" strings and a ToString() representation "category:id".

2) Public API
- Namespace: CHAL.Data
- Types
  - public readonly struct ItemKey
    - Public fields
      - public readonly string Category; // Category part of the key
      - public readonly string Id;       // Id part of the key
    - Public constructors
      - public ItemKey(string category, string id)
    - Public static methods
      - public static bool TryParse(string s, out ItemKey key)
    - Public instance methods
      - public override string ToString()

3) Key Behavior & Side Effects
- Immutability: ItemKey is a readonly struct; fields are readonly.
- TryParse behavior:
  - key = default on entry; returns false if s is null, empty, or whitespace.
  - Splits s by ':'; returns false unless exactly two parts.
  - On success, key = new ItemKey(parts[0], parts[1]); returns true.
- ToString behavior: returns $"{Category}:{Id}".
- No trimming of parts; exact parts are used as provided.
- If TryParse fails, the out key remains default (likely with null Category/Id).

4) Constraints & Failure Modes
- TryParse guard conditions:
  - string.IsNullOrWhiteSpace(s) => false.
  - s.Split(':') must yield exactly 2 parts; otherwise false.
- No validation on content of Category/Id beyond the parsing rule.
- If input is ":", results in key with empty Category and Id (allowed by code).
- ToString does not throw for normal values; null fields become empty in the string interpolation.

5) Example
```csharp
// Example usage
if (CHAL.Data.ItemKey.TryParse("tools:hammer", out var key)) {
    // key.Category == "tools"; key.Id == "hammer"
    Console.WriteLine(key.ToString()); // prints "tools:hammer"
}
```

6) Unknowns
- No additional constructors, operators, or methods are defined in this file.
- Interaction with other code (serialization, equality, hashing) is not specified here.
- Behavior when Category or Id contain ':' or whitespace beyond what TryParse allows is not further constrained.

