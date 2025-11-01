# CHAL.UI.RecipeDetailPanelView

_Automatically generated/updated from `Assets/src/UI/RecipeDetailPanelView.cs`._

```text
1) Purpose
- Defines a Unity UI panel (CHAL.UI.RecipeDetailPanel) that presents recipe details and manages craft interactions.
- Exposes an OnCraftClicked event to notify when the craft action is triggered.
- Maintains references to UI elements (icon, name, stats, tooltip, ingredients, refinement controls, gold, craft button, and feedback label) and provides methods to clear and populate the panel.

2) Public API
- Namespace/module
  - CHAL.UI
- Types
  - public sealed class RecipeDetailPanel : MonoBehaviour
    - Public event Action OnCraftClicked
    - Public void Clear()
      - Resets UI state (name, stats, icon, ingredients, gold, button enabled state, tooltip, refine panel)
    - Public void Show(RecipeDef r,
                         CraftingService.RecipePreview preview,
                         int needGold, int haveGold,
                         Dictionary<string, int> haveByItemId)
      - Populates panel with recipe data, renders ingredients, updates gold display, enables/disables craft button
    - Public void ShowFail(string message)
      - Displays failure message in the fail label
    - Public void ShowSuccess()
      - Clears fail message (potentially shows success via external effects)
  - (Internal/private fields and methods are not part of the public API surface)

3) Key Behavior & Side Effects
- Awake
  - Locates UIDocument if not assigned; retrieves root VisualElement; queries and stores UI element references; wires help tooltip click to ToggleTooltip; subscribes craft button to OnCraftClicked; hooks refine slider to update refine value label on changes.
- Clear
  - Clears name, base stats, icon, ingredients list; resets gold; disables craft button; clears fail/tooltip/ refine UI.
- Show
  - Sets name (prefers displayKey if present), assigns base stats placeholder, updates icon, renders ingredients with per-item counts and availability coloring, updates gold display and opacity if funds are insufficient, updates craft button enabled state based on preview.canCraft, hides refinement panel for now.
- ShowFail
  - Sets the fail label to the provided message.
- ShowSuccess
  - Clears the fail label (allows external effects for actual success feedback).
- ToggleTooltip
  - Toggles tooltip display between none and flex; lazily populates tooltip list with placeholder items if empty.

4) Constraints & Failure Modes
- Null/Missing References
  - doc defaults to GetComponent<UIDocument>() if not provided; many UI element lookups are guarded (e.g., _ingList may be null; _craftBtn and _refineSlider are checked before use).
  - _tooltip or help may be null; actions guarded accordingly.
- Data Safety
  - Ingredients rendering handles null r.inputs; haveByItemId may be null; safe fallback to 0 have quantity when missing.
  - Icon and material resources are guarded; missing icon results in no background image.
- UI Toolkit specifics
  - Uses UIElements (VisualElement, Label, SliderInt, Button); relies on names/id strings to query elements.
- Performance/ allocations
  - Ingredient rows are created per Show call; tooltips are populated only when first shown.

5) Example
// Example usage (surface pattern; types assumed from project)
var panel = FindObjectOfType<CHAL.UI.RecipeDetailPanel>();
panel.OnCraftClicked += () => Debug.Log("Craft requested");

// Prepare data (RecipeDef r, CraftingService.RecipePreview preview, haveByItemId)
panel.Show(r, preview, needGold: 100, haveGold: 50, haveByItemId: new Dictionary<string,int>());

6) Unknowns
- Exact definitions of RecipeDef and CraftingService.RecipePreview beyond what is used here (fields like displayKey, name, icon, inputs, and canCraft).
- The full structure and content of RecipeDef.inputs (type, members like itemId and qty) beyond their usage.
- Behavior/content of base stats population (currently a placeholder string).
- The specific UI layout and styling defined in the UXML/Styles (classes like ing-row, ing-icon, ing-text) are external to this file.
- Any external side effects triggered by ShowFail/ShowSuccess (beyond label updates) or by external systems consuming OnCraftClicked.
- Any additional data bindings or lifecycle interactions not shown in this single file.
