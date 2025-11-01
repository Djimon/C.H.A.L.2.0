# BayatGames.SaveGameFree.Examples.ExampleMoveObject

_Automatically generated/updated from `Assets/src/xTernal/SaveGameFree/Examples/Auto Save/ExampleMoveObject.cs`._

Purpose
- Defines a Unity MonoBehaviour that moves the GameObject each frame based on input axes.
- Located in the BayatGames.SaveGameFree.Examples namespace.

Public API
- Namespace/module
  - BayatGames.SaveGameFree.Examples
- Types
  - public class ExampleMoveObject : MonoBehaviour
    - Public fields/properties: none
    - Public methods:
      - private void Update()
        - Reads input axes and updates the GameObject’s world-position accordingly.

Key Behavior & Side Effects
- Each frame:
  - Vector3 position = transform.position;
  - position.x += Input.GetAxis("Horizontal");
  - position.y += Input.GetAxis("Vertical");
  - transform.position = position;
- Effect: moves the object in world space along the X and Y axes according to Horizontal/Vertical input.
- Z component is not modified.

Constraints & Failure Modes
- No null checks; relies on Unity’s components (Transform, Input) being present.
- Movement speed is frame-rate dependent (no Time.deltaTime factor).
- Uses axis names "Horizontal" and "Vertical" via Input.GetAxis; behavior depends on Input Manager definitions for these axes.

Unknowns
- Whether this script is enabled at runtime (Update runs only when enabled).
- The specific Input Manager configuration values for Horizontal/Vertical (not defined in this file).
- Interaction with other scripts/components that might affect transform.position.
