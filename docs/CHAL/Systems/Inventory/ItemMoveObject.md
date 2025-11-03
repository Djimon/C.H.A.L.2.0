# CHAL.Systems.Inventory.ItemMoveObject

_Automatically generated/updated from `Assets/src/Systems/Inventory/core/MoveRequest.cs`._

1) Purpose
- Defines data containers for inventory move operations.
- MoveRequest: describes a request to move items between inventories.
- ItemMoveObject: identifies a specific item location by instance ID and slot.
- MoveMode: enumerates how a move should be performed (Move, Merge, Swap, Split).

2) Public API
- Namespace/module
  - CHAL.Systems.Inventory

- Types
  - public class MoveRequest
    - fromInventory: ItemMoveObject
      - Public field identifying source inventory/item location.
    - toInventory: ItemMoveObject
      - Public field identifying destination inventory/item location.
    - amount: int?
      - Public field optional amount to move.
    - moveMode: MoveMode
      - Public field indicating move behavior (Move/Merge/Swap/Split).
  - public struct ItemMoveObject
    - instanceID: string
      - Public field item instance identifier.
    - slot: int
      - Public field slot index within the inventory.
  - public enum MoveMode
    - Move
    - Merge
    - Swap
    - Split

3) Key Behavior & Side Effects
- No executable logic present; this file defines data shapes only.
- No methods; all behavior implied by consumer code using these types.

4) Constraints & Failure Modes
- amount is nullable (int?), allowing absence of a specified amount.
- instanceID is a string; nullability and validation are not defined here.
- No guards, threading, or async behavior defined in this file.

5) Example
- Minimal usage example (in same namespace):
```csharp
var req = new MoveRequest
{
    fromInventory = new ItemMoveObject { instanceID = "item-42", slot = 0 },
    toInventory = new ItemMoveObject { instanceID = "bag-01", slot = 3 },
    amount = 5,
    moveMode = MoveMode.Move
};
```

6) Unknowns
- Semantics of each MoveMode value in runtime context.
- How ItemMoveObject.instanceID maps to actual inventory systems.
- Validation rules, defaults, or serialization behavior for these types.
- Any lifecycle or integration details outside this file.

