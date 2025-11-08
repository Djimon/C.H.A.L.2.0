# CHAL.UI.HeroSelectionUI

_Automatically generated/updated from `Assets/src/UI/HeroSelectionUI.cs`._

# Purpose
- Manages the user interface for selecting heroes in the game.

# Public API
- Namespace: CHAL.UI
- Types
  - public class HeroSelectionUI : IngameUI
    - Public fields/properties
      - List<string> availableHeroes: List of heroes available for selection.
      - List<string> selectedHeroes: List of heroes currently selected.
    - Public methods
      - void Init(MapManager mapMGR): Initializes the map manager and sets up the hero slots.
      - void OnChooseHeroClicked(): Handles the event when a hero is chosen.
      - void OnExitToHideoutClicked(): Exits to the hideout.
      - void OnStartWaveClicked(): Starts the wave with the selected heroes.

# Key Behavior & Side Effects
- On initialization, the UI sets up hero slots based on the current map's hero slots.
- Clicking on a hero in the hero container updates the pending hero for the selected slot.
- Selecting a slot highlights it and displays the available heroes for selection.
- When a hero is chosen, it updates the selected heroes and modifies the available heroes list.

# Constraints & Failure Modes
- If the current slot is invalid or no hero is pending, the hero selection will not proceed.
- The selected heroes list is initialized to the maximum number of slots; if it is null or smaller, it is re-initialized.
- The UI elements are only displayed if the corresponding slots are available based on the map's configuration.

# Example
```csharp
var heroSelectionUI = new HeroSelectionUI();
heroSelectionUI.Init(mapManagerInstance);
```

# Unknowns
- The exact implementation details of `GameManager.Instance.Profile` and how it retrieves unlocked heroes.
- The behavior of `DebugManager.Log` and its impact on performance or functionality.

