# Assets/src/Systems/Inventory/core/TransactionResult.cs

_Automatic generated/updated._

```markdown
# Purpose
- Defines the `TransactionResult` class for handling transaction outcomes in an inventory system.

# Public API
- Namespace: `CHAL.Systems.Inventory`
- Types
  - public class `TransactionResult`
    - Public fields/properties:
      - `bool success`: Indicates if the transaction was successful.
      - `string reason`: Provides a reason for the transaction result.
      - `List<(int slotIndex, ItemStack? newStack)> SlotDeltas`: Holds changes to inventory slots, including the index and the new item stack.

# Key Behavior & Side Effects
- No explicit methods or behaviors defined; primarily serves as a data structure for transaction results.

# Constraints & Failure Modes
- `newStack` in `SlotDeltas` can be null, indicating no item in that slot.

# Example
```csharp
var result = new TransactionResult
{
    success = true,
    reason = "Transaction completed successfully.",
    SlotDeltas = new List<(int, ItemStack?)> { (0, new ItemStack()), (1, null) }
};
```

# Unknowns
- No information on the `ItemStack` type or its properties/methods.
```
