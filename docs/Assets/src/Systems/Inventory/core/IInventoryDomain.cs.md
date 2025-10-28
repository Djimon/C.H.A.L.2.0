# Assets/src/Systems/Inventory/core/IInventoryDomain.cs

_Automatic generated/updated._

```markdown
## Purpose
- Defines the `IInventoryDomain` interface for inventory management operations.

## Public API
- Namespace: `CHAL.Systems.Inventory`
- Types
  - `public interface IInventoryDomain`
    - Public methods:
      - `bool TryAdd(string instanceId, in ItemStack stack, out TransactionResult result);`
      - `bool TryMove(in MoveRequest req, out TransactionResult result);`
      - `bool TryRemove(string instanceId, int slotIndex, int amount, out TransactionResult result);`
      - `ItemStack? Peek(string instanceId, int slotIndex);`
      - `int SlotCount(string instanceId);`
    - Public event:
      - `event Action<string, int, ItemStack?> OnSlotChanged;` // (instanceId, slot, newStack)

## Key Behavior & Side Effects
- `TryAdd`: Attempts to add an `ItemStack` to the inventory; modifies `result` to indicate success or failure.
- `TryMove`: Attempts to move an item within the inventory; modifies `result` to indicate success or failure.
- `TryRemove`: Attempts to remove a specified amount of items from a slot; modifies `result` to indicate success or failure.
- `OnSlotChanged`: Triggered when an item stack in a slot changes.

## Constraints & Failure Modes
- Methods return `false` on failure, with `result` providing additional context.
- `Peek` returns `null` if the slot is empty or does not exist.
- `SlotCount` returns the number of slots for the specified `instanceId`.

## Unknowns
- The implementation details of `ItemStack`, `TransactionResult`, and `MoveRequest` are not provided in this file.
```
