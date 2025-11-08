# CHAL.Systems.Crafting.MaterialLine

_Automatically generated/updated from `Assets/src/Systems/Crafting/CraftingService.cs`._

# Purpose
- Defines the `CraftingService` for managing crafting operations in the game, including previewing recipes and attempting to craft items.

# Public API
- Namespace: `CHAL.Systems.Crafting`
- Types
  - public static class `CraftingService`
    - Public methods:
      - `static RecipePreview GetPreview(RecipeDef recipe, string outputInventoryId, InventoryDomain inv, IWallet wallet)`: Returns a preview of the recipe output.
      - `static bool CanCraft(RecipeDef recipe, InventoryDomain inv, string outputInventoryId, IWallet wallet)`: Determines if a recipe can be crafted.
      - `static bool TryCraftToInventory(RecipeDef recipe, InventoryDomain inv, IWallet wallet, string outputInventoryId, out string failReason)`: Attempts to craft an item and add it to the specified inventory.

  - public struct `MaterialLine`
    - Public fields:
      - `string itemId`: The ID of the required item.
      - `int required`: The amount required.
      - `int playerAmount`: The amount the player has.
      - `bool enough`: Indicates if the player has enough of the item.

  - public struct `CurrencyLine`
    - Public fields:
      - `string currencyId`: The ID of the required currency.
      - `int required`: The amount required.
      - `int playerAmount`: The amount the player has.
      - `bool enough`: Indicates if the player has enough currency.

  - public readonly struct `RecipePreview`
    - Public fields:
      - `bool canCraft`: Indicates if the recipe can be crafted.
      - `CraftBlocker blocker`: The reason why crafting may be blocked.
      - `bool outputOk`: Indicates if the output inventory can accept the item.
      - `bool materialsOk`: Indicates if the player has enough materials.
      - `bool currencyOk`: Indicates if the player has enough currency.

- Enums
  - public enum `CraftBlocker`
    - None
    - LockedByResearch
    - OutputInventoryFull
    - MissingMaterials
    - NotEnoughCurrency
    - InvalidRefinement
    - UnknownError

# Key Behavior & Side Effects
- `GetPreview` checks if the output inventory can accept the crafted item, verifies if the player has enough materials and currency, and logs detailed rejection reasons if crafting fails.
- `CanCraft` simply calls `GetPreview` to determine if crafting is possible.
- `TryCraftToInventory` attempts to craft an item, consuming materials and currency, and handles rollback in case of failure.

# Constraints & Failure Modes
- `GetPreview` and `TryCraftToInventory` handle null or empty inputs for materials and currency.
- The crafting process is atomic; if any step fails, previously consumed materials are rolled back.
- The output inventory must have space to accept the crafted item.

# Example
```csharp
var recipe = new RecipeDef(); // Assume this is defined and populated
var inv = new InventoryDomain(); // Assume this is defined and populated
var wallet = new Wallet(); // Assume this is defined and populated
string outputInventoryId = "output_inventory_id";
string failReason;

bool canCraft = CraftingService.CanCraft(recipe, inv, outputInventoryId, wallet);
if (canCraft)
{
    bool success = CraftingService.TryCraftToInventory(recipe, inv, wallet, outputInventoryId, out failReason);
    if (!success)
    {
        Debug.Log($"Crafting failed: {failReason}");
    }
}
```

# Unknowns
- The structure and properties of `RecipeDef`, `InventoryDomain`, and `IWallet` are not defined in this file.
- The behavior of `DebugManager.Log` and its logging levels are not detailed in this file.

