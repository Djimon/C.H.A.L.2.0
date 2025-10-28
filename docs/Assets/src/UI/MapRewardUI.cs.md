# Assets/src/UI/MapRewardUI.cs

_Automatic generated/updated._

```markdown
# Purpose
- Defines the `MapRewardUI` class for managing the UI related to map rewards in the game.

# Public API
- Namespace: None
- Types
  - `public class MapRewardUI : IngameUI`
    - Public fields/properties: None
    - Public methods:
      - `public void populateText(bool succeded)` 
        - Sets the text and color of the details based on success or failure.

# Key Behavior & Side Effects
- `Awake()`: Initializes buttons and assigns click event handlers.
- `populateText(bool succeded)`: Updates the UI text and color based on the success state.
- `OnHideoutBtnClicked()`: Calls `GameManager.Instance.ExitToHideout()` to exit to the hideout.
- `OnRetryBtnClicked()`: Calls `mapManager.ResetWave()` to reset the current wave.

# Constraints & Failure Modes
- Assumes that the `root` object contains the UI elements with the specified names.
- No explicit error handling is present for UI element retrieval or button click actions.

# Example
```csharp
MapRewardUI mapRewardUI = new MapRewardUI();
mapRewardUI.populateText(true); // Displays "Successful!" in success color.
```

# Unknowns
- The behavior of `GameManager.Instance.ExitToHideout()` and `mapManager.ResetWave()` cannot be determined from this file.
```
