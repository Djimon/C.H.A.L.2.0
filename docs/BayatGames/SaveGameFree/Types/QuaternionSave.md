# BayatGames.SaveGameFree.Types.QuaternionSave

_Automatically generated/updated from `Assets/src/xTernal/SaveGameFree/Scripts/Types/QuaternionSave.cs`._

# Purpose
- Defines a `QuaternionSave` struct for representing rotations using quaternion values.

# Public API
- Namespace: `BayatGames.SaveGameFree.Types`
- Types
  - **struct** `QuaternionSave`
    - Public fields/properties:
      - `float x`: X component of the quaternion.
      - `float y`: Y component of the quaternion.
      - `float z`: Z component of the quaternion.
      - `float w`: W component of the quaternion.
    - Public methods:
      - `QuaternionSave(float x)`: Initializes with x, sets y, z, w to 0.
      - `QuaternionSave(float x, float y)`: Initializes with x, y, sets z, w to 0.
      - `QuaternionSave(float x, float y, float z)`: Initializes with x, y, z, sets w to 0.
      - `QuaternionSave(float x, float y, float z, float w)`: Initializes with x, y, z, w.
      - `QuaternionSave(Quaternion quaternion)`: Initializes from a Unity `Quaternion`.

# Key Behavior & Side Effects
- Implicit conversion from `Quaternion` to `QuaternionSave` and vice versa.

# Constraints & Failure Modes
- No explicit guards or null handling present.
- Assumes valid quaternion values are provided for conversions.

# Example
```csharp
Quaternion unityQuaternion = new Quaternion(1, 0, 0, 0);
QuaternionSave saveQuaternion = unityQuaternion; // Implicit conversion to QuaternionSave
Quaternion convertedBack = saveQuaternion; // Implicit conversion back to Quaternion
```

# Unknowns
- None.

