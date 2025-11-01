# CHAL.Systems.Crafting.MaterialLine

_Automatically generated/updated from `Assets/src/Systems/Crafting/CraftingService.cs`._

1) Purpose
- Static crafting service: preview, validate, and commit-atomic crafting for recipes.
- Data structures for material and currency previews, plus a recipe preview result.
- Interacts with InventoryDomain, IWallet, and RecipeDef to determine feasibility and perform crafts.

2) Public API
- Namespace: CHAL.Systems.Crafting

- Types

  - public struct MaterialLine (fields)
    - public string itemId
    - public int required
    - public int playerAmount
    - public bool enough
      - enough is true when playerAmount >= required

  - public struct CurrencyLine (fields)
    - public string currencyId
    - public int required
    - public int playerAmount
    - public bool enough
      - enough is true when playerAmount >= required

  - public readonly struct RecipePreview
    - public readonly bool canCraft
    - public readonly CraftBlocker blocker
    - public readonly bool outputOk
    - public readonly bool materialsOk
    - public readonly bool currencyOk
    - Constructor RecipePreview(bool canCraft, CraftBlocker blocker, bool outputOk, bool materialsOk, bool currencyOk)

- Public static methods

  - public static RecipePreview GetPreview(
      RecipeDef recipe,
      string outputInventoryId,
      InventoryDomain inv,
      string materialsInventoryId,
      IWallet wallet)

  - public static bool CanCraft(
      RecipeDef recipe,
      InventoryDomain inv,
      string outputInventoryId,
      string materialsInventoryId,
      IWallet wallet)

  - public static bool TryCraftToInventory(
      RecipeDef recipe,
      InventoryDomain inv,
      string materialsInventoryId,
      IWallet wallet,
      string outputInventoryId,
      out string failReason)

- Public enum

  - public enum CraftBlocker
    - None = 0
    - LockedByResearch
    - OutputInventoryFull
    - MissingMaterials
    - NotEnoughCurrency
    - InvalidRefinement
    - UnknownError

3) Key Behavior & Side Effects
- GetPreview flow
  - Builds an output stack: new ItemStack(recipe.outputItemId, Mathf.Max(1, recipe.outputCount)).
  - OutputOk: checks if the outputInventory can accept the stack.
  - MaterialsOk: reads the materials inventory instance; if missing or slots null, returns false; counts total have per needed itemId; requires at least need.qty (or 1 if at least 1) for every input.
  - GoldNeed: sums currencyCosts entries where currencyId is "gold".
  - CurrencyOk: wallet canSpend "gold" for the required amount (if any).
  - Guards in order: outputOk -> materialsOk -> currencyOk; blocker is set to first failing guard; canCraft is true only if all three ok.
  - Returns a RecipePreview describing feasibility and guard state.

- CanCraft flow
  - Delegates to GetPreview and returns its canCraft.

- TryCraftToInventory flow (atomar/rollback-capable)
  - Pre-check: output inventory capacity via CanAccept for the output stack; if false, fail with a message.
  - Guard: if CanCraft(...) is false, fail with "Requirements not met."
  - Commit phase (atomic with rollback on fail):
    - 1) TryConsumeMaterials: remove required materials from materialsInventoryId; record removed slots for rollback.
    - 2) TrySpendCurrencies: validate and spend currencies; record spent currencies for potential refund.
    - 3) TryAdd output: add produced item into outputInventoryId; on failure, rollback currencies and materials, report output-full fail reason.
  - On success, return true.

- Helper behavior (private API surface, not public)
  - CountOf: sum of itemId occurrences in an inventory instance.
  - TryConsumeMaterials: for each input, consume required quantity across slots with matching itemId; record (slot, oldStack, amount) for rollback.
  - RollbackMaterials: re-add previously removed items to their original itemIds/amounts; clear removal log.
  - TrySpendCurrencies: pre-checks all costs with wallet.CanSpend; then spends and records per-currency amounts; on failure, report reason.
  - RefundCurrencies: refunds spent currencies to wallet; clear spent-log.

4) Constraints & Failure Modes
- Output capacity guard: can only craft if output inventory can accept the resulting stack.
- Material checks are readonly in preview but material consumption is atomic during craft; rollback exists for material and currency failures.
- Currency handling is limited to costs defined in recipe.currencyCosts; gold is treated specially in GoldNeed and CurrencyOk.
- MaterialsOk guards rely on non-null inventory instance and non-null slots; otherwise false.
- Crafting is gated by sequential guards, ensuring deterministic blocker reason (OutputInventoryFull, MissingMaterials, NotEnoughCurrency, etc.).
- Atomicity: if any step in commit fails, previously consumed materials and currencies are rolled back, and any added output is reverted if necessary.

5) Example
- Not derivable from this file; no usage example provided.

6) Unknowns
- Definitions and behavior of RecipeDef, ItemStack, IInventoryDomain, and the exact semantics of InventoryDomain.GetInstance, Slot/Peek/TryRemove/TryAdd are not shown here.
- Exact UI coupling with CraftBlocker values beyond their names is not specified.
- Concurrency model, thread-safety, and how this integrates with other systems are not defined in this file.
