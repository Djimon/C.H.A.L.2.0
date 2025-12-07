# Assets/src/UI/CharacterCreationUI.cs

_Automatically generated/updated from `Assets/src/UI/CharacterCreationUI.cs`._

# Purpose
- Manages the user interface for character creation.
- Handles scene transitions and user input for character names.

# Public API
- Namespace: `CHAL.UI`
- Types
  - `public class CharacterCreationUI : MonoBehaviour`
    - **Public fields/properties**
      - `_startSceneName`: Name of the scene to start.
    - **Public methods**
      - None

# Key Behavior & Side Effects
- On `Awake`: Initializes the root visual element and sets a default color.
- On `OnEnable`: Binds button click events for starting a new game and going back.
- `OnNewGameBtnClicked`: Creates a new `PlayerProfile`, initializes it with the input name and color, and starts a new game via `GameManager`.
- `OnBackBtnClicked`: Deactivates the current game object.

# Constraints & Failure Modes
- Assumes that the `UIDocument` component is attached to the same GameObject.
- No explicit error handling for user input or game state transitions.

# Example
```csharp
// Example usage in a Unity scene
CharacterCreationUI characterCreationUI = gameObject.AddComponent<CharacterCreationUI>();
```

# Unknowns
- None
