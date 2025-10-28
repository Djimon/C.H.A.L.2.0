# CHAL.Systems.Loot.UnluckyProtection

_Automatically generated/updated from `Assets/src/Systems/Loot/UnluckyProtection.cs`._

# Purpose
- Manages dry streaks per rarity and calculates chance multipliers.

# Public API
- Namespace: `CHAL.Systems.Loot`
- Types
  - public class `UnluckyProtection`
    - Public fields/properties: None
    - Public methods:
      - `UnluckyProtection()`
      - `void OnDrop(Rarity rarity)`
      - `void OnFail(Rarity rarity)`
      - `float GetMultiplier(Rarity rarity)`: Returns the multiplier for the current rarity.
      - `string DebugInfo()`: Returns debug information for logs.

# Key Behavior & Side Effects
- `OnDrop(Rarity rarity)`: Resets the streak for the specified rarity to 0 if it is tracked.
- `OnFail(Rarity rarity)`: Increases the streak for the specified rarity by 1 if it is tracked.
- `GetMultiplier(Rarity rarity)`: Calculates and returns a multiplier based on the current streak and rarity.

# Constraints & Failure Modes
- Only tracks specific rarities: Rare, Epic, Legendary, Daemonic, Holy, Mythic.
- If a rarity is not tracked, methods will not affect the streak.

# Example
```csharp
var protection = new UnluckyProtection();
protection.OnFail(Rarity.Rare);
float multiplier = protection.GetMultiplier(Rarity.Rare);
```

# Unknowns
- None.

