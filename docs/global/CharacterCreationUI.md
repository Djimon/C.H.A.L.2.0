# global.CharacterCreationUI

_Automatically generated/updated from `Assets/src/UI/CharacterCreationUI.cs`._

# Purpose
- Defines the `CharacterCreationUI` class for managing the character creation user interface in a Unity game.

# Public API
- Namespace: None
- Types
  - public class CharacterCreationUI : MonoBehaviour
    - Public fields/properties:
      - _startSceneName: Name of the starting scene (serialized).
    - Public methods:
      - void Awake(): Initializes the UI elements and color.
      - void OnEnable(): Sets up button click event handlers.
      - void OnNewGameBtnClicked(): Initializes a new player profile and starts a new game.
      - void OnBackBtnClicked(): Deactivates the character creation UI.

# Key Behavior & Side Effects
- `Awake`: Initializes the root visual element and sets a default color.
- `OnEnable`: Binds button click events to their respective handlers.
- `OnNewGameBtnClicked`: Creates a new `PlayerProfile` and starts a new game using `GameManager`.
- `OnBackBtnClicked`: Hides the character creation UI.

# Constraints & Failure Modes
- Assumes that the `UIDocument` component is attached to the same GameObject.
- No explicit error handling for UI interactions or player profile initialization.

# Example
```csharp
// Example usage in a Unity scene
CharacterCreationUI characterCreationUI = gameObject.AddComponent<CharacterCreationUI>();
```

# Unknowns
- The behavior of `GameManager.Instance.StartNewGame(profile)` and its impact on the game state.
- The structure and properties of the `PlayerProfile` class.

