# CHAL.Data.EnemyScaling

_Automatically generated/updated from `Assets/src/Data/Config/GameBalanceConfig.cs`._

# Purpose
- Defines game balance configuration settings for loot, waves, enemies, skills, and economy.

# Public API
- Namespace: `CHAL.Data`
- Types
  - **public class GameBalanceConfig** [extends ScriptableObject]
    - **public LootSettings loot**: Configuration for loot settings.
    - **public EnemySettings enemies**: Configuration for enemy settings.
    - **public SkillRanges skillRanges**: Configuration for skill ranges.
    - **public bool AllowFriendlyFire**: Indicates if friendly fire is allowed.
    - **public float GetRangeValue(SkillRange range)**: Returns the range value for the specified skill range.
    - **public struct LootBudgetSettings**: Settings for loot budget.
    - **public struct LootFloorSettings**: Settings for loot floors.
    - **public struct LootUnluckySettings**: Settings for unlucky loot.
    - **public struct LootTrimSettings**: Settings for trimming loot.
    - **public struct LootRankMultipliers**: Multipliers for loot ranks.
    - **public struct LootSettings**: Aggregates loot-related settings.
    - **public struct EnemyBudget**: Budget settings for enemies.
    - **public struct EnemyScaling**: Scaling settings for enemies.
    - **public struct WaveSettings**: Settings for waves.
    - **public struct RankScaling**: Scaling settings for enemy ranks.
    - **public struct EnemyRankSettings**: Settings for enemy ranks.
    - **public struct EnemySettings**: Aggregates enemy-related settings.
    - **public struct SkillRanges**: Ranges for skills.
    - **public struct CurrencySettings**: Settings for currency.
    - **public struct XpSettings**: Settings for experience points.
    - **public struct EconomySettings**: Aggregates economy-related settings.

# Key Behavior & Side Effects
- Provides structured settings for various game balance aspects.
- Contains methods to retrieve specific values based on enumerated types (e.g., `GetRangeValue`, `GetMultiplier`, `GetScaling`).

# Constraints & Failure Modes
- Uses `[Range(0, 1)]` attributes for certain float fields to enforce value constraints.
- Default values are not explicitly defined; reliance on Unity's inspector for initialization.

# Example
```csharp
GameBalanceConfig config = ScriptableObject.CreateInstance<GameBalanceConfig>();
float meleeRange = config.GetRangeValue(SkillRange.Melee);
```

# Unknowns
- Specific implementation details of `EnemyRank` and `SkillRange` enumerations are not provided in this file.

