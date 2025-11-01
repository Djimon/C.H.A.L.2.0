# CHAL.UI.MapSelectionUI

_Automatically generated/updated from `Assets/src/UI/MapSelectionIUI.cs`._

```text
1) Purpose
- Defines a UI component MapSelectionUI for selecting and starting a map, and exiting the menu.
- Builds a list of map selection buttons from availableMaps and shows map details when selected.
- Delegates map-start action to GameManager and hides itself on exit.

2) Public API
- Namespace/module: CHAL.UI
- Types
  - protected override void Awake()
    - Purpose: initialize UI, build map list from availableMaps, wire up button events, and locate UI elements.
  - private void OnExitMenuBtnClicked()
    - Purpose: hide the UI by calling Show(false).
  - private void OnMapSelected(MapDef map)
    - Parameters: MapDef map
    - Purpose: set _selectedMap and update detailsText to show the selected map name and monster level.
  - private void OnStartMapBtnClicked()
    - Purpose: start the selected map via GameManager if one is selected; otherwise log a warning.
- Fields
  - [SerializeField] private string mapSceneName = "04_Map"
    - Scene name to load when starting a map.
  - [SerializeField] private List<MapDef> availableMaps
    - Source data for map buttons (each MapDef becomes a button).
  - private MapDef _selectedMap
    - Currently selected map (null if none selected).
  - private Button btnStartMap
    - Reference to the StartMap button.
  - private Button btnExitMenu
    - Reference to the Exit button.
  - private TextElement detailsText
    - UI text element showing details of the selected map.

3) Key Behavior & Side Effects
- Awake
  - Retrieves root VisualElement via UIDocument.
  - Clears the MapList container and creates a Button per item in availableMaps.
  - Each generated Button wires its click to OnMapSelected(map).
  - Locates StartMap and Exit buttons and wires their events.
  - Locates Details text element.
- OnMapSelected
  - Updates _selectedMap and detailsText to show the map’s displayNameKey and baseLevel.
- OnStartMapBtnClicked
  - If no map is selected: logs a warning via DebugManager and returns.
  - Otherwise calls GameManager.Instance.StartMap(mapSceneName, _selectedMap).
- OnExitMenuBtnClicked
  - Hides the UI by calling Show(false).

4) Constraints & Failure Modes
- Nullability/guards
  - No null checks for availableMaps, root, detailsText, btnStartMap, btnExitMenu; potential NullReferenceException if UI wiring or data is missing.
  - If _selectedMap is null, Start path is blocked with a warning.
- Threading
  - All operations assumed to run on Unity main thread (UI and game manager interactions).
- Data dependencies
  - MapDef must expose at least displayNameKey and baseLevel for UI and details text.
  - GameManager.StartMap must be callable with (string, MapDef).
- Serialization
  - mapSceneName and availableMaps must be serialized/assigned in the Unity Editor for proper behavior.

5) Example
- Not derivable from this file alone; usage is through Unity Editor and runtime when this component is attached.

6) Unknowns
- The full structure of MapDef (beyond displayNameKey and baseLevel) is not defined here.
- The exact behavior of GameManager.StartMap and DebugManager.Warning is not shown.
- Whether UI elements (MapList container, StartMap/Exit buttons, Details label) exist in all scenes or require specific UI structure.
```
