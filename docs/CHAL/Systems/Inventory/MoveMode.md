# CHAL.Systems.Inventory.MoveMode

_Automatically generated/updated from `Assets/src/Systems/Inventory/core/MoveRequest.cs`._

```csharp
// Documentation file for MoveRequest.cs
```

1) Purpose
- Defines data structures to represent an item move operation between inventories.
- Exposes a MoveRequest class with source/destination inventory references, optional amount, and move mode.
- Exposes an ItemMoveObject struct to identify an inventory slot by instanceID and slot index.
- Defines MoveMode enum to describe the operation type (Move, Merge, Swap, Split).

2) Public API
- Namespace: CHAL.Systems.Inventory
- Types
  - public class MoveRequest
    - fromInventory: ItemMoveObject
    - toInventory: ItemMoveObject
    - amount: int?
    - moveMode: MoveMode
  - public struct ItemMoveObject
    - instanceID: string
    - slot: int
  - public enum MoveMode
    - Move
    - Merge
    - Swap
    - Split

3) Key Behavior & Side Effects
- No methods or behavior defined in this file; only data containers.
- No implicit side effects; all fields are public and mutable.

4) Constraints & Failure Modes
- amount is nullable (int?), may be unspecified.
- No validation or guards are defined in this file.
- All fields are plain data; no serialization or threading semantics specified.

5) Example
```csharp
var req = new CHAL.Systems.Inventory.MoveRequest
{
    fromInventory = new CHAL.Systems.Inventory.ItemMoveObject
    {
        instanceID = "inventoryA",
        slot = 0
    },
    toInventory = new CHAL.Systems.Inventory.ItemMoveObject
    {
        instanceID = "inventoryB",
        slot = 2
    },
    amount = 5,
    moveMode = CHAL.Systems.Inventory.MoveMode.Move
};
```

6) Unknowns
- Semantics of how MoveRequest is consumed by other systems.
- Expected constraints on instanceID format or valid slot indexes.
- How MoveMode values affect the operation in practice (beyond naming).
- Serialization, persistence, or threading considerations.
