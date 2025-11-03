# BayatGames.SaveGameFree.Types.Color32Save

_Automatically generated/updated from `Assets/src/xTernal/SaveGameFree/Scripts/Types/Color32Save.cs`._

Purpose
- Represent RGBA color in 32-bit format.
- Be serializable by Unity ([Serializable]).
- Provide implicit conversions between UnityEngine.Color32 and this struct.

Public API
- Namespace/Module: BayatGames.SaveGameFree.Types
- Types
  - public struct Color32Save [Serializable]
    - Public fields:
      - public byte r; // red channel
      - public byte g; // green channel
      - public byte b; // blue channel
      - public byte a; // alpha channel
    - Public constructors
      - public Color32Save ( Color32 color )
        - Initializes r, g, b, a from color.r, color.g, color.b, color.a
    - Public operators
      - public static implicit operator Color32Save ( Color32 color )
        - Returns new Color32Save ( color )
      - public static implicit operator Color32 ( Color32Save color )
        - Returns new Color32 ( color.r, color.g, color.b, color.a )

Key Behavior & Side Effects
- Construction from Color32 copies r/g/b/a into the struct fields.
- Implicit conversion Color32 -> Color32Save creates a new Color32Save.
- Implicit conversion Color32Save -> Color32 creates a new UnityEngine.Color32 with stored channels.
- No mutating methods; struct is a plain data holder with value-type semantics.

Constraints & Failure Modes
- Public fields with no validation; bytes store 0–255 per channel.
- Struct (value type); assignments/pass-by-value semantics apply.
- No threading, asynchronous behavior, or side effects beyond conversions.
- Serialization behavior relies on Unity's [Serializable] handling (not detailed here).

Example
- Minimal usage derived from the file:

```csharp
Color32 someColor = new Color32(128, 64, 32, 255);
Color32Save saved = someColor;      // implicit conversion to Color32Save
Color32 restored = saved;            // implicit conversion back to Color32
```

Unknowns
- How this type is serialized by the surrounding SaveGameFree framework (beyond [Serializable]).
- Any additional overloads or constructors not present in this file.
- Performance implications of repeated implicit conversions.
- Interaction with Unity version-specific serialization rules.

