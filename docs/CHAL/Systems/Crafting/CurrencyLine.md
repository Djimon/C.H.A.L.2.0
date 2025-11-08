# CHAL.Systems.Crafting.CurrencyLine

_Automatically generated/updated from `Assets/src/Systems/Crafting/CraftingService.cs`._

# Purpose
- Defines the `CraftingService` for managing crafting operations in the game.

# Public API
- Namespace: `CHAL.Systems.Crafting`
- Types
  - public static class `CraftingService`
    - Public methods:
      - `static RecipePreview GetPreview(RecipeDef recipe, string outputInventoryId, InventoryDomain inv, IWallet wallet)`
        - Returns a `RecipePreview` object representing the recipe output.
      - `static bool CanCraft(RecipeDef recipe, InventoryDomain inv, string outputInventoryId, IWallet wallet)`
        - Returns true if the recipe can be crafted; otherwise, false.
      - `static bool TryCraftToInventory(RecipeDef recipe, InventoryDomain inv, IWallet wallet, string outputInventoryId, out string failReason)`
        - Returns true if crafting was successful; otherwise, false with a failure reason.

  - public struct `MaterialLine`
    - Public fields:
      - `string itemId`
      - `int required`
      - `int playerAmount`
      - `bool enough` (true if playerAmount >= required)

  - public struct `CurrencyLine`
    - Public fields:
      - `string currencyId`
      - `int required`
      - `int playerAmount`
      - `bool enough` (true if playerAmount >= required)

  - public readonly struct `RecipePreview`
    - Public fields:
      - `bool canCraft`
      - `CraftBlocker blocker`
      - `bool outputOk`
      - `bool materialsOk`
      - `bool currencyOk`

  - public enum `CraftBlocker`
    - Enum values:
      - `None`
      - `LockedByResearch`
      - `OutputInventoryFull`
      - `MissingMaterials`
      - `NotEnoughCurrency`
      - `InvalidRefinement`
      - `UnknownError`

# Key Behavior & Side Effects
- `GetPreview` checks if the output inventory can accept the crafted item, verifies material availability, and checks if the player has enough currency.
- `CanCraft` utilizes `GetPreview` to determine if crafting is possible.
- `TryCraftToInventory` attempts to craft an item, consuming materials and currency, and handles failures with rollback mechanisms.

# Constraints & Failure Modes
- `GetPreview` and `TryCraftToInventory` require valid `RecipeDef`, `InventoryDomain`, and `IWallet` instances.
- Handles null or empty inputs for materials and currency.
- Rollbacks material consumption if crafting fails at any stage.
- Requires that the output inventory can accept the crafted item.

# Example
```csharp
var recipe = new RecipeDef(); // Assume this is defined elsewhere
var inv = new InventoryDomain(); // Assume this is defined elsewhere
var wallet = new Wallet(); // Assume this is defined elsewhere
string outputInventoryId = "output_inventory_id";
string failReason;

bool canCraft = CraftingService.CanCraft(recipe, inv, outputInventoryId, wallet);
bool success = CraftingService.TryCraftToInventory(recipe, inv, wallet, outputInventoryId, out failReason);
```

# Unknowns
- The structure and properties of `RecipeDef`, `InventoryDomain`, and `IWallet` are not defined in this file.
- The behavior of `DebugManager.Log` and its impact on performance is not specified.

