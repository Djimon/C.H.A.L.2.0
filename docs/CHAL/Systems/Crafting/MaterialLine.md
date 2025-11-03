# CHAL.Systems.Crafting.MaterialLine

_Automatically generated/updated from `Assets/src/Systems/Crafting/CraftingService.cs`._

```csharp
// Documentation for Assets/src/Systems/Crafting/CraftingService.cs

1) Purpose
- Provides preview, validation, and commit logic for crafting items.
- Encapsulates material/currency checks and atomic commit with rollback on failure.
- Exposes a small public surface for consumer code to query craftability and perform crafts.

2) Public API

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

Notes on surface:
- RecipePreview is a public readonly struct used to convey craftability and per-category statuses.
- TryCraftToInventory returns a success flag and provides a failReason if crafting cannot proceed.

3) Key Behavior & Side Effects

- GetPreview flow
  - Builds an intended output ItemStack from recipe.outputItemId and recipe.outputCount (minimum 1).
  - OutputOk: checks if outputInventoryId can accept the output; logs detailed info if not.
  - MaterialsOk: for each recipe input, resolves the material’s inventory by convention, reads available counts from the corresponding inventory instance, and verifies sufficient quantity (at least max(1, need.qty)).
  - GoldNeed: sums gold currency costs from recipe.currencyCosts where currencyId equals "gold".
  - CurrencyOk: verifies wallet can spend the required gold.
  - Determines blocker in priority: OutputInventoryFull > MissingMaterials > NotEnoughCurrency > None.
  - Returns RecipePreview with combined canCraft flag and per-category flags.

- CanCraft flow
  - Delegates to GetPreview and returns its canCraft value.

- TryCraftToInventory flow (atomically attempts craft)
  - [G0] Output capability: ensures outputInventoryId can accept the output.
  - [G1] Guards read-only: computes GetPreview; if not craftable, returns false with blocker as failReason.
  - Commit phase:
    - Collects material removals in a list.
    - For each recipe input, attempts to remove required quantities from material inventories (via TryConsumeOne); if any input cannot be satisfied, rolls back previously removed materials, sets failReason to "Missing materials: {itemId}", and returns false.
    - Currency handling: computes total gold required; spends via wallet if needed; on failure, rolls back material removals.
    - Output handling: tries to add the produced item to the output inventory; on failure, refunds spent gold (if any) and rolls back material removals.
  - Returns true on complete success; false with an explanatory failReason on any guard/commit failure.

- Helper behaviors (internal to the file)
  - TryGetMaterialsInventoryIdByConvention maps material itemId types to player inventory identifiers (Remains/Part/Rune/Module) or null for other gear, and checks inventory presence.
  - DebugOutputReject logs detailed diagnostic information when OutputOk fails.

4) Constraints & Failure Modes

- Null/empty handling
  - Materials validation skips if recipe.inputs is null or empty (MaterialsOk returns true).
  - TryGetMaterialsInventoryIdByConvention returns false if itemId is null/empty or if no corresponding material-inventory exists.

- Guard ordering and blockers
  - Blocker priority in GetPreview: OutputInventoryFull > MissingMaterials > NotEnoughCurrency > None.

- Currency handling
  - Gold is treated as a currency with id "gold" when present in recipe.currencyCosts.
  - If gold > 0 and wallet cannot spend, craft fails with NotEnoughCurrency.

- Rollback semantics
  - If material consumption or currency spend fails after partial removal, previously removed materials are rolled back to their original stacks.
  - On output failure after currency spend, gold is refunded.

- Threading and async
  - All operations are synchronous; no explicit threading/async behavior is defined.

- Assumptions limited to this file
  - RecipeDef, ItemStack, InventoryDomain, IWallet, and related types are external to this file; their behavior is relied upon but not defined here.
  - Debug and logging behavior relies on DebugOutputReject and DebugManager but their internal semantics are not defined in this file.

5) Example

- Minimal usage example (illustrative)
  - // Determine if crafting a recipe is possible
  - var preview = CraftingService.GetPreview(recipe, "inventory_slot_01", inv, wallet);
  - bool canCraft = preview.canCraft;

  - // Attempt to craft and receive a failure reason if it fails
  - if (!CraftingService.TryCraftToInventory(recipe, inv, wallet, "inventory_slot_01", out var reason))
  - {
  -     // reason contains the cause (e.g., MissingMaterials, OutputInventoryFull, NotEnoughCurrency)
  - }

6) Unknowns

- Details of:
  - RecipeDef, ItemStack, InventoryDomain, and IWallet implementations beyond usage in this file.
  - Exact behavior of ItemTypeUtils.FromId and ItemType enum beyond its use here.
  - DebugManager and DebugOutput logging specifics.
  - Any external side effects from inventory methods (CanAccept, TryAdd, TryRemove, Peek, HasInstance, etc.) beyond what is invoked here.
  - Concurrency considerations or broader game-state implications outside the crafting workflow as implemented in this file.
```
