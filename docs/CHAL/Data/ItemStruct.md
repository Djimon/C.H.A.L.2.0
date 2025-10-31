# CHAL.Data.ItemStruct

_Automatically generated/updated from `Assets/src/Data/Structs/ItemStruct.cs`._

# Purpose
- Defines the `ItemKey` struct for representing an item with a category and an ID.

# Public API
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

# Key Behavior & Side Effects
- `TryParse` method handles parsing of a string into an `ItemKey`, returning false for invalid formats or null/whitespace strings.
- `ToString` method formats the `ItemKey` as "Category:Id".

# Constraints & Failure Modes
- `TryParse` guards against null or whitespace input and ensures the input string contains exactly two parts separated by a colon.

# Example
```csharp
var success = ItemKey.TryParse("Weapon:001", out ItemKey key);
if (success) {
    Console.WriteLine(key.ToString()); // Outputs: Weapon:001
}
```

# Unknowns
- None.

