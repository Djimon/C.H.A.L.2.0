# CHAL.Systems.Crafting.CraftBlocker

_Automatically generated/updated from `Assets/src/Systems/Crafting/CraftingService.cs`._

1) Purpose
- Central crafting service for previewing and committing CraftingRecipe operations against inventory and wallet.
- Exposes lightweight UI-friendly preview data (RecipePreview, with per-field flags) and public craft-check API.
- Implements atomic commit of materials, currency, and output with rollback on failure; includes internal helpers for material/currency handling.
- Adds detailed logging for output rejection in preview flow.

2) Public API
- Namespace: CHAL.Systems.Crafting

- public static class CraftingService
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
  - public static RecipePreview GetPreview(RecipeDef recipe, string outputInventoryId, InventoryDomain inv, IWallet wallet)
  - public static bool CanCraft(RecipeDef recipe, InventoryDomain inv, string outputInventoryId, IWallet wallet)
  - public static bool TryCraftToInventory(RecipeDef recipe, InventoryDomain inv, IWallet wallet, string outputInventoryId, out string failReason)

- public enum CraftBlocker
  - None
  - LockedByResearch
  - OutputInventoryFull
  - MissingMaterials
  - NotEnoughCurrency
  - InvalidRefinement
  - UnknownError

3) Key Behavior & Side Effects
- Preview flow (GetPreview)
  - Builds expected output ItemStack (at least 1, with recipe.outputCount)
  - OutputOk: checks inv.CanAccept for the output slot; logs detailed rejection via DebugOutputReject on failure
  - MaterialsOk: if inputs exist, verifies presence of materials by convention (TryGetMaterialsInventoryIdByConvention) and counts in inventory instances
  - CurrencyOk: sums gold costs; checks wallet.CanSpend("gold", amount)
  - Blocker resolution: OutputInventoryFull > MissingMaterials > NotEnoughCurrency > None
  - Returns RecipePreview with canCraft (overall), blocker, and per-group flags (outputOk, materialsOk, currencyOk)

- Commit flow (TryCraftToInventory)
  - Output guard: requires inv.CanAccept(outputInventoryId, outStack)
  - Build preview; if not preview.canCraft, fail with blocker.ToString()
  - Collect removals for materials as a list (removed)
  - Consume materials per input using TryConsumeOne (which uses TryGetMaterialsInventoryIdByConvention)
  - If any material consumption fails, rollback removed materials to inventory, set failReason, return false
  - Currency handling: compute gold total; if >0, spend via wallet.SpendCurrency("gold", amount); on failure rollback materials and fail
  - Output: attempt to add output to inventory; on failure, refund gold (if spent) and rollback materials; fail with Output failure reason
  - On success, return true

- Debugging helpers
  - DebugOutputReject invoked when output cannot be accepted to log detailed per-slot state for debugging

- Internal material/currency helpers
  - TryGetMaterialsInventoryIdByConvention maps material item types to player inventory IDs (Remains/Part/Rune/Module) and checks existence
  - CountOf, TryConsumeMaterials, RollbackMaterials, TrySpendCurrencies, RefundCurrencies support internal accounting/rollback flows (not public)

4) Constraints & Failure Modes
- Null/empty handling
  - recipe.inputs null or empty treated as no-materials
  - currency costs checked for null before summing
- Currency handling
  - Only "gold" currency is recognized in GoldNeed; other currencies are ignored in preview
  - Currency spend is attempted only if gold > 0
- Rollback semantics
  - If material consumption or currency spend fails, previous changes are rolled back to keep state consistent
  - Output addition failure triggers a gold refund (if spent) and material rollback
- Guard/order logic
  - Preview blockers are derived in a defined order to guide UI (OutputInventoryFull, MissingMaterials, NotEnoughCurrency)
- Threading/async
  - All operations are synchronous in this file; no explicit async behavior
- External dependencies
  - Public API relies on external types: RecipeDef, InventoryDomain, ItemStack, IWallet, and debug/logging utilities

5) Example
- Not applicable (no direct code example derivable from this file)

6) Unknowns
- Exact definitions and members of RecipeDef, InventoryDomain, IWallet, ItemStack, and related inventory operations are not defined in this file
- Behavior of DebugManager.Log and DebugOutputReject beyond what’s shown here
- Any additional currency types beyond "gold" are not used in preview; their handling is not defined in this file

