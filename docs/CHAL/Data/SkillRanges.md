# CHAL.Data.SkillRanges

_Automatically generated/updated from `Assets/src/Data/Config/GameBalanceConfig.cs`._

# Purpose
- Defines a configuration asset for game balance, including loot parameters, enemy settings, skill ranges, and economy settings.

# Public API
- Namespace: `CHAL.Data`
- Types
  - public class `GameBalanceConfig` [extends ScriptableObject]
    - Public fields/properties:
      - `LootSettings loot` - Configuration for loot parameters.
      - `EnemySettings enemies` - Configuration for enemy parameters.
      - `SkillRanges skillRanges` - Configuration for skill ranges.
      - `bool AllowFriendlyFire` - Indicates if friendly fire is allowed.
      - `EconomySettings economy` - Configuration for economy parameters.
    - Public methods:
      - `int GetMultiplier(EnemyRank rank)` - Gets the multiplier based on the specified enemy rank.
      - `RankScaling GetScaling(EnemyRank rank)` - Gets the scaling associated with the specified enemy rank.
      - `float GetRangeValue(SkillRange range)` - Gets the range value based on the specified skill range.

# Key Behavior & Side Effects
- The `GetMultiplier` method returns a multiplier based on the enemy rank provided.
- The `GetScaling` method returns the rank scaling for the specified enemy rank.
- The `GetRangeValue` method returns the corresponding range value based on the skill range provided.

# Constraints & Failure Modes
- The `GetMultiplier`, `GetScaling`, and `GetRangeValue` methods use a switch expression and default to a fallback value if the provided rank or range is unrecognized.

# Example
```csharp
GameBalanceConfig config = ScriptableObject.CreateInstance<GameBalanceConfig>();
int multiplier = config.loot.rankMultipliers.GetMultiplier(EnemyRank.Normal);
float rangeValue = config.GetRangeValue(SkillRange.Melee);
```

# Unknowns
- None.

