# Assets/src/Systems/Items/ItemRegistry.cs

_Automatically generated/updated from `Assets/src/Systems/Items/ItemRegistry.cs`._

# Purpose
- Defines the `ItemRegistry` class, which manages item definitions and their validation in the game.

# Public API
- Namespace: `CHAL.Systems.Items`
- Types
  - **public sealed class ItemRegistry : ScriptableObject**
    - **Public fields/properties**
      - `static ItemRegistry Instance`: Singleton instance of `ItemRegistry`.
    - **Public methods**
      - `void Reload()`: Reloads item definitions from the Resources folder.
      - `bool TryGet(string itemId, out ItemDef def)`: Tries to retrieve an item definition based on its identifier.
      - `Rarity GetRarity(string itemId)`: Retrieves the rarity of an item based on its identifier.
      - `int GetLootValue(string itemId)`: Retrieves the loot value for a specified item by its identifier.
      - `bool Exists(string itemId)`: Checks if an item exists by its identifier.
      - `IEnumerable<ItemDef> GetAllItemsByType(string typePrefix)`: Retrieves all items that match the specified type prefix.
      - `void CreatePlaceholderitem(string itemId)`: Creates a placeholder item asset with the specified item ID.
      - `ItemType GetTypeOf(string itemId)`: Retrieves the item type associated with the specified item ID.
      - `bool IsType(string itemId, ItemType t)`: Checks if the specified item ID matches the given item type.
      - `void TriggerInstance()`: Triggers an instance action in the ItemRegistry.
      - `void ExportItemIndexCsv(string outputPath)`: Exports the item index to a CSV file at the specified path.

# Key Behavior & Side Effects
- The `Reload` method clears existing item definitions and loads new ones from the Resources folder, validating gear and recipes.
- The `CreatePlaceholderitem` method generates a placeholder item asset if an item ID is missing or invalid.
- Validation reports are generated and saved to a CSV file if discrepancies are found during loading.
- The `ExportItemIndexCsv` method exports item data to a CSV file, logging warnings for null or empty output paths.

# Constraints & Failure Modes
- The `Reload` method handles null or empty item IDs and duplicates, logging warnings for each issue.
- The `LoadModulePartMap` method warns if the ModulePartMap asset is not found.
- File operations in `ValidateGearAndRecipes` and `ValidateModulePartMap` may fail, with exceptions logged.
- The `ExportItemIndexCsv` method checks for null or empty output paths and logs a warning if found.

# Example
```csharp
var itemRegistry = ItemRegistry.Instance;
itemRegistry.Reload();
if (itemRegistry.TryGet("itemId", out var itemDef))
{
    Debug.Log($"Item found: {itemDef.name}");
}
```

# Unknowns
- The structure of `ItemDef`, `RecipeDef`, and `ModulePartMapWrapper` is not defined in this file.
- The behavior of `DebugManager` is not detailed in this file.
