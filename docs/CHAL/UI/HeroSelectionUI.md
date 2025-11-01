# CHAL.UI.HeroSelectionUI

_Automatically generated/updated from `Assets/src/UI/HeroSelectionUI.cs`._

```text
1) Purpose
- Defines HeroSelectionUI, a UI controller for selecting heroes into map slots during a game.
- Maintains availableHeroes (unlocked heroes) and selectedHeroes (per-slot choices) based on the current map's heroSlots.
- Interfaces with MapManager and GameManager to configure the map, start waves, and navigate to hideout.

```

```csharp
2) Public API
- Namespace: CHAL.UI
- Types
  - public class HeroSelectionUI : IngameUI
    - Public fields/properties: none
    - Public methods:
      - public void Init(MapManager mapMGR)
```

```text
3) Key Behavior & Side Effects
- Awake
  - Obtains root VisualElement, ScrollView, and other UI elements via UI Toolkit queries.
  - Wires button callbacks:
    - ChooseHero -> OnChooseHeroClicked
    - StartWave -> OnStartWaveClicked
    - Hideout -> OnExitToHideoutClicked
  - Hides the hero details root initially.

- Init(MapManager mapMGR)
  - Stores a reference to MapManager.
  - Sets maxSlots from mapManager.CurrentMap.heroSlots.
  - Builds availableHeroes from the current profile's unlocked heroes (or empty).
  - Initializes selectedHeroes to have length maxSlots.
  - Toggles Slot1..Slot4 visibility based on maxSlots.

- OnSlotSelectClicked(int slot)
  - Sets currentSlot to the chosen slot.
  - Clears highlight on all slots and highlights the active one.
  - Makes heroRoot visible and fills the hero container with available heroes.

- FillHeroContainer()
  - Clears the hero container.
  - For each hero in availableHeroes, creates a Button labeled with the hero name.
  - Wires button click to OnHeroSelected(h) for that hero.
  - Adds the button to the hero container.

- OnHeroSelected(string h)
  - Sets _pendingHero to the selected hero.
  - Logs a debug message about showing details.
  - Updates heroDetails with the selected hero name.

- OnChooseHeroClicked()
  - Requires a valid currentSlot and a non-empty _pendingHero; otherwise returns.
  - Ensures selectedHeroes is initialized to length maxSlots.
  - If there is an existing hero in the current slot, and it's not in availableHeroes, it is re-added to availableHeroes.
  - Assigns the pending hero to selectedHeroes[currentSlot - 1].
  - Removes the chosen hero from availableHeroes.
  - Calls UpdateSlotVisual for the current slot with the chosen hero.
  - Clears _pendingHero and hides heroRoot.

- UpdateSlotVisual(int slot, string hero)
  - Determines the target avatar element (Slot1-4 variants).
  - If hero is null/empty: sets a gray placeholder (no image, gray background).
  - If a hero is set: clears image and assigns a random color (colorful placeholder before real image).

- OnExitToHideoutClicked()
  - Calls GameManager.Instance.ExitToHideout().

- OnStartWaveClicked()
  - Passes selectedHeroes to mapManager via SetSelectedHeroes.
  - Calls mapManager.StartWave().
  - Hides this UI (Show(false)).

- HighlightSlot(int slot, bool active)
  - Updates border colors on the appropriate avatar to indicate selection.

```

```text
4) Constraints & Failure Modes
- Init must be called before use to populate maxSlots, availableHeroes, and selectedHeroes; otherwise mapManager references or lists may be null.
- OnChooseHeroClicked guards against:
  - currentSlot <= 0
  - _pendingHero null/empty
- UpdateSlotVisual guards against invalid slot indices (target null) and handles empty vs. filled hero visuals.
- OnStartWaveClicked assumes mapManager is non-null; otherwise may throw.
- FillHeroContainer depends on availableHeroes; if empty, the container will be cleared with no buttons.
- Slot visibility is determined by maxSlots; slots beyond maxSlots are hidden.
- Closures in FillHeroContainer capture loop variable safely per foreach usage.

```

```text
5) Example
```csharp
// Example: initialize UI for a given MapManager
// assuming 'mapManager' is a valid MapManager instance in scope
var heroUI = FindObjectOfType<CHAL.UI.HeroSelectionUI>();
if (heroUI != null)
{
    heroUI.Init(mapManager);
}
```

```

```text
6) Unknowns
- Details of IngameUI base class behavior.
- The exact structure and sources of MapManager.CurrentMap.heroSlots.
- How MapManager.SetSelectedHeroes and StartWave affect game state beyond this file.
- The full behavior/shape of availableHeroes population (localization, persistence).
- Visual assets for heroes (currently using color placeholders; real images not implemented here).
- Threading considerations beyond Unity's main thread (none evident in this file).
```
