# Assets/src/UI/CharacterCreationUI.cs

_Automatic generated/updated._

```markdown
## Purpose
- Defines the `CharacterCreationUI` class for managing the character creation interface in the game.

## Public API
- Namespace: None
- Types
  - public class CharacterCreationUI : MonoBehaviour
    - Public fields/properties:
      - _startSceneName: Name of the starting scene (serialized).
    - Public methods:
      - void Awake(): Initializes the root visual element and sets a default color.
      - void OnEnable(): Sets up button click event handlers.
      - void OnNewGameBtnClicked(): Initializes a new player profile and starts a new game.
      - void OnBackBtnClicked(): Deactivates the character creation UI.

## Key Behavior & Side Effects
- `Awake`: Initializes the UI and sets a default color.
- `OnEnable`: Binds button click events to their respective handlers.
- `OnNewGameBtnClicked`: Creates a new `PlayerProfile` and starts a new game using `GameManager`.
- `OnBackBtnClicked`: Hides the character creation UI.

## Constraints & Failure Modes
- Assumes that the `UIDocument` component is attached to the same GameObject.
- No explicit error handling for UI interactions or player profile initialization.

## Example
```csharp
// Example usage in a Unity scene
CharacterCreationUI characterCreationUI = gameObject.AddComponent<CharacterCreationUI>();
```

## Unknowns
- The behavior of `GameManager.Instance.StartNewGame(profile)` is not defined in this file.
- The structure and properties of `PlayerProfile` are not detailed in this file.
```
