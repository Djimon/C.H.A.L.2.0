# Assets/src/UI/MapSelectionIUI.cs

_Automatic generated/updated._

```markdown
## Purpose
- Defines the `MapSelectionUI` class for managing map selection in the game UI.

## Public API
- Namespace: None
- Types
  - public class `MapSelectionUI` [extends `IngameUI`]
    - Public fields/properties:
      - `mapSceneName`: Name of the scene to load for the map.
      - `availableMaps`: List of available maps for selection.
    - Public methods:
      - `void Awake()`: Initializes UI elements and sets up button callbacks.
      - `void OnExitMenuBtnClicked()`: Hides the map selection UI.
      - `void OnMapSelected(MapDef map)`: Updates the selected map and displays its details.
      - `void OnStartMapBtnClicked()`: Starts the selected map or shows a warning if none is selected.

## Key Behavior & Side Effects
- `Awake`: Initializes UI components and populates map buttons based on `availableMaps`.
- `OnMapSelected`: Updates the selected map and modifies the details text.
- `OnStartMapBtnClicked`: Checks if a map is selected before starting the map; logs a warning if not.

## Constraints & Failure Modes
- If `_selectedMap` is null when `OnStartMapBtnClicked` is called, a warning is logged.
- UI elements are expected to be present in the `UIDocument`.

## Example
```csharp
// Example usage in a Unity scene
MapSelectionUI mapSelectionUI = new MapSelectionUI();
mapSelectionUI.Awake();
```

## Unknowns
- The structure and properties of `MapDef` are not defined in this file.
- The behavior of `GameManager.Instance.StartMap` is not detailed in this file.
```
