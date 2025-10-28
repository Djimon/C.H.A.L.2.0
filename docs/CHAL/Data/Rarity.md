# CHAL.Data.Rarity

_Automatically generated/updated from `Assets/src/Data/Enums/ItemRarity.cs`._

# Purpose
- Defines an enumeration for item rarity levels.
- Provides a static class to map rarity levels to corresponding colors.

# Public API
- Namespace: CHAL.Data
- Types
  - public enum Rarity
    - Values: unknown, Common, Uncommon, Rare, Epic, Legendary, Mythic, Holy, Daemonic
  - public static class RarityColors
    - Public methods:
      - static Color Get(Rarity rarity) => returns the color associated with the specified rarity; defaults to Color.white if rarity is not found.

# Key Behavior & Side Effects
- The `Get` method retrieves a color based on the rarity; if the rarity is not found in the map, it returns Color.white.

# Constraints & Failure Modes
- The `Get` method handles cases where the rarity is not present in the dictionary by returning a default color (Color.white).

# Example
```csharp
Color rarityColor = RarityColors.Get(Rarity.Epic); // returns the color associated with Epic rarity
```

# Unknowns
- None.

