# global.DebugCraftingRunner

_Automatically generated/updated from `Assets/src/Systems/_test/DebugCraftingRunner.cs`._

```text
1) Purpose
- Unity MonoBehaviour debug runner for CraftingService workflows; allows running a recipe, previewing state, and logging results.
- Exposes catalog, recipe index, and inventory IDs to configure which recipe to test and where to read/write materials and outputs.
- Supports simulating missing currency via a wallet proxy to observe success/failure behavior and state rollback during crafting.

```

```text
2) Public API
- Namespace/module: global (no explicit namespace)
- Types
  - public class CraftingDebugRunner : MonoBehaviour
    - Public fields
      - public CraftingCatalog catalog — source of recipes to test
      - public int recipeIndex — index of the recipe within catalog.recipes
      - public string materialsInventoryId — instanceId of the materials inventory
      - public string outputInventoryId — target inventory for crafted output
      - public bool runOnStart — whether to auto-run on Start
      - public bool simulateCurrencyMissing — if true, simulate currency missing during crafting
      - public int grantCrafts — used by GrantRequirements to determine how many crafts to simulate
    - Public methods
      - public void RunOnce() — executes one crafting attempt for the selected recipe; logs result and shows previews
      - public void GrantRequirements() — computes a preview and (when enabled) would fill materials/currency to satisfy craft requirements; currently has disabled sections
    - Notes on surface
      - Awake/Start are Unity lifecycle methods (not public API surface here; they are private by default in this file)
      - WalletProxyMissing is a private nested type (not part of public API)
```

```text
3) Key Behavior & Side Effects
- Awake()
  - Calls GameManager.Instance.TestInitInventory()
  - Caches _inv = GameManager.Instance.Inventory and _wallet = GameManager.Instance.Profile
- Start()
  - Ensures inventories exist/registered:
    - "player_parts" with PlayerInventoryType.Part
    - "All_Inventory" with PlayerInventoryType.all
  - If runOnStart is true, invokes RunOnce()
- RunOnce()
  - Retrieves recipe = catalog.recipes[recipeIndex]
  - Chooses wallet wrapper:
    - If simulateCurrencyMissing is true, uses WalletProxyMissing(_wallet); else uses _wallet
  - Logs the selected recipe name
  - PrintPreview(recipe)
  - Attempts crafting:
    - CraftingService.TryCraftToInventory(recipe, _inv, materialsInventoryId, _wallet, outputInventoryId, out var reason)
    - On success: logs success and destination
    - On failure: logs failure with reason
  - PrintPreview(recipe) to show post-state
- GrantRequirements()
  - Gets recipe and current preview: CraftingService.GetPreview(recipe, outputInventoryId, _inv, materialsInventoryId, _wallet)
  - (Commented-out sections) would fill materials and currency to satisfy craft requirements (disabled)
  - Recomputes after-state preview: after = CraftingService.GetPreview(...)
  - Logs whether crafting would be possible for x grantCrafts crafts
- PrintPreview(RecipeDef)
  - Calls CraftingService.GetPreview(...) and logs canCraft
  - Builds a small textual preview including Materials and Currency sections (actual lists are commented-out in this file)
- NameOf(RecipeDef)
  - Returns displayKey if present; otherwise r.name
- WalletProxyMissing (private nested class)
  - Implements IWallet
  - GetCurrency(string id) => 0
  - CanSpend(string id, int amt) => false
  - SpendCurrency(string id, int amt) => false
  - Refund(string id, int amt) delegates to inner wallet
  - Effect: simulates currency missing so crafting attempts fail due to lack of funds
```

```text
4) Constraints & Failure Modes
- Preconditions
  - catalog != null and catalog.recipes must be accessible; recipeIndex must be within bounds
  - inventory IDs (materialsInventoryId, outputInventoryId) must correspond to existing inventories after Start (EnsureInstance calls)
- Failure modes
  - CraftingService.TryCraftToInventory can fail; reason is logged
  - If simulateCurrencyMissing is true, currency spending is blocked via WalletProxyMissing, potentially causing craft to fail
  - GrantRequirements relies on GetPreview; if preview data is invalid, logging may be misleading
- Guards and handling
  - No null checks shown for catalog or recipe; runtime exceptions could occur if misconfigured
  - The materials/currency fill sections in GrantRequirements are currently commented out; no actual modification occurs during GrantRequirements
- Performance/allocation hints
  - Small, debug-oriented surface; uses string building for previews; no long-running operations on the main thread outside Unity updates
```

```text
5) Example
// Example: attach in a scene and run a single craft via script
// (Assumes you have a CraftingCatalog instance available as 'catalogInstance')
var go = new GameObject("CraftingDebugRunner");
var runner = go.AddComponent<CraftingDebugRunner>();
runner.catalog = catalogInstance;
runner.recipeIndex = 0;
runner.materialsInventoryId = "player_parts";
runner.outputInventoryId = "All_Inventory";
runner.runOnStart = false;
runner.simulateCurrencyMissing = false;
runner.RunOnce();
```

```text
6) Unknowns
- Details of CraftingCatalog, RecipeDef, and CraftingService implementations are not provided here
- Behavior of GameManager, Inventory, Wallet, and related types beyond their usage in this file
- Exact structure of CraftingService.GetPreview results and what constitutes canCraft beyond the boolean flag
- Any side effects of EnsureInstance or TestInitInventory not visible in this file
- Any threading implications or asynchronous behavior beyond Unity’s typical main-thread usage
```
