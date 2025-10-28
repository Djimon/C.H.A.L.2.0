# Assets/Resources/VFX/Status and Auras FREE/Scripts/OrbitScript.cs

_Automatic generated/updated._

```markdown
# Purpose
- Defines a `RotationParticle` class that rotates a transform around a static point.

# Public API
- Namespace: `MatthewAssets`
- Types
  - `public class RotationParticle : MonoBehaviour`
    - Public fields/properties:
      - `public Transform Orbit1;` - The transform to rotate.
      - `public Transform Static;` - The static point around which to rotate.
      - `public float Speed;` - The speed of rotation.
    - Public methods:
      - `void Update();` - Rotates `Orbit1` around `Static` based on `Speed`.

# Key Behavior & Side Effects
- The `Update` method is called once per frame, causing `Orbit1` to rotate around `Static` continuously.

# Constraints & Failure Modes
- No explicit guards or null handling for `Orbit1` or `Static`; may lead to null reference exceptions if not assigned.
- Assumes `Speed` is a valid float value.

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
- No information on the expected range or constraints for the `Speed` variable.
- No details on how `Orbit1` and `Static` are intended to be set or modified outside of the class.
```
