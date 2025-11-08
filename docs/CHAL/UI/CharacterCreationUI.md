# CHAL.UI.CharacterCreationUI

_Automatically generated/updated from `Assets/src/UI/CharacterCreationUI.cs`._

# Purpose
- Manages the user interface for character creation.
- Handles scene transitions and user input for character names.

# Public API
- Namespace: CHAL.UI
- Types
  - public class CharacterCreationUI : MonoBehaviour
    - Private fields:
      - string _startSceneName
      - VisualElement root
      - Button btnNewGame
      - Button btnBack
      - TextField name_input
      - Color[] colors
    - Public methods:
      - void Awake() 
      - void OnEnable() 
      - void OnNewGameBtnClicked() 
      - void OnBackBtnClicked() 

# Key Behavior & Side Effects
- `Awake`: Initializes the root visual element and sets a color.
- `OnEnable`: Sets up button click event handlers for starting a new game and going back.
- `OnNewGameBtnClicked`: Creates a new `PlayerProfile`, initializes it with the input name and color, and starts a new game.
- `OnBackBtnClicked`: Deactivates the character creation UI.

# Constraints & Failure Modes
- Assumes that the `UIDocument` component is attached to the same GameObject.
- Does not handle null or empty input for the character name.

# Example
```csharp
// Example usage in a Unity scene
CharacterCreationUI characterCreationUI = gameObject.AddComponent<CharacterCreationUI>();
```

# Unknowns
- The behavior of `GameManager.Instance.StartNewGame(profile)` is not defined in this file.
- The structure and properties of `PlayerProfile` are not detailed in this file.

