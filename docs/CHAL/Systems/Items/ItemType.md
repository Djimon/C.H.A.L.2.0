# CHAL.Systems.Items.ItemType

_Automatically generated/updated from `Assets/src/Systems/Items/ItemType.cs`._

# Purpose
- Provides utility functions for converting item ID strings to corresponding `ItemType` enums.

# Public API
- Namespace: `CHAL.Systems.Items`
- Types
  - public static class `ItemTypeUtils`
    - Public methods:
      - `static ItemType FromId(string itemId)` 
        - Converts an item ID string to its corresponding `ItemType`. Returns `ItemType.Unknown` if the item ID is null or empty.

# Key Behavior & Side Effects
- Returns `ItemType.Unknown` for null or empty item IDs.
- Parses the item ID to determine the prefix and maps it to the corresponding `ItemType`.

# Constraints & Failure Modes
- Handles null and empty strings by returning `ItemType.Unknown`.
- Assumes valid prefixes are defined; any unrecognized prefix results in `ItemType.Unknown`.
