# CHAL.Systems.Inventory.IInventoryDomain

_Automatically generated/updated from `Assets/src/Systems/Inventory/core/IInventoryDomain.cs`._

# Purpose
- Defines the interface for inventory domain operations.

# Public API
- Namespace: `CHAL.Systems.Inventory`
- Types
  - public interface `IInventoryDomain`
    - Public methods:
      - `bool CanAccept(string instanceId, in ItemStack stack);`
      - `bool TryAdd(string instanceId, in ItemStack stack, out TransactionResult result);`
      - `bool TryMove(in MoveRequest req, out TransactionResult result);`
      - `bool TryRemove(string instanceId, int slotIndex, int amount, out TransactionResult result);`
      - `ItemStack? Peek(string instanceId, int slotIndex);`
      - `int SlotCount(string instanceId);`
    - Public fields/properties:
      - `event Action<string, int, ItemStack?> OnSlotChanged;` // (instanceId, slot, newStack)

# Key Behavior & Side Effects
- `OnSlotChanged` event is triggered when a slot in the inventory changes.

# Constraints & Failure Modes
- Method parameters and return types must adhere to the defined signatures.
- `TryAdd`, `TryMove`, and `TryRemove` methods provide an output `TransactionResult` to indicate success or failure.

# Example
```csharp
IInventoryDomain inventory = ...;
TransactionResult result;
if (inventory.TryAdd("instance1", new ItemStack(), out result))
{
    // Item added successfully
}
```

# Unknowns
- The definitions and behaviors of `ItemStack`, `TransactionResult`, and `MoveRequest` are not provided in this file.
