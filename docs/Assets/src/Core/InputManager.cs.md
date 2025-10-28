# Assets/src/Core/InputManager.cs

_Automatic generated/updated._

```markdown
## Purpose
- Defines the `InputManager` class for handling user input related to clickable objects in the game.

## Public API
- Namespace/module: None
- Types
  - `public class InputManager : MonoBehaviour`
    - Public methods:
      - `void Update()`
        - Handles input and updates the state of clickable objects.
      - `private void HandleClickableObjects()`
        - Manages hover and click interactions with clickable objects.

## Key Behavior & Side Effects
- In `Update()`, checks for the Escape key press to trigger a transition to the main menu.
- In `HandleClickableObjects()`, performs raycasting to detect clickable objects under the mouse cursor.
- Manages hover state changes by calling `OnHoverEnter()` and `OnHoverExit()` on `ClickableObject` instances.
- Triggers `OnClick()` on the `ClickableObject` when the left mouse button is pressed.

## Constraints & Failure Modes
- Assumes that the camera is set up correctly to perform raycasting.
- Requires `ClickableObject` components to be present on objects tagged as "clickableObject".
- Handles null checks for `lastHovered` and `clickable` to avoid null reference exceptions.

## Example
```csharp
// Attach InputManager to a GameObject in the scene
// Ensure there are GameObjects with the "clickableObject" tag and ClickableObject component.
```

## Unknowns
- The implementation details of `ClickableObject`, `GameManager`, and `DebugManager` are not provided in this file.
```
