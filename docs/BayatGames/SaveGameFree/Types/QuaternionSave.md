# BayatGames.SaveGameFree.Types.QuaternionSave

_Automatically generated/updated from `Assets/src/xTernal/SaveGameFree/Scripts/Types/QuaternionSave.cs`._

```txt
Purpose
- Defines a serializable value type QuaternionSave that stores quaternion components x, y, z, w.
- Provides constructors to initialize with 1–4 components, defaulting missing components to 0.
- Provides implicit conversions to/from UnityEngine.Quaternion for easy usage.

Public API
- Namespace/module
  - BayatGames.SaveGameFree.Types

- Types
  - public struct QuaternionSave
    - public float x
      - Quaternion component x
    - public float y
      - Quaternion component y
    - public float z
      - Quaternion component z
    - public float w
      - Quaternion component w

    - public QuaternionSave ( float x )
      - Initializes x; y/z/w = 0
    - public QuaternionSave ( float x, float y )
      - Initializes x, y; z/w = 0
    - public QuaternionSave ( float x, float y, float z )
      - Initializes x, y, z; w = 0
    - public QuaternionSave ( float x, float y, float z, float w )
      - Initializes all components
    - public QuaternionSave ( Quaternion quaternion )
      - Initializes from UnityEngine.Quaternion components

    - public static implicit operator QuaternionSave ( Quaternion quaternion )
      - Converts Quaternion to QuaternionSave

    - public static implicit operator Quaternion ( QuaternionSave quaternion )
      - Converts QuaternionSave to Quaternion

Key Behavior & Side Effects
- Constructors assign provided component values; unspecified components are set to 0.
- Implicit conversions create new instances without mutating input.
- Serialization is enabled via [Serializable] attribute, enabling Unity serialization of the struct.
- Performs direct field-to-field mapping between QuaternionSave and UnityEngine.Quaternion.

Constraints & Failure Modes
- Public fields are mutable; changes affect the stored representation.
- No null handling (Quaternion is a value type; QuaternionSave is a struct).
- Implicit conversions rely on UnityEngine types; requires UnityEngine reference.
- No thread-safety or async specifics declared.

Example
```csharp
using UnityEngine;
using BayatGames.SaveGameFree.Types;

public class QuaternionSaveExample
{
    void Demo()
    {
        Quaternion q = new Quaternion(0f, 0.7071f, 0f, 0.7071f);

        // Implicit conversion from Quaternion to QuaternionSave
        QuaternionSave qs = q;

        // Implicit conversion back to Quaternion
        Quaternion qRestored = qs;
    }
}
```

Unknowns
- How this struct is used by the broader SaveGameFree framework (serialization/deserialization flow) is not defined in this file.
- Performance characteristics and memory implications beyond the 4 floats are not specified.
- Behavior with non-normalized quaternions or NaN/Inf values is not described here.
```
