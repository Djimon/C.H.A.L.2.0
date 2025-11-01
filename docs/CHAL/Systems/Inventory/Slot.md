# CHAL.Systems.Inventory.Slot

_Automatically generated/updated from `Assets/src/Systems/Inventory/core/Slot.cs`._

```csharp
# Documentation

```

1) Purpose
- Defines a Slot type for an inventory system, representing a single slot with an index, maximum stack size, an optional filter, and a current item stack (nullable).
- Defines a SlotFilter type to express allowed and blocked item criteria (types, ids, and tags) and to test whether a given item id passes the filter.
- Encapsulates basic filter logic via Allows(itemId) and Passes(itemId, tagResolver) that rely on external item-type utilities.

2) Public API

Namespace/Module
- CHAL.Systems.Inventory

Types

- public sealed class Slot
  - Public properties
    - int index { get; }
    - int maxStack { get; internal set; }
    - SlotFilter Filter { get; internal set; }
    - ItemStack? stack { get; internal set; } // null => leer (empty)
  - Public constructor
    - Slot(int i, int mStack, SlotFilter filter = null)

- public sealed class SlotFilter
  - Public fields
    - List<ItemType> AllowedItemTypes
    - List<string> AllowedItemIds
    - List<string> AllowedTags
    - List<ItemType> BlockedItemTypes
    - List<string> BlockedItemIds
    - List<string> BlockedTags
  - Public methods
    - bool Allows(string itemId)
      - Returns Passes(itemId)
    - bool Passes(string itemId, Func<string, IReadOnlyCollection<string>> tagResolver = null)

Notes:
- ItemType, ItemStack, and ItemTypeUtils.FromId(itemId) are referenced types/utilities not defined in this file.
- All public surface items are listed above; internal/private behavior is not exposed here.

3) Key Behavior & Side Effects
- Slot constructor behavior
  - Sets index to i, maxStack to mStack, Filter to filter, and initializes stack to null (empty slot).
- SlotFilter.Allows(itemId)
  - Delegates to Passes(itemId) without a tagResolver.
- SlotFilter.Passes(itemId, tagResolver)
  - If itemId is null/whitespace: returns false.
  - Determines type via ItemTypeUtils.FromId(itemId).
  - Local predicates:
    - InIds(set, id): any match in a string collection (case-insensitive).
    - InTypes(set, t): contains type in the collection.
    - InTags(set, tags): any tag in the item's tags matches a rule in the set (case-insensitive).
  - Resolves item tags via tagResolver(itemId) when provided.
  - Flow:
    1) If item is blocked by Id, Type, or Tag lists → return false.
    2) If no Allowed* lists are configured (none non-empty) → return true.
    3) If any Allowed* rule matches (Id, Type, or Tag) → return true.
    4) Otherwise → return false.

4) Constraints & Failure Modes
- Item blockers precede allowances: BlockedItemIds, BlockedItemTypes, BlockedTags take precedence over Allowed lists.
- If no allowed lists are configured, any item not blocked is allowed.
- Null/empty itemId handling: returns false.
- Tag resolution is optional; if tagResolver is null, tag checks are effectively skipped.
- Comparisons:
  - Item id comparisons use string equality with case-insensitive semantics for IDs.
  - Tag comparisons use case-insensitive semantics.
- State mutability:
  - Some properties have internal setters, indicating mutation is restricted to the defining assembly.
  - Stack being null indicates an empty slot.
- Serializable attribute on SlotFilter indicates it is intended for serialization, e.g., saving/loading state.

5) Example

// Minimal example: create a slot with a filter and check passability
using CHAL.Systems.Inventory;
using CHAL.Systems.Items;
using System.Collections.Generic;

var filter = new SlotFilter
{
    AllowedItemTypes = new List<ItemType> { ItemType.Weapon }
};

var slot = new Slot(0, 64, filter);

bool canPlace = slot.Filter?.Allows("weapon_sword01") ?? false;

```

6) Unknowns
- Definitions and members of ItemType, ItemStack, and ItemTypeUtils.FromId are not present in this file.
- Exact semantics of ItemType values and item tagging conventions are external to this file.
- Behavior of external code that might mutate Slot or SlotFilter state (due to internal setters) is not visible here.
