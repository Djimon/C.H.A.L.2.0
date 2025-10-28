# Assets/Resources/VFX/Status and Auras FREE/Scripts/CameraRotation.cs

_Automatic generated/updated._

```markdown
# Purpose
- Defines a `CameraOrbit` class that allows a camera to orbit around a specified target.

# Public API
- Namespace: `MatthewAssets`
- Types
  - `public class CameraOrbit : MonoBehaviour`
    - Public fields/properties:
      - `public Transform target` - The object around which the camera will rotate.
      - `public float distance` - Distance from the object (default is 10.0f).
      - `public float orbitSpeed` - Orbit speed (default is 10.0f).
      - `public Vector3 orbitAxis` - Axis around which the camera will rotate (default is `Vector3.up`).
    - Public methods:
      - `void Update()` - Updates the camera position and rotation based on the target.

# Key Behavior & Side Effects
- The camera orbits around the target based on the `orbitSpeed` and `distance`.
- The camera's height remains constant while it orbits around the target.

# Constraints & Failure Modes
- The camera only updates its position if the `target` is not null.
- No explicit error handling for null `target` beyond skipping updates.

# Example
```csharp
CameraOrbit cameraOrbit = new CameraOrbit();
cameraOrbit.target = someTransform; // Assign a target Transform
cameraOrbit.distance = 5.0f; // Set distance from target
cameraOrbit.orbitSpeed = 15.0f; // Set orbit speed
```

# Unknowns
- No information on how this class interacts with other components or systems.
```
