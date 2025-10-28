# Assets/src/Systems/Loot/UnluckyProtection.cs

_Automatic generated/updated._

```markdown
# Purpose
- Manages dry streaks per rarity and calculates chance multipliers.

# Public API
- Namespace: `CHAL.Systems.Loot`
- Types
  - `public class UnluckyProtection`
    - Public methods:
      - `public void OnDrop(Rarity rarity);` // Resets streak for the given rarity.
      - `public void OnFail(Rarity rarity);` // Increases streak for the given rarity.
      - `public float GetMultiplier(Rarity rarity);` // Returns multiplier based on current streak for the rarity.
      - `public string DebugInfo();` // Returns debug information for current streaks.

# Key Behavior & Side Effects
- `OnDrop(Rarity rarity)`: Resets the streak for the specified rarity to 0 and logs the action.
- `OnFail(Rarity rarity)`: Increments the streak for the specified rarity and logs the action.
- `GetMultiplier(Rarity rarity)`: Calculates and returns a multiplier based on the current streak for the specified rarity.

# Constraints & Failure Modes
- Only tracks specific rarities: Rare, Epic, Legendary, Daemonic, Holy, Mythic.
- If rarity is not tracked, methods `OnDrop` and `OnFail` do not affect streaks.

# Unknowns
- The implementation details of `BalanceManager` and `DebugManager`.
- The definition of the `Rarity` enum.
```
