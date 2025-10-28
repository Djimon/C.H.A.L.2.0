# CHAL.Data.SkillRanges

_Automatically generated/updated from `Assets/src/Data/Config/GameBalanceConfig.cs`._

# Purpose
- Defines a configuration asset for game balance settings, including loot, waves, enemies, skills, and economy.

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
    - **public struct LootSettings**: Aggregates all loot-related settings.
    - **public struct EnemyBudget**: Budget settings for enemies.
    - **public struct EnemyScaling**: Scaling settings for enemies.
    - **public struct WaveSettings**: Settings for enemy waves.
    - **public struct RankScaling**: Scaling settings for enemy ranks.
    - **public struct EnemyRankSettings**: Settings for enemy ranks.
    - **public struct EnemySettings**: Aggregates all enemy-related settings.
    - **public struct SkillRanges**: Ranges for different skill types.
    - **public struct CurrencySettings**: Settings for currency rewards.
    - **public struct XpSettings**: Settings for experience points.
    - **public struct EconomySettings**: Aggregates all economy-related settings.

# Key Behavior & Side Effects
- Provides methods to retrieve specific configuration values based on enumerated types (e.g., `GetRangeValue`, `GetMultiplier`, `GetScaling`).
- Uses `switch` expressions to determine values based on input enums.

# Constraints & Failure Modes
- No explicit guards or null handling noted.
- Assumes valid `SkillRange` and `EnemyRank` inputs for methods.

# Example
```csharp
GameBalanceConfig config = ScriptableObject.CreateInstance<GameBalanceConfig>();
float meleeRange = config.GetRangeValue(SkillRange.Melee);
```

# Unknowns
- No information on how this configuration interacts with other game systems or components.

