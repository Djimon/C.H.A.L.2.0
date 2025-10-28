# Assets/src/UI/MainMenuUI.cs

_Automatic generated/updated._

```markdown
# Purpose
- Defines the `MainMenuUI` class for managing the main menu interface in a Unity game.

# Public API
- Namespace: None
- Types
  - public class MainMenuUI : MonoBehaviour
    - Public fields/properties:
      - GameObject characterCreationMenue: Reference to the character creation menu.
    - Public methods:
      - void Awake(): Initializes the root visual element.
      - void Start(): Sets the continue button's enabled state based on the game profile.
      - void OnEnable(): Subscribes to button click events.
      - void OnStartBtnClicked(): Activates the character creation menu.
      - void OnContinueBtnClicked(): Logs a message and continues the game.
      - void OnExitBtnClicked(): Quits the game.
      - void OnOptoinsBtnClicked(): Logs a message for options (to-do).

# Key Behavior & Side Effects
- `Awake`: Initializes the UI document's root visual element.
- `Start`: Disables the continue button if there is no game profile.
- Button click events trigger respective methods for starting a new game, continuing, exiting, and options.

# Constraints & Failure Modes
- The continue button is disabled if `GameManager.Instance?.Profile` is null.
- Assumes that the `UIDocument` component is present on the same GameObject.

# Example
```csharp
public class ExampleUsage : MonoBehaviour
{
    void Start()
    {
        MainMenuUI mainMenu = GetComponent<MainMenuUI>();
        mainMenu.characterCreationMenue.SetActive(false); // Example of accessing the character creation menu.
    }
}
```

# Unknowns
- The implementation details of `GameManager`, `DebugManager`, and the structure of the game profile are not provided in this file.
```
