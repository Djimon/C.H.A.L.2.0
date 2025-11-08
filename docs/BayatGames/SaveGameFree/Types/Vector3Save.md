# BayatGames.SaveGameFree.Types.Vector3Save

_Automatically generated/updated from `Assets/src/xTernal/SaveGameFree/Scripts/Types/Vector3Save.cs`._

# Purpose
- Defines a structure for representing 3D vectors and points.

# Public API
- Namespace: `BayatGames.SaveGameFree.Types`
- Types
  - `struct Vector3Save`
    - Public fields/properties:
      - `float x`: X coordinate.
      - `float y`: Y coordinate.
      - `float z`: Z coordinate.
    - Public methods:
      - `Vector3Save(float x)`: Initializes with x, y and z set to 0.
      - `Vector3Save(float x, float y)`: Initializes with x, y, and z set to 0.
      - `Vector3Save(float x, float y, float z)`: Initializes with specified x, y, and z.
      - `Vector3Save(Vector2 vector)`: Initializes from a `Vector2`, setting z to 0.
      - `Vector3Save(Vector3 vector)`: Initializes from a `Vector3`.
      - `Vector3Save(Vector4 vector)`: Initializes from a `Vector4`.
      - `Vector3Save(Vector2Save vector)`: Initializes from a `Vector2Save`, setting z to 0.
      - `Vector3Save(Vector3Save vector)`: Initializes from another `Vector3Save`.
      - `Vector3Save(Vector4Save vector)`: Initializes from a `Vector4Save`.

# Key Behavior & Side Effects
- Implicit conversions are defined for:
  - `Vector2` to `Vector3Save`
  - `Vector3Save` to `Vector2`
  - `Vector3` to `Vector3Save`
  - `Vector3Save` to `Vector3`
  - `Vector4` to `Vector3Save`
  - `Vector3Save` to `Vector4`
  - `Vector2Save` to `Vector3Save`
  - `Vector3Save` to `Vector2Save`
  - `Vector4Save` to `Vector3Save`
  - `Vector3Save` to `Vector4Save`

# Constraints & Failure Modes
- No explicit guards or error handling are present in the code.
- Assumes that the input vectors are valid and do not contain null references.

# Example
```csharp
Vector3Save vector = new Vector3Save(1.0f, 2.0f, 3.0f);
Vector2 vector2 = vector; // Implicit conversion to Vector2
Vector4Save vector4Save = vector; // Implicit conversion to Vector4Save
```

# Unknowns
- The behavior of the `Vector2Save`, `Vector4Save`, and their conversions cannot be determined from this file.
