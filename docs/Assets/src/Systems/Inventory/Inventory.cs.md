# Assets/src/Systems/Inventory/Inventory.cs

_Automatic generated/updated._

```markdown
## Purpose
- Defines an `Inventory` class for managing a collection of items.
- Provides methods to add, remove, and query items in the inventory.

## Public API
- Namespace: `CHAL.Systems.Inventory`
- Types:
  - **public class** `Inventory`
    - **public readonly string** `invID` - Identifier for the inventory.
    - **public bool** `AddItem(string itemId, int amount = 1)` - Adds an item to the inventory; returns false if item cannot be added.
    - **public bool** `RemoveItem(string itemId, int amount = 1)` - Removes an item from the inventory; returns false if item cannot be removed.
    - **public int** `GetItemCount(string itemId)` - Returns the count of a specific item; returns 0 if item does not exist.
    - **public List<InventoryItem>** `GetAllItems()` - Returns a list of all items in the inventory.
    - **public Dictionary<string, int>** `ToDictionary()` - Converts the inventory items to a dictionary of item IDs and counts.
    - **public void** `FromDictionary(Dictionary<string, int> dict)` - Initializes the inventory from a dictionary of item IDs and counts.
  - **public class** `InventoryItem`
    - **public string** `ItemId` - Identifier for the inventory item.
    - **public int** `Count` - Quantity of the item in the inventory.

## Key Behavior & Side Effects
- `AddItem`: 
  - Checks if the item ID starts with the inventory ID.
  - Limits the number of items added based on maximum stack size and available slots.
  - Logs debug messages for various conditions (e.g., max stacks reached, items added).
- `RemoveItem`: 
  - Reduces the item count or removes the item if the count reaches zero.
- `ToDictionary`: 
  - Converts the inventory items to a dictionary format.
- `FromDictionary`: 
  - Populates the inventory from a provided dictionary.

## Constraints & Failure Modes
- `AddItem` fails if:
  - The item ID does not match the inventory ID prefix.
  - The maximum stack size or slots are exceeded.
- `RemoveItem` fails if:
  - The item does not exist or the requested amount exceeds the available count.
- Uses `List<InventoryItem>` for storage, which may have performance implications with large inventories.

## Example
```csharp
var inventory = new Inventory("ItemPrefix");
inventory.AddItem("ItemPrefix_1", 5);
var count = inventory.GetItemCount("ItemPrefix_1"); // count will be 5
inventory.RemoveItem("ItemPrefix_1", 2);
```

## Unknowns
- The implementation details of `InventoryRules.GetMaxStack` and `InventoryRules.GetMaxSlots`.
- The behavior of `DebugManager.Log` and `DebugManager.DebugLog`.
```
