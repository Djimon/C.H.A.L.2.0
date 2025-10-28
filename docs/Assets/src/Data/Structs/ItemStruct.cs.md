# Assets/src/Data/Structs/ItemStruct.cs

_Automatic generated/updated._

```markdown
## Purpose
- Defines the `ItemKey` struct for representing an item with a category and an ID.

## Public API
- Namespace: `CHAL.Data`
- Types
  - `public readonly struct ItemKey`
    - Public fields/properties:
      - `public readonly string Category` - Represents the category of the item.
      - `public readonly string Id` - Represents the unique identifier of the item.
    - Public methods:
      - `public ItemKey(string category, string id)` - Constructor to initialize `ItemKey` with category and ID.
      - `public static bool TryParse(string s, out ItemKey key)` - Parses a string to create an `ItemKey`; returns true if successful.
      - `public override string ToString()` - Returns a string representation of the `ItemKey` in the format "Category:Id".

## Key Behavior & Side Effects
- `TryParse` method handles string parsing and returns a boolean indicating success or failure.
- If parsing fails, `key` is set to default (`ItemKey` with empty fields).

## Constraints & Failure Modes
- `TryParse` returns false for null, empty, or improperly formatted strings (not containing exactly one colon).
- The `ItemKey` struct is immutable due to the `readonly` modifier.

## Unknowns
- No unknowns present in the file.
```
