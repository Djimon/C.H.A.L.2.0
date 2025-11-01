# CHAL.Systems.Crafting.CurrencyLine

_Automatically generated/updated from `Assets/src/Systems/Crafting/CraftingService.cs`._

```text
Section: Purpose
- Defines the crafting service logic for previewing and performing crafts.
- Provides data structures for previewing craft requirements (materials, currency, output) and a blocker indicator.
- Exposes public API to preview craft feasibility, check craft capability, and execute an atomic craft into a target inventory.

```

```csharp
Section: Public API
- Namespace/module
  - CHAL.Systems.Crafting

- Types
  - public struct MaterialLine
    - public string itemId
    - public int required
    - public int playerAmount
    - public bool enough => playerAmount >= required

  - public struct CurrencyLine
    - public string currencyId
    - public int required
    - public int playerAmount
    - public bool enough => playerAmount >= required

  - public readonly struct RecipePreview
    - public readonly bool canCraft
    - public readonly CraftBlocker blocker
    - public readonly bool outputOk
    - public readonly bool materialsOk
    - public readonly bool currencyOk
    - public RecipePreview(bool canCraft, CraftBlocker blocker, bool outputOk, bool materialsOk, bool currencyOk)

- Public API (methods)
  - public static RecipePreview GetPreview(RecipeDef recipe, string outputInventoryId, InventoryDomain inv, IWallet wallet)
  - public static bool CanCraft(RecipeDef recipe, InventoryDomain inv, string outputInventoryId, IWallet wallet)
  - public static bool TryCraftToInventory(RecipeDef recipe, InventoryDomain inv, IWallet wallet, string outputInventoryId, out string failReason)

- Public types (enum)
  - public enum CraftBlocker
    - None = 0
    - LockedByResearch
    - OutputInventoryFull
    - MissingMaterials
    - NotEnoughCurrency
    - InvalidRefinement
    - UnknownError

```

```text
Section: Key Behavior & Side Effects
- Preview flow (GetPreview)
  - Builds an output ItemStack from recipe.outputItemId and at least 1 unit.
  - OutputOk: checks if target inventory can accept the output stack.
  - MaterialsOk: for every input material, maps itemId to a materials-inventory via convention, then sums available counts across relevant slots; requires at least max(1, need.qty) per input.
  - CurrencyOk: sums gold costs (only currencyId "gold") and checks wallet.CanSpend("gold", amount).
  - Overall canCraft = OutputOk && MaterialsOk && CurrencyOk.
  - Blocker prioritization: OutputInventoryFull > MissingMaterials > NotEnoughCurrency > None; returned in RecipePreview.blocker.
- Craft execution flow (TryCraftToInventory)
  - G0: Output must be acceptible; otherwise failReason set to inventory message.
  - G1: Evaluate preview; if not craftable, failReason set to blocker name and abort.
  - Commit phase:
    - Remove materials atomically per recipe.inputs, recording removed material details for rollback.
    - If material removal fails for any input, rollback previously removed materials and fail with "Missing materials: {itemId}".
    - Spend currencies (gold) if needed; on failure, rollback material removals and fail with message.
    - Add output to target inventory; on failure, refund gold (if spent) and rollback material removals; fail with "Output inventory full: {outputInventoryId}".
  - Success: craft completed; returns true.
- Helpers and private flows
  - TryGetMaterialsInventoryIdByConvention: maps item types to physical player inventories (Remains/Part/Rune/Module); returns false for Gear/Unknown.
  - TryConsumeOne: consumes a quantity of a given itemId from the mapped materials inventory; records removal for rollback.
  - RollbackMaterials: re-adds previously removed material portions to restore state.
  - TrySpendCurrencies / RefundCurrencies: safe pre-check before spending currencies; refund support on failure.
  - CountOf / TryConsumeMaterials / RollbackMaterials: internal utilities used by crafting logic.

```

```text
Section: Constraints & Failure Modes
- Null/empty handling
  - If recipe.inputs is null or empty, MaterialsOk() returns true.
- Inventory checks
  - Output must be able to accept the output stack before any operation.
  - Rollback path exists for material or currency failures to restore prior state.
- Material handling
  - Materials are consumed from convention-mapped inventories; if mapping fails, materials cannot be validated or consumed.
  - Rollback relies on recording original slot state to re-add on failure.
- Currency handling
  - Gold costs are aggregated from recipe.currencyCosts where currencyId == "gold".
  - Spend is attempted only after materials are prepared; on failure, rollback occurs.
  - If output addition fails after spending, gold is refunded.
- Error signaling
  - Failures produce a failReason string (e.g., "Output inventory full: ...", "Missing materials: ...", or blocker name).
  - The blocker enum provides deterministic failure categories for UI/UX.
- Concurrency/async
  - All operations are synchronous; no explicit threading or async behavior.
- External dependencies (not defined in this file)
  - RecipeDef, ItemStack, InventoryDomain, IWallet, and related systems are assumed to provide the used methods (e.g., CanAccept, GetInstance, Remove/Add operations, wallet spend/refund).

```

```text
Section: Example
```csharp
// Example: preview and attempt a craft to a specific inventory
var preview = CraftingService.GetPreview(recipe, "player_inventory", inv, wallet);
if (preview.canCraft)
{
    if (CraftingService.TryCraftToInventory(recipe, inv, wallet, "player_inventory", out var reason))
    {
        // Craft succeeded
    }
    else
    {
        // reason contains the failure message (e.g., blocker or missing materials)
    }
}
```

```

```text
Section: Unknowns
- Exact definitions and behavior of external types used here:
  - RecipeDef, ItemStack, InventoryDomain, IWallet, ItemTypeUtils, ItemType, and their members/methods.
- Semantics of inventory slot layout, and how TryAdd/TryRemove/CanAccept interact with specific inventory implementations.
- Any side effects beyond the described flows (e.g., eventing, UI updates) are not visible in this file.
- Any pricing/currency rules beyond the hard-coded "gold" currency are not defined here.
```
