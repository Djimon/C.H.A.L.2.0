# MatthewAssets.CameraOrbit

_Automatically generated/updated from `Assets/Resources/VFX/Status and Auras FREE/Scripts/CameraRotation.cs`._

# Purpose
- Defines a `CameraOrbit` class for rotating a camera around a target object.

# Public API
- Namespace: `MatthewAssets`
- Types
  - public class `CameraOrbit` [extends MonoBehaviour]
    - Public fields/properties:
      - `Transform target`: The object around which the camera will rotate.
      - `float distance`: Distance from the object.
      - `float orbitSpeed`: Orbit speed.
      - `Vector3 orbitAxis`: Axis around which the camera will rotate.
    - Public methods:
      - `void Update()`: Updates camera position and rotation based on the target.

# Key Behavior & Side Effects
- Rotates the camera around the specified target based on `orbitSpeed` and `distance`.
- Maintains the camera's height while updating its position.

# Constraints & Failure Modes
- The camera only orbits if `target` is not null.
- No explicit error handling for null `target`.

# Example
```csharp
CameraOrbit cameraOrbit = gameObject.AddComponent<CameraOrbit>();
cameraOrbit.target = someTargetTransform;
cameraOrbit.distance = 10.0f;
cameraOrbit.orbitSpeed = 10.0f;
```

# Unknowns
- None.

