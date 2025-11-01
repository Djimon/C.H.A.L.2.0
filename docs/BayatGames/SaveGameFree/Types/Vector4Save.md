# BayatGames.SaveGameFree.Types.Vector4Save

_Automatically generated/updated from `Assets/src/xTernal/SaveGameFree/Scripts/Types/Vector4Save.cs`._

```text
1) Purpose
- Serializable struct representing a four-dimensional vector with fields x, y, z, w.
- Provides multiple constructors to initialize from scalar values or from Vector2/Vector3/Vector4 and their Save counterparts.
- Defines implicit conversions to/from UnityEngine vectors (Vector2, Vector3, Vector4) and related Save types (Vector2Save, Vector3Save, Vector4Save).

2) Public API
- Namespace/module
  - BayatGames.SaveGameFree.Types

- Types
  - public struct Vector4Save
    - Public fields
      - public float x
        - X component
      - public float y
        - Y component
      - public float z
        - Z component
      - public float w
        - W component

    - Constructors
      - public Vector4Save ( float x )
      - public Vector4Save ( float x, float y )
      - public Vector4Save ( float x, float y, float z )
      - public Vector4Save ( float x, float y, float z, float w )
      - public Vector4Save ( Vector2 vector )
      - public Vector4Save ( Vector3 vector )
      - public Vector4Save ( Vector4 vector )
      - public Vector4Save ( Vector2Save vector )
      - public Vector4Save ( Vector3Save vector )
      - public Vector4Save ( Vector4Save vector )

    - Implicit operators
      - public static implicit operator Vector4Save ( Vector2 vector )
      - public static implicit operator Vector2 ( Vector4Save vector )
      - public static implicit operator Vector4Save ( Vector3 vector )
      - public static implicit operator Vector3 ( Vector4Save vector )
      - public static implicit operator Vector4Save ( Vector4 vector )
      - public static implicit operator Vector4 ( Vector4Save vector )
      - public static implicit operator Vector4Save ( Vector2Save vector )
      - public static implicit operator Vector2Save ( Vector4Save vector )
      - public static implicit operator Vector4Save ( Vector3Save vector )
      - public static implicit operator Vector3Save ( Vector4Save vector )

3) Key Behavior & Side Effects
- Field assignments occur only in constructors (no external I/O or side effects).
- Conversions create new instances; round-trip conversions between Vector2/Vector3/Vector4 and Vector4Save are supported through implicit operators.
- Default parameterless construction (via struct) yields zeros for all components.

4) Constraints & Failure Modes
- Public fields; no nullability concerns.
- Struct type (value semantics); copies result in independent instances.
- Uses UnityEngine types (Vector2, Vector3, Vector4) and related Save types (Vector2Save, Vector3Save, Vector4Save) defined elsewhere.
- [Serializable] indicates Unity/serializer compatibility.

5) Example
```csharp
using BayatGames.SaveGameFree.Types;
using UnityEngine;

public class Example
{
    void Demo()
    {
        // Construct from scalars
        Vector4Save v4s = new Vector4Save(1f, 2f, 3f, 4f);

        // Convert to Unity types
        Vector4 v4 = v4s;      // implicit
        Vector3 v3 = v4s;      // implicit (drops w)
        Vector2 v2 = v4s;      // implicit (drops z and w)

        // Construct from Unity vectors
        Vector3 unityVec = new Vector3(5f, 6f, 7f);
        Vector4Save fromVec3 = new Vector4Save(unityVec);

        // Convert back to Save types
        Vector3Save vs3 = (Vector3Save)fromVec3;
        Vector4Save toSave4 = (Vector4Save)unityVec;
    }
}
```

6) Unknowns
- Definitions of Vector2Save, Vector3Save, and Vector2/Vector3/Vector4 are not shown in this file.
- No other methods or behavior beyond constructors and implicit conversions are defined here.
