# global.MapSelectionUI

_Automatically generated/updated from `Assets/src/UI/MapSelectionIUI.cs`._

# Purpose
- Defines the `MapSelectionUI` class for managing map selection in the game UI.

# Public API
- Namespace: None
- Types
  - public class MapSelectionUI : IngameUI
    - Public fields/properties:
      - `mapSceneName`: Name of the scene to load for the map.
      - `availableMaps`: List of available maps for selection.
    - Public methods:
      - `void Awake()`: Initializes the UI and sets up buttons for map selection.
      - `void OnExitMenuBtnClicked()`: Hides the map selection UI.
      - `void OnMapSelected(MapDef map)`: Updates the selected map and displays its details.
      - `void OnStartMapBtnClicked()`: Starts the selected map; logs a warning if no map is selected.

# Key Behavior & Side Effects
- Initializes UI elements in `Awake()`, including buttons for each available map.
- Updates the details text when a map is selected.
- Starts the selected map or logs a warning if no map is selected when the start button is clicked.

# Constraints & Failure Modes
- Requires `availableMaps` to be populated for buttons to be created.
- Logs a warning if the start button is clicked without a selected map.

# Example
```csharp
MapSelectionUI mapSelectionUI = new MapSelectionUI();
mapSelectionUI.Awake(); // Initializes the UI
```

# Unknowns
- The structure and properties of `MapDef` are not defined in this file.
- The behavior of `GameManager.Instance.StartMap` is not detailed in this file.

