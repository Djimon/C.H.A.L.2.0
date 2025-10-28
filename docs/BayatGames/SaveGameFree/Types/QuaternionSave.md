# BayatGames.SaveGameFree.Types.QuaternionSave

_Automatically generated/updated from `Assets/src/xTernal/SaveGameFree/Scripts/Types/QuaternionSave.cs`._

# Purpose
- Defines a `QuaternionSave` struct for representing rotations using quaternions.

# Public API
- Namespace: `BayatGames.SaveGameFree.Types`
- Types
  - `struct QuaternionSave`
    - Public fields/properties:
      - `float x`: X component of the quaternion.
      - `float y`: Y component of the quaternion.
      - `float z`: Z component of the quaternion.
      - `float w`: W component of the quaternion.
    - Public methods:
      - `QuaternionSave(float x)`: Initializes with x; y, z, w set to 0.
      - `QuaternionSave(float x, float y)`: Initializes with x, y; z, w set to 0.
      - `QuaternionSave(float x, float y, float z)`: Initializes with x, y, z; w set to 0.
      - `QuaternionSave(float x, float y, float z, float w)`: Initializes with x, y, z, w.
      - `QuaternionSave(Quaternion quaternion)`: Initializes from a Unity `Quaternion`.
      - `static implicit operator QuaternionSave(Quaternion quaternion)`: Converts Unity `Quaternion` to `QuaternionSave`.
      - `static implicit operator Quaternion(QuaternionSave quaternion)`: Converts `QuaternionSave` to Unity `Quaternion`.

# Key Behavior & Side Effects
- Implicit conversions allow seamless use between `Quaternion` and `QuaternionSave`.

# Constraints & Failure Modes
- No explicit guards or error handling present.
- Assumes valid float values for quaternion components.

# Example
```csharp
QuaternionSave qSave = new QuaternionSave(1.0f, 0.0f, 0.0f, 0.0f);
Quaternion unityQuaternion = qSave; // Implicit conversion to Unity Quaternion
```

# Unknowns
- No information on performance characteristics or threading considerations.

