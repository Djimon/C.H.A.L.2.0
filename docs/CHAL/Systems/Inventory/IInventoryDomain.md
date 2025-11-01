# CHAL.Systems.Inventory.IInventoryDomain

_Automatically generated/updated from `Assets/src/Systems/Inventory/core/IInventoryDomain.cs`._

```text
1) Purpose
- Defines the IInventoryDomain interface for inventory domain operations.
- Declares methods for validating, mutating, and querying inventory state.
- Declares an event to notify slot changes within an inventory instance.
```

```text
2) Public API
- Namespace/module
  - CHAL.Systems.Inventory

- Types
  - public interface IInventoryDomain
    - bool CanAccept(string instanceId, in ItemStack stack)
      - Determine if the given stack can be accepted into the inventory identified by instanceId.
    - bool TryAdd(string instanceId, in ItemStack stack, out TransactionResult result)
      - Attempt to add the given stack to the inventory; returns success and a result.
    - bool TryMove(in MoveRequest req, out TransactionResult result)
      - Attempt to move items according to req; returns success and a result.
    - bool TryRemove(string instanceId, int slotIndex, int amount, out TransactionResult result)
      - Attempt to remove a quantity from a specific slot; returns success and a result.
    - event Action<string, int, ItemStack?> OnSlotChanged
      - (instanceId, slot, newStack) notification invoked when a slot changes.
    - ItemStack? Peek(string instanceId, int slotIndex)
      - Return the stack currently in the specified slot, or null if none.
    - int SlotCount(string instanceId)
      - Return the number of slots available for the given instance.
```

```text
3) Key Behavior & Side Effects
- CanAccept: reads validation for accepting an ItemStack into a per-instance inventory.
- TryAdd: mutates inventory state if successful; outputs detailed TransactionResult.
- TryMove: mutates inventory state according to MoveRequest; outputs detailed TransactionResult.
- TryRemove: mutates inventory state by removing items from a slot; outputs detailed TransactionResult.
- OnSlotChanged: fires when a slot's contents changes; arguments are (instanceId, slot, newStack).
- Peek: reads and returns the current stack in a slot without mutation.
- SlotCount: reads the number of slots for the given instanceId without mutation.
```

```text
4) Constraints & Failure Modes
- Try* methods return true on success; false on failure and supply a TransactionResult detailing the outcome.
- Peek returns ItemStack?; may be null if the slot is empty or invalid.
- SlotCount returns an int; behavior on invalid instanceId is not defined here.
- in ItemStack stack parameters are passed by readonly reference (in); MoveRequest is also passed by readonly reference (in).
- No concurrency/threading guarantees specified in this file.
```

```csharp
// Example
// Subscribe to slot change notifications and react to updates
// (Assumes an instance of IInventoryDomain named inventory)
inventory.OnSlotChanged += (instanceId, slotIndex, newStack) =>
{
    // Handle slot update for instanceId and slotIndex
    // newStack can be null if the slot was cleared
};
```

```text
5) Unknowns
- Implementations of ItemStack, MoveRequest, TransactionResult types.
- How instanceId maps to inventories, and thread-safety guarantees.
- Exact error semantics beyond the boolean return value and TransactionResult content.
- Whether OnSlotChanged is raised synchronously or asynchronously.
```
