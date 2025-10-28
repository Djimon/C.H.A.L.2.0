# CHAL.Systems.Items.ItemRegistry

_Automatically generated/updated from `Assets/src/Systems/Items/ItemRegistry.cs`._

# Purpose
- Defines the `ItemRegistry` class as a ScriptableObject for managing item definitions in the game.

# Public API
- Namespace: `CHAL.Systems.Items`
- Types
  - `public sealed class ItemRegistry : ScriptableObject`
    - Public fields/properties
      - `public static ItemRegistry Instance`: Singleton instance of `ItemRegistry`.
    - Public methods
      - `public void Reload()`: Reloads item definitions and validates module-part mappings.
      - `public bool TryGet(string itemId, out ItemDef def)`: Attempts to retrieve an `ItemDef` by its ID.
      - `public Rarity GetRarity(string itemId)`: Returns the rarity of the item by its ID.
      - `public int GetLootValue(string itemId)`: Returns the loot value of the item by its ID.
      - `public bool Exists(string itemId)`: Checks if an item exists by its ID.
      - `public IEnumerable<ItemDef> GetAllItemsByType(string typePrefix)`: Retrieves all items matching a type prefix.
      - `public void CreatePlaceholderitem(string itemId)`: Creates a placeholder item if an item ID does not exist.
      - `public ItemType GetTypeOf(string itemId)`: Gets the type of an item by its ID.
      - `public bool IsType(string itemId, ItemType t)`: Checks if an item is of a specific type.
      - `public void TriggerInstance()`: Logs a message to trigger the instance.

# Key Behavior & Side Effects
- `Reload()` clears existing items and loads new item definitions from resources.
- Validates module-part mappings and logs warnings for any issues found.
- Creates placeholder items for missing definitions and saves them as assets.
- Logs validation results and saves a report if errors are found.

# Constraints & Failure Modes
- Handles null or empty item IDs by skipping invalid entries during loading.
- Checks for duplicate item IDs and logs warnings.
- Creates directories for placeholder items if they do not exist.
- Uses `Resources.LoadAll<ItemDef>("data/Items")` to load item definitions, which may fail if the path is incorrect.

# Example
```csharp
var itemRegistry = ItemRegistry.Instance;
itemRegistry.Reload();
if (itemRegistry.TryGet("itemId", out var itemDef))
{
    Debug.Log(itemDef.description);
}
```

# Unknowns
- The structure of `ItemDef`, `Rarity`, `ItemType`, and `ItemTypeUtils` cannot be determined from this file.
- The implementation details of `DebugManager` and its logging methods are not provided.

