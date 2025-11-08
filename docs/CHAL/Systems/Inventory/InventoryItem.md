# CHAL.Systems.Inventory.InventoryItem

_Automatically generated/updated from `Assets/src/Systems/Inventory/Inventory.cs`._

# Purpose
- Defines an `Inventory` class that manages a collection of items.
- Provides methods to add, remove, and retrieve items from the inventory.

# Public API
- Namespace: `CHAL.Systems.Inventory`
- Types
  - **public class** `Inventory`
    - **Public fields/properties**
      - `readonly string invID`: Identifier for the inventory.
    - **Public methods**
      - `public Inventory(string prefix)`: Initializes the inventory with a prefix.
      - `public bool AddItem(string itemId, int amount = 1)`: Adds an item to the inventory; returns true if successful.
      - `public bool RemoveItem(string itemId, int amount = 1)`: Removes an item from the inventory; returns true if successful.
      - `public int GetItemCount(string itemId)`: Returns the count of a specific item in the inventory.
      - `public List<InventoryItem> GetAllItems()`: Retrieves all items from the inventory.
      - `public Dictionary<string, int> ToDictionary()`: Converts inventory items to a dictionary of item IDs and their counts.
      - `public void FromDictionary(Dictionary<string, int> dict)`: Initializes the inventory from a dictionary of item IDs and counts.
  - **public class** `InventoryItem`
    - **Public fields/properties**
      - `public string ItemId`: Identifier for the inventory item.
      - `public int Count`: Number of items in the inventory.

# Key Behavior & Side Effects
- `AddItem`: Checks if the item ID starts with the inventory prefix; limits item addition based on maximum stack size and slot count.
- `RemoveItem`: Reduces the item count and removes the item if the count drops to zero.
- `GetItemCount`: Returns zero if the item is not found.
- `ToDictionary`: Converts the inventory items into a dictionary format.

# Constraints & Failure Modes
- `AddItem` fails if the item ID does not match the inventory prefix, if the maximum stack size is reached, or if the maximum slots are filled.
- `RemoveItem` fails if the item is not found or if the requested amount exceeds the available count.
- The inventory can only hold a limited number of unique items based on `maxSlots`.

# Example
```csharp
var inventory = new Inventory("ItemPrefix");
inventory.AddItem("ItemPrefix_1", 5);
int count = inventory.GetItemCount("ItemPrefix_1"); // count will be 5
inventory.RemoveItem("ItemPrefix_1", 2);
```

# Unknowns
- The implementation details of `InventoryRules.GetMaxStack` and `InventoryRules.GetMaxSlots` are not provided in this file.
