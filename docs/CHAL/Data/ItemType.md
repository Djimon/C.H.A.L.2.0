# CHAL.Data.ItemType

_Automatically generated/updated from `Assets/src/Data/Enums/ItemType.cs`._

# Purpose
- Defines an enumeration for different types of items in the game.

# Public API
- Namespace: `CHAL.Data`
- Types:
  - `public enum ItemType`
    - Public fields:
      - `Unknown` = 0
      - `Remains` // Resources
      - `Part` // Materials
      - `Module` // Skill
      - `Gear`
      - `Rune`

# Key Behavior & Side Effects
- Represents distinct categories of items, which can be used for item classification in the game.

# Constraints & Failure Modes
- No explicit guards or error handling present.
- No threading or async considerations noted.

# Example
```csharp
ItemType itemType = ItemType.Gear;
```

# Unknowns
- No unknowns present; all information is derived from the file.
