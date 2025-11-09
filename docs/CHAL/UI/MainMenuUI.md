# Assets/src/UI/MainMenuUI.cs

_Automatically generated/updated from `Assets/src/UI/MainMenuUI.cs`._

# Purpose
- Manages the main menu user interface for the game.
- Handles scene transitions and menu interactions.

# Public API
- Namespace: CHAL.UI
- Types
  - public class MainMenuUI : MonoBehaviour
    - Public fields/properties:
      - GameObject characterCreationMenue: Reference to the character creation menu.
    - Public methods:
      - void Awake(): Initializes the root visual element.
      - void Start(): Sets the continue button state based on the game profile.
      - void OnEnable(): Registers button click event handlers.
      - void OnStartBtnClicked(): Activates the character creation menu.
      - void OnContinueBtnClicked(): Continues the game and logs the action.
      - void OnExitBtnClicked(): Quits the game.
      - void OnOptoinsBtnClicked(): Logs a placeholder message for options.

# Key Behavior & Side Effects
- The continue button is enabled or disabled based on the presence of a game profile in the GameManager.
- Button click events trigger specific actions, such as starting the character creation menu or continuing the game.

# Constraints & Failure Modes
- The continue button is disabled if there is no profile in the GameManager.
- Assumes that the UIDocument component is attached to the same GameObject.

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
- The exact implementation details of GameManager and DebugManager are not provided in this file.
- The structure and contents of the characterCreationMenue are not defined in this file.

