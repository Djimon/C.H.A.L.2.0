# CHAL.Systems.Items.ItemType

_Automatically generated/updated from `Assets/src/Systems/Items/ItemType.cs`._

1) Purpose
- Defines a stateless utility to map item identifiers to ItemType values.
- FromId(string itemId) parses a prefix before ':' (or the full string if no colon) and maps known prefixes to ItemType values.
- Returns ItemType.Unknown for null/empty input or unrecognized prefixes. Relies on ItemType defined in CHAL.Data.

2) Public API
- Namespace/module: CHAL.Systems.Items
- Types
  - public static class ItemTypeUtils
    - Public methods
      - public static ItemType FromId(string itemId)
        - Returns corresponding ItemType (Remains, Rune, Part, Module, Gear) or Unknown
        - Side effects: none (pure function)

3) Key Behavior & Side Effects
- Input handling:
  - If itemId is null or empty, returns ItemType.Unknown.
- Prefix extraction:
  - Finds first ':'; if present, prefix = substring before ':'; else prefix = itemId.
- Mapping:
  - "remains" -> ItemType.Remains
  - "rune"    -> ItemType.Rune
  - "part"    -> ItemType.Part
  - "module"  -> ItemType.Module
  - "gear"    -> ItemType.Gear
  - default   -> ItemType.Unknown
- No state changes or exceptions; deterministic mapping based on input.

4) Constraints & Failure Modes
- Guards:
  - Handles null/empty input gracefully (returns Unknown).
  - Safely handles strings without ':' by using the whole string as prefix.
- Threading/async:
  - Stateless static method; thread-safe (no shared mutable state).
- Performance/allocation:
  - Uses string.IsNullOrEmpty, IndexOf, and Substring; minimal allocations for typical usage.

5) Example
- Mapping with prefix and colon:
  - var t = ItemTypeUtils.FromId("gear:plate");
  - t == ItemType.Gear
- No-colon case:
  - var t2 = ItemTypeUtils.FromId("rune");
  - t2 == ItemType.Rune
- Null/empty:
  - ItemTypeUtils.FromId(null) // ItemType.Unknown
  - ItemTypeUtils.FromId(string.Empty) // ItemType.Unknown

6) Unknowns
- Full ItemType enum definition is not present in this file (only usage). Unknowns include:
  - Whether ItemType defines additional members beyond Remains, Rune, Part, Module, Gear, Unknown.
  - Any broader semantics of ItemType values beyond this mapping.
- Behavior of ItemType (enum exact underlying type, serialization specifics) is not shown here.

