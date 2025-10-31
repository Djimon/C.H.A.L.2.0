# CHAL.Systems.Inventory.InventoryItem

_Automatically generated/updated from `Assets/src/Systems/Inventory/Inventory.cs`._

# Purpose
- Defines an `Inventory` class for managing a collection of items.
- Provides methods to add, remove, and query items in the inventory.

# Public API
- Namespace: `CHAL.Systems.Inventory`
- Types:
  - **public class** `Inventory`
    - **public readonly string** `invID` - Identifier for the inventory.
    - **private List<InventoryItem>** `_items` - List of items in the inventory.
    - **public Inventory(string prefix)** - Constructor that initializes `invID` with the given prefix.
    - **public bool** `AddItem(string itemId, int amount = 1)` - Adds an item to the inventory; returns false if item cannot be added.
    - **public bool** `RemoveItem(string itemId, int amount = 1)` - Removes an item from the inventory; returns false if item cannot be removed.
    - **public int** `GetItemCount(string itemId)` - Returns the count of a specific item; returns 0 if item is not found.
    - **public List<InventoryItem>** `GetAllItems()` - Returns a list of all items in the inventory.
    - **public Dictionary<string, int>** `ToDictionary()` - Converts the inventory items to a dictionary.
    - **public void** `FromDictionary(Dictionary<string, int> dict)` - Initializes the inventory from a dictionary of items.

  - **public class** `InventoryItem`
    - **public string** `ItemId` - Identifier for the inventory item.
    - **public int** `Count` - Quantity of the item.

# Key Behavior & Side Effects
- `AddItem`: Checks if the item ID starts with `invID`, limits additions based on max stack size and max slots, and logs actions.
- `RemoveItem`: Decreases item count or removes the item if count reaches zero.
- `GetItemCount`: Returns the count of the specified item or 0 if not found.
- `ToDictionary`: Converts the inventory items to a dictionary format.

# Constraints & Failure Modes
- `AddItem` fails if:
  - The item ID does not match the inventory prefix.
  - The maximum stack size is reached.
  - The maximum slots are filled.
- `RemoveItem` fails if:
  - The item is not found or the requested amount exceeds the available count.
- Null or empty handling is not explicitly defined for method parameters.

# Example
```csharp
var inventory = new Inventory("ItemPrefix");
inventory.AddItem("ItemPrefix_1", 5);
int count = inventory.GetItemCount("ItemPrefix_1"); // count will be 5
inventory.RemoveItem("ItemPrefix_1", 2);
```

# Unknowns
- The implementation details of `InventoryRules.GetMaxStack` and `InventoryRules.GetMaxSlots` are not provided.
- The behavior of `DebugManager` is not defined in this file.

