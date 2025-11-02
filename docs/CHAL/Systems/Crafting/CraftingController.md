# CHAL.Systems.Crafting.CraftingController

_Automatically generated/updated from `Assets/src/Systems/Crafting/CraftingController.cs`._

Purpose
- Defines a Unity MonoBehaviour that coordinates crafting UI, data, and interactions.
- Maintains visible recipes (filtered by unlocks) and preview/detail state.
- Interfaces with CraftingService to compute previews and perform crafting, and wires UI events.

Public API
- Namespace: CHAL.Systems.Crafting
- Types
  - public class CraftingController : MonoBehaviour
    - Public fields
      - CraftingCatalog catalog
        - Catalog of recipes exposed to crafting UI.
      - InventoryDomain inv
        - Central inventory system used for crafting inputs/outputs.
      - ResearchUnlockRegistry unlocks
        - Registry used to filter recipes by unlocks.
      - RecipeListView listView
        - UI list component; raises OnSelect when a recipe is chosen.
      - RecipeDetailPanel detailPanel
        - UI detail panel; raises OnCraftClicked for crafting attempts.
    - Public methods
      - None

Key Behavior & Side Effects
- Initialization and wiring
  - OnEnable: wires UI handlers via WireUI.
  - Awake: initializes _relevantInvIds (case-insensitive set).
  - Start: starts InitAfterOneFrame coroutine.
  - InitAfterOneFrame (coroutine)
    - Resolves _wallet from GameManager.Instance.Profile if available; logs warning if null.
    - Resolves unlocks from GameManager.Instance.ResearchUnlocks; logs warning if null.
    - If inv is null and GameManager is available, resolves inv from GameManager.Inventory; logs status.
    - If inv remains null, logs warning and aborts initialization.
    - Subscribes to inv.OnSlotChanged with HandleSlotChanged.
    - Calls RebuildRecipeList and RefreshPreviewAndDetail to initialize UI.
- Rebuild and refresh
  - RebuildRecipeList
    - Clears _visibleRecipes; handles null checks for inv, catalog, and catalog.recipes.
    - Warns if unlocks is null; filters catalog.recipes by unlocked status via unlocks.IsUnlockedRecipe.
    - Builds _visibleRecipes with unlocked recipes.
    - Builds craftableMap by computing previews (via CraftingService.GetPreview) when inv and wallet are available; stores per-recipe canCraft.
    - Updates listView data with visible recipes and craftableMap.
    - Preselects first recipe if any; otherwise clears selection and detail panel.
    - Rebuilds _relevantInvIds with the target output inventory and material input inventories for visible recipes.
  - RefreshPreviewAndDetail
    - If no selection or missing inv/wallet, clears detail panel and logs state.
    - Resolves output inventory id for the selected recipe.
    - If output id is missing, shows failure in detail panel.
    - Computes _preview via CraftingService.GetPreview; updates detail panel with recipe details, gold cost, and material counts.
- UI and event handling
  - HandleSelectRecipe: updates _selected and refreshes preview/detail.
  - HandleCraftClicked: attempts crafting via CraftingService.TryCraftToInventory; on failure shows mapped blocker reason and refreshes preview/detail; on success shows success state and refreshes preview/detail.
  - HandleSlotChanged: refreshes preview/detail only if the changed slot belongs to a relevant inventory id (_relevantInvIds).
- Helpers (read-only)
  - GetGoldNeed(RecipeDef r)
    - Sums gold costs from r.currencyCosts (only items with currencyId "gold").
  - ResolveOutputInventoryId(RecipeDef r)
    - Uses GameManager to resolve the outputItemId to an inventory type and instance; ensures the instance exists via EnsureInstance; returns the instance id or null on failure.
  - CountMaterials(RecipeDef r)
    - For each input itemId, determines a conventional instance id (e.g., player_remains, player_part, player_rune, player_module) based on ItemTypeUtils.FromId.
    - If a valid instance exists, sums counts across its slots for that itemId; returns a map of itemId -> total owned count.
  - MapBlockerToText(CraftBlocker blocker, string fallback)
    - Converts blocker values to user-facing messages, with fallback if provided.

Constraints & Failure Modes
- Null checks and guarded flows
  - Many early returns if inv, wallet, or catalog data are missing; warnings logged to DebugManager.
- Lifecycle and event management
  - OnDisable unsubscribes from inv.OnSlotChanged and unwires UI to avoid leaks.
  - InitAfterOneFrame uses a frame-delay to resolve dependencies reliably.
- UI/state coupling
  - If output inventory resolution fails, detail panel shows a failure message.
  - Crafting attempts propagate failure reason to the UI and refresh state accordingly.
- Performance
  - RebuildRecipeList iterates catalog recipes and constructs per-recipe previews when wallet/inv are available; reasonable for typical recipe counts.

Unknowns
- Exact shapes of types and data structures:
  - RecipeDef, RecipePreview, CraftBlocker, RecipeListView, RecipeDetailPanel, CraftingCatalog, InventoryDomain, ResearchUnlockRegistry, IWallet, and CraftingService behavior.
  - Details of CurrencyCosts structure (fields like currencyId, amount).
- Behavior of GameManager methods:
  - TryResolveByItemId, EnsureInstance, and overall inventory instantiation semantics.
- UI behavior specifics:
  - How detailPanel.ShowRecipeDetails and ShowFail/ShowSuccess render in the actual UI.
- Any external side effects not explicit in this file:
  - Additional listeners or side effects triggered by inventory changes beyond HandleSlotChanged.

Example
- Not applicable (no code examples visible beyond this file).
