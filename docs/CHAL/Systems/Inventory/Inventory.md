# Assets/src/Systems/Inventory/Inventory.cs

_Automatically generated/updated from `Assets/src/Systems/Inventory/Inventory.cs`._

# Purpose
- Defines an `Inventory` class that manages a collection of items.
- Provides methods to add, remove, and retrieve items from the inventory.

# Public API
- Namespace: `CHAL.Systems.Inventory`
- Types
  - public class `Inventory`
    - Public fields/properties:
      - `readonly string invID`: Identifier for the inventory.
    - Public methods:
      - `Inventory(string prefix)`: Constructor that initializes the inventory with a prefix.
      - `bool AddItem(string itemId, int amount = 1)`: Adds an item to the inventory; returns true if successful.
      - `bool RemoveItem(string itemId, int amount = 1)`: Removes a specified amount of an item; returns true if successful.
      - `int GetItemCount(string itemId)`: Returns the count of a specific item in the inventory.
      - `List<InventoryItem> GetAllItems()`: Retrieves all items from the inventory.
      - `Dictionary<string, int> ToDictionary()`: Converts inventory items to a dictionary of item IDs and their counts.
      - `void FromDictionary(Dictionary<string, int> dict)`: Initializes the inventory from a dictionary of item IDs and counts.
  - public class `InventoryItem`
    - Public fields/properties:
      - `string ItemId`: Identifier for the inventory item.
      - `int Count`: Number of items in the inventory.

# Key Behavior & Side Effects
- `AddItem`: 
  - Validates if the item ID starts with the inventory prefix.
  - Checks for existing items and manages stacking based on maximum stack size and available slots.
  - Logs debug information for various conditions (e.g., max stacks reached, items added).
- `RemoveItem`: 
  - Checks if the item exists and if there is enough quantity to remove.
  - Removes the item from the inventory if the count drops to zero.
- `GetItemCount`: 
  - Returns zero if the item is not found.
- `ToDictionary`: 
  - Converts the inventory items into a dictionary format.

# Constraints & Failure Modes
- `AddItem` fails if:
  - The item ID does not match the inventory prefix.
  - The maximum stack size or maximum slots are reached.
- `RemoveItem` fails if:
  - The item does not exist or the requested amount exceeds the available count.
- Threading/async notes: None evident.
- Performance hints: Uses `List<T>` for storage; consider performance implications for large inventories.

# Example
```csharp
var inventory = new Inventory("ItemPrefix");
inventory.AddItem("ItemPrefix_1", 5);
int count = inventory.GetItemCount("ItemPrefix_1"); // count will be 5
inventory.RemoveItem("ItemPrefix_1", 2);
```

# Unknowns
- None.

