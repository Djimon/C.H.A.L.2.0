# CHAL.Systems.Crafting.CraftBlocker

_Automatically generated/updated from `Assets/src/Systems/Crafting/CraftingService.cs`._

# Purpose
- Defines the `CraftingService` for handling crafting operations in the game, including previewing recipes and attempting to craft items.

# Public API
- Namespace: `CHAL.Systems.Crafting`
- Types
  - public static class `CraftingService`
    - Public methods:
      - `static RecipePreview GetPreview(RecipeDef recipe, string outputInventoryId, InventoryDomain inv, IWallet wallet)`: Returns a `RecipePreview` object representing the recipe output.
      - `static bool CanCraft(RecipeDef recipe, InventoryDomain inv, string outputInventoryId, IWallet wallet)`: Returns true if the recipe can be crafted; otherwise, false.
      - `static bool TryCraftToInventory(RecipeDef recipe, InventoryDomain inv, IWallet wallet, string outputInventoryId, out string failReason)`: Attempts to craft an item from a recipe and add it to the specified inventory. Returns true if successful, otherwise false with a failure reason.

  - public struct `MaterialLine`
    - Public fields:
      - `string itemId`: The ID of the item.
      - `int required`: The amount required.
      - `int playerAmount`: The amount the player has.
      - `bool enough`: Indicates if the player has enough of the item.

  - public struct `CurrencyLine`
    - Public fields:
      - `string currencyId`: The ID of the currency.
      - `int required`: The amount required.
      - `int playerAmount`: The amount the player has.
      - `bool enough`: Indicates if the player has enough currency.

  - public readonly struct `RecipePreview`
    - Public fields:
      - `bool canCraft`: Indicates if the recipe can be crafted.
      - `CraftBlocker blocker`: The first blocker reason in guard order.
      - `bool outputOk`: Indicates if the output is acceptable.
      - `bool materialsOk`: Indicates if the materials are sufficient.
      - `bool currencyOk`: Indicates if the currency is sufficient.

- Enums
  - public enum `CraftBlocker`
    - Values:
      - `None`: All conditions are met.
      - `LockedByResearch`: Locked by research.
      - `OutputInventoryFull`: No space in output inventory.
      - `MissingMaterials`: Missing required materials.
      - `NotEnoughCurrency`: Insufficient currency.
      - `InvalidRefinement`: Invalid refinement (if feature active).
      - `UnknownError`: Fallback error.

# Key Behavior & Side Effects
- `GetPreview` checks if the output inventory can accept the crafted item, verifies if the player has enough materials and currency, and returns a `RecipePreview` indicating the crafting feasibility.
- `CanCraft` utilizes `GetPreview` to determine if crafting is possible.
- `TryCraftToInventory` attempts to craft an item, consuming materials and currency, and handles rollback in case of failure.

# Constraints & Failure Modes
- `GetPreview` and `TryCraftToInventory` require valid `RecipeDef`, `InventoryDomain`, and `IWallet` instances.
- Handles null or empty inputs for materials and currency.
- Rollbacks material consumption if crafting fails.
- Logs detailed rejection reasons for debugging.

# Example
```csharp
var recipe = new RecipeDef(); // Assume this is defined elsewhere
var outputInventoryId = "player_inventory";
var inventoryDomain = new InventoryDomain(); // Assume this is defined elsewhere
var wallet = new Wallet(); // Assume this is defined elsewhere

// Check if the recipe can be crafted
bool canCraft = CraftingService.CanCraft(recipe, inventoryDomain, outputInventoryId, wallet);

// Attempt to craft the item
string failReason;
bool success = CraftingService.TryCraftToInventory(recipe, inventoryDomain, wallet, outputInventoryId, out failReason);
```

# Unknowns
- The structure and properties of `RecipeDef`, `InventoryDomain`, and `IWallet` are not defined in this file.
- The behavior of `DebugManager.Log` and its implications are not detailed in this file.

