# CHAL.Systems.Inventory.TransactionResult

_Automatically generated/updated from `Assets/src/Systems/Inventory/core/TransactionResult.cs`._

# Purpose
- Defines the `TransactionResult` class for handling inventory transaction outcomes.

# Public API
- Namespace: `CHAL.Systems.Inventory`
- Types
  - public class `TransactionResult`
    - Public fields/properties:
      - `bool success`: Indicates if the transaction was successful.
      - `string reason`: Provides a reason for the transaction result.
      - `List<(int slotIndex, ItemStack? newStack)> SlotDeltas`: Stores changes in inventory slots, including the index and the new item stack.

# Key Behavior & Side Effects
- No explicit behavior or state changes defined beyond the structure of the class.

# Constraints & Failure Modes
- `newStack` in `SlotDeltas` can be null, indicating no item in that slot.

# Example
```csharp
var transactionResult = new TransactionResult
{
    success = true,
    reason = "Transaction completed successfully.",
    SlotDeltas = new List<(int, ItemStack?)>
    {
        (0, new ItemStack()),
        (1, null)
    }
};
```

# Unknowns
- No information on the `ItemStack` type or its behavior.

