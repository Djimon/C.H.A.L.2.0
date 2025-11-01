# CHAL.Systems.Crafting.CraftingController

_Automatically generated/updated from `Assets/src/Systems/Crafting/CraftingController.cs`._

```text
1) Purpose
- Defines CraftingController MonoBehaviour that coordinates crafting UI, catalog, and inventory interactions.
- Builds and refreshes the visible recipe list from catalog, filtered by unlocks.
- Manages preview/detail UI and crafting actions, reacting to inventory changes.

2) Public API
- Namespace/module
  - CHAL.Systems.Crafting

- Types
  - public class CraftingController : MonoBehaviour
    - Public fields
      - CraftingCatalog catalog: Catalog of recipes.
      - InventoryDomain inv: Inventory to use for crafting inputs and outputs.
      - ResearchUnlockRegistry unlocks: Unlock gating for recipes.
      - RecipeListView listView: UI list showing available recipes.
      - RecipeDetailPanel detailPanel: UI panel showing recipe details and craft action.
      - string materialsInventoryId: Inventory id for materials (default "player:materials").
      - string outputInventoryId: Inventory id for crafted output (default "player:gear").
    - Public methods
      - None.

3) Key Behavior & Side Effects
- OnEnable
  - Subscribes to inventory slot changes if inv is set.
  - Wires UI event handlers.
  - Rebuilds the recipe list.
- Start
  - Captures wallet from GameManager.Instance.Profile.
- OnDisable
  - Unsubscribes from inventory slot changes.
  - Unwires UI event handlers.
- RebuildRecipeList
  - Clears _visibleRecipes.
  - If catalog or catalog.recipes is null: logs a warning, clears UI, and returns.
  - Filters catalog.recipes by unlocks.IsUnlockedRecipe(r.name) when unlocks is provided.
  - Updates listView with visible recipes; preselects first item if any; otherwise clears detail panel.
- RefreshPreviewAndDetail
  - If _selected, inv, or _wallet is null: clears detail panel.
  - Otherwise, computes _preview via CraftingService.GetPreview(_selected, outputInventoryId, inv, _wallet).
  - Calls detailPanel.Show with (_selected, _preview, GetGoldNeed(_selected), _wallet.GetCurrency("gold"), CountMaterials(_selected)).
- HandleSelectRecipe
  - Sets _selected and refreshes preview/detail.
- HandleCraftClicked
  - If no _selected or missing inv/_wallet: shows failure and exits.
  - Calls CraftingService.TryCraftToInventory(_selected, inv, _wallet, outputInventoryId, out var reason).
  - On failure: logs info, shows failure text via MapBlockerToText(_preview.blocker, reason), refreshes preview/detail.
  - On success: logs success, shows success, refreshes preview/detail.
- HandleSlotChanged
  - If the changed slot belongs to materialsInventoryId or outputInventoryId: refreshes preview/detail.

- Helpers
  - GetGoldNeed(RecipeDef r)
    - Sums gold costs in r.currencyCosts; returns 0 if none.
  - CountMaterials(RecipeDef r)
    - Builds a mapping of needed itemId -> total count available in relevant inventory instances.
    - Uses ItemTypeUtils.FromId to map itemId to an instanceId (Remains/Part/Rune/Module) and sums matching stacks.
  - MapBlockerToText(CraftBlocker blocker, string fallback)
    - Maps CraftBlocker values to user-facing German messages; uses fallback if blocker is None or unknown.

4) Constraints & Failure Modes
- Null/empty guards
  - RebuildRecipeList handles null catalog or catalog.recipes.
  - RefreshPreviewAndDetail requires _selected, inv, and _wallet to proceed.
  - Craft path guards inv and _wallet; potential null _preview if crafting fails before a preview is generated.
- Unlock gating
  - Filtering relies on unlocks.IsUnlockedRecipe(r.name); exact key mapping depends on implementation (note in code comment).
- Inventory access risks
  - CountMaterials assumes instance IDs derived from item type exist; if not present or inventory missing slots, results may be zero.
- Safety notes
  - HandleSlotChanged only refreshes when relevant inventories change; other inventory changes are ignored for UI efficiency.
- Performance
  - CountMaterials iterates slots of relevant instances; performance scales with number of recipes and slot count.

5) Example
- Not derivable from this file alone; no minimal usage example provided.

6) Unknowns
- Exact definitions and behavior of:
  - CraftingCatalog, RecipeDef, RecipeListView, RecipeDetailPanel, InventoryDomain, IWallet, CraftingService, CraftBlocker, CraftingCatalog.recipes, and Currency handling beyond GetCurrency("gold").
  - The internal structure and contents of RecipePreview and how CraftingService.GetPreview constructs it.
  - The exact unlocking keys used by ResearchUnlockRegistry (the code uses recipe.name as key).
  - ItemTypeUtils.FromId behavior and the mapping to instance IDs (player_remains, player_part, etc.) and how inventories store those instances.
  - Any side effects of CraftingService.TryCraftToInventory beyond the out reason and its interaction with inventories.
```
