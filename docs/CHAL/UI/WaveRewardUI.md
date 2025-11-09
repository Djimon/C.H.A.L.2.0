# Assets/src/UI/WaveRewardUI.cs

_Automatically generated/updated from `Assets/src/UI/WaveRewardUI.cs`._

# Purpose
- Manages the user interface for wave rewards in the game.

# Public API
- Namespace: `CHAL.UI`
- Types
  - `public class WaveRewardUI : IngameUI`
    - Public fields/properties:
      - `private Button btnRetry`
      - `private Button btnNext`
      - `private Button btnHideout`
      - `private TextElement detailsText`
      - `private Toggle _autoStartToggle`
      - `private Label _autoStartCountdown`
    - Public methods:
      - `public override void Show(bool visible)`
      - `public void populateText(bool succeeded)`

# Key Behavior & Side Effects
- `Show(bool visible)`: Displays or hides the UI and manages the auto-start countdown based on the visibility state.
- `populateText(bool succeeded)`: Updates the details text based on the success status.
- Auto-start countdown starts if conditions are met when the UI is shown.
- Cancels the countdown if the toggle is turned off or the UI is closed.

# Constraints & Failure Modes
- If `_autoStartToggle` or `_autoStartCountdown` is not found, a warning is logged.
- The countdown can be interrupted if the UI is closed or the auto-start toggle is disabled.
- The countdown routine prevents double starts.

# Example
```csharp
WaveRewardUI waveRewardUI = new WaveRewardUI();
waveRewardUI.Show(true);
waveRewardUI.populateText(true);
```

# Unknowns
- The success condition for starting the next wave is not explicitly defined in the code.

