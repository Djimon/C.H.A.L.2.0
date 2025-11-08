# MatthewAssets.CameraOrbit

_Automatically generated/updated from `Assets/Resources/VFX/Status and Auras FREE/Scripts/CameraRotation.cs`._

# Purpose
- Controls the camera's orbit around a target object.
- Allows customization of distance, speed, and orbit axis.

# Public API
- Namespace: `MatthewAssets`
- Types
  - `public class CameraOrbit : MonoBehaviour`
    - Public fields/properties:
      - `Transform target`: The object around which the camera will rotate.
      - `float distance`: Distance from the object.
      - `float orbitSpeed`: Orbit speed.
      - `Vector3 orbitAxis`: Axis around which the camera will rotate.
    - Public methods:
      - `void Update()`: Updates the camera position and rotation based on the target.

# Key Behavior & Side Effects
- The camera orbits around the target object based on the specified distance and speed.
- The camera maintains its height while orbiting.

# Constraints & Failure Modes
- If `target` is null, the camera will not update its position or rotation.

# Example
```csharp
CameraOrbit cameraOrbit = gameObject.AddComponent<CameraOrbit>();
cameraOrbit.target = someTargetTransform;
cameraOrbit.distance = 10.0f;
cameraOrbit.orbitSpeed = 10.0f;
```

# Unknowns
- None.
