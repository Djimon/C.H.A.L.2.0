# BayatGames.SaveGameFree.Types.Vector3Save

_Automatically generated/updated from `Assets/src/xTernal/SaveGameFree/Scripts/Types/Vector3Save.cs`._

# Purpose
- Defines a structure for representing 3D vectors and points.

# Public API
- Namespace: `BayatGames.SaveGameFree.Types`
- Types
  - `struct Vector3Save`
    - Public fields/properties:
      - `float x`: X component of the vector.
      - `float y`: Y component of the vector.
      - `float z`: Z component of the vector.
    - Public methods:
      - `Vector3Save(float x)`: Initializes with x, y and z set to 0.
      - `Vector3Save(float x, float y)`: Initializes with x, y, and z set to 0.
      - `Vector3Save(float x, float y, float z)`: Initializes with specified x, y, and z.
      - `Vector3Save(Vector2 vector)`: Initializes from a `Vector2`, z set to 0.
      - `Vector3Save(Vector3 vector)`: Initializes from a `Vector3`.
      - `Vector3Save(Vector4 vector)`: Initializes from a `Vector4`.
      - `Vector3Save(Vector2Save vector)`: Initializes from a `Vector2Save`, z set to 0.
      - `Vector3Save(Vector3Save vector)`: Initializes from another `Vector3Save`.
      - `Vector3Save(Vector4Save vector)`: Initializes from a `Vector4Save`.
      - `static implicit operator Vector3Save(Vector2 vector)`: Converts `Vector2` to `Vector3Save`.
      - `static implicit operator Vector2(Vector3Save vector)`: Converts `Vector3Save` to `Vector2`.
      - `static implicit operator Vector3Save(Vector3 vector)`: Converts `Vector3` to `Vector3Save`.
      - `static implicit operator Vector3(Vector3Save vector)`: Converts `Vector3Save` to `Vector3`.
      - `static implicit operator Vector3Save(Vector4 vector)`: Converts `Vector4` to `Vector3Save`.
      - `static implicit operator Vector4(Vector3Save vector)`: Converts `Vector3Save` to `Vector4`.
      - `static implicit operator Vector3Save(Vector2Save vector)`: Converts `Vector2Save` to `Vector3Save`.
      - `static implicit operator Vector2Save(Vector3Save vector)`: Converts `Vector3Save` to `Vector2Save`.
      - `static implicit operator Vector3Save(Vector4Save vector)`: Converts `Vector4Save` to `Vector3Save`.
      - `static implicit operator Vector4Save(Vector3Save vector)`: Converts `Vector3Save` to `Vector4Save`.

# Key Behavior & Side Effects
- Implicit conversions allow seamless transformation between `Vector3Save` and other vector types (`Vector2`, `Vector3`, `Vector4`, `Vector2Save`, `Vector4Save`).

# Constraints & Failure Modes
- No explicit guards or null handling present.
- Assumes valid input types for conversions.

# Example
```csharp
Vector3Save vector = new Vector3Save(1.0f, 2.0f, 3.0f);
Vector2 vec2 = vector; // Implicit conversion to Vector2
Vector3 vec3 = vector; // Implicit conversion to Vector3
```

# Unknowns
- No information on the context of use or integration with other systems.

