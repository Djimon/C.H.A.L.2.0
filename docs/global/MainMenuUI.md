# global.MainMenuUI

_Automatically generated/updated from `Assets/src/UI/MainMenuUI.cs`._

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
      - void Start(): Configures the continue button based on the game profile state.
      - void OnEnable(): Sets up button click event handlers.
      - void OnStartBtnClicked(): Activates the character creation menu.
      - void OnContinueBtnClicked(): Logs a message and continues the game.
      - void OnExitBtnClicked(): Quits the game.
      - void OnOptoinsBtnClicked(): Logs a message for options (to be implemented).

# Key Behavior & Side Effects
- `Awake`: Initializes the UI document's root visual element.
- `Start`: Disables the continue button if there is no game profile.
- Button click handlers trigger specific actions, such as starting a new game, continuing a game, or quitting.

# Constraints & Failure Modes
- The continue button is disabled if `GameManager.Instance?.Profile` is null.
- Assumes that the `UIDocument` component is present on the same GameObject.

# Example
```csharp
// Example of how to use MainMenuUI in a Unity scene
public class GameInitializer : MonoBehaviour
{
    void Start()
    {
        // Assuming MainMenuUI is attached to a GameObject in the scene
        MainMenuUI mainMenu = GetComponent<MainMenuUI>();
        mainMenu.characterCreationMenue.SetActive(false); // Initially hide character creation menu
    }
}
```

# Unknowns
- The implementation details of `GameManager`, `DebugManager`, and their methods are not provided in this file.
- The structure and contents of the `characterCreationMenue` GameObject are not defined.

