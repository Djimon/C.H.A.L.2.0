# Assets/src/Systems/Inventory/core/ItemStack.cs

_Automatic generated/updated._

```markdown
1) Purpose
- Defines the `ItemStack` structure for managing items in an inventory system.

2) Public API
- Namespace: `CHAL.Systems.Inventory`
- Types
  - public readonly struct `ItemStack`
    - Public fields/properties:
      - `string itemID`: Identifier for the item.
      - `int count`: Quantity of the item.
    - Public methods:
      - `ItemStack(string id, int itemcount)`: Constructor to initialize an `ItemStack`.
      - `ItemStack WithCount(int newCount)`: Returns a new `ItemStack` with the specified count.

3) Key Behavior & Side Effects
- The `WithCount` method creates a new `ItemStack` instance with a modified count, leaving the original instance unchanged.

4) Constraints & Failure Modes
- No explicit guards or null handling present in the code.
- Assumes valid string and integer inputs for the constructor.

5) Example
```csharp
var stack = new ItemStack("item_001", 10);
var updatedStack = stack.WithCount(5);
```

6) Unknowns
- None.
```
