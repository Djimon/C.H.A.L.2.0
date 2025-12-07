# Assets/src/Systems/Crafting/CraftingService.cs

_Automatically generated/updated from `Assets/src/Systems/Crafting/CraftingService.cs`._

# Purpose
- Defines the `CraftingService` for managing crafting operations in the game.

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
      - `string itemId`: The ID of the item.
      - `int required`: The required amount of the item.
      - `int playerAmount`: The amount the player currently has.
      - `bool enough`: Indicates if the player has enough of the item.

  - public struct `CurrencyLine`
    - Public fields:
      - `string currencyId`: The ID of the currency.
      - `int required`: The required amount of the currency.
      - `int playerAmount`: The amount the player currently has.
      - `bool enough`: Indicates if the player has enough currency.

  - public readonly struct `RecipePreview`
    - Public fields:
      - `bool canCraft`: Indicates if the recipe can be crafted.
      - `CraftBlocker blocker`: The reason why crafting is blocked.
      - `bool outputOk`: Indicates if the output is acceptable.
      - `bool materialsOk`: Indicates if the materials are sufficient.
      - `bool currencyOk`: Indicates if the currency is sufficient.

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
- `GetPreview` checks if the output inventory can accept the crafted item, verifies if the player has enough materials and currency, and returns a `RecipePreview` object.
- `CanCraft` uses `GetPreview` to determine if crafting is possible.
- `TryCraftToInventory` attempts to craft an item, consuming materials and currency, and handles failures by rolling back changes if necessary.
- `DebugOutputReject` logs detailed information when crafting fails due to inventory constraints.

# Constraints & Failure Modes
- `GetPreview` and `TryCraftToInventory` require valid `RecipeDef`, `InventoryDomain`, and `IWallet` instances.
- `TryCraftToInventory` will return false and set `failReason` if:
  - The output inventory cannot accept the crafted item.
  - Required materials are missing.
  - Not enough currency is available.
  - The output inventory is full.

# Example
```csharp
var recipe = new RecipeDef(); // Assume this is defined
var outputInventoryId = "player_inventory";
var inventoryDomain = new InventoryDomain(); // Assume this is defined
var wallet = new Wallet(); // Assume this is defined

var canCraft = CraftingService.CanCraft(recipe, inventoryDomain, outputInventoryId, wallet);
var preview = CraftingService.GetPreview(recipe, outputInventoryId, inventoryDomain, wallet);
string failReason;
var success = CraftingService.TryCraftToInventory(recipe, inventoryDomain, wallet, outputInventoryId, out failReason);
```

# Unknowns
- The structure and properties of `RecipeDef`, `InventoryDomain`, and `IWallet` are not defined in this file.
- The behavior of `DebugManager.Log` and its impact on performance is not specified.
