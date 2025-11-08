# CHAL.Data.XpSettings

_Automatically generated/updated from `Assets/src/Data/Config/GameBalanceConfig.cs`._

# Purpose
- Defines a configuration asset for game balance, including loot parameters, enemy settings, skill ranges, and economy settings.

# Public API
- Namespace: `CHAL.Data`
- Types
  - `public class GameBalanceConfig : ScriptableObject`
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
- The `GetMultiplier`, `GetScaling`, and `GetRangeValue` methods use a switch expression, defaulting to 1 or the normal scaling/range if an unknown rank/range is provided.
- Serialized fields are subject to Unity's serialization rules.

# Example
```csharp
GameBalanceConfig config = ScriptableObject.CreateInstance<GameBalanceConfig>();
int multiplier = config.loot.rankMultipliers.GetMultiplier(EnemyRank.Normal);
float rangeValue = config.GetRangeValue(SkillRange.Melee);
```

# Unknowns
- None.

