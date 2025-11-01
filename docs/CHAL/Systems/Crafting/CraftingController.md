# CHAL.Systems.Crafting.CraftingController

_Automatically generated/updated from `Assets/src/Systems/Crafting/CraftingController.cs`._

1) Purpose
- Defines a Unity MonoBehaviour that wires crafting UI to catalogs, inventory, and unlocks, and coordinates previews and execution of crafts.
- Builds and refreshes the visible recipe list, including gating by unlocked recipes and computing craftability previews.
- Responds to inventory changes, resolves target output inventories, and updates preview/detail UI accordingly.

2) Public API
- Namespace/module
  - CHAL.Systems.Crafting
- Types
  - public class CraftingController : MonoBehaviour
    - Public fields
      - public CraftingCatalog catalog; // recipe catalog
      - public InventoryDomain inv; // current inventory domain used for crafting
      - public ResearchUnlockRegistry unlocks; // unlock gating for recipes
      - public RecipeListView listView; // UI: list of recipes
      - public RecipeDetailPanel detailPanel; // UI: recipe details
    - Public methods
      - None (Unity lifecycle methods are declared with private visibility; public surface consists of the public fields above)

3) Key Behavior & Side Effects
- Unity lifecycle and wiring
  - OnEnable: WireUI() (connects UI events)
  - Awake: initializes _relevantInvIds as an empty set
  - Start: starts InitAfterOneFrame coroutine
  - InitAfterOneFrame (coroutine): after one frame
    - _wallet = GameManager.Instance?.Profile
    - Warns if wallet is null
    - If inv is null and GameManager exists, assigns inv from GameManager and logs status
    - If inv remains null, warns and stops initialization
    - Subscribes to inv.OnSlotChanged
    - Calls RebuildRecipeList()
    - Calls RefreshPreviewAndDetail()
  - OnDisable: unsubscribes from OnSlotChanged and calls UnwireUI

- Wiring
  - WireUI: subscribes listView.OnSelect to HandleSelectRecipe; subscribes detailPanel.OnCraftClicked to HandleCraftClicked
  - UnwireUI: unsubscribes those events

- Build & refresh flow
  - RebuildRecipeList:
    - Clears _visibleRecipes
    - If inv is null → clear detailPanel and listView data; log warning; return
    - If catalog or catalog.recipes is null → log warning; clear UI; return
    - Iterates catalog.recipes; skips null entries; if unlocks exists and IsUnlockedRecipe(r.name) is false, skip
    - Adds remaining recipes to _visibleRecipes
    - Builds craftableMap: for each visible recipe, resolves output inventory id; if null, uses a default non-craftable preview; otherwise uses CraftingService.GetPreview(r, outId, inv, _wallet) and stores p.canCraft
    - Applies data to listView.SetData(_visibleRecipes, craftableMap)
    - If there are visible recipes: preselect first via HandleSelectRecipe(_visibleRecipes[0]); else clear _selected and detailPanel
    - Rebuilds _relevantInvIds with outputs and required inputs (via GameManager.ResolveByItemId and instantiation checks)
    - Logs the count of visible recipes

- Preview & detail update
  - RefreshPreviewAndDetail:
    - If _selected is null or inv or _wallet is null → detailPanel.Clear and log null-state; return
    - Resolves outId for _selected; if unresolved → detailPanel.ShowFail("Ziel-Inventar unbekannt."); return
    - _preview = CraftingService.GetPreview(_selected, outId, inv, _wallet)
    - detailPanel.ShowRecipeDetails(_selected, _preview, GetGoldNeed(_selected), _wallet.GetCurrency("gold"), CountMaterials(_selected))

- Crafting & slot-change handling
  - HandleSelectRecipe(RecipeDef recipe): sets _selected and RefreshPreviewAndDetail()
  - HandleCraftClicked():
    - If _selected is null → return
    - If inv or _wallet is null → detailPanel.ShowFail("Systeme nicht initialisiert."); return
    - Resolve output inventory; if unresolved → detailPanel.ShowFail("Ziel-Inventar unbekannt."); return
    - ok = CraftingService.TryCraftToInventory(_selected, inv, _wallet, outId, out var reason)
    - If !ok: log craft fail, show failure via MapBlockerToText(_preview.blocker, reason), RefreshPreviewAndDetail()
    - If ok: log craft success, detailPanel.ShowSuccess(), RefreshPreviewAndDetail()
  - HandleSlotChanged(string instanceId, int slotIndex, ItemStack? newStack):
    - If instanceId is in _relevantInvIds, then RefreshPreviewAndDetail()

4) Constraints & Failure Modes
- Null safety
  - Handles null catalog/inv/wallet gracefully with warnings and UI resets
  - Validates output inventory resolution; shows user-facing failures when unresolved
- Dependency boundaries
  - Relies on external services/classes (CraftingService, GameManager, DebugManager, etc.) defined elsewhere
  - Unlock gating uses unlocks.IsUnlockedRecipe; absence of unlock registry is tolerated
- Async behavior
  - Initialization is deferred to the next frame via InitAfterOneFrame coroutine
- UI coupling
  - UI wiring is optional (null-checked) and safely detached on disable
- Localization/messages
  - German strings used for user-facing feedback (e.g., Ziel-Inventar unbekannt, Kein Platz im Zielinventar)

5) Example
- Not derivable from this file alone (no self-contained usage example provided)

6) Unknowns
- External type definitions not present in this file (CraftingCatalog, InventoryDomain, RecipeListView, RecipeDetailPanel, CraftingService, RecipeDef, CraftBlocker, ItemStack, ItemTypeUtils, etc.)
- Exact behavior of CraftingService.GetPreview, CraftingService.TryCraftToInventory, and CraftingService.RecipePreview
- How inventory structures (slots, stacks) are implemented beyond usage here
- Any side effects of GameManager.ResolveByItemId, EnsureInstance, or DebugManager specifics beyond their usage
- Any UI specifics of RecipeListView and RecipeDetailPanel beyond exposed events and assumed methods

Code references (for quick cross-check)
- Unity lifecycle: OnEnable, Awake, Start, OnDisable
- Events: inv.OnSlotChanged, listView.OnSelect, detailPanel.OnCraftClicked
- Helper methods: ResolveOutputInventoryId, GetGoldNeed, CountMaterials, MapBlockerToText
- Fabric: Guard clauses and null checks throughout RebuildRecipeList and RefreshPreviewAndDetail
