# Assets/src/xTernal/SaveGameFree/Scripts/Types/Vector4Save.cs

_Automatic generated/updated._

```markdown
## Purpose
- Defines a structure for representing four-dimensional vectors (`Vector4Save`).
- Provides constructors for initializing `Vector4Save` from various vector types.

## Public API
- Namespace: `BayatGames.SaveGameFree.Types`
- Types:
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
      - `Vector4Save(Vector2 vector)`: Initializes from a `Vector2`; z, w set to 0.
      - `Vector4Save(Vector3 vector)`: Initializes from a `Vector3`; w set to 0.
      - `Vector4Save(Vector4 vector)`: Initializes from a `Vector4`.
      - `Vector4Save(Vector2Save vector)`: Initializes from a `Vector2Save`; z, w set to 0.
      - `Vector4Save(Vector3Save vector)`: Initializes from a `Vector3Save`.
      - `Vector4Save(Vector4Save vector)`: Initializes from another `Vector4Save`.
      - Implicit conversions to/from `Vector2`, `Vector3`, `Vector4`, `Vector2Save`, and `Vector3Save`.

## Key Behavior & Side Effects
- Implicit conversions allow seamless use between `Vector4Save` and other vector types (`Vector2`, `Vector3`, `Vector4`, `Vector2Save`, `Vector3Save`).
- Each constructor initializes the vector components, with unspecified components defaulting to 0.

## Constraints & Failure Modes
- No explicit error handling or guards are present.
- Assumes valid input types for conversions.

## Example
```csharp
Vector4Save vector = new Vector4Save(1.0f, 2.0f, 3.0f, 4.0f);
Vector2 vec2 = vector; // Implicit conversion to Vector2
Vector3 vec3 = vector; // Implicit conversion to Vector3
```

## Unknowns
- No information on the `Vector2Save` and `Vector3Save` types as they are not defined in this file.
```
