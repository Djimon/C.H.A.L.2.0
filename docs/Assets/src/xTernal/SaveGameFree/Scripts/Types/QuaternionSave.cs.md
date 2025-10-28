# Assets/src/xTernal/SaveGameFree/Scripts/Types/QuaternionSave.cs

_Automatic generated/updated._

```markdown
## Purpose
- Defines a serializable struct for representing quaternions used for rotations.

## Public API
- Namespace: `BayatGames.SaveGameFree.Types`
- Types
  - `struct QuaternionSave`
    - Public fields/properties:
      - `float x`: X component of the quaternion.
      - `float y`: Y component of the quaternion.
      - `float z`: Z component of the quaternion.
      - `float w`: W component of the quaternion.
    - Public methods:
      - `QuaternionSave(float x)`: Initializes with x, y, z, w set to 0.
      - `QuaternionSave(float x, float y)`: Initializes with x, y, z set to 0, w set to 0.
      - `QuaternionSave(float x, float y, float z)`: Initializes with x, y, z, w set to 0.
      - `QuaternionSave(float x, float y, float z, float w)`: Initializes with specified x, y, z, w.
      - `QuaternionSave(Quaternion quaternion)`: Initializes from a Unity `Quaternion`.
      - `static implicit operator QuaternionSave(Quaternion quaternion)`: Converts Unity `Quaternion` to `QuaternionSave`.
      - `static implicit operator Quaternion(QuaternionSave quaternion)`: Converts `QuaternionSave` to Unity `Quaternion`.

## Key Behavior & Side Effects
- Implicit conversions between `Quaternion` and `QuaternionSave` allow for seamless integration with Unity's quaternion system.

## Constraints & Failure Modes
- No explicit guards or null handling present.
- Struct is serializable, suitable for saving game states.

## Example
```csharp
QuaternionSave myQuaternion = new QuaternionSave(1.0f, 0.0f, 0.0f, 0.0f);
Quaternion unityQuaternion = myQuaternion; // Implicit conversion to Unity Quaternion
```

## Unknowns
- No information on performance characteristics or threading considerations.
```
