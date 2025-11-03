# CHAL.Systems.Crafting.CurrencyLine

_Automatically generated/updated from `Assets/src/Systems/Crafting/CraftingService.cs`._

Purpose
- Exposes a static CraftingService for previewing and executing recipe crafts.
- Defines lightweight data structures used by the UI (MaterialLine, CurrencyLine, RecipePreview).
- Provides a public CraftBlocker enum to describe why crafting is blocked.
- Adds detailed logging for output rejection in the GetPreview method.

Public API
- Namespace/Module
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

  - public static class CraftingService
    - public static RecipePreview GetPreview(RecipeDef recipe, string outputInventoryId, InventoryDomain inv, IWallet wallet)
    - public static bool CanCraft(RecipeDef recipe, InventoryDomain inv, string outputInventoryId, IWallet wallet)
    - public static bool TryCraftToInventory(RecipeDef recipe, InventoryDomain inv, IWallet wallet, string outputInventoryId, out string failReason)

  - public enum CraftBlocker
    - None = 0
    - LockedByResearch
    - OutputInventoryFull
    - MissingMaterials
    - NotEnoughCurrency
    - InvalidRefinement
    - UnknownError

Key Behavior & Side Effects
- Preview flow (GetPreview)
  - Builds an output ItemStack from recipe.outputItemId and recipe.outputCount (min 1).
  - OutputOk: checks inv.CanAccept(outputInventoryId, outputStack); logs detailed rejection via DebugOutputReject on failure.
  - MaterialsOk: for each recipe input, resolves a materials inventory by convention, scans slots to sum counts of the needed itemId, requires at least need.qty (min 1).
  - GoldNeed: sums currency costs where currencyId == "gold".
  - CurrencyOk: wallet.CanSpend("gold", g) if gold > 0.
  - Blocker: determined in order: OutputInventoryFull -> MissingMaterials -> NotEnoughCurrency -> None.
  - Returns RecipePreview with canCraft and the per-condition flags.

- Craft decision (CanCraft)
  - Delegates to GetPreview and returns its canCraft result.

- Commit path (TryCraftToInventory)
  - G0: Output must be Acceptable; otherwise fail with reason.
  - G1: Guards (via GetPreview). If not craftable, fail with blocker.ToString().
  - Commit phase:
    - Remove inputs (TryConsumeOne) from their material inventories; tracks removed items for rollback.
    - If any material removal fails, rollback removed materials and fail with Missing materials.
    - Consume currency (TrySpendCurrencies); on failure rollback materials and fail with reason.
    - Add output to outputInventoryId; on failure, refund currency (if spent) and rollback materials; fail with Output inventory full.
  - Success: returns true.

- Helpers (behavioral notes)
  - DebugOutputReject logs detailed, actionable info when output is rejected (used during preview).
  - TryGetMaterialsInventoryIdByConvention maps item types to a specific player material inventory (Remains/Part/Rune/Module); unknown gear returns false (no material inventory).
  - RollbackMaterials and Refunds provide containment for transactional semantics on failure.

- Notes on usage
  - The preview logic is used to drive UI states (canCraft, blockers, and specific flags).
  - The TryCraftToInventory method performs an atomic-like operation with manual rollback on failure.

Constraints & Failure Modes
- Guards
  - Output acceptance must succeed before composing a preview.
  - If any material cannot be mapped to a material inventory, MaterialsOk fails.
  - Currency spending gated by Gold (currencyId == "gold"); non-gold currencies are not treated here.
- Rollback semantics
  - On material removal failure or currency spend failure, previously removed items are re-added to their slots.
  - If output addition fails after currency spend, spent currency is refunded.
- Null/empty handling
  - recipe.inputs null or empty implies materials are not required.
  - currencyCosts null implies no currency is required.
- Conventions and limitations
  - Material inventory resolution relies on item type conventions; gear/unknown items do not map to a material inventory.
- Performance notes
  - Preview and commit paths walk inventory slots; no explicit async/parallel semantics here.
- Logging
  - Detailed logging is invoked on output rejection during preview (DebugOutputReject).

Example
- Minimal usage scenario

```csharp
// Assume existing recipe, inventory, wallet, and outputInventoryId are available
var preview = CraftingService.GetPreview(recipeDef, "player_inventory", inventory, wallet);
if (preview.canCraft)
{
    if (CraftingService.TryCraftToInventory(recipeDef, inventory, wallet, "player_inventory", out string failReason))
    {
        // Craft succeeded
    }
    else
    {
        // Craft failed; inspect failReason
        Debug.Log($"Craft failed: {failReason}");
    }
}
```

Unknowns
- Exact structure and members of RecipeDef, InventoryDomain, IWallet, and ItemStack are not defined here beyond their usage.
- Internal behavior of ItemTypeUtils.FromId and TryGetMaterialsInventoryIdByConvention outside this file.
- DebugManager, its log levels, and the exact logging side effects.
- Any external constraints on CraftBlocker values beyond naming; no explicit guarantees about UI semantics.
- Any multithreading considerations or Unity lifecycle interactions beyond typical single-threaded usage in this file.

