# CHAL.Systems.Crafting.CraftBlocker

_Automatically generated/updated from `Assets/src/Systems/Crafting/CraftingService.cs`._

```text
1) Purpose
- Defines a static CraftingService with preview and atomic commit logic for crafting recipes.
- Provides data structures for previewing craft requirements (MaterialLine, CurrencyLine, RecipePreview).
- Exposes high-level API: GetPreview, CanCraft, TryCraftToInventory; plus a public CraftBlocker enum to categorize blockers.

```

```csharp
2) Public API
- Namespace/module
  - CHAL.Systems.Crafting

- Types
  - public struct CraftingService.MaterialLine
    - public string itemId
    - public int required
    - public int playerAmount
    - public bool enough => playerAmount >= required

  - public struct CraftingService.CurrencyLine
    - public string currencyId
    - public int required
    - public int playerAmount
    - public bool enough => playerAmount >= required

  - public readonly struct CraftingService.RecipePreview
    - public readonly bool canCraft
    - public readonly CraftBlocker blocker
    - public readonly bool outputOk
    - public readonly bool materialsOk
    - public readonly bool currencyOk
    - public RecipePreview(bool canCraft, CraftBlocker blocker, bool outputOk, bool materialsOk, bool currencyOk)

- Public methods
  - public static RecipePreview GetPreview(RecipeDef recipe, string outputInventoryId, InventoryDomain inv, IWallet wallet)

  - public static bool CanCraft(RecipeDef recipe, InventoryDomain inv, string outputInventoryId, IWallet wallet)

  - public static bool TryCraftToInventory(RecipeDef recipe, InventoryDomain inv, IWallet wallet, string outputInventoryId, out string failReason)

- Enum (public)
  - CraftBlocker
    - None
    - LockedByResearch
    - OutputInventoryFull
    - MissingMaterials
    - NotEnoughCurrency
    - InvalidRefinement
    - UnknownError

```

```text
3) Key Behavior & Side Effects
- GetPreview
  - Builds an output ItemStack from recipe.outputItemId and recipe.outputCount (minimum 1).
  - OutputOk: checks if outputInventoryId can accept the output stack.
  - MaterialsOk: for each input in recipe.inputs, maps itemId to a materials inventory via convention, verifies instance exists, sums counts across slots, requires at least max(1, qty).
  - CurrencyOk: sums gold costs from recipe.currencyCosts; requires wallet.CanSpend("gold", amount) if gold cost > 0.
  - Overall canCraft = OutputOk && MaterialsOk && CurrencyOk.
  - Blocker set in priority: OutputInventoryFull -> MissingMaterials -> NotEnoughCurrency -> None.
  - Returns RecipePreview(canCraft, blocker, outputOk, materialsOk, currencyOk) without mutating state.

- CanCraft
  - Returns GetPreview(...).canCraft.

- TryCraftToInventory (atomic commit)
  - Validates output inventory acceptance first; on failure, failReason set and false.
  - Builds preview; if not preview.canCraft, failReason = blocker and return false.
  - Commit phase:
    - Consume materials from inventories (per material convention); records removed materials for rollback.
    - If any material consumption fails, roll back previously removed materials; failReason = "Missing materials: <itemId>".
    - Calculate required gold; attempt wallet.SpendCurrency("gold", amount); on failure, roll back materials; failReason = "Gold spend failed.".
    - Attempt to add output to outputInventoryId; on failure, refund spent gold and roll back materials; failReason = "Output inventory full: <id>".
  - On success, return true (state mutated: materials removed, currency spent, output added).

- Helpers (usage/behavior observed in code)
  - TryGetMaterialsInventoryIdByConvention: maps item types (Remains→player_remains, Part→player_part, Rune→player_rune, Module→player_module) to an inventory instance; requires inv.HasInstance(instanceId).
  - TrySpendCurrencies: pre-checks wallet.CanSpend for each currency cost; then spends each amount via wallet.SpendCurrency; records spent currencies.
  - RollbackMaterials: re-adds previously removed materials to their original slots as new stacks with the same counts.
  - RefundCurrencies: refunds previously spent currencies via wallet.Refund.

```

```text
4) Constraints & Failure Modes
- Null/empty materials: recipe.inputs null or empty => materials check passes.
- Currency handling: only gold is explicitly accumulated for preview; TrySpendCurrencies handles all currencyCosts, but preview logic specifically checks gold.
- Output capacity: initial CanAccept check is required before locking in a craft; otherwise immediate failure.
- Atomicity: TryCraftToInventory attempts to make the craft atomic by recording removals and rolling back on any failure, including material removal, currency spending, or output insertion.
- State visibility: GetPreview does not mutate inventory or wallet; TryCraftToInventory mutates inventory and wallet only on success.
- Threading: no explicit synchronization; behavior assumes single-threaded/controlled access per craft operation.
- External dependencies: RecipeDef, CurrencyCost, ItemStack, InventoryDomain, IWallet, and related APIs are assumed from other parts of the project; their exact behavior is not defined in this file.
- Unknown/Unsupported items: materials for unknown item types are ignored (TryGetMaterialsInventoryIdByConvention returns false for unknown types).

```

```text
5) Example
// Example usage (minimal)
var preview = CraftingService.GetPreview(recipe, "player_inventory", inv, wallet);
bool canCraft = CraftingService.CanCraft(recipe, inv, "player_inventory", wallet);

string failReason;
bool success = CraftingService.TryCraftToInventory(recipe, inv, wallet, "player_inventory", out failReason);
```

```text
6) Unknowns
- Exact structures and semantics of:
  - RecipeDef, Recipe inputs, and currencyCosts
  - CurrencyCost (fields like currencyId, amount)
  - ItemStack (fields like itemID, count, HasValue/Value)
  - IWallet (methods CanSpend, SpendCurrency, Refund)
  - InventoryDomain methods (CanAccept, GetInstance, HasInstance, TryAdd, TryRemove, etc.)
  - ItemTypeUtils.FromId(ItemId) and ItemType enum
- Any side effects beyond what is implemented here (e.g., events, analytics).
- Behavior when multiple currencies beyond gold are present in currencyCosts (beyond pre-checks and Spend logic).
