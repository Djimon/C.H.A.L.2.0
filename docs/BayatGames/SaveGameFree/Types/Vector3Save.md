# BayatGames.SaveGameFree.Types.Vector3Save

_Automatically generated/updated from `Assets/src/xTernal/SaveGameFree/Scripts/Types/Vector3Save.cs`._

```
Purpose
- Serializable struct Vector3Save representing a 3D vector (x, y, z) for use in SaveGameFree.
- Provides multiple constructors to initialize from scalars, Unity vectors (Vector2/Vector3/Vector4), and corresponding *Save variants.
- Exposes implicit conversions to and from Vector2, Vector3, Vector4 and their Save counterparts.

Public API
- Namespace: BayatGames.SaveGameFree.Types

- Type: public struct Vector3Save [Serializable]
  - Public fields
    - public float x
    - public float y
    - public float z
  - Constructors
    - public Vector3Save ( float x )
    - public Vector3Save ( float x, float y )
    - public Vector3Save ( float x, float y, float z )
    - public Vector3Save ( Vector2 vector )
    - public Vector3Save ( Vector3 vector )
    - public Vector3Save ( Vector4 vector )
    - public Vector3Save ( Vector2Save vector )
    - public Vector3Save ( Vector3Save vector )
    - public Vector3Save ( Vector4Save vector )
  - Implicit conversion operators
    - public static implicit operator Vector3Save ( Vector2 vector )
    - public static implicit operator Vector2 ( Vector3Save vector )
    - public static implicit operator Vector3Save ( Vector3 vector )
    - public static implicit operator Vector3 ( Vector3Save vector )
    - public static implicit operator Vector3Save ( Vector4 vector )
    - public static implicit operator Vector4 ( Vector3Save vector )
    - public static implicit operator Vector3Save ( Vector2Save vector )
    - public static implicit operator Vector2Save ( Vector3Save vector )
    - public static implicit operator Vector3Save ( Vector4Save vector )
    - public static implicit operator Vector4Save ( Vector3Save vector )

Key Behavior & Side Effects
- Constructors assign the provided inputs to x, y, z (no additional logic).
- Implicit conversions create new Vector3Save or Unity vector instances as part of type conversion.
- The type is [Serializable], enabling Unity/serializer usage; no runtime side effects during normalization or mutation.

Constraints & Failure Modes
- Struct type: value semantics; cannot be null.
- All fields are public; no encapsulation or validation performed in constructors.
- No threading or async behavior explicit in this file.
- No explicit error handling; conversions rely on underlying Unity types.

Example
```csharp
// Minimal usage example
Vector3Save s = new Vector3Save(1f, 2f, 3f);
Vector3 v = s;        // implicit Vector3 from Vector3Save
Vector2 sv = s;       // implicit Vector2 from Vector3Save (drops z)
Vector3Save t = v;    // implicit Vector3Save from Vector3
```

Unknowns
- Definitions of Vector2Save and Vector4Save (referenced types) are not provided in this file.
- How Vector3Save integrates with the broader SaveGameFree serialization pipelines beyond [Serializable].
- Any runtime constraints or performance considerations specific to the hosting environment.
```
