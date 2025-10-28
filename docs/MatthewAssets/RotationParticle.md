# MatthewAssets.RotationParticle

_Automatically generated/updated from `Assets/Resources/VFX/Status and Auras FREE/Scripts/OrbitScript.cs`._

# Purpose
- Defines a `RotationParticle` class that rotates a transform around a static point.

# Public API
- Namespace: `MatthewAssets`
- Types
  - public class `RotationParticle` : `MonoBehaviour`
    - Public fields/properties:
      - `Transform Orbit1`: The transform to rotate.
      - `Transform Static`: The static point around which `Orbit1` rotates.
      - `float Speed`: The speed of rotation.
    - Public methods:
      - `void Update()`: Rotates `Orbit1` around `Static` based on `Speed`.

# Key Behavior & Side Effects
- The `Update` method is called once per frame, causing `Orbit1` to rotate around `Static` continuously.

# Constraints & Failure Modes
- No explicit guards or null handling for `Orbit1` or `Static`.
- Assumes `Orbit1` and `Static` are assigned in the Unity Inspector.

# Example
```csharp
public class ExampleUsage : MonoBehaviour
{
    public RotationParticle rotationParticle;

    void Start()
    {
        rotationParticle.Orbit1 = someTransform;
        rotationParticle.Static = anotherTransform;
        rotationParticle.Speed = 10f;
    }
}
```

# Unknowns
- No information on the expected range or limits for `Speed`.
- No error handling for potential null references on `Orbit1` or `Static`.

