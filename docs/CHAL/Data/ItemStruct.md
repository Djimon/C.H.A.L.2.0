# Assets/src/Data/Structs/ItemStruct.cs

_Automatically generated/updated from `Assets/src/Data/Structs/ItemStruct.cs`._

# Purpose
- Defines the `ItemKey` struct for representing an item with a category and an ID.
- Provides methods for parsing a string into an `ItemKey` and for converting an `ItemKey` to a string representation.

# Public API
- Namespace: `CHAL.Data`
- Types
  - `public readonly struct ItemKey`
    - Public fields/properties:
      - `public readonly string Category`: The category of the item.
      - `public readonly string Id`: The unique identifier of the item.
    - Public methods:
      - `public ItemKey(string category, string id)`: Constructor to initialize an `ItemKey` with a category and ID.
      - `public static bool TryParse(string s, out ItemKey key)`: Attempts to parse a string into an `ItemKey`. Returns true if successful.
      - `public override string ToString()`: Returns a string representation of the `ItemKey` in the format "Category:Id".

# Key Behavior & Side Effects
- `TryParse` method handles parsing a string and returns a boolean indicating success or failure. It initializes `key` to default if parsing fails.
- `ToString` method provides a formatted string output of the `ItemKey`.

# Constraints & Failure Modes
- `TryParse` returns false if the input string is null, empty, or does not contain exactly one colon.
- The `ItemKey` struct is immutable due to the `readonly` modifier on its fields.

# Example
```csharp
var success = ItemKey.TryParse("Weapon:001", out ItemKey itemKey);
if (success)
{
    Console.WriteLine(itemKey.ToString()); // Outputs: Weapon:001
}
```

# Unknowns
- None.
