# Assets/src/Data/Enums/ItemRarity.cs

_Automatic generated/updated._

```markdown
## Purpose
- Defines an enumeration for item rarity levels.
- Provides a static class to map rarity levels to corresponding colors.

## Public API
- Namespace: `CHAL.Data`
- Types
  - `enum Rarity`
    - Values: `unknown`, `Common`, `Uncommon`, `Rare`, `Epic`, `Legendary`, `Mythic`, `Holy`, `Daemonic`
  - `static class RarityColors`
    - Public methods:
      - `static Color Get(Rarity rarity)`: Returns the color associated with the specified rarity; defaults to `Color.white` if rarity is not found.

## Key Behavior & Side Effects
- `Get` method retrieves a color based on the rarity; if the rarity is not in the map, it returns `Color.white`.

## Constraints & Failure Modes
- The `Get` method handles cases where the rarity is not found by returning a default color (`Color.white`).

## Example
```csharp
Color rarityColor = RarityColors.Get(Rarity.Epic);
```

## Unknowns
- None.
```
