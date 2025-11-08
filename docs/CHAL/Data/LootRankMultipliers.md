# CHAL.Data.LootRankMultipliers

_Automatically generated/updated from `Assets/src/Data/Config/GameBalanceConfig.cs`._

# Purpose
- Defines a configuration asset for game balance, including loot parameters, enemy settings, skill ranges, and economy settings.

# Public API
- Namespace: `CHAL.Data`
- Types
  - **public class GameBalanceConfig** [extends ScriptableObject]
    - **public LootSettings loot**: Configuration for loot parameters.
    - **public EnemySettings enemies**: Configuration for enemy parameters.
    - **public SkillRanges skillRanges**: Configuration for skill range parameters.
    - **public bool AllowFriendlyFire**: Indicates if friendly fire is allowed.
    - **public EconomySettings economy**: Configuration for economy parameters.
    - **public float GetRangeValue(SkillRange range)**: Returns the range value based on the specified skill range.
    - **public int GetMultiplier(EnemyRank rank)**: Gets the multiplier based on the specified enemy rank.
    - **public RankScaling GetScaling(EnemyRank rank)**: Gets the scaling associated with the specified enemy rank.

# Key Behavior & Side Effects
- The `GetRangeValue` method returns a float based on the provided `SkillRange`.
- The `GetMultiplier` method returns an integer multiplier based on the provided `EnemyRank`.
- The `GetScaling` method returns a `RankScaling` struct based on the provided `EnemyRank`.

# Constraints & Failure Modes
- The `GetRangeValue`, `GetMultiplier`, and `GetScaling` methods use a switch expression, defaulting to specific values if the provided rank or range is unrecognized.

# Example
```csharp
GameBalanceConfig config = ScriptableObject.CreateInstance<GameBalanceConfig>();
float meleeRangeValue = config.GetRangeValue(SkillRange.Melee);
int normalMultiplier = config.loot.rankMultipliers.GetMultiplier(EnemyRank.Normal);
```

# Unknowns
- None.

