# CHAL.UI.MapSelectionUI

_Automatically generated/updated from `Assets/src/UI/MapSelectionIUI.cs`._

# Purpose
- Manages the user interface for selecting maps in the game.

# Public API
- Namespace: CHAL.UI
- Types
  - public class MapSelectionUI : IngameUI
    - Public fields/properties:
      - string mapSceneName: Name of the scene to load for the selected map.
      - List<MapDef> availableMaps: List of maps available for selection.
    - Public methods:
      - void Awake(): Initializes the UI and sets up buttons for map selection.
      - void OnExitMenuBtnClicked(): Hides the map selection UI.
      - void OnMapSelected(MapDef map): Updates the selected map and displays its details.
      - void OnStartMapBtnClicked(): Starts the selected map; logs a warning if no map is selected.

# Key Behavior & Side Effects
- Initializes UI elements and buttons for each available map in the Awake method.
- Updates the details text when a map is selected.
- Starts the selected map when the start button is clicked, with a warning if no map is selected.

# Constraints & Failure Modes
- Requires that availableMaps is populated for buttons to be created.
- Handles null selection by logging a warning when attempting to start a map without selection.

