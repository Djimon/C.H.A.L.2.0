# CHAL.Systems.Inventory.Inventory

_Automatically generated/updated from `Assets/src/Systems/Inventory/Inventory.cs`._

# Purpose
- Defines an `Inventory` class for managing a collection of items.
- Provides methods to add, remove, and query items in the inventory.

# Public API
- Namespace: `CHAL.Systems.Inventory`
- Types
  - **public class** `Inventory`
    - Public fields/properties:
      - `readonly string invID`: Identifier for the inventory.
    - Public methods:
      - `public Inventory(string prefix)`: Constructor that initializes `invID`.
      - `public bool AddItem(string itemId, int amount = 1)`: Adds items to the inventory; returns false if item cannot be added.
      - `public bool RemoveItem(string itemId, int amount = 1)`: Removes items from the inventory; returns false if item cannot be removed.
      - `public int GetItemCount(string itemId)`: Returns the count of a specific item; returns 0 if not found.
      - `public List<InventoryItem> GetAllItems()`: Returns a list of all items in the inventory.
      - `public Dictionary<string, int> ToDictionary()`: Converts inventory items to a dictionary of item IDs and counts.
      - `public void FromDictionary(Dictionary<string, int> dict)`: Initializes inventory from a dictionary of item IDs and counts.
  - **public class** `InventoryItem`
    - Public fields/properties:
      - `public string ItemId`: Identifier for the inventory item.
      - `public int Count`: Quantity of the item.

# Key Behavior & Side Effects
- `AddItem`: 
  - Checks if the item ID starts with `invID`.
  - Limits the number of items added based on maximum stack size and slots.
  - Logs debug messages for various conditions (e.g., max stacks reached).
- `RemoveItem`: 
  - Reduces the item count or removes the item if count reaches zero.
- `GetItemCount`: 
  - Returns the count of an item or 0 if not found.
- `ToDictionary`: 
  - Converts the inventory items to a dictionary format.
- `FromDictionary`: 
  - Populates the inventory from a provided dictionary.

# Constraints & Failure Modes
- `AddItem` fails if:
  - Item ID does not match `invID`.
  - Maximum stack size is reached.
  - Maximum slots are filled.
- `RemoveItem` fails if:
  - Item does not exist or insufficient count.
- Uses `List<InventoryItem>` for storage; performance may vary with size.

# Example
```csharp
var inventory = new Inventory("ItemPrefix");
inventory.AddItem("ItemPrefix_001", 5);
int count = inventory.GetItemCount("ItemPrefix_001"); // count will be 5
inventory.RemoveItem("ItemPrefix_001", 2);
```

# Unknowns
- The implementation details of `InventoryRules.GetMaxStack` and `InventoryRules.GetMaxSlots`.
- The behavior of `DebugManager` and its logging methods.

