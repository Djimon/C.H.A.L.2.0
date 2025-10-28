# global.WaveRewardUI

_Automatically generated/updated from `Assets/src/UI/WaveRewardUI.cs`._

# Purpose
- Defines the `WaveRewardUI` class for managing the UI related to wave rewards in the game.

# Public API
- Namespace: None
- Types
  - public class WaveRewardUI : IngameUI
    - Public fields/properties: None
    - Public methods:
      - void populateText(bool succeded)
        - Updates the details text based on success or failure; logs a message.

# Key Behavior & Side Effects
- `Awake()`: Initializes UI elements and sets up button click event handlers.
- `populateText(bool succeded)`: Changes the text and color of `detailsText` based on the success state and logs the update.
- Button click handlers:
  - `OnHideoutBtnClicked()`: Calls `GameManager.Instance.ExitToHideout()`.
  - `OnNexBtnClicked()`: Calls `mapManager.NextWave()`.
  - `OnRetryBtnClicked()`: Calls `mapManager.StartWave()`.

# Constraints & Failure Modes
- Assumes that the `UIDocument` component is present and correctly configured.
- Requires a valid `MapManager` instance to function properly.

# Example
```csharp
WaveRewardUI waveRewardUI = new WaveRewardUI();
waveRewardUI.populateText(true); // Updates text to "Successful!" with success color.
```

# Unknowns
- No information on the `IngameUI` base class or the `MapManager` implementation details.

