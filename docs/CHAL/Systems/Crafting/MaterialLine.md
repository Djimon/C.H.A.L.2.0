# CHAL.Systems.Crafting.MaterialLine

_Automatically generated/updated from `Assets/src/Systems/Crafting/CraftingService.cs`._

```text
Purpose
- Defines a static CraftingService in CHAL.Systems.Crafting.
- Provides preview, guard checks, and atomic commit logic for crafting a recipe into a target inventory.
- Exposes lightweight data types used for UI and decision logic: MaterialLine, CurrencyLine, RecipePreview, CraftBlocker.
```

```csharp
// Public API surface
namespace CHAL.Systems.Crafting
{
    public static class CraftingService
    {
        // ---- Data types used by the API/UI ----

        public struct MaterialLine
        {
            public string itemId;
            public int required;
            public int playerAmount;
            public bool enough => playerAmount >= required;
        }

        public struct CurrencyLine
        {
            public string currencyId;
            public int required;
            public int playerAmount;
            public bool enough => playerAmount >= required;
        }

        public readonly struct RecipePreview
        {
            public readonly bool canCraft;
            public readonly CraftBlocker blocker;

            // optional per-field flags for UI guidance
            public readonly bool outputOk;
            public readonly bool materialsOk;
            public readonly bool currencyOk;

            public RecipePreview(bool canCraft, CraftBlocker blocker,
                                 bool outputOk, bool materialsOk, bool currencyOk)
            {
                this.canCraft = canCraft;
                this.blocker = blocker;
                this.outputOk = outputOk;
                this.materialsOk = materialsOk;
                this.currencyOk = currencyOk;
            }
        }

        // ---- PREVIEW ----
        public static RecipePreview GetPreview(RecipeDef recipe, string outputInventoryId, InventoryDomain inv, IWallet wallet)

        public static bool CanCraft(RecipeDef recipe, InventoryDomain inv, string outputInventoryId, IWallet wallet)

        // ---- COMMIT (atomar) ----
        public static bool TryCraftToInventory(RecipeDef recipe, InventoryDomain inv, IWallet wallet, string outputInventoryId, out string failReason)
    }

    public enum CraftBlocker
    {
        None = 0,
        LockedByResearch,
        OutputInventoryFull,
        MissingMaterials,
        NotEnoughCurrency,
        InvalidRefinement,
        UnknownError
    }
}
```

```text
Public API (surface details)
- Namespace: CHAL.Systems.Crafting
- Public types:
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
    - None, LockedByResearch, OutputInventoryFull, MissingMaterials, NotEnoughCurrency, InvalidRefinement, UnknownError
```

```text
Key Behavior & Side Effects
- Preview generation (GetPreview)
  - Builds an output ItemStack from recipe.outputItemId and max(1, recipe.outputCount).
  - OutputOk: checks if outputInventory can accept the output stack.
  - MaterialsOk: for each input in recipe.inputs, maps itemId to a material inventory via convention, reads available counts across matching stacks, requires at least max(1, need.qty).
  - GoldNeed: sums gold costs from recipe.currencyCosts where currencyId == "gold".
  - CurrencyOk: true if gold needed <= 0 or wallet.CanSpend("gold", g) is true.
  - Blocker: first failing guard in order: OutputInventoryFull -> MissingMaterials -> NotEnoughCurrency -> None.
  - Returns a RecipePreview with canCraft, blocker, and per-condition flags (outputOk, materialsOk, currencyOk).

- Craft feasibility (CanCraft)
  - Returns GetPreview(...).canCraft.

- Atomic craft (TryCraftToInventory)
  - G0: Output must be accepted by the target inventory; on failure, failReason set and false returned.
  - G1: Guard check via GetPreview; if not craftable, failReason set to blocker.ToString() and false returned.
  - Commit phase:
    - Remove materials from their material inventories (per itemId) to fulfill recipe.inputs.
    - Rolls back material removal if any input cannot be satisfied (restores previously removed materials) and sets failReason to "Missing materials: {itemId}".
    - Currency deduction: computes total gold required; spends via wallet.SpendCurrency("gold", amount). Rolls back material removal and returns false on failure (failReason "Gold spend failed.").
    - Output addition: tries to add the crafted stack to outputInventoryId; on failure, refunds gold (if any), rolls back material removal, and returns false (failReason "Output inventory full: {outputInventoryId}").
  - On success: returns true (inventory and wallet updated atomically as far as possible within the function’s rollback moments).

- Material and currency handling
  - Materials are consumed from inventories determined by itemId -> convention mapping (Remain/Part/Rune/Module -> specific player inventories).
  - Gold is the only supported explicit currency in the preview and commit paths (true for currencyOk based on "gold" costs).
  - Rollback strategies exist for materials and currency to restore prior state on failure.

- Guard and error handling
  - Null/empty recipe inputs are treated gracefully in materials checks.
  - Output and currency checks drive explicit CraftBlocker states for UI guidance.
  - Fail reasons are surfaced as strings from TryCraftToInventory (e.g., "Output inventory cannot accept: ...", "Missing materials: ...", "Gold spend failed.", "Output inventory full: ...").

```

```text
Constraints & Failure Modes
- Null/empty handling
  - If recipe.inputs is null or empty, MaterialsOk is true.
  - If recipe.currencyCosts is null, Gold calculation yields 0.
- Currency handling
  - Only currencyId == "gold" is considered in gold-related checks and spending.
  - CurrencySpend is attempted only if total gold > 0.
  - On currency spend failure after material removal, materials are rolled back.
- Inventory commitments
  - Output must be able to accept the produced item stack before any changes.
  - If output cannot be accepted, no materials are consumed.
  - If output add fails after materials are consumed and currency spent, a partial rollback occurs (currency refund if spent; materials restored).
- Material mapping
  - Materials are resolved via convention-based mapping (itemId -> material inventory). If mapping fails or inventory instance is missing, materials cannot be consumed.
- Concurrency and atomicity
  - The commit is performed in a single method with in-function rollback logic; there is no cross-frame/async transactional support beyond in-method rollback.
- Known blockers (UI-facing)
  - CraftBlocker values indicate first gating reason: OutputInventoryFull, MissingMaterials, NotEnoughCurrency, None.

```

```text
Example
// Minimal usage example (syntax shows intended call pattern)
RecipeDef recipe = /* obtain recipe definition */;
InventoryDomain inv = /* obtain inventory domain for the player */;
IWallet wallet = /* obtain wallet reference */;
string outputInventoryId = "player_backpack";

if (CraftingService.TryCraftToInventory(recipe, inv, wallet, outputInventoryId, out var reason))
{
    // Craft succeeded; items consumed and output placed in inventory.
}
else
{
    // Craft failed; reason contains a user-friendly explanation.
}
```

```text
Unknowns
- Exact definitions of RecipeDef, InventoryDomain, IWallet, ItemStack, and related types are not provided here.
- Internal conventions for itemId to material inventory mapping (TryGetMaterialsInventoryIdByConvention) are based on ItemTypeUtils.FromId and specific instance IDs (e.g., "player_remains", "player_part", "player_rune", "player_module"), but their broader semantics are not defined in this file.
- Structure and contents of recipe.currencyCosts (beyond "gold" handling) are not shown.
- Any behavior outside this file’s public surface (e.g., external UI bindings) is not specified.
