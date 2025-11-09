# Assets/src/Core/InputManager.cs

_Automatically generated/updated from `Assets/src/Core/InputManager.cs`._

# Purpose
- Manages user input and interactions with clickable objects in the game.

# Public API
- Namespace: `CHAL.Core`
- Types
  - public class `InputManager` [extends `MonoBehaviour`]
    - Private fields:
      - `lastHovered`: Tracks the last hovered clickable object.
    - Public methods:
      - `void Update()`: Handles input and updates the state of clickable objects.
      - `private void HandleClickableObjects()`: Manages hover and click interactions with clickable objects.

# Key Behavior & Side Effects
- Calls `HandleClickableObjects()` every frame to manage user interactions.
- On pressing the Escape key, invokes `GameManager.Instance.GoToMainMenu()` to transition to the main menu.
- Detects mouse hover and click events on objects tagged as "clickableObject".

# Constraints & Failure Modes
- Requires a camera in the scene for raycasting.
- Assumes clickable objects have the `ClickableObject` component.
- Handles null checks for `lastHovered` and `clickable` to prevent null reference exceptions.

# Example
```csharp
// Example usage in a Unity scene
public class ExampleUsage : MonoBehaviour
{
    private InputManager inputManager;

    void Start()
    {
        inputManager = FindObjectOfType<InputManager>();
    }
}
```

# Unknowns
- None.
