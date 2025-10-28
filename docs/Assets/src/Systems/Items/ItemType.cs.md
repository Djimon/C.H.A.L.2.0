# Assets/src/Systems/Items/ItemType.cs

_Automatic generated/updated._

```markdown
# Purpose
- Defines utility methods for converting item IDs to `ItemType`.

# Public API
- Namespace: `CHAL.Systems.Items`
- Types
  - `public static class ItemTypeUtils`
    - Public methods:
      - `public static ItemType FromId(string itemId)`: Converts a string `itemId` to an `ItemType`. Returns `ItemType.Unknown` if `itemId` is null or empty.

# Key Behavior & Side Effects
- Returns specific `ItemType` based on the prefix of the provided `itemId`.
- Handles null or empty `itemId` by returning `ItemType.Unknown`.

# Constraints & Failure Modes
- If `itemId` is null or empty, the method returns `ItemType.Unknown`.
- The method uses string manipulation and a switch statement to determine the `ItemType`.

# Example
```csharp
ItemType itemType = ItemTypeUtils.FromId("rune:123");
// itemType will be ItemType.Rune
```

# Unknowns
- The definition of `ItemType` and its possible values are not provided in this file.
```
