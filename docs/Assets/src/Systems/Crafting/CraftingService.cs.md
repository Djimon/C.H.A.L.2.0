# Assets/src/Systems/Crafting/CraftingService.cs

_Automatic generated/updated._

```markdown
## Purpose
- Defines the `CraftingService` for managing crafting operations in the game.
- Provides methods to preview crafting requirements and attempt crafting items.

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
    - `public List<MaterialLine> materials` - List of materials required for crafting.
    - `public List<CurrencyLine> currencies` - List of currencies required for crafting.
    - `public bool canCraft` - Indicates if crafting is possible.
  - `public static RecipePreview GetPreview(RecipeDef recipe, IInventoryDomain inv, string materialsInventoryId, IWallet wallet)` - Returns a preview of crafting requirements.
  - `public static bool CanCraft(RecipeDef recipe, IInventoryDomain inv, string materialsInventoryId, IWallet wallet)` - Checks if crafting is possible.
  - `public static bool TryCraftToInventory(RecipeDef recipe, IInventoryDomain inv, string materialsInventoryId, IWallet wallet, string outputInventoryId, out string failReason)` - Attempts to craft an item and add it to the inventory.

## Key Behavior & Side Effects
- `GetPreview` calculates required materials and currencies for a given recipe.
- `CanCraft` checks if the player has enough materials and currencies to craft an item.
- `TryCraftToInventory` performs an atomic crafting operation, including:
  - Validating crafting requirements.
  - Consuming materials and currencies.
  - Adding the crafted item to the specified inventory.
  - Rolling back changes if any step fails.

## Constraints & Failure Modes
- Requires valid `RecipeDef`, `IInventoryDomain`, and `IWallet` instances.
- Handles null or empty inputs for materials and currencies gracefully.
- Ensures atomicity in crafting operations; if any step fails, all changes are rolled back.
- Returns failure reasons through the `out` parameter in `TryCraftToInventory`.

## Example
```csharp
var recipe = new RecipeDef(); // Assume this is defined
var inventory = new InventoryDomain(); // Assume this is defined
var wallet = new Wallet(); // Assume this is defined
string failReason;

if (CraftingService.TryCraftToInventory(recipe, inventory, "materialsInventoryId", wallet, "outputInventoryId", out failReason))
{
    // Crafting succeeded
}
else
{
    // Handle failure, failReason contains the reason
}
```

## Unknowns
- The structure and properties of `RecipeDef`, `IInventoryDomain`, and `IWallet` are not defined in this file.
```
