# global.DebugCraftingRunner

_Automatically generated/updated from `Assets/src/Systems/_test/DebugCraftingRunner.cs`._

1) Purpose
- Defines CraftingDebugRunner, a Unity MonoBehaviour to run and inspect crafting flows against a catalog recipe.
- Provides a private WalletProxyMissing helper to simulate currency-spend failure for testing.
- Hooks into GameManager to initialize inventories and obtain InventoryDomain and IWallet references for crafting operations.

2) Public API
- Namespace/module: global namespace (no explicit namespace in file)

- Types
  - public sealed class CraftingDebugRunner : MonoBehaviour
    - Public fields (inspector-configurable):
      - CraftingCatalog catalog
        - Catalog of recipes used by the debug runner.
      - int recipeIndex
        - Index into catalog.recipes for the recipe to test.
      - string materialsInventoryId
        - Inventory instanceId for input materials (e.g., "player_parts").
      - string outputInventoryId
        - Inventory instanceId for craft output (e.g., "All_Inventory").
      - bool runOnStart
        - If true, RunOnce() is invoked automatically on Start().
      - bool simulateCurrencyMissing
        - If true, uses a wallet proxy that cannot spend currency (for failure testing).
      - int grantCrafts
        - Multiplier for how many crafts to simulate in GrantRequirements (used in preview/logs).
    - Public methods:
      - void RunOnce()
        - Executes a craft for the configured recipe using the configured inventories and wallet; logs result and prints post-state preview.
      - void GrantRequirements()
        - Logs a preview after attempting to determine required resources; includes commented-out scaffolding to auto-grant materials/currency.
    - Context:
      - RunOnce and GrantRequirements are decorated with [ContextMenu] allowing editor menu invocation.

- Nested/private API (not public surface)
  - private sealed class WalletProxyMissing : IWallet
    - Constructor WalletProxyMissing(IWallet inner)
    - GetCurrency(string id): int
    - CanSpend(string id, int amt): bool
    - SpendCurrency(string id, int amt): bool
    - Refund(string id, int amt): void
    - Purpose: simulate currency spend failure by delegating refunds to inner wallet but blocking spends.

3) Key Behavior & Side Effects
- Awake():
  - Calls GameManager.Instance.TestInitInventory().
  - Sets private fields _inv = GameManager.Instance.Inventory and _wallet = GameManager.Instance.Profile.
- Start():
  - Calls GameManager.Instance.EnsureInstance("player_parts", PlayerInventoryType.Part).
  - Calls GameManager.Instance.EnsureInstance("All_Inventory", PlayerInventoryType.all).
  - If runOnStart is true, invokes RunOnce().
- RunOnce():
  - Reads recipe = catalog.recipes[recipeIndex].
  - Chooses wallet = simulateCurrencyMissing ? new WalletProxyMissing(_wallet) : _wallet.
  - Logs the recipe being tested.
  - Calls CraftingService.TryCraftToInventory(recipe, _inv, materialsInventoryId, _wallet, outputInventoryId, out var reason).
  - On success: logs success and output placement.
  - On failure: logs warning with reason.
  - Calls PrintPreview(recipe) to show post-state.
- GrantRequirements():
  - Reads recipe = catalog.recipes[recipeIndex].
  - Calls CraftingService.GetPreview(recipe, outputInventoryId, _inv, materialsInventoryId, _wallet) to capture current preview.
  - (Commented out blocks show intended material/currency augmentation steps for testing.)
  - Calls CraftingService.GetPreview(recipe, outputInventoryId, _inv, materialsInventoryId, _wallet) again to capture post-grant preview.
  - Logs whether canCraft for x grantCrafts crafts.
- PrintPreview(RecipeDef):
  - Calls CraftingService.GetPreview(recipe, outputInventoryId, _inv, materialsInventoryId, _wallet).
  - Builds and logs a small summary:
    - canCraft flag
    - Materials section (commented out in code)
    - Currency section (commented out in code)
- NameOf(RecipeDef):
  - Returns r.displayKey if non-empty; otherwise r.name.
- WalletProxyMissing (tests):
  - When simulateCurrencyMissing is true, craft attempts may fail due to GetCurrency/CanSpend/SpendCurrency overrides to simulate missing currency.

4) Constraints & Failure Modes
- Run outcomes depend on CraftingService.TryCraftToInventory result; on failure, reason is logged.
- simulateCurrencyMissing flag switches to WalletProxyMissing, which causes currency spending to fail (spend attempts return false and GetCurrency returns 0).
- Start requires that catalog and inventories are properly set up; EnsureInstance calls may create inventories if absent.
- Editor-only controls:
  - RunOnce and GrantRequirements can be invoked from the Unity editor via context menu.
- No explicit threading or asynchronous behavior beyond Unity’s lifecycle; operations follow synchronous calls to CraftingService in this file.

5) Example
- Minimal usage in Unity (inspector or programmatic setup)

Programmatic setup example:
```csharp
// Example: attach and configure CraftingDebugRunner at runtime
public class SetupExample : MonoBehaviour
{
    public CraftingCatalog catalog;

    void Start()
    {
        var go = new GameObject("CraftingDebugRunner");
        var runner = go.AddComponent<CraftingDebugRunner>();
        runner.catalog = catalog;
        runner.recipeIndex = 0;
        runner.materialsInventoryId = "player_parts";
        runner.outputInventoryId = "All_Inventory";
        runner.runOnStart = true;
        runner.simulateCurrencyMissing = false;
        runner.grantCrafts = 1;
    }
}
```

6) Unknowns
- Exact structure and members of CraftingCatalog, RecipeDef, and the contents of CraftingService.GetPreview/TryCraftToInventory are not defined in this file.
- Details of IWallet, Wallet implementations beyond the WalletProxyMissing behavior here are not shown.
- Behavior of GameManager, InventoryDomain, and related inventory initialization are not defined in this file; their behavior is assumed from usage.
- The precise format of the CraftingPreview data (materials, currencies) is not defined here; only usage surfaced through CraftingService.GetPreview.
- Any side effects of CraftingService methods beyond what is logged are not described in this file.

