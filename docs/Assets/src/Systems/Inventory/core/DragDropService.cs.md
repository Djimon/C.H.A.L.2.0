# Assets/src/Systems/Inventory/core/DragDropService.cs

_Automatic generated/updated._

```markdown
# Purpose
- Defines the `DragDropService` class for handling drag-and-drop operations in an inventory system.

# Public API
- Namespace: `CHAL.Systems.Inventory`
- Types
  - `public sealed class DragDropService`
    - Public fields/properties:
      - `public bool HasFrom` - Indicates if there is an item being dragged.
      - `public ItemMoveObject From` - The source item being dragged.
      - `public bool IsSplit` - Indicates if the drag operation is a split.
    - Public methods:
      - `public DragDropService(IInventoryDomain domain)` - Constructor that initializes the service with an inventory domain.
      - `public event Action<ItemStack,bool> OnBeginDrag` - Event triggered when dragging begins.
      - `public event Action OnEndDrag` - Event triggered when dragging ends.
      - `public void BeginDrag(ItemMoveObject from, bool splitHalf)` - Starts the drag operation with the specified item and split mode.
      - `public void Cancel()` - Cancels the current drag operation.
      - `public void TryDropOn(ItemMoveObject to)` - Attempts to drop the dragged item onto a specified target.

# Key Behavior & Side Effects
- `BeginDrag`: Initializes drag operation, sets the source item, and triggers `OnBeginDrag` if the item exists in the inventory.
- `Cancel`: Resets drag state and triggers `OnEndDrag`.
- `TryDropOn`: Checks if the drop target is the same as the source; if so, cancels if splitting. If different, attempts to move the item and cancels on failure.

# Constraints & Failure Modes
- Drag operation can only proceed if an item is set as the source (`_hasFrom`).
- If the drop target is the same as the source and a split is requested, the operation is canceled.
- If the move operation fails, the drag is canceled and an error is logged.

# Example
```csharp
var dragDropService = new DragDropService(inventoryDomain);
dragDropService.BeginDrag(itemMoveObject, true);
dragDropService.TryDropOn(targetItemMoveObject);
```

# Unknowns
- The implementation details of `IInventoryDomain`, `ItemMoveObject`, `ItemStack`, and `MoveRequest` are not provided in this file.
```
