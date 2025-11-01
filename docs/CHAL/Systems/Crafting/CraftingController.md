# CHAL.Systems.Crafting.CraftingController

_Automatically generated/updated from `Assets/src/Systems/Crafting/CraftingController.cs`._

1) Purpose
- CraftingController wires and orchestrates the crafting UI and data flow.
- It filters recipes by unlocks, computes craftability, and handles crafting interactions with inventory and wallet.
- It integrates catalog, inventory, unlocks, and UI components (list and detail panels).

2) Public API
- Namespace/module: CHAL.Systems.Crafting

- Types
  - public class CraftingController : MonoBehaviour

- Public fields
  - public CraftingCatalog catalog; // recipe catalog to use for listing
  - public InventoryDomain inv; // inventory domain used for crafting
  - public ResearchUnlockRegistry unlocks; // unlock gate for recipes
  - public RecipeListView listView; // UI: list of recipes
  - public RecipeDetailPanel detailPanel; // UI: recipe details and actions
  - public string outputInventoryId = "player:gear"; // target inventory for crafted items

- Public methods
  - None

- Public surface summary
  - The class is public and derives from MonoBehaviour; the public surface consists of its exposed fields above.

3) Key Behavior & Side Effects
- Unity lifecycle
  - OnEnable: wires UI (WireUI).
  - Awake: initializes _relevantInvIds and seeds with outputInventoryId.
  - Start: begins InitAfterOneFrame coroutine.
  - InitAfterOneFrame: after one frame, resolves wallet from GameManager, resolves inventory if possible, subscribes to inv.OnSlotChanged, rebuilds recipe list, and refreshes preview/detail.
  - OnDisable: unsubscribes from slot changes and unwires UI.

- Wiring
  - WireUI: subscribes listView.OnSelect to HandleSelectRecipe and detailPanel.OnCraftClicked to HandleCraftClicked.
  - UnwireUI: unsubscribes the above event handlers.

- Build & Refresh
  - RebuildRecipeList:
    - Clears current list; guards against null catalog/inv.
    - Filters catalog.recipes by unlocks.IsUnlockedRecipe when unlocks is present.
    - Builds _visibleRecipes; for each, computes craftability via CraftingService.GetPreview and populates a map of RecipeDef -> canCraft.
    - Updates listView with recipes and craftability map.
    - Preselects first recipe if any; otherwise clears selection and detail.
    - Recomputes _relevantInvIds by scanning recipe inputs and resolving item-ids to instance IDs via GameManager.
    - Logs visible recipe count.

  - RefreshPreviewAndDetail:
    - If no selection or missing wallet/inventory, clears detail panel.
    - Otherwise obtains a preview via CraftingService.GetPreview and displays details via detailPanel.ShowRecipeDetails with gold cost and material counts.

- User actions
  - HandleSelectRecipe: updates _selected and refreshes preview/detail.
  - HandleCraftClicked:
    - Validates selection and initialization; if not ready, shows failure.
    - Calls CraftingService.TryCraftToInventory; on failure shows mapped blocker text and refreshes preview/detail.
    - On success, logs, shows success in UI, and refreshes preview/detail.
  - HandleSlotChanged:
    - Refreshes preview/detail only if the changed slot’s instanceId is in _relevantInvIds.

- Helpers
  - GetGoldNeed(RecipeDef): sums gold costs from currencyCosts where currencyId == "gold".
  - CountMaterials(RecipeDef): counts how many of each input itemId are available in the relevant predefined instances (Remains/Part/Rune/Module) based on itemId conventions and current inventory.
  - MapBlockerToText(CraftBlocker, string): translates blockers to user-facing text (with fallback).

4) Constraints & Failure Modes
- Guards
  - If InventoryDomain (inv) is null during init, UI initialization is aborted with a warning.
  - If catalog or catalog.recipes is null, UI is cleared with a warning.
  - If wallet (_wallet) is null after initial frame, crafting UI initialization is warned but UI may still be shown (crafting may not work).
- Threading/async
  - Initialization uses a one-frame coroutine; UI wiring happens after a frame.
- Nullability
  - Many checks for nulls before proceeding (catalog, inv, unlocks, listView, detailPanel).
- State/Flow
  - Preselection occurs only if there are visible recipes.
  - Rebuild may update _relevantInvIds, influencing which slots trigger refreshes.
- Performance
  - Rebuild iterates catalog and per-recipe inputs to compute counts and previews; delegates heavy work to CraftingService and ItemTypeUtils.

5) Example
- Not derivable from this file alone (no public example surface provided). Omitted.

6) Unknowns
- Exact structure and semantics of:
  - RecipeDef (fields like inputs, currencyCosts, outputItemId, outputCount, etc.)
  - CraftBlocker enum and its full set of values beyond those handled
  - CraftingService.GetPreview and CraftingService.TryCraftToInventory behavior, side effects, and guarantees
  - ItemTypeUtils.FromId and mapping to ItemType (Remains, Part, Rune, Module)
  - GameManager.TryResolveByItemId and what instId represents in all cases
  - CraftingCatalog, RecipeListView, RecipeDetailPanel, InventoryDomain, and their internal expectations
  - Wallet currency retrieval via GetCurrency("gold") and wallet implementation
- How this interacts in multi-scene setups or when GameManager is null, beyond what’s logged
- Any concurrency nuances or race conditions if inventory changes occur during crafting flow
