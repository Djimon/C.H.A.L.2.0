# Assets/src/Systems/Inventory/core/Slot.cs

_Automatic generated/updated._

```markdown
## Purpose
- Defines the `Slot` class for inventory management.
- Provides the `SlotFilter` class to specify item filtering criteria.

## Public API
- Namespace: `CHAL.Systems.Inventory`
- Types
  - `public sealed class Slot`
    - Public fields/properties:
      - `int index`: The index of the slot.
      - `int maxStack`: The maximum stack size for items in the slot.
      - `SlotFilter Filter`: Optional filter for items that can be placed in the slot.
      - `ItemStack? stack`: The current item stack in the slot; null if empty.
    - Public methods:
      - `Slot(int i, int mStack, SlotFilter filter = null)`: Constructor to initialize a slot with index, max stack size, and an optional filter.

  - `public sealed class SlotFilter`
    - Public fields/properties:
      - `List<ItemType> AllowedItemTypes`: Types of items allowed in the slot.
      - `List<string> AllowedItemIds`: Specific item IDs allowed in the slot.
      - `List<string> AllowedTags`: Tags of items allowed in the slot.
      - `List<ItemType> BlockedItemTypes`: Types of items blocked from the slot.
      - `List<string> BlockedItemIds`: Specific item IDs blocked from the slot.
      - `List<string> BlockedTags`: Tags of items blocked from the slot.

## Key Behavior & Side Effects
- The `Slot` constructor initializes the slot with an index, maximum stack size, and an optional filter.
- The `stack` property can be set to null to indicate that the slot is empty.

## Constraints & Failure Modes
- The `maxStack` property can be modified internally, but is read-only externally.
- The `Filter` and `stack` properties can be set internally, allowing for controlled access.

## Example
```csharp
var slot = new Slot(0, 10);
slot.Filter = new SlotFilter
{
    AllowedItemTypes = new List<ItemType> { ItemType.Weapon },
    BlockedTags = new List<string> { "QuestItem" }
};
```

## Unknowns
- No information on how `ItemStack` or `ItemType` are defined or used.
- No details on the behavior of the `SlotFilter` in relation to item management.
```
