# CHAL.Data.RankScaling

_Automatically generated/updated from `Assets/src/Data/Config/GameBalanceConfig.cs`._

# Purpose
- Defines a configuration asset for game balance, including loot parameters, enemy settings, skill ranges, and economy settings.

# Public API
- Namespace: `CHAL.Data`
- Types
  - **public class GameBalanceConfig** [extends ScriptableObject]
    - **public LootSettings loot**: Configuration for loot parameters.
    - **public EnemySettings enemies**: Configuration for enemy parameters.
    - **public SkillRanges skillRanges**: Configuration for skill range settings.
    - **public bool AllowFriendlyFire**: Indicates if friendly fire is allowed.
    - **public EconomySettings economy**: Configuration for economy parameters.
    - **public float GetRangeValue(SkillRange range)**: Returns the range value based on the specified skill range.
    - **public int GetMultiplier(EnemyRank rank)**: Returns the multiplier based on the specified enemy rank.
    - **public RankScaling GetScaling(EnemyRank rank)**: Returns the scaling associated with the specified enemy rank.

# Key Behavior & Side Effects
- The `GetRangeValue` method retrieves the range value for a specified skill range.
- The `GetMultiplier` method retrieves the loot multiplier based on enemy rank.
- The `GetScaling` method retrieves the rank scaling for enemies based on their rank.

# Constraints & Failure Modes
- The `GetRangeValue`, `GetMultiplier`, and `GetScaling` methods default to specific values if an unrecognized enum value is provided.
- Serialized fields are subject to Unity's serialization rules.

# Example
```csharp
GameBalanceConfig config = ScriptableObject.CreateInstance<GameBalanceConfig>();
float meleeRange = config.GetRangeValue(SkillRange.Melee);
int multiplier = config.loot.rankMultipliers.GetMultiplier(EnemyRank.Normal);
```

# Unknowns
- None.

