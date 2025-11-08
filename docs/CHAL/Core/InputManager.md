# CHAL.Core.InputManager

_Automatically generated/updated from `Assets/src/Core/InputManager.cs`._

# Purpose
- Manages user input and interactions with clickable objects in the game.

# Public API
- Namespace: CHAL.Core
- Types
  - public class InputManager : MonoBehaviour
    - Private fields:
      - ClickableObject lastHovered
    - Public methods:
      - void Update() 
        - Handles input and updates clickable object interactions.
      - private void HandleClickableObjects() 
        - Manages hover and click interactions with clickable objects.

# Key Behavior & Side Effects
- Calls `HandleClickableObjects` in `Update` to manage user interactions.
- If the Escape key is pressed, it triggers `GameManager.Instance.GoToMainMenu()` to navigate to the main menu.
- Detects mouse hover and click events on objects tagged as "clickableObject".

# Constraints & Failure Modes
- Assumes the presence of a camera tagged as `Camera.main`.
- Requires `ClickableObject` components on objects to handle hover and click events.
- Handles null checks for `lastHovered` and `clickable` to prevent null reference exceptions.

# Example
```csharp
// Example usage of InputManager in a Unity scene
public class Game : MonoBehaviour
{
    void Start()
    {
        // InputManager is automatically managed by Unity's MonoBehaviour lifecycle.
    }
}
```

# Unknowns
- The implementation details of `ClickableObject` and `GameManager` are not provided in this file.
- The behavior of `DebugManager.Log` is not defined in this file.

