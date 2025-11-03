# CHAL.Systems.Inventory.ItemStack

_Automatically generated/updated from `Assets/src/Systems/Inventory/core/ItemStack.cs`._

1) Purpose
- Defines public readonly struct ItemStack in CHAL.Systems.Inventory.
- Represents an immutable pair: itemID (string) and count (int).
- Exposes a constructor ItemStack(string id, int itemcount) and a method WithCount(int newCount) to create a new instance with updated count.

2) Public API
- Namespace/module: CHAL.Systems.Inventory
- Types
  - public readonly struct ItemStack
    - Public properties
      - string itemID { get; } — item identifier
      - int count { get; } — item quantity
    - Public methods
      - public ItemStack(string id, int itemcount)
      - public ItemStack WithCount(int newCount)

3) Key Behavior & Side Effects
- ItemStack is an immutable value type (readonly struct; get-only properties).
- WithCount(int) creates and returns a new ItemStack with the same itemID and the provided count.
- The constructor assigns itemID and count; no mutation occurs after construction.

4) Constraints & Failure Modes
- No input validation: itemID can be null; count can be any int (no guards).
- As a readonly struct, instances are value-type copied on assignment.
- No explicit threading or async behavior; immutability implies thread-safety for the data.

5) Example
```csharp
using CHAL.Systems.Inventory;

var stack = new ItemStack("sword_iron", 3);
var updated = stack.WithCount(5);
```

6) Unknowns
- Equality behavior beyond default struct value equality (no Equals/GetHashCode override shown).
- Serialization attributes or Unity-specific serialization behavior not present.
- Validation rules for itemID format or count (not defined in this file).
- How ItemStack interacts with other inventory systems or persistence.

