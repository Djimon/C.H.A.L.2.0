# CHAL.Systems.Crafting.RecipePreview

_Automatically generated/updated from `Assets/src/Systems/Crafting/CraftingService.cs`._

# Purpose
- Defines the `CraftingService` for handling crafting operations in the game.

# Public API
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
    - `public List<MaterialLine> materials` - List of materials needed for crafting.
    - `public List<CurrencyLine> currencies` - List of currencies needed for crafting.
    - `public bool canCraft` - Indicates if the recipe can be crafted.
  - `public static RecipePreview GetPreview(RecipeDef recipe, IInventoryDomain inv, string materialsInventoryId, IWallet wallet)` - Returns a preview of the crafting recipe.
  - `public static bool CanCraft(RecipeDef recipe, IInventoryDomain inv, string materialsInventoryId, IWallet wallet)` - Checks if the recipe can be crafted.
  - `public static bool TryCraftToInventory(RecipeDef recipe, IInventoryDomain inv, string materialsInventoryId, IWallet wallet, string outputInventoryId, out string failReason)` - Attempts to craft the item and add it to the inventory.

# Key Behavior & Side Effects
- `GetPreview` calculates if the player has enough materials and currencies to craft an item.
- `CanCraft` checks if crafting is possible based on the recipe and current inventory.
- `TryCraftToInventory` performs an atomic crafting operation, which includes:
  - Checking if crafting requirements are met.
  - Consuming materials and spending currencies.
  - Adding the crafted item to the specified inventory or rolling back changes if any step fails.

# Constraints & Failure Modes
- If crafting requirements are not met, `TryCraftToInventory` returns false with a failure reason.
- If materials or currencies cannot be consumed/spent, the operation rolls back any changes made.
- If the output inventory is full, the operation rolls back all changes and returns an error.

# Example
```csharp
var recipePreview = CraftingService.GetPreview(recipe, inventory, "materialsInventory", wallet);
if (recipePreview.canCraft)
{
    string failReason;
    bool success = CraftingService.TryCraftToInventory(recipe, inventory, "materialsInventory", wallet, "outputInventory", out failReason);
}
```

# Unknowns
- The structure and properties of `RecipeDef` are not defined in this file.
- The behavior of `IInventoryDomain` and `IWallet` interfaces is not detailed in this file.

