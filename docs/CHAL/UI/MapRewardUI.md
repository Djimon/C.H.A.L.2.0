# CHAL.UI.MapRewardUI

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
      - `MapManager mapManager`: Manages map-related operations.
    - Public methods:
      - `public void populateText(bool succeded)`: Updates the details text based on success status.

# Key Behavior & Side Effects
- `Awake()`: Initializes buttons and assigns click event handlers. Retrieves the `MapManager` instance.
- `populateText(bool succeded)`: Changes the text and color of `detailsText` based on the success status.

# Constraints & Failure Modes
- Assumes that the UI elements with specified names exist in the UI hierarchy.
- No explicit error handling for missing UI elements or `MapManager`.

# Example
```csharp
MapRewardUI mapRewardUI = new MapRewardUI();
mapRewardUI.populateText(true); // Updates text to "Successful!" with success color.
```

# Unknowns
- None.
