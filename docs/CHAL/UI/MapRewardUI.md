# Assets/src/UI/MapRewardUI.cs

_Automatically generated/updated from `Assets/src/UI/MapRewardUI.cs`._

# Purpose
- Manages the user interface for map rewards in the game.
- Inherits from `IngameUI` to provide additional functionality.

# Public API
- Namespace: `CHAL.UI`
- Types
  - `public class MapRewardUI : IngameUI`
    - Public fields/properties:
      - `Button btnRetry`: Button for retrying the map.
      - `Button btnHideout`: Button for exiting to the hideout.
      - `TextElement detailsText`: Displays the status of the map.
      - `MapManager mapManager`: Reference to the map manager.
    - Public methods:
      - `public void populateText(bool succeded)`: Updates the details text based on success status.

# Key Behavior & Side Effects
- `Awake()`: Initializes buttons and assigns click event handlers. Finds the `MapManager` instance.
- `populateText(bool succeded)`: Changes the text and color of `detailsText` based on the success status.
- `OnHideoutBtnClicked()`: Calls `GameManager.Instance.ExitToHideout()` to exit to the hideout.
- `OnRetryBtnClicked()`: Calls `mapManager.ResetWave()` to reset the current wave.

# Constraints & Failure Modes
- Assumes that the UI elements with specified names exist in the UI hierarchy.
- `FindFirstObjectByType<MapManager>()` may return null if no `MapManager` is present in the scene.

# Example
```csharp
MapRewardUI mapRewardUI = new MapRewardUI();
mapRewardUI.populateText(true); // Updates text to "Successful!" with success color.
```

# Unknowns
- None.
