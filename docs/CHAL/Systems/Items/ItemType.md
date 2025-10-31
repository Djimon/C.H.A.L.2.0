# CHAL.Systems.Items.ItemType

_Automatically generated/updated from `Assets/src/Systems/Items/ItemType.cs`._

# Purpose
- Defines utility methods for converting item IDs to `ItemType`.

# Public API
- Namespace: `CHAL.Systems.Items`
- Types
  - **public static class** `ItemTypeUtils`
    - **Public methods**
      - `FromId(string itemId) : ItemType`
        - Converts a string `itemId` to an `ItemType`. Returns `ItemType.Unknown` if `itemId` is null or empty.

# Key Behavior & Side Effects
- Returns specific `ItemType` based on the prefix of the provided `itemId`.
- Handles null or empty `itemId` by returning `ItemType.Unknown`.

# Constraints & Failure Modes
- Returns `ItemType.Unknown` for null, empty, or unrecognized prefixes in `itemId`.
- Assumes `itemId` is formatted correctly with a prefix followed by a colon (if applicable).

# Example
```csharp
ItemType itemType = ItemTypeUtils.FromId("gear:123");
// itemType will be ItemType.Gear
```

# Unknowns
- The definition of `ItemType` and its possible values are not provided in this file.

