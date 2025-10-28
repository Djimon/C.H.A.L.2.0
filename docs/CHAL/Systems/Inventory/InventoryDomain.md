# CHAL.Systems.Inventory.InventoryDomain

_Automatically generated/updated from `Assets/src/Systems/Inventory/core/InventoryDomain.cs`._

# Purpose
- Defines the `InventoryDomain` class for managing inventory instances and item stacks.

# Public API
- Namespace: `CHAL.Systems.Inventory`
- Types
  - `public sealed class InventoryDomain : IInventoryDomain`
    - Public fields/properties:
      - `Func<string, bool> ItemExists;`
      - `Func<string, string, bool> ItemHasTag;`
      - `event Action<string, int, ItemStack?> OnSlotChanged;`
    - Public methods:
      - `bool HasInstance(string instanceId);`
      - `InventoryInstance GetInstance(string instanceId);`
      - `void RegisterInstance(InventoryInstance inst);`
      - `ItemStack? Peek(string instanceId, int slotIndex);`
      - `int SlotCount(string instanceId);`
      - `void ClearAllSlots(string instanceId);`
      - `bool TryAdd(string instanceId, in ItemStack stack, out TransactionResult result);`
      - `bool TryMove(in MoveRequest req, out TransactionResult result);`
      - `bool TryRemove(string instanceId, int slotIndex, int amount, out TransactionResult result);`
      - `internal bool TryGetInstance(string inventoryID, out InventoryInstance inst);`

# Key Behavior & Side Effects
- `HasInstance`: Checks if an instance exists by ID.
- `GetInstance`: Retrieves an instance by ID, returning null if not found.
- `RegisterInstance`: Registers a new inventory instance.
- `Peek`: Returns the item stack at a specified slot index or null if invalid.
- `SlotCount`: Returns the number of slots in the specified inventory instance.
- `ClearAllSlots`: Clears all slots in the specified inventory instance and invokes `OnSlotChanged` for each slot.
- `TryAdd`: Attempts to add an item stack to the inventory, filling existing stacks first, then empty slots. Logs and returns failure reasons.
- `TryMove`: Moves or splits item stacks between inventory slots, checking filters and logging reasons for failure.
- `TryRemove`: Removes a specified amount of items from a slot, logging changes and invoking `OnSlotChanged`.

# Constraints & Failure Modes
- Methods guard against null or empty instance IDs.
- `TryAdd`, `TryMove`, and `TryRemove` handle various failure modes, including instance not found, index out of range, and invalid amounts.
- Filters are applied to item stacks during addition and movement.

# Example
```csharp
var inventoryDomain = new InventoryDomain();
var instance = new InventoryInstance("exampleInstance");
inventoryDomain.RegisterInstance(instance);
var itemStack = new ItemStack("itemID", 10);
inventoryDomain.TryAdd("exampleInstance", in itemStack, out var result);
```

# Unknowns
- The implementation details of `InventoryInstance`, `ItemStack`, `TransactionResult`, and `MoveRequest` are not provided in this file.
- The behavior of `DebugManager.Log` and its impact on performance or side effects is not defined.

