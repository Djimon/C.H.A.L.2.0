# CHAL.Systems.Inventory.InventoryDomain

_Automatically generated/updated from `Assets/src/Systems/Inventory/core/InventoryDomain.cs`._

# Purpose
- Defines the `InventoryDomain` class for managing inventory instances and their operations.

# Public API
- Namespace: `CHAL.Systems.Inventory`
- Types
  - `sealed class InventoryDomain : IInventoryDomain`
    - Public fields/properties:
      - `Func<string, bool> ItemExists`: Checks if an item exists by its ID.
      - `Func<string, string, bool> ItemHasTag`: Checks if an item has a specific tag.
      - `event Action<string, int, ItemStack?> OnSlotChanged`: Invoked when a slot changes.
    - Public methods:
      - `bool HasInstance(string instanceId)`: Checks if an instance exists by its ID.
      - `InventoryInstance GetInstance(string instanceId)`: Retrieves an inventory instance by its ID.
      - `void RegisterInstance(InventoryInstance inst)`: Registers an instance of an inventory.
      - `ItemStack? Peek(string instanceId, int slotIndex)`: Retrieves the item stack at a specific slot index.
      - `int SlotCount(string instanceId)`: Gets the number of slots for the specified instance.
      - `void ClearAllSlots(string instanceId)`: Clears all slots for the specified instance.
      - `bool CanAccept(string instanceId, in ItemStack stack)`: Determines if an item stack can be accepted for the given instance.
      - `bool TryAdd(string instanceId, in ItemStack stack, out TransactionResult result)`: Attempts to add an item stack to the inventory.
      - `bool TryMove(in MoveRequest req, out TransactionResult result)`: Attempts to move an item from one inventory to another.
      - `bool TryRemove(string instanceId, int slotIndex, int amount, out TransactionResult result)`: Attempts to remove a specified amount from a given slot of an instance.

# Key Behavior & Side Effects
- `OnSlotChanged` event is triggered whenever a slot's item stack changes.
- `TryAdd` method logs detailed debug information about item addition attempts.
- `TryMove` method handles various move modes (Move, Split, Merge, Swap) with specific behaviors and checks.

# Constraints & Failure Modes
- Methods check for null or empty instance IDs and return appropriate failure reasons.
- `TryAdd` and `TryMove` methods validate slot filters before performing operations.
- `TryRemove` checks for valid slot indices and item presence before removal.

# Example
```csharp
var inventoryDomain = new InventoryDomain();
var instance = new InventoryInstance("instance1");
inventoryDomain.RegisterInstance(instance);
var itemStack = new ItemStack("item1", 10);
inventoryDomain.TryAdd("instance1", in itemStack, out var result);
```

# Unknowns
- The implementation details of `InventoryInstance`, `ItemStack`, `TransactionResult`, and `MoveRequest` are not provided in this file.

