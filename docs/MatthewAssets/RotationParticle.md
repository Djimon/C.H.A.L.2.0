# MatthewAssets.RotationParticle

_Automatically generated/updated from `Assets/Resources/VFX/Status and Auras FREE/Scripts/OrbitScript.cs`._

1) Purpose
- Defines a Unity MonoBehaviour RotationParticle inside namespace MatthewAssets.
- Rotates Orbit1 around Static on the Y axis at a given Speed.
- Contains commented-out references (Orbit3, Orbit) and related code hints (inactive).

2) Public API
- Namespace/module
  - MatthewAssets
- Types
  - public class RotationParticle : MonoBehaviour
    - Public fields
      - Transform Orbit1
        - The target Transform that will orbit around Static
      - Transform Static
        - The pivot Transform around which Orbit1 revolves
      - float Speed
        - Angular speed in degrees per second (used as Turn angle per frame = Speed * Time.deltaTime)
    - Public methods
      - void Update()
        - Called every frame; rotates Orbit1 around Static.position along Vector3.up by Speed * Time.deltaTime

3) Key Behavior & Side Effects
- Every frame, Orbit1.RotateAround(Static.position, Vector3.up, Speed * Time.deltaTime) is executed.
- Relies on Orbit1 and Static being assigned; otherwise NullReferenceException at runtime.
- Only affects Orbit1’s transform; Static and Orbit1 positions may change over time due to rotation.

4) Constraints & Failure Modes
- No null guards for Orbit1 or Static.
- Single-axis (Y) rotation via Vector3.up.
- Public fields intended for inspector/manual wiring; no additional threading or async behavior.

5) Example
- Inspector configuration (recommended)
  - Attach RotationParticle to a GameObject.
  - Set Orbit1 to the object that should orbit.
  - Set Static to the pivot/center Transform.
  - Set Speed to a positive value (e.g., 30).

```csharp
// Example: programmatic setup
var rp = orbitingObject.AddComponent<MatthewAssets.RotationParticle>();
rp.Orbit1 = orbitingTarget.transform;
rp.Static = pivotTransform;
rp.Speed = 45f;
```

6) Unknowns
- Intended usage beyond these public fields is not specified.
- Behavior when multiple Orbit1 objects are needed is not defined.
- Any runtime expectations for initial positions beyond what RotateAround computes are not stated.

