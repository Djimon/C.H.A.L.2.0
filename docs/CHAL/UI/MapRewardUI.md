# CHAL.UI.MapRewardUI

_Automatically generated/updated from `Assets/src/UI/MapRewardUI.cs`._

1) Purpose
- Defines the `MapRewardUI` class for managing the UI related to map rewards in the game.

2) Public API
- Namespace: `CHAL.UI`
- Types
  - public class `MapRewardUI` [extends `IngameUI`]
    - Public fields/properties: None
    - Public methods:
      - `void populateText(bool succeded)` - Updates the status text based on success or failure.

3) Key Behavior & Side Effects
- `Awake()`: Initializes buttons and assigns click event handlers. Retrieves the `MapManager` instance.
- `populateText(bool succeded)`: Changes the text and color of the `detailsText` based on the success of an operation.
- `OnHideoutBtnClicked()`: Calls `GameManager.Instance.ExitToHideout()` to exit to the hideout.
- `OnRetryBtnClicked()`: Calls `mapManager.ResetWave()` to reset the current wave.

4) Constraints & Failure Modes
- Assumes that the UI elements ("Retry", "Hideout", "MapStatus") exist in the UI hierarchy.
- No explicit error handling for missing components or null references.

5) Example
```csharp
MapRewardUI mapRewardUI = new MapRewardUI();
mapRewardUI.populateText(true); // Displays "Successful!" in the UI.
```

6) Unknowns
- None.
