# BayatGames.SaveGameFree.Types.Vector2Save

_Automatically generated/updated from `Assets/src/xTernal/SaveGameFree/Scripts/Types/Vector2Save.cs`._

# Purpose
- Defines a structure for representing 2D vectors and points with serialization support.

# Public API
- Namespace: `BayatGames.SaveGameFree.Types`
- Types
  - `struct Vector2Save`
    - Public fields/properties:
      - `float x`: The x-coordinate of the vector.
      - `float y`: The y-coordinate of the vector.
    - Public methods:
      - `Vector2Save(float x)`: Initializes a new instance with x-coordinate and y set to 0.
      - `Vector2Save(float x, float y)`: Initializes a new instance with specified x and y coordinates.
      - `Vector2Save(Vector2 vector)`: Initializes from a Unity `Vector2`.
      - `Vector2Save(Vector3 vector)`: Initializes from a Unity `Vector3`.
      - `Vector2Save(Vector4 vector)`: Initializes from a Unity `Vector4`.
      - `Vector2Save(Vector2Save vector)`: Initializes from another `Vector2Save`.
      - `Vector2Save(Vector3Save vector)`: Initializes from a `Vector3Save`.
      - `Vector2Save(Vector4Save vector)`: Initializes from a `Vector4Save`.
      - `static implicit operator Vector2Save(Vector2 vector)`: Converts a `Vector2` to `Vector2Save`.
      - `static implicit operator Vector2(Vector2Save vector)`: Converts a `Vector2Save` to `Vector2`.
      - `static implicit operator Vector2Save(Vector3 vector)`: Converts a `Vector3` to `Vector2Save`.
      - `static implicit operator Vector3(Vector2Save vector)`: Converts a `Vector2Save` to `Vector3`.
      - `static implicit operator Vector2Save(Vector4 vector)`: Converts a `Vector4` to `Vector2Save`.
      - `static implicit operator Vector4(Vector2Save vector)`: Converts a `Vector2Save` to `Vector4`.
      - `static implicit operator Vector2Save(Vector3Save vector)`: Converts a `Vector3Save` to `Vector2Save`.
      - `static implicit operator Vector3Save(Vector2Save vector)`: Converts a `Vector2Save` to `Vector3Save`.
      - `static implicit operator Vector2Save(Vector4Save vector)`: Converts a `Vector4Save` to `Vector2Save`.
      - `static implicit operator Vector4Save(Vector2Save vector)`: Converts a `Vector2Save` to `Vector4Save`.

# Key Behavior & Side Effects
- Implicit conversions allow seamless transformation between `Vector2`, `Vector3`, `Vector4`, and their respective save types.

# Constraints & Failure Modes
- No explicit guards or null handling is present in the constructors or conversion operators.
- Assumes valid input types for conversions.

# Example
```csharp
Vector2Save vector2Save = new Vector2Save(1.0f, 2.0f);
Vector2 unityVector2 = vector2Save; // Implicit conversion to Vector2
```

# Unknowns
- The definitions and implementations of `Vector3Save` and `Vector4Save` are not provided in this file.
