# global.HeroSelectionUI

_Automatically generated/updated from `Assets/src/UI/HeroSelectionUI.cs`._

# HeroSelectionUI.cs

## Purpose
- Defines the UI for hero selection in the game.
- Manages the selection and display of available heroes and their details.

## Public API
- Namespace: None
- Types:
  - public class HeroSelectionUI : IngameUI
    - Public fields/properties:
      - List<string> availableHeroes: List of heroes available for selection.
      - List<string> selectedHeroes: List of currently selected heroes.
    - Public methods:
      - void Init(MapManager mapMGR): Initializes the UI with the provided MapManager.
      - void OnChooseHeroClicked(): Handles the event when a hero is chosen.
      - void OnExitToHideoutClicked(): Exits to the hideout.
      - void OnStartWaveClicked(): Starts the wave with selected heroes.

## Key Behavior & Side Effects
- `Awake()`: Initializes UI elements and sets up button click events.
- `Init(MapManager mapMGR)`: Sets the maximum number of hero slots based on the current map and populates available heroes.
- `OnChooseHeroClicked()`: Updates the selected hero in the current slot and manages available heroes.
- `OnSlotSelectClicked(int slot)`: Highlights the selected slot and fills the hero container with available heroes.
- `OnHeroSelected(string h)`: Sets the pending hero and updates the hero details display.

## Constraints & Failure Modes
- Ensures that the current slot is valid and that a hero is pending before selection.
- Handles null or empty values for selected heroes and available heroes.
- Uses a fixed number of slots based on the map configuration.

## Example
```csharp
var heroSelectionUI = new HeroSelectionUI();
heroSelectionUI.Init(mapManagerInstance);
```

## Unknowns
- The exact implementation details of `GameManager.Instance.Profile.GetUnlockedHeroes()`.
- The behavior of `mapManager.SetSelectedHeroes(selectedHeroes)` and `mapManager.StartWave()`.

