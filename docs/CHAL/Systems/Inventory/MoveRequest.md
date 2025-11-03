# CHAL.Systems.Inventory.MoveRequest

_Automatically generated/updated from `Assets/src/Systems/Inventory/core/MoveRequest.cs`._

```csharp
1) Purpose
- Defines MoveRequest class with public fields for item movement between inventories.
- Defines ItemMoveObject struct to reference a specific item location (by instanceID and slot).
- Defines MoveMode enum to describe the move operation semantics (Move, Merge, Swap, Split).

2) Public API
- Namespace/module
  - CHAL.Systems.Inventory
- Types
  - public class MoveRequest
    - public ItemMoveObject fromInventory;
    - public ItemMoveObject toInventory;
    - public int? amount;
    - public MoveMode moveMode;
  - public struct ItemMoveObject
    - public string instanceID;
    - public int slot;
  - public enum MoveMode
    - Move
    - Merge
    - Swap
    - Split

3) Key Behavior & Side Effects
- No methods defined; acts as a data container.
- All fields are public; can be read or assigned directly.
- No constructors defined; default parameterless constructor provided by compiler.

4) Constraints & Failure Modes
- amount is nullable (int?); may be null.
- No validation or guards present in this file.
- Strings and structs are public fields; nullability follows C# defaults (string may be null).

5) Example
```csharp
var req = new CHAL.Systems.Inventory.MoveRequest
{
    fromInventory = new CHAL.Systems.Inventory.ItemMoveObject { instanceID = "itemA", slot = 0 },
    toInventory   = new CHAL.Systems.Inventory.ItemMoveObject { instanceID = "inventoryB", slot = 3 },
    amount        = 5,
    moveMode      = CHAL.Systems.Inventory.MoveMode.Move
};
```

6) Unknowns
- Semantics of how MoveMode values affect behavior in the surrounding system.
- Intended meaning of instanceID formatting and how inventories are resolved.
- How MoveRequest is used (serialization, threading, validation) outside this file.
- Any additional invariants or lifecycle expectations in consuming code.
```
