# CHAL.UI.MainMenuUI

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
      - void Start(): Sets the continue button's enabled state based on the game profile.
      - void OnEnable(): Subscribes to button click events.
      - void OnStartBtnClicked(): Activates the character creation menu.
      - void OnContinueBtnClicked(): Continues the game and logs the action.
      - void OnExitBtnClicked(): Quits the game.
      - void OnOptoinsBtnClicked(): Logs a placeholder message for options.

# Key Behavior & Side Effects
- The continue button is disabled if there is no game profile available.
- Button click events trigger specific actions, such as starting a new game or continuing an existing one.

# Constraints & Failure Modes
- The script assumes that the UIDocument component is attached to the same GameObject.
- The characterCreationMenue must be assigned in the inspector for the new game button to function correctly.

# Example
```csharp
// Example usage in a Unity scene
public class GameInitializer : MonoBehaviour
{
    public MainMenuUI mainMenuUI;

    private void Start()
    {
        // Ensure the main menu UI is set up correctly
        mainMenuUI.characterCreationMenue.SetActive(false);
    }
}
```

# Unknowns
- The behavior of the GameManager class and its methods (e.g., ContinueGame, Quit) cannot be determined from this file.
- The structure and contents of the game profile are not defined in this file.

