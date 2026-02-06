# Assets/src/UI/MapSelectionIUI.cs

_Automatically generated/updated from `Assets/src/UI/MapSelectionIUI.cs`._

# Purpose
- Manages the user interface for selecting maps in the game.

# Public API
- Namespace: CHAL.UI
- Types
  - public class MapSelectionUI : IngameUI
    - Public fields/properties:
      - string mapSceneName: Name of the scene to load for the map.
      - List<MapDef> availableMaps: List of maps available for selection.
    - Public methods:
      - void Awake(): Initializes the UI and sets up map selection buttons.
      - void OnExitMenuBtnClicked(): Hides the map selection UI.
      - void OnMapSelected(MapDef map): Updates the selected map and displays its details.
      - void OnStartMapBtnClicked(): Starts the selected map; logs a warning if no map is selected.

# Key Behavior & Side Effects
- Initializes UI elements and populates map selection buttons based on `availableMaps`.
- Updates the details text when a map is selected.
- Starts the selected map when the start button is clicked, with a warning if no map is selected.

# Constraints & Failure Modes
- Requires `availableMaps` to be populated for map selection buttons to be created.
- If no map is selected, a warning is logged when attempting to start a map.

# Example
```csharp
// Example usage in a Unity scene
MapSelectionUI mapSelectionUI = new MapSelectionUI();
mapSelectionUI.Awake(); // Call to initialize the UI
```

# Unknowns
- The behavior of `IngameUI` and the structure of `MapDef` are not defined in this file.

