# CHAL.Systems.Crafting.CurrencyLine

_Automatically generated/updated from `Assets/src/Systems/Crafting/CraftingService.cs`._

# CraftingService.cs

## Purpose
- Provides crafting functionality, including previewing recipes and attempting to craft items.

## Public API
- Namespace: `CHAL.Systems.Crafting`
- Types
  - **public struct** `MaterialLine`
    - `string itemId`: ID of the item.
    - `int required`: Required quantity of the item.
    - `int playerAmount`: Amount of the item the player has.
    - `bool enough`: Indicates if the player has enough of the item.
  - **public struct** `CurrencyLine`
    - `string currencyId`: ID of the currency.
    - `int required`: Required amount of the currency.
    - `int playerAmount`: Amount of the currency the player has.
    - `bool enough`: Indicates if the player has enough currency.
  - **public struct** `RecipePreview`
    - `List<MaterialLine> materials`: List of materials required for the recipe.
    - `List<CurrencyLine> currencies`: List of currencies required for the recipe.
    - `bool canCraft`: Indicates if the recipe can be crafted.
  - **public static** `RecipePreview GetPreview(RecipeDef recipe, IInventoryDomain inv, string materialsInventoryId, IWallet wallet)`: Returns a preview of the recipe.
  - **public static** `bool CanCraft(RecipeDef recipe, IInventoryDomain inv, string materialsInventoryId, IWallet wallet)`: Checks if the recipe can be crafted.
  - **public static** `bool TryCraftToInventory(RecipeDef recipe, IInventoryDomain inv, string materialsInventoryId, IWallet wallet, string outputInventoryId, out string failReason)`: Attempts to craft the item and add it to the inventory.

## Key Behavior & Side Effects
- `GetPreview`: Calculates if the player has enough materials and currency to craft a recipe.
- `CanCraft`: Returns true if the recipe can be crafted based on inventory and wallet checks.
- `TryCraftToInventory`: 
  - Checks if crafting is possible.
  - Consumes materials and currency.
  - Adds the crafted item to the specified inventory.
  - Rolls back changes if any step fails.

## Constraints & Failure Modes
- Requires valid `RecipeDef`, `IInventoryDomain`, and `IWallet` implementations.
- Handles null or empty inputs for materials and currencies.
- Rollbacks occur on failure to ensure atomicity.
- Returns failure reasons through the `out` parameter in `TryCraftToInventory`.

## Example
```csharp
var recipePreview = CraftingService.GetPreview(recipe, inventory, "materialsInventory", wallet);
if (recipePreview.canCraft)
{
    string failReason;
    bool success = CraftingService.TryCraftToInventory(recipe, inventory, "materialsInventory", wallet, "outputInventory", out failReason);
}
```

## Unknowns
- The structure and properties of `RecipeDef` are not defined in this file.
- The behavior of `IInventoryDomain` and `IWallet` interfaces is not detailed here.

