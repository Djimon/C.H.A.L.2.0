# CHAL.UI.WaveRewardUI

_Automatically generated/updated from `Assets/src/UI/WaveRewardUI.cs`._

# Purpose
- Manages the user interface for wave rewards in the game.

# Public API
- Namespace: `CHAL.UI`
- Types
  - `public class WaveRewardUI : IngameUI`
    - Public fields/properties:
      - `Button btnRetry`: Button to retry the wave.
      - `Button btnNext`: Button to proceed to the next wave.
      - `Button btnHideout`: Button to exit to the hideout.
      - `TextElement detailsText`: Displays the status of the wave.
      - `Toggle _autoStartToggle`: Toggle for auto-starting the next wave.
      - `Label _autoStartCountdown`: Displays the countdown for auto-start.
    - Public methods:
      - `public override void Show(bool visible)`: Displays or hides the UI.
      - `public void populateText(bool succeeded)`: Updates the details text based on success status.

# Key Behavior & Side Effects
- `Show(bool visible)`: 
  - When shown, it synchronizes the auto-start toggle with the current map state and starts a countdown if conditions are met.
  - When hidden, it stops any running countdown.
- `populateText(bool succeeded)`: Updates the UI text and color based on the success of the operation.
- Button click handlers (`OnRetryBtnClicked`, `OnNexBtnClicked`, `OnHideoutBtnClicked`) stop the countdown and perform respective actions.

# Constraints & Failure Modes
- If the auto-start toggle is turned off during a countdown, the countdown is canceled.
- If the UI is closed while the countdown is active, the countdown is stopped.
- Warnings are logged if the required UI elements are not found.

# Example
```csharp
WaveRewardUI waveRewardUI = new WaveRewardUI();
waveRewardUI.Show(true);
waveRewardUI.populateText(true);
```

# Unknowns
- The behavior of `mapManager.HasNextWave()` and `mapManager.AutoStartAllWaves` cannot be determined from this file.
- The implementation details of `DebugManager` and `GameManager` are not provided.

