# Assets/src/Data/Enums/ItemRarity.cs

_Automatically generated/updated from `Assets/src/Data/Enums/ItemRarity.cs`._

1) Purpose
- Defines an enumeration for item rarity levels.
- Provides a static class to retrieve colors associated with each rarity.

2) Public API
- Namespace: `CHAL.Data`
- Types
  - `public enum Rarity`
    - Values: `unknown`, `Common`, `Uncommon`, `Rare`, `Epic`, `Legendary`, `Mythic`, `Holy`, `Daemonic`
  - `public static class RarityColors`
    - Public methods:
      - `public static Color Get(Rarity rarity)` : Retrieves the color associated with the specified rarity, returning white if not found.

3) Key Behavior & Side Effects
- The `Get` method retrieves a color from a predefined dictionary based on the rarity provided.

4) Constraints & Failure Modes
- If the rarity is not found in the dictionary, the `Get` method returns `Color.white`.

5) Example
```csharp
Color rarityColor = RarityColors.Get(Rarity.Epic);
```

6) Unknowns
- None.
