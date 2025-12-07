# Assets/src/UI/HeroSelectionUI.cs

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
- Initializes UI elements and sets visibility based on the number of hero slots available.
- Updates the selected heroes and available heroes when a hero is chosen.
- Displays hero details when a hero is selected from the available list.
- Starts a wave with the selected heroes when the corresponding button is clicked.
- Highlights the selected slot and shows the hero selection UI when a slot is clicked.

# Constraints & Failure Modes
- Ensures that the current slot is valid and that a hero is pending before selection.
- Handles null or empty values for selected heroes and available heroes.
- UI elements are hidden or shown based on the number of available hero slots.

# Example
```csharp
var heroSelectionUI = new HeroSelectionUI();
heroSelectionUI.Init(mapManagerInstance);
```

# Unknowns
- The exact implementation details of the `IngameUI` class and `MapManager` class.
- The behavior of the `GameManager.Instance.Profile.GetUnlockedHeroes()` method.
