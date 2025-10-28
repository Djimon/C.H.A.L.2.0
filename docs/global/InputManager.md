# global.InputManager

_Automatically generated/updated from `Assets/src/Core/InputManager.cs`._

# Purpose
- Defines the `InputManager` class for handling user input and interactions with clickable objects in the game.

# Public API
- Namespace/module: None specified
- Types
  - public class InputManager : MonoBehaviour
    - Public fields/properties:
      - lastHovered: Tracks the last hovered `ClickableObject`.
    - Public methods:
      - void Update(): Handles input and updates clickable object interactions.
      - private void HandleClickableObjects(): Manages hover and click interactions with clickable objects.

# Key Behavior & Side Effects
- Calls `HandleClickableObjects` in `Update` to manage user interactions.
- On pressing the Escape key, invokes `GameManager.Instance.GoToMainMenu()` to transition to the main menu.
- Detects mouse hover and click events on objects tagged as "clickableObject".

# Constraints & Failure Modes
- Uses raycasting to detect clickable objects; requires colliders with the "clickableObject" tag.
- Handles null checks for `lastHovered` and `clickable` to prevent null reference exceptions.

# Example
```csharp
// Example usage of InputManager in a Unity scene
void Start()
{
    // Attach InputManager to a GameObject in the scene
    gameObject.AddComponent<InputManager>();
}
```

# Unknowns
- No information on the implementation details of `ClickableObject` or `GameManager`.
- No details on the behavior of `DebugManager.Log` or its impact on performance.

