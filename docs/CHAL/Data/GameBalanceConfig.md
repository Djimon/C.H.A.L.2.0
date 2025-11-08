# CHAL.Data.GameBalanceConfig

_Automatically generated/updated from `Assets/src/Data/Config/GameBalanceConfig.cs`._

# Purpose
- Defines configuration settings for game balance, including loot parameters, enemy settings, skill ranges, and economy settings.

# Public API
- Namespace: `CHAL.Data`
- Types
  - public class `GameBalanceConfig` [extends `ScriptableObject`]
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
- The `GetMultiplier`, `GetScaling`, and `GetRangeValue` methods return values based on the provided rank or range, with default fallbacks.

# Constraints & Failure Modes
- The `GetMultiplier`, `GetScaling`, and `GetRangeValue` methods use a switch expression, which defaults to a specific value if the provided rank or range is unrecognized.

# Example
```csharp
GameBalanceConfig config = ScriptableObject.CreateInstance<GameBalanceConfig>();
int multiplier = config.loot.rankMultipliers.GetMultiplier(EnemyRank.Normal);
float rangeValue = config.GetRangeValue(SkillRange.Melee);
```

# Unknowns
- None.

