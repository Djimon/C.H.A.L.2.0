# CHAL.UI.RecipeDetailPanelView

_Automatically generated/updated from `Assets/src/UI/RecipeDetailPanelView.cs`._

```csharp
// Documentation for Assets/src/UI/RecipeDetailPanelView.cs
```

1) Purpose
- Defines a Unity MonoBehaviour (RecipeDetailPanel) that renders and manages a recipe detail UI panel.
- Exposes a public craft-click event and public methods to populate, clear, and show success/failure states.
- Handles UI wiring for icon, name, base stats, tooltip, ingredients, refinement controls, and gold costs.

2) Public API
- Namespace/module
  - CHAL.UI

- Types
  - public class RecipeDetailPanel : MonoBehaviour
    - Public fields/properties
      - public event Action OnCraftClicked;
    - Public methods
      - public void Clear()
      - public void ShowRecipeDetails(RecipeDef r,
                         CraftingService.RecipePreview preview,
                         int needGold, int haveGold,
                         Dictionary<string, int> haveByItemId)
      - public void ShowFail(string message)
      - public void ShowSuccess()

3) Key Behavior & Side Effects
- Awake
  - Resolves UIDocument reference (doc) and caches root VisualElement.
  - Looks up UI elements: icon, item-name, base-stats, implicits-tooltip, implicits-list, ingredients-list, refine-panel, refine-slider, refine-value, gold-label, craft-btn, fail-label.
  - Sets up:
    - Tooltip toggle on implicit-help click (if present).
    - OnCraftClicked invocation when craft button is clicked (if craft-btn found).
    - Refinement slider updates refine-value text on value change (if refine-slider found).
- Clear
  - Resets UI to empty/default state:
    - Name/base stats/icon cleared
    - Ingredients cleared
    - Gold label reset to "0/0 G" and fully visible
    - Craft button disabled
    - Fail label cleared
    - Tooltip and refine panel hidden
- ShowRecipeDetails
  - Populates UI from recipe data:
    - Name: r.displayKey if non-empty, else r.name
    - Base stats: placeholder text "base stats" (to be filled by GearStatsProvider later)
    - Icon: sets background image if r.icon present; otherwise clears it
  - Renders ingredients list (if r.inputs present):
    - For each input, creates a row with an icon placeholder and a text label showing "have/qty" (have defaults to 0 if not provided)
    - Dims ingredient text if have < qty
  - Gold line
    - Sets _goldLabel.text twice:
      - First to "needGold G"
      - Then to "haveGold/needGold G" (final value used)
  - Craft feasibility
    - Determines hasAllIngs by comparing haveByItemId against inputs (min 1 qty shown in UI)
    - hasGold = haveGold > needGold
    - finalCanCraft = hasAllIngs && hasGold && preview.canCraft
    - Enables/disables craft button accordingly
    - Sets _failLabel text to indicate missing ingredients, insufficient gold, or blocker from preview when not craftable
- ToggleTooltip
  - Toggles _tooltip display between None and Flex
  - If _tooltipList is empty, populates with placeholder items
- ShowFail / ShowSuccess
  - ShowFail: sets _failLabel to provided message (or empty if null)
  - ShowSuccess: clears _failLabel (comment: could trigger SFX externally)
- OnCraftClicked
  - Invoked when craft button is clicked (if any subscribers exist)

4) Constraints & Failure Modes
- Null handling and guards
  - doc may be null; if so, Awake tries to obtain UIDocument component; potential NRE if UIDocument is missing.
  - Some UI elements (e.g., _craftBtn, _refineSlider) are checked for null before wiring in Awake, but their usage in ShowRecipeDetails assumes non-null (e.g., _craftBtn.SetEnabled)—could NRE if not assigned.
  - _ingList, _tooltip, _tooltipList, and other UI elements are accessed with null-conditional patterns where used, but not everywhere; expect null if UI structure changes.
  - In ShowRecipeDetails, r.inputs and haveByItemId may be null; code guards for r.inputs, and uses null-coalescing for haveByItemId retrieval.
- Logic quirks
  - _goldLabel.text is assigned twice in ShowRecipeDetails; final value is the haveGold/needGold display.
  - Gold feasibility uses haveGold > needGold (strictly greater), not >=; effect depends on caller data.
  - Tooltip population uses hardcoded placeholders when empty.
- Threading/Unity
  - All UI updates occur on the main thread (Unity UIElements). No async/file IO here.
- Performance
  - Ingredients are rebuilt each call to ShowRecipeDetails; reasonable for typical UI refreshes but can allocate multiple VisualElement instances if called frequently.

5) Example
- Minimal usage pattern: subscribe to craft events and drive UI updates externally.
```csharp
using UnityEngine;
using CHAL.UI;
using CHAL.Systems.Crafting;
using System.Collections.Generic;

public class RecipeUIController : MonoBehaviour
{
    [SerializeField] private RecipeDetailPanel detailPanel;

    void Awake()
    {
        if (detailPanel != null)
            detailPanel.OnCraftClicked += OnCraftClicked;
    }

    void OnDestroy()
    {
        if (detailPanel != null)
            detailPanel.OnCraftClicked -= OnCraftClicked;
    }

    void OnCraftClicked()
    {
        // Handle crafting logic here
        Debug.Log("Craft button clicked!");
    }

    // Example: populate panel (data types assumed to exist in project)
    void ShowExample(RecipeDef recipe, CraftingService.RecipePreview preview)
    {
        int needGold = 100;
        int haveGold = 150;
        var haveByItemId = new Dictionary<string, int> { { "item_1", 2 } };
        detailPanel.ShowRecipeDetails(recipe, preview, needGold, haveGold, haveByItemId);
    }
}
```

6) Unknowns
- Definitions and structure of:
  - RecipeDef (properties like displayKey, name, icon, inputs)
  - CraftingService.RecipePreview (fields like canCraft, blocker)
- Exact UXML layout and what visual classes (e.g., ing-row, ing-icon, ing-text) map to in project styles
- External systems for filling base stats, gear stats, or refinement behavior
- Any additional behavior connected to refinement (beyond UI visibility)

Notes
- This file is Unity-specific and targets UIElements-based UI, with some German comments and placeholders for future data wiring.
