# CHAL.UI.MapSelectionUI

_Automatically generated/updated from `Assets/src/UI/MapSelectionIUI.cs`._

# Purpose
- Defines a Unity UI component MapSelectionUI for selecting and starting a map.
- Dynamically builds a list of map selection buttons from availableMaps in Awake.
- Bridges UI interactions to game logic (start map, exit menu) and updates details display.

# Public API
- Namespace/module: CHAL.UI
- Types
  - public class MapSelectionUI : IngameUI
    - Public fields/properties:
      - mapSceneName: Scene name for the map to start (default "04_Map").
      - availableMaps: List of available maps for selection.
    - Public methods: none

# Key Behavior & Side Effects
- Awake (Unity lifecycle)
  - Calls base.Awake().
  - Retrieves root VisualElement from the UIDocument on this GameObject.
  - Locates the MapList container, clears it, and creates a Button per item in availableMaps.
  - Each map button sets up a click handler to OnMapSelected(map).
  - Locates StartMap and Exit buttons and wires their click handlers to OnStartMapBtnClicked and OnExitMenuBtnClicked.
  - Locates the Details label for showing selected map info.
- OnExitMenuBtnClicked
  - Hides the UI via Show(false).
- OnMapSelected(MapDef map)
  - Sets _selectedMap to the chosen map.
  - Updates detailsText to show the map name and base monster level.
- OnStartMapBtnClicked
  - If no map is selected (_selectedMap is null), logs a warning via DebugManager.Warning("No map selected!", "UI") and returns.
  - Otherwise, calls GameManager.Instance.StartMap(mapSceneName, _selectedMap) to start the map (centralized start logic).
- Note: mapSceneName defaults to "04_Map" and is passed along with the selected map to the start logic.

# Constraints & Failure Modes
- Potential null/reference risks
  - availableMaps is not null before iteration; there is no guard, so null leads to NullReferenceException in Awake.
  - root = GetComponent<UIDocument>() may fail if UIDocument is missing; then rootVisualElement access will fail.
  - container = root.Q<VisualElement>("MapList"); if not found, container is null and container.Clear() would throw.
  - btnStartMap = root.Q<Button>("StartMap"); if not found, btnStartMap is null and adding a listener will throw.
  - btnExitMenu = root.Q<Button>("Exit"); if not found, same null risk.
  - detailsText = root.Q<Label>("Details"); if not found, detailsText is null and later access causes NRE.
- External dependencies
  - Display names rely on map.displayNameKey (string) being meaningful; actual localization is noted as a TODO in code.
  - GameManager.Instance.StartMap(mapSceneName, _selectedMap) is external to this file; behavior depends on GameManager.
  - Show(false) relies on IngameUI implementation for hiding UI.
- Runtime assumptions
  - Awake runs on initialization and UI root exists.
  - UIElements API (Q, Button, Label) is available and matches the queried names.
- Performance/allocation
  - Creates a Button per availableMaps item at startup; no per-frame allocations noted.

# Example
- Not provided, as surface usage relies on project-wide types (MapDef, GameManager, DebugManager) and Unity scene setup not shown in this file.

# Unknowns
- MapDef type details (structure of displayNameKey, baseLevel, etc.) are not defined in this file.
- Exact behavior of GameManager.StartMap and what scene loading entails.
- Exact behavior of DebugManager.Warning and any UI blocking during startup.
- Whether availableMaps can be null or modified at runtime after Awake.
- Localization specifics (how displayNameKey maps to text) are not defined here.
- The type relationship and exact nature of IngameUI (base class) and Show(bool) behavior.

