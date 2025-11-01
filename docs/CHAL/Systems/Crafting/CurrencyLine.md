# CHAL.Systems.Crafting.CurrencyLine

_Automatically generated/updated from `Assets/src/Systems/Crafting/CraftingService.cs`._

```text
Purpose
- Implements a crafting service with preview and atomic commit flows against inventories and a wallet.
- Defines data types for preview results and simple material/currency lines used by UI.
- Exposes public API to preview, check feasibility, and attempt crafting to a target inventory.

```

```csharp
CHAL.Systems.Crafting

Public API

Namespace/Module
- CHAL.Systems.Crafting

Types

public struct MaterialLine
- public string itemId;
- public int required;
- public int playerAmount;
- public bool enough => playerAmount >= required;

public struct CurrencyLine
- public string currencyId;
- public int required;
- public int playerAmount;
- public bool enough => playerAmount >= required;

public readonly struct RecipePreview
- public readonly bool canCraft;
- public readonly CraftBlocker blocker;
- public readonly bool outputOk;
- public readonly bool materialsOk;
- public readonly bool currencyOk;
- public RecipePreview(bool canCraft, CraftBlocker blocker, bool outputOk, bool materialsOk, bool currencyOk)

public static RecipePreview GetPreview(RecipeDef recipe, string outputInventoryId, InventoryDomain inv, string materialsInventoryId, IWallet wallet)

public static bool CanCraft(RecipeDef recipe, InventoryDomain inv, string outputInventoryId, string materialsInventoryId, IWallet wallet)

public static bool TryCraftToInventory(
    RecipeDef recipe,
    InventoryDomain inv,
    string materialsInventoryId,
    IWallet wallet,
    string outputInventoryId,
    out string failReason)

public enum CraftBlocker
- None = 0
- LockedByResearch
- OutputInventoryFull
- MissingMaterials
- NotEnoughCurrency
- InvalidRefinement
- UnknownError

```

```csharp
Key Behavior & Side Effects

- GetPreview
  - Builds an ItemStack for the recipe output (at least 1 unit).
  - OutputOk: checks if the output inventory can accept the produced stack.
  - MaterialsOk: checks all recipe inputs exist in the materials inventory with required amounts.
  - CurrencyOk: computes gold-costs and checks wallet.CanSpend for "gold" currency; treats null costs as zero.
  - Blocker: first failing guard in order -> OutputInventoryFull, MissingMaterials, NotEnoughCurrency, None.
  - Returns a RecipePreview with canCraft and per-guard flags (outputOk, materialsOk, currencyOk).

- CanCraft
  - Convenience wrapper: returns GetPreview(...).canCraft.

- TryCraftToInventory (atomic commit path)
  - G0: Verify output inventory capacity (CanAccept) for the produced item; on fail, failReason set and false returned.
  - G1: Requires CanCraft(recipe, ...); on fail, failReason = "Requirements not met." and false.
  - Commit Phase (atomar with rollback on failure):
    - 1) TryConsumeMaterials: remove required materials from materialsInventoryId; record removals for rollback; on failure, rollback materials and return false.
    - 2) TrySpendCurrencies: verify and then spend currencies; on failure, rollback materials, refund spent currencies, return false.
    - 3) Add output: inv.TryAdd(outputInventoryId, outStack, out addTx); on failure, rollback materials, refund currencies, set failReason to "Output inventory full: ..." and false.
  - On success, returns true.

- Helpers (internal/private)
  - CountOf: sum of a given itemId in a given inventory instance.
  - TryConsumeMaterials: consumes required inputs, records removed stacks for rollback.
  - RollbackMaterials: re-adds consumed items to their slots.
  - TrySpendCurrencies: validates all currency costs first, then spends and records what was spent.
  - RefundCurrencies: refunds all spent currencies.

- Side effects
  - Mutates inventories (material removal, output add) only on success of all prior steps; otherwise rolls back.
  - Wallet is charged only if all material checks pass and after successful material consumption.
  - FailReason is populated on failure paths.

- Assumptions evident in code
  - Gold currency id used for currency checks/stores is "gold".
  - Output stack size defaults to at least 1 unit.
  - Currency costs may be null or empty; treated as zero-cost.
  - Rollback assumes there is space to re-add previously removed materials.

```

```csharp
Constraints & Failure Modes

- Guards/guards ordering
  - OutputInventoryFull checked before materials and currency checks in the blocker logic.
  - Materials and currency checks are performed only if prior guards pass in preview/commit flows.
- Null handling
  - MaterialsOk guards against null instantiation or null slots in the materials inventory.
  - Currency checks skip when recipe.currencyCosts is null.
- Currency handling
  - Gold costs are read via recipe.currencyCosts; spends only if wallet.CanSpend returns true for all costs.
- Atomicity and rollback
  - If material removal or currency spend fails, previously removed materials are rolled back.
  - If output add fails, currencies are refunded and materials rolled back.
- Concurrency considerations
  - No explicit threading; flows imply serial execution with explicit rollback.
- Data surface constraints
  - Uses external types: RecipeDef, InventoryDomain, IWallet, ItemStack, etc. Their behavior is assumed to be defined elsewhere.
- Potential edge cases
  - If recipe.inputs contains an item not present in materials inventory, MaterialsOk will fail.
  - If output inventory cannot accept the new item, TryCraftToInventory fails and nothing is consumed or charged.

```

```csharp
Example

// Acquire a preview
var preview = CraftingService.GetPreview(recipe, "OutputInv", playerInv, "MaterialsInv", wallet);
if (preview.canCraft)
{
    string reason;
    bool success = CraftingService.TryCraftToInventory(
        recipe,
        playerInv,
        "MaterialsInv",
        wallet,
        "OutputInv",
        out reason
    );
    // handle success/failure and reason
}
```

```text
Unknowns

- Definitions and behavior of:
  - RecipeDef
  - InventoryDomain
  - IWallet
  - ItemStack
  - Inventory operations: CanAccept, GetInstance, TryAdd, Peek, SlotCount, TryRemove
- Exact semantics of Refinement and related features (Referenced in comments as potential future use)
- Thread-safety guarantees and how concurrent crafting requests are coordinated
```
