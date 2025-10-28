# BayatGames.SaveGameFree.Types.Vector4Save

_Automatically generated/updated from `Assets/src/xTernal/SaveGameFree/Scripts/Types/Vector4Save.cs`._

# Purpose
- Defines a structure for representing four-dimensional vectors (`Vector4Save`).

# Public API
- Namespace: `BayatGames.SaveGameFree.Types`
- Types
  - `struct Vector4Save`
    - Public fields/properties:
      - `float x`: X component of the vector.
      - `float y`: Y component of the vector.
      - `float z`: Z component of the vector.
      - `float w`: W component of the vector.
    - Public methods:
      - `Vector4Save(float x)`: Initializes with x; y, z, w set to 0.
      - `Vector4Save(float x, float y)`: Initializes with x, y; z, w set to 0.
      - `Vector4Save(float x, float y, float z)`: Initializes with x, y, z; w set to 0.
      - `Vector4Save(float x, float y, float z, float w)`: Initializes with x, y, z, w.
      - `Vector4Save(Vector2 vector)`: Initializes from a `Vector2`.
      - `Vector4Save(Vector3 vector)`: Initializes from a `Vector3`.
      - `Vector4Save(Vector4 vector)`: Initializes from a `Vector4`.
      - `Vector4Save(Vector2Save vector)`: Initializes from a `Vector2Save`.
      - `Vector4Save(Vector3Save vector)`: Initializes from a `Vector3Save`.
      - `Vector4Save(Vector4Save vector)`: Initializes from another `Vector4Save`.
      - `static implicit operator Vector4Save(Vector2 vector)`: Converts `Vector2` to `Vector4Save`.
      - `static implicit operator Vector2(Vector4Save vector)`: Converts `Vector4Save` to `Vector2`.
      - `static implicit operator Vector4Save(Vector3 vector)`: Converts `Vector3` to `Vector4Save`.
      - `static implicit operator Vector3(Vector4Save vector)`: Converts `Vector4Save` to `Vector3`.
      - `static implicit operator Vector4Save(Vector4 vector)`: Converts `Vector4` to `Vector4Save`.
      - `static implicit operator Vector4(Vector4Save vector)`: Converts `Vector4Save` to `Vector4`.
      - `static implicit operator Vector4Save(Vector2Save vector)`: Converts `Vector2Save` to `Vector4Save`.
      - `static implicit operator Vector2Save(Vector4Save vector)`: Converts `Vector4Save` to `Vector2Save`.
      - `static implicit operator Vector4Save(Vector3Save vector)`: Converts `Vector3Save` to `Vector4Save`.
      - `static implicit operator Vector3Save(Vector4Save vector)`: Converts `Vector4Save` to `Vector3Save`.

# Key Behavior & Side Effects
- Implicit conversions allow seamless transformation between `Vector4Save` and other vector types (`Vector2`, `Vector3`, `Vector4`, `Vector2Save`, `Vector3Save`).

# Constraints & Failure Modes
- No explicit error handling or constraints are defined in the code.
- Assumes valid input types for conversions.

# Example
```csharp
Vector4Save vector = new Vector4Save(1.0f, 2.0f, 3.0f, 4.0f);
Vector2 vec2 = vector; // Implicit conversion to Vector2
```

# Unknowns
- No information on the context or usage of `Vector2Save` and `Vector3Save` types.

