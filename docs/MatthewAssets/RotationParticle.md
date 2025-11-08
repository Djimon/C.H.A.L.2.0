# MatthewAssets.RotationParticle

_Automatically generated/updated from `Assets/Resources/VFX/Status and Auras FREE/Scripts/OrbitScript.cs`._

# Purpose
- Controls the rotation of a particle around a static point.

# Public API
- Namespace: `MatthewAssets`
- Types
  - `public class RotationParticle : MonoBehaviour`
    - Public fields/properties:
      - `public Transform Orbit1;` - The transform to rotate around the static point.
      - `public Transform Static;` - The static point around which the rotation occurs.
      - `public float Speed;` - The speed of rotation.
    - Public methods:
      - `void Update()` - Rotates `Orbit1` around `Static` based on `Speed`.

# Key Behavior & Side Effects
- The `Update` method is called once per frame, causing `Orbit1` to rotate around `Static` continuously.

# Constraints & Failure Modes
- No explicit guards or null handling present; assumes `Orbit1` and `Static` are assigned.
- Performance may be impacted if `Speed` is set to a high value, leading to rapid rotations.

# Example
```csharp
public class ExampleUsage : MonoBehaviour
{
    public RotationParticle rotationParticle;

    void Start()
    {
        rotationParticle.Speed = 10f; // Set rotation speed
    }
}
```

# Unknowns
- No information on the expected range or limits for `Speed`.
- No error handling for unassigned `Transform` references.
