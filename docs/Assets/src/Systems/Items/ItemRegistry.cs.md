# Assets/src/Systems/Items/ItemRegistry.cs

_Automatic generated/updated._

```markdown
# Purpose
- Defines the `ItemRegistry` class as a ScriptableObject for managing item definitions in the game.

# Public API
- Namespace: `CHAL.Systems.Items`
- Types
  - `public sealed class ItemRegistry : ScriptableObject`
    - Public fields/properties:
      - `static ItemRegistry Instance`: Singleton instance of `ItemRegistry`.
    - Public methods:
      - `public void Reload()`: Reloads item definitions from resources.
      - `public bool TryGet(string itemId, out ItemDef def)`: Attempts to retrieve an `ItemDef` by its ID.
      - `public Rarity GetRarity(string itemId)`: Returns the rarity of an item by its ID.
      - `public int GetLootValue(string itemId)`: Returns the loot value of an item by its ID.
      - `public bool Exists(string itemId)`: Checks if an item exists by its ID.
      - `public IEnumerable<ItemDef> GetAllItemsByType(string typePrefix)`: Retrieves all items matching a type prefix.
      - `public void CreatePlaceholderitem(string itemId)`: Creates a placeholder item if an item ID does not exist.
      - `public ItemType GetTypeOf(string itemId)`: Gets the type of an item by its ID.
      - `public bool IsType(string itemId, ItemType t)`: Checks if an item is of a specified type.
      - `public void TriggerInstance()`: Triggers an instance action (no side effects noted).

# Key Behavior & Side Effects
- `Reload()` clears existing items and loads new item definitions from `Resources/data/Items`.
- Validates the module-part mapping and logs warnings for any issues found.
- Creates placeholder items for missing definitions and logs their creation.
- Generates a validation report in CSV format if validation errors occur.

# Constraints & Failure Modes
- Handles null or empty item IDs during loading and validation.
- Uses `DebugManager` for logging warnings and errors.
- Creates directories and assets on the filesystem, which may fail if permissions are insufficient.

# Example
```csharp
ItemRegistry registry = ItemRegistry.Instance;
registry.Reload();
if (registry.TryGet("itemId", out ItemDef itemDef)) {
    Debug.Log(itemDef.description);
}
```

# Unknowns
- The structure and properties of `ItemDef`, `Rarity`, and `ItemType` are not defined in this file.
- The implementation details of `ItemKey.TryParse` and `ItemTypeUtils.FromId` are not provided.
```
