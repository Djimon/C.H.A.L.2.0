# CHAL.Data.ItemStruct

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
      - `public static bool TryParse(string s, out ItemKey key)`: Attempts to parse a string into an `ItemKey`. Returns true if successful.
      - `public override string ToString()`: Returns a string representation of the `ItemKey`.

# Key Behavior & Side Effects
- `TryParse` method parses a string formatted as "Category:Id" into an `ItemKey`. Returns false if the input string is null, empty, or improperly formatted.
- `ToString` method provides a formatted string output of the `ItemKey`.

# Constraints & Failure Modes
- `TryParse` handles null or whitespace strings by returning false.
- The method requires the input string to contain exactly one colon to successfully parse into an `ItemKey`.

# Example
```csharp
ItemKey key;
if (ItemKey.TryParse("Weapon:001", out key))
{
    Console.WriteLine(key.ToString()); // Outputs: Weapon:001
}
```

# Unknowns
- None.
