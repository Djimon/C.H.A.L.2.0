# Assets/src/Systems/Inventory/core/IInventoryDomain.cs

_Automatically generated/updated from `Assets/src/Systems/Inventory/core/IInventoryDomain.cs`._

# Purpose
- Defines the interface for inventory domain operations.

# Public API
- Namespace: `CHAL.Systems.Inventory`
- Types
  - `public interface IInventoryDomain`
    - Public methods:
      - `bool CanAccept(string instanceId, in ItemStack stack);`
      - `bool TryAdd(string instanceId, in ItemStack stack, out TransactionResult result);`
      - `bool TryMove(in MoveRequest req, out TransactionResult result);`
      - `bool TryRemove(string instanceId, int slotIndex, int amount, out TransactionResult result);`
      - `ItemStack? Peek(string instanceId, int slotIndex);`
      - `int SlotCount(string instanceId);`
    - Public event:
      - `event Action<string, int, ItemStack?> OnSlotChanged;` // (instanceId, slot, newStack)

# Key Behavior & Side Effects
- `OnSlotChanged` event is triggered when a slot in the inventory changes.

# Constraints & Failure Modes
- Methods may return `false` to indicate failure (e.g., `TryAdd`, `TryMove`, `TryRemove`).
- `Peek` may return `null` if the specified slot is empty.

# Example
```csharp
IInventoryDomain inventory = ...;
if (inventory.CanAccept("instance1", new ItemStack()))
{
    TransactionResult result;
    if (inventory.TryAdd("instance1", new ItemStack(), out result))
    {
        // Item added successfully
    }
}
```

# Unknowns
- None.
