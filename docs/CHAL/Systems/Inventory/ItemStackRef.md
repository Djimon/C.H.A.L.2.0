# Assets/src/Systems/Inventory/core/ItemStackRef.cs

_Automatically generated/updated from `Assets/src/Systems/Inventory/core/ItemStackRef.cs`._

# Purpose
- Defines the `ItemStackRef` struct for representing an item stack in an inventory system.

# Public API
- Namespace: `CHAL.Systems.Inventory`
- Types
  - `public readonly struct ItemStackRef`
    - Public fields/properties:
      - `string itemID` - Identifier for the item.
      - `int count` - Number of items in the stack.
      - `string instanceId` - Identifier for the specific instance; null/empty indicates no instance.
    - Public methods:
      - `ItemStackRef WithCount(int newCount)` - Returns a new `ItemStackRef` with the specified count.
      - `ItemStackRef WithInstance(string newInstanceId)` - Returns a new `ItemStackRef` with the specified instance ID.
      - `override string ToString()` - Returns a string representation of the item stack.

# Key Behavior & Side Effects
- `IsEmpty` property checks if the item stack is empty (either `itemID` is null/whitespace or `count` is less than or equal to 0).
- `IsInstanced` property checks if the item stack has an associated instance ID.
- Constructor ensures `itemID` is never null (defaults to an empty string) and `count` is non-negative.

# Constraints & Failure Modes
- `itemID` is set to an empty string if null is provided.
- `count` is clamped to a minimum of 0.
- `instanceId` can be null or empty, indicating no instance.

# Example
```csharp
var itemStack = new ItemStackRef("item_001", 5);
var updatedStack = itemStack.WithCount(10);
var instancedStack = itemStack.WithInstance("instance_001");
Console.WriteLine(instancedStack.ToString()); // Outputs: item_001 x5 (inst:instance_001)
```

# Unknowns
- None.
