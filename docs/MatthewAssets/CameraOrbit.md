# MatthewAssets.CameraOrbit

_Automatically generated/updated from `Assets/Resources/VFX/Status and Auras FREE/Scripts/CameraRotation.cs`._

1) Purpose
- Defines a Unity MonoBehaviour that orbits the camera around a target on the XZ plane.
- Allows configuration of distance and angular speed; maintains the camera's current Y height.
- orbitAxis field is defined but not applied in the logic.

```

```text
2) Public API
- Namespace/module
  - MatthewAssets

- Types
  - public class CameraOrbit : MonoBehaviour
    - Public fields
      - Transform target: The object around which the camera will rotate
      - float distance: Distance from object
      - float orbitSpeed: Orbit speed
      - Vector3 orbitAxis: Axis around which the camera will rotate (not used)
    - Private fields
      - float currentAngle: Internal angle accumulator
    - Public methods
      - void Update()
        - If target is set:
          - currentAngle += orbitSpeed * Time.deltaTime
          - x = Mathf.Cos(currentAngle) * distance
          - z = Mathf.Sin(currentAngle) * distance
          - newPosition = new Vector3(x, 0, z) + target.position
          - newPosition.y = transform.position.y
          - transform.position = newPosition
          - transform.LookAt(target)

```

```text
3) Key Behavior & Side Effects
- Per frame (Update):
  - If target exists, increment currentAngle by orbitSpeed * Time.deltaTime
  - Compute orbit position in XZ: (cos(currentAngle) * distance, 0, sin(currentAngle) * distance)
  - Translate around target using target.position
  - Preserve current camera Y (newPosition.y = transform.position.y)
  - Apply new position to camera and orient toward target (transform.LookAt(target))

```

```text
4) Constraints & Failure Modes
- Guard: No action if target is null.
- orbitAxis is defined but not applied; may be unused by design.
- No validation of distance/orbitSpeed values in code.
- No threading or async concerns; uses Time.deltaTime for frame consistency.
- Public fields are inspector-configurable.

```

```text
5) Example
// Example: attach to the camera and configure via script
void Start() {
    var orbit = camera.GetComponent<CameraOrbit>();
    if (orbit != null) {
        orbit.target = someTargetTransform;
        orbit.distance = 12f;
        orbit.orbitSpeed = 20f;
    }
}
```

```text
6) Unknowns
- File path: Assets/Resources/VFX/Status and Auras FREE/Scripts/CameraRotation.cs
  - Class is named CameraOrbit; file name suggests CameraRotation.cs (mismatch not resolvable from code alone).
- Intended use of orbitAxis: defined but not applied in Update.
- Any additional behaviors (e.g., vertical offset, clamping) are not present in this file.

