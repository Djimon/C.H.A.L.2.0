# CHAL.Systems.Crafting.MaterialLine

_Automatically generated/updated from `Assets/src/Systems/Crafting/CraftingService.cs`._

# CraftingService.cs

## Purpose
- Provides crafting functionality including previewing recipes and attempting to craft items.

## Public API
- Namespace: `CHAL.Systems.Crafting`
- Types
  - `public struct MaterialLine`
    - `public string itemId` - ID of the item.
    - `public int required` - Required quantity of the item.
    - `public int playerAmount` - Amount of the item the player has.
    - `public bool enough` - Indicates if the player has enough of the item.
  - `public struct CurrencyLine`
    - `public string currencyId` - ID of the currency.
    - `public int required` - Required amount of the currency.
    - `public int playerAmount` - Amount of the currency the player has.
    - `public bool enough` - Indicates if the player has enough currency.
  - `public struct RecipePreview`
    - `public List<MaterialLine> materials` - List of materials required for the recipe.
    - `public List<CurrencyLine> currencies` - List of currencies required for the recipe.
    - `public bool canCraft` - Indicates if the recipe can be crafted.
  - `public static RecipePreview GetPreview(RecipeDef recipe, IInventoryDomain inv, string materialsInventoryId, IWallet wallet)`
    - Returns a preview of the recipe including materials and currencies.
  - `public static bool CanCraft(RecipeDef recipe, IInventoryDomain inv, string materialsInventoryId, IWallet wallet)`
    - Returns true if the recipe can be crafted.
  - `public static bool TryCraftToInventory(RecipeDef recipe, IInventoryDomain inv, string materialsInventoryId, IWallet wallet, string outputInventoryId, out string failReason)`
    - Attempts to craft the item and add it to the specified inventory. Returns false if crafting fails, with a reason in `failReason`.

## Key Behavior & Side Effects
- `GetPreview` checks if the player has enough materials and currencies to craft a recipe.
- `CanCraft` uses `GetPreview` to determine if crafting is possible.
- `TryCraftToInventory` performs several steps atomically:
  - Checks if crafting is possible.
  - Consumes required materials.
  - Spends required currencies.
  - Adds the crafted item to the output inventory.
  - Rolls back changes if any step fails.

## Constraints & Failure Modes
- If crafting requirements are not met, `TryCraftToInventory` returns false with a reason.
- If materials or currencies cannot be consumed/spent, it rolls back any changes made.
- If the output inventory is full, it rolls back all changes and returns an error.

## Example
```csharp
var recipePreview = CraftingService.GetPreview(recipe, inventory, "materialsInventoryId", wallet);
if (recipePreview.canCraft)
{
    string failReason;
    bool success = CraftingService.TryCraftToInventory(recipe, inventory, "materialsInventoryId", wallet, "outputInventoryId", out failReason);
}
```

## Unknowns
- The structure and properties of `RecipeDef` are not defined in this file.
- The behavior of `IInventoryDomain` and `IWallet` interfaces is not detailed in this file.

