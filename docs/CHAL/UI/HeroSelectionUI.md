# CHAL.UI.HeroSelectionUI

_Automatically generated/updated from `Assets/src/UI/HeroSelectionUI.cs`._

```csharp
# Documentation: Assets/src/UI/HeroSelectionUI.cs

1) Purpose
- Defines a Unity UI component HeroSelectionUI for selecting heroes into map slots and starting waves.
- Manages available vs. selected heroes, per-slot visuals, and pending hero details while interacting with MapManager and game state.
- Wires up UI events (slot selection, hero picking, Start Wave, Hideout) and initializes slot visibility based on the map's heroSlots.

2) Public API
- Namespace/module
  - CHAL.UI
- Types
  - public class HeroSelectionUI : IngameUI
    - Public fields/properties: 
      - List<string> availableHeroes
      - List<string> selectedHeroes
    - Public methods
      - public void Init(MapManager mapMGR)

3) Key Behavior & Side Effects
- Awake (protected override)
  - Binds UI elements from the UIDocument rootVisualElement.
  - Hooks up event handlers:
    - ChooseHero.clicked -> OnChooseHeroClicked
    - BtnSlot1/2/3/4.clicked -> OnSlotSelectClicked(1/2/3/4)
    - StartWave.clicked -> OnStartWaveClicked
    - Hideout.clicked -> OnExitToHideoutClicked
  - Hides the hero root details panel initially.
- Init(MapManager mapMGR)
  - Stores mapManager reference.
  - Sets maxSlots from mapManager.CurrentMap.heroSlots.
  - Loads availableHeroes from the current profile (GetUnlockedHeroes) or empty if no profile.
  - Initializes selectedHeroes to a new list sized to maxSlots.
  - Shows/hides Slot1..Slot4 visuals based on maxSlots.
- OnChooseHeroClicked()
  - Requires a valid currentSlot (>0) and a non-empty _pendingHero.
  - Ensures selectedHeroes is initialized to length maxSlots.
  - If an old hero exists in the current slot and is not in availableHeroes, re-adds it to availableHeroes.
  - Moves _pendingHero into the selected slot; removes it from availableHeroes.
  - Updates the slot visual via UpdateSlotVisual.
  - Clears _pendingHero and hides the hero detail panel.
- UpdateSlotVisual(int slot, string hero)
  - Updates the corresponding slot avatar (Slot1/2/3/4) visual.
  - If hero is empty/null: shows a gray placeholder (backgroundColor) with no image.
  - If hero is set: clears image and assigns a random colorful background (placeholder for future avatar).
- FillHeroContainer()
  - Clears heroContainer.
  - Iterates availableHeroes to create a Button per hero; each button selects that hero (OnHeroSelected).
  - Note: TODO to subtract already-selected heroes from availableHeroes (not implemented here).
- OnExitToHideoutClicked()
  - Triggers GameManager.Instance.ExitToHideout().
- OnStartWaveClicked()
  - Passes selectedHeroes to the map via mapManager.SetSelectedHeroes(selectedHeroes).
  - Calls mapManager.StartWave() and hides this UI (Show(false)).
- OnSlotSelectClicked(int slot)
  - Sets currentSlot to the chosen slot.
  - Visually un-highlights all slots, then highlights the selected one.
  - Shows the hero root panel and populates heroContainer with available heroes (FillHeroContainer).
- OnHeroSelected(string h)
  - Stores pending hero in _pendingHero.
  - Logs a debug message.
  - Updates heroDetails with the selected hero name (HeroName label).
- HighlightSlot(int slot, bool active)
  - Applies a colored border to the corresponding slot avatar (left/right/top/bottom colors).
  - Active = true uses a yellowish highlight; false uses gray.

4) Constraints & Failure Modes
- Init(MapManager) must be called before usage to populate mapManager, maxSlots, and available/selected heroes.
- Access to mapManager or mapManager.CurrentMap.heroSlots may throw if mapManager or its current map is null.
- UI element lookups (root.Q<...>("name")) assume UI elements exist; mismatches return nulls and can cause null refs when used.
- selectedHeroes is reinitialized in OnChooseHeroClicked() only if null or too-short; may lead to unexpected state if maxSlots changes or Init not called first.
- FillHeroContainer() currently does not subtract already-selected heroes from availableHeroes (TODO in code).
- OnStartWaveClicked() assumes mapManager is initialized; otherwise NullReferenceException.
- Random color usage in UpdateSlotVisual() is a placeholder; no actual avatar imagery is provided.
- Threading: all UI operations occur on Unity main thread as typical.

5) Example
- Minimal usage (derivable from Init and public API):
  - var heroUI = FindObjectOfType<CHAL.UI.HeroSelectionUI>();
  - heroUI.Init(myMapManager);

6) Unknowns
- Details of IngameUI base class behavior (Show/Hide, lifecycle).
- Implementations of MapManager, GameManager, DebugManager, and how they expose Profile, CurrentMap, and StartWave/ExitToHideout.
- How hero avatars/images are to be populated beyond the placeholder color logic.
- Exact UI structure and the resolution of UI element names in the Unity editor (names assumed by root.Q calls).
- Any external side effects from selecting heroes beyond SetSelectedHeroes and StartWave.
```
