# Assets/src/Systems/Inventory/core/InventoryDomain.cs

_Automatic generated/updated._

```markdown
## Purpose
- Defines the `InventoryDomain` class for managing inventory instances and item stacks.

## Public API
- Namespace: `CHAL.Systems.Inventory`
- Types
  - `sealed class InventoryDomain : IInventoryDomain`
    - Public fields/properties:
      - `Func<string, bool> ItemExists` - Checks if an item exists by ID.
      - `Func<string, string, bool> ItemHasTag` - Checks if an item has a specific tag.
      - `event Action<string, int, ItemStack?> OnSlotChanged` - Invoked when a slot changes.
    - Public methods:
      - `bool HasInstance(string instanceId)` - Checks if an inventory instance exists.
      - `InventoryInstance GetInstance(string instanceId)` - Retrieves an inventory instance by ID.
      - `void RegisterInstance(InventoryInstance inst)` - Registers a new inventory instance.
      - `ItemStack? Peek(string instanceId, int slotIndex)` - Returns the item stack at a specific slot.
      - `int SlotCount(string instanceId)` - Returns the number of slots in an inventory instance.
      - `void ClearAllSlots(string instanceId)` - Clears all slots in an inventory instance.
      - `bool TryAdd(string instanceId, in ItemStack stack, out TransactionResult result)` - Attempts to add an item stack to an inventory.
      - `bool TryMove(in MoveRequest req, out TransactionResult result)` - Attempts to move an item stack between slots or instances.
      - `bool TryRemove(string instanceId, int slotIndex, int amount, out TransactionResult result)` - Attempts to remove an item stack from a slot.
      - `internal bool TryGetInstance(string inventoryID, out InventoryInstance inst)` - Attempts to retrieve an inventory instance internally.

## Key Behavior & Side Effects
- `OnSlotChanged` event is triggered whenever a slot's item stack changes.
- `TryAdd` method attempts to fill existing stacks first, then fills empty slots, logging actions and results.
- `TryMove` method handles moving items between slots, including merging and splitting stacks, with various failure conditions.
- `TryRemove` method removes a specified amount from a slot, updating the slot and triggering the `OnSlotChanged` event.

## Constraints & Failure Modes
- Methods validate input parameters (e.g., instance ID, slot index) and handle null or empty values.
- `TryAdd` and `TryMove` methods check for filters and capacity constraints before modifying stacks.
- Logging is performed for debugging purposes, indicating reasons for failures (e.g., "InstanceNotFound", "NoSpace").

## Example
```csharp
var inventoryDomain = new InventoryDomain();
var instance = new InventoryInstance("instance1");
inventoryDomain.RegisterInstance(instance);
var itemStack = new ItemStack("item1", 10);
inventoryDomain.TryAdd("instance1", in itemStack, out var result);
```

## Unknowns
- The structure and implementation details of `InventoryInstance`, `ItemStack`, `TransactionResult`, and `MoveRequest` are not defined in this file.
- The behavior of `ItemTypeUtils.FromId` and `DebugManager.Log` is not specified.
```
