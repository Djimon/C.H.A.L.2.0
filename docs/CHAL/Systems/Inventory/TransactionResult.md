# Assets/src/Systems/Inventory/core/TransactionResult.cs

_Automatically generated/updated from `Assets/src/Systems/Inventory/core/TransactionResult.cs`._

# Purpose
- Defines the `TransactionResult` class used for representing the outcome of inventory transactions.

# Public API
- Namespace: `CHAL.Systems.Inventory`
- Types
  - public class `TransactionResult`
    - Public fields/properties:
      - `bool success`: Indicates if the transaction was successful.
      - `string reason`: Provides a reason for the transaction result.
      - `List<(int slotIndex, ItemStack? newStack)> SlotDeltas`: Holds changes to inventory slots, including the slot index and the new item stack (nullable).

# Key Behavior & Side Effects
- No explicit behavior or side effects are defined in this file.

# Constraints & Failure Modes
- No specific guards, null/empty handling, threading/async notes, or performance hints are evident in this file.

# Example
```csharp
var transactionResult = new TransactionResult
{
    success = true,
    reason = "Transaction completed successfully.",
    SlotDeltas = new List<(int, ItemStack?)>
    {
        (0, new ItemStack()), // Example of a new stack in slot 0
        (1, null) // Example of an empty slot at index 1
    }
};
```

# Unknowns
- The definition of `ItemStack` is not provided in this file.
