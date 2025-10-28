# CHAL.Systems.Inventory.IInventoryDomain

_Automatically generated/updated from `Assets/src/Systems/Inventory/core/IInventoryDomain.cs`._

# Purpose
- Defines the `IInventoryDomain` interface for inventory management operations.

# Public API
- Namespace: `CHAL.Systems.Inventory`
- Types
  - public interface `IInventoryDomain`
    - Public methods:
      - `bool TryAdd(string instanceId, in ItemStack stack, out TransactionResult result);`
      - `bool TryMove(in MoveRequest req, out TransactionResult result);`
      - `bool TryRemove(string instanceId, int slotIndex, int amount, out TransactionResult result);`
      - `ItemStack? Peek(string instanceId, int slotIndex);`
      - `int SlotCount(string instanceId);`
    - Public event:
      - `event Action<string, int, ItemStack?> OnSlotChanged;` // (instanceId, slot, newStack)

# Key Behavior & Side Effects
- `TryAdd`: Attempts to add an `ItemStack` to the inventory; modifies the inventory state.
- `TryMove`: Attempts to move an item within the inventory; modifies the inventory state.
- `TryRemove`: Attempts to remove a specified amount of items from a slot; modifies the inventory state.
- `OnSlotChanged`: Triggered when an inventory slot changes, providing the instance ID, slot index, and new stack.

# Constraints & Failure Modes
- Methods return `false` on failure, indicating the operation did not succeed.
- `Peek` returns `null` if the specified slot is empty or does not exist.
- `SlotCount` provides the number of slots available for a given instance ID.

# Example
```csharp
IInventoryDomain inventory = ...; // Obtain an instance of IInventoryDomain
TransactionResult result;
if (inventory.TryAdd("instance1", new ItemStack(), out result))
{
    // Item added successfully
}
```

# Unknowns
- The definitions of `ItemStack`, `TransactionResult`, and `MoveRequest` are not provided in this file.

