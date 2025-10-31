# global.MapRewardUI

_Automatically generated/updated from `Assets/src/UI/MapRewardUI.cs`._

# Purpose
- Defines the `MapRewardUI` class for managing UI interactions related to map rewards in the game.

# Public API
- Namespace: None
- Types
  - public class MapRewardUI : IngameUI
    - Public fields/properties: None
    - Public methods:
      - void populateText(bool succeded)
        - Updates the details text based on success or failure.

# Key Behavior & Side Effects
- `Awake()`: Initializes buttons and sets up click event handlers.
- `populateText(bool succeded)`: Changes the text and color of `detailsText` based on the success state.
- `OnHideoutBtnClicked()`: Calls `GameManager.Instance.ExitToHideout()` to exit to the hideout.
- `OnRetryBtnClicked()`: Calls `mapManager.ResetWave()` to reset the current wave.

# Constraints & Failure Modes
- Assumes `root` is properly initialized and contains the required UI elements.
- No explicit error handling for button clicks or UI updates.

# Example
```csharp
MapRewardUI mapRewardUI = new MapRewardUI();
mapRewardUI.populateText(true); // Sets text to "Successful!" with success color.
```

# Unknowns
- No information on the `IngameUI` base class or the `MapManager` class functionality.
- No details on the structure of `root` or how it is initialized.

