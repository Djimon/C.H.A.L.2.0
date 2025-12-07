# Assets/src/Systems/Items/ItemType.cs

_Automatically generated/updated from `Assets/src/Systems/Items/ItemType.cs`._

# Purpose
- Provides utility functions for converting item ID strings to their corresponding `ItemType`.

# Public API
- Namespace: `CHAL.Systems.Items`
- Types
  - `public static class ItemTypeUtils`
    - Public methods
      - `public static ItemType FromId(string itemId)` 
        - Converts an item ID string to its corresponding `ItemType`. Returns `ItemType.Unknown` if the item ID is null or empty.

# Key Behavior & Side Effects
- Returns `ItemType.Unknown` for null or empty item IDs.
- Parses the item ID to determine the prefix and returns the corresponding `ItemType` based on predefined cases.

# Constraints & Failure Modes
- Handles null and empty strings by returning `ItemType.Unknown`.
- Assumes valid prefixes are defined; any unrecognized prefix defaults to `ItemType.Unknown`.
