# Assets/src/UI/HeroSelectionUI.cs

_Automatic generated/updated._

```markdown
# Purpose
- Defines the `HeroSelectionUI` class for managing hero selection in the game UI.

# Public API
- Namespace: None
- Types
  - public class HeroSelectionUI : IngameUI
    - Public fields/properties:
      - List<string> availableHeroes: List of heroes available for selection.
      - List<string> selectedHeroes: List of currently selected heroes.
    - Public methods:
      - void Init(MapManager mapMGR): Initializes the UI with the provided map manager.
      - void OnChooseHeroClicked(): Handles the event when a hero is chosen.
      - void OnExitToHideoutClicked(): Exits to the hideout.
      - void OnStartWaveClicked(): Starts the wave with selected heroes.

# Key Behavior & Side Effects
- `Awake()`: Initializes UI elements and sets up button click events.
- `Init(MapManager mapMGR)`: Sets the maximum slots based on the current map and populates available heroes.
- `OnChooseHeroClicked()`: Updates the selected heroes and modifies the available heroes list.
- `OnSlotSelectClicked(int slot)`: Highlights the selected slot and fills the hero container with available heroes.
- `OnHeroSelected(string h)`: Sets the pending hero and updates the hero details display.

# Constraints & Failure Modes
- `OnChooseHeroClicked()`: Checks for valid current slot and pending hero before proceeding.
- `Init(MapManager mapMGR)`: Handles null profile gracefully by using an empty array for unlocked heroes.
- UI elements are only displayed based on the number of available slots.

# Example
```csharp
HeroSelectionUI heroSelectionUI = new HeroSelectionUI();
heroSelectionUI.Init(mapManagerInstance);
```

# Unknowns
- The exact implementation details of `GameManager.Instance.Profile` and `mapManager.CurrentMap.heroSlots`.
- The behavior of `DebugManager.Log` and its impact on performance.
```
