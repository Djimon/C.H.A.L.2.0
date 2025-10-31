# BayatGames.SaveGameFree.Types.Vector2Save

_Automatically generated/updated from `Assets/src/xTernal/SaveGameFree/Scripts/Types/Vector2Save.cs`._

# Purpose
- Defines a structure for representing 2D vectors and points.

# Public API
- Namespace: `BayatGames.SaveGameFree.Types`
- Types
  - `struct Vector2Save`
    - Public fields/properties:
      - `float x`: X component of the vector.
      - `float y`: Y component of the vector.
    - Public methods:
      - `Vector2Save(float x)`: Initializes with x value, y set to 0.
      - `Vector2Save(float x, float y)`: Initializes with specified x and y values.
      - `Vector2Save(Vector2 vector)`: Initializes from a Unity `Vector2`.
      - `Vector2Save(Vector3 vector)`: Initializes from a Unity `Vector3`.
      - `Vector2Save(Vector4 vector)`: Initializes from a Unity `Vector4`.
      - `Vector2Save(Vector2Save vector)`: Initializes from another `Vector2Save`.
      - `Vector2Save(Vector3Save vector)`: Initializes from a `Vector3Save`.
      - `Vector2Save(Vector4Save vector)`: Initializes from a `Vector4Save`.
      - `static implicit operator Vector2Save(Vector2 vector)`: Converts `Vector2` to `Vector2Save`.
      - `static implicit operator Vector2(Vector2Save vector)`: Converts `Vector2Save` to `Vector2`.
      - `static implicit operator Vector2Save(Vector3 vector)`: Converts `Vector3` to `Vector2Save`.
      - `static implicit operator Vector3(Vector2Save vector)`: Converts `Vector2Save` to `Vector3`.
      - `static implicit operator Vector2Save(Vector4 vector)`: Converts `Vector4` to `Vector2Save`.
      - `static implicit operator Vector4(Vector2Save vector)`: Converts `Vector2Save` to `Vector4`.
      - `static implicit operator Vector2Save(Vector3Save vector)`: Converts `Vector3Save` to `Vector2Save`.
      - `static implicit operator Vector3Save(Vector2Save vector)`: Converts `Vector2Save` to `Vector3Save`.
      - `static implicit operator Vector2Save(Vector4Save vector)`: Converts `Vector4Save` to `Vector2Save`.
      - `static implicit operator Vector4Save(Vector2Save vector)`: Converts `Vector2Save` to `Vector4Save`.

# Key Behavior & Side Effects
- Implicit conversions allow seamless interaction between `Vector2Save` and Unity's `Vector2`, `Vector3`, `Vector4`, as well as `Vector3Save` and `Vector4Save`.

# Constraints & Failure Modes
- No explicit guards or null handling present.
- Assumes valid input types for conversions.

# Example
```csharp
Vector2Save myVector = new Vector2Save(1.0f, 2.0f);
Vector2 unityVector = myVector; // Implicit conversion to Unity Vector2
```

# Unknowns
- No information on the context or usage of `Vector3Save` and `Vector4Save` types.

