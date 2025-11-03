# CHAL.Systems.Inventory.DragDropService

_Automatically generated/updated from `Assets/src/Systems/Inventory/core/DragDropService.cs`._

```csharp
- Purpose
  - Defines a DragDropService that tracks the source of a drag-and-drop operation within the inventory.
  - Uses IInventoryDomain to peek at source stacks and to perform moves (including split moves).
  - Emits events for drag start and end; controls internal state for source item and split flag.

- Public API
- Namespace/module
  - CHAL.Systems.Inventory

- Types
  - public sealed class DragDropService
    - public DragDropService(IInventoryDomain domain)

    - public bool HasFrom { get; }
      - true when a source item has been set via BeginDrag

    - public ItemMoveObject From { get; }
      - the source ItemMoveObject for the current drag

    - public bool IsSplit { get; }
      - true if the current drag represents a split quantity

    - public event Action<ItemStack, bool> OnBeginDrag
      - invoked when a drag begins and a source stack is available
      - parameters: ItemStack stack, bool splitHalf

    - public event Action OnEndDrag
      - invoked when a drag ends or is canceled

    - public void BeginDrag(ItemMoveObject from, bool splitHalf)
      - initializes drag state with source item and split flag
      - logs the drag start
      - peeks the domain for the source stack and, if available, raises OnBeginDrag with (stack, splitHalf)

    - public void Cancel()
      - clears drag state (HasFrom, IsSplit)
      - raises OnEndDrag

    - public void TryDropOn(ItemMoveObject to)
      - attempts to drop the dragged item onto target slot
      - no-op if there is no active source (HasFrom is false)
      - if dropping onto the same slot:
        - if splitting (IsSplit): Cancel()
        - else: no-op (ghost remains)
      - constructs a MoveRequest with fromInventory, toInventory, moveMode (Split or Move), amount = null
      - calls _domain.TryMove(req, out res); on failure, logs error, Cancel()
      - on success, logs success and Cancel()

- Key Behavior & Side Effects
- Drag start
  - BeginDrag updates internal state: _from = from, _hasFrom = true, _splitHalf = splitHalf
  - Logs begin event
  - Peeks inventory domain for the source stack: _domain.Peek(from.instanceID, from.slot)
  - If a stack is found (stack.HasValue), raises OnBeginDrag with (stack.Value, splitHalf)

- Drag cancel
  - Cancel resets internal state: _hasFrom = false, _splitHalf = false
  - Raises OnEndDrag

- Dropping onto a target
  - TryDropOn guards against missing source (_hasFrom)
  - If destination is the same slot as source:
    - If splitting (_splitHalf): Cancel() (no meaningful split on same slot)
    - Else: return (ghost remains; no move)
  - If destination differs:
    - Builds MoveRequest:
      - fromInventory = _from
      - toInventory = to
      - moveMode = _splitHalf ? MoveMode.Split : MoveMode.Move
      - amount = null
    - Calls _domain.TryMove(req, out var res)
    - If move fails:
      - Logs failure with res.reason
      - Cancel()
    - If move succeeds:
      - Logs success
      - Cancel()

- Side effects and logging
  - Logs indicate Begin, Move failure, and Move success
  - OnBeginDrag, OnEndDrag may be invoked depending on flow
  - Internal state (From, HasFrom, IsSplit) is updated accordingly

- Constraints & Failure Modes
- State safety
  - TryDropOn returns early if there is no active source (HasFrom is false)
  - Cancel resets both _hasFrom and _splitHalf to safe defaults

- Same-slot handling
  - Dropping onto the same slot with split requested triggers Cancel (no operation)

- Move attempts
  - If IInventoryDomain.TryMove fails, a debug message is logged and the drag is canceled
  - If TryMove succeeds, a success log is emitted and the drag is canceled

- Observability
  - OnBeginDrag is only invoked if the domain.Peek returns a value
  - OnEndDrag is invoked on cancel or after a drop attempt completes (success or failure)

- Threading/async
  - No asynchronous code; all operations are synchronous within the provided methods

- Null handling
  - From/stack values are assumed non-null when engaged; safety via HasFrom checks

- Example
// Example usage (minimal)
IInventoryDomain domain = /* obtain domain instance */;
var dragService = new CHAL.Systems.Inventory.DragDropService(domain);

var source = new ItemMoveObject { instanceID = 1, slot = 0 };
dragService.BeginDrag(source, splitHalf: true);

// ... user drags over a target slot ...

var target = new ItemMoveObject { instanceID = 2, slot = 1 };
dragService.TryDropOn(target);

// On cancel (e.g., ESC)
dragService.Cancel();

- Unknowns
- Definitions not present in this file:
  - IInventoryDomain (Peek, TryMove)
  - ItemMoveObject
  - ItemStack
  - MoveRequest
  - MoveMode
  - DebugManager
- Exact behaviors of Peek/TryMove and the data structures they manipulate are not specified here
- Any side effects from OnBeginDrag/OnEndDrag beyond what is shown depend on external subscribers
```
