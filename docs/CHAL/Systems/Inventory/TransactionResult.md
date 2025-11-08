# CHAL.Systems.Inventory.TransactionResult

_Automatically generated/updated from `Assets/src/Systems/Inventory/core/TransactionResult.cs`._

# Purpose
- Defines the `TransactionResult` class used for representing the outcome of inventory transactions.

# Public API
- Namespace: `CHAL.Systems.Inventory`
- Types
  - public class `TransactionResult`
    - Public fields/properties:
      - `bool success`: Indicates if the transaction was successful.
      - `string reason`: Provides the reason for the transaction result.
      - `List<(int slotIndex, ItemStack? newStack)> SlotDeltas`: Stores changes to item stacks in inventory slots.

# Key Behavior & Side Effects
- The `TransactionResult` class encapsulates the result of an inventory transaction, including success status, reason for failure, and any changes to item stacks.

# Constraints & Failure Modes
- The `newStack` in `SlotDeltas` can be null, indicating no new stack was assigned to the slot.

# Example
```csharp
var result = new TransactionResult
{
    success = true,
    reason = "Transaction completed successfully.",
    SlotDeltas = new List<(int, ItemStack?)>
    {
        (0, new ItemStack()), // Example of a new stack in slot 0
        (1, null) // Example of no new stack in slot 1
    }
};
```

# Unknowns
- The definition and behavior of `ItemStack` are not provided in this file.
