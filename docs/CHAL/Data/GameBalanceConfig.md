# CHAL.Data.GameBalanceConfig

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
      - **public float levelFactor**
      - **public float budgetVariance**
      - **public float beta**
    - **public struct LootFloorSettings**: Settings for loot floors.
      - **public float rare**
      - **public float epic**
      - **public float legendary**
      - **public float specials**
    - **public struct LootUnluckySettings**: Settings for unlucky loot.
      - **public float alphaRare**
      - **public float alphaEpic**
      - **public float alphaLegendary**
      - **public float alphaSpecials**
    - **public struct LootTrimSettings**: Settings for trimming loot.
      - **public float common**
      - **public float uncommon**
      - **public float rare**
      - **public float epic**
      - **public float legendary**
    - **public struct LootRankMultipliers**: Multipliers for loot ranks.
      - **public int spawn**
      - **public int normal**
      - **public int magic**
      - **public int elite**
      - **public int boss**
      - **public int champion**
      - **public int GetMultiplier(EnemyRank rank)**: Returns the multiplier for the specified enemy rank.
    - **public struct LootSettings**: Aggregates loot-related settings.
      - **public LootBudgetSettings budget**
      - **public LootFloorSettings floors**
      - **public LootUnluckySettings unlucky**
      - **public LootTrimSettings trim**
      - **public LootRankMultipliers rankMultipliers**
    - **public struct EnemyBudget**: Budget settings for enemies.
      - **public int spawn**
      - **public int normal**
      - **public int magic**
      - **public int elite**
      - **public int boss**
      - **public int champion**
    - **public struct EnemyScaling**: Scaling settings for enemies.
      - **public float hpPerLevel**
      - **public float dmgPerLevel**
    - **public struct WaveSettings**: Settings for enemy waves.
      - **public EnemyBudget budgetPoints**
      - **public EnemyScaling scaling**
    - **public struct RankScaling**: Scaling settings for enemy ranks.
      - **public float hpMultiplier**
      - **public float dmgMultiplier**
      - **public float xpMultiplier**
    - **public struct EnemyRankSettings**: Settings for enemy ranks.
      - **public RankScaling spawn**
      - **public RankScaling normal**
      - **public RankScaling magic**
      - **public RankScaling elite**
      - **public RankScaling boss**
      - **public RankScaling champion**
      - **public RankScaling GetScaling(EnemyRank rank)**: Returns scaling for the specified enemy rank.
    - **public struct EnemySettings**: Aggregates enemy-related settings.
      - **public EnemyBudget budgetPoints**
      - **public EnemyScaling scaling**
      - **public EnemyRankSettings rankScaling**
      - **public List<string> magicTagPool**
      - **public int minEliteTags**
    - **public struct SkillRanges**: Settings for skill ranges.
      - **public float selfRange**
      - **public float meleeRange**
      - **public float reachRange**
      - **public float midDistanceRange**
      - **public float farDistanceRange**
    - **public struct CurrencySettings**: Settings for currency.
      - **public int baseGoldReward**
      - **public float goldPerLevel**
    - **public struct XpSettings**: Settings for experience points.
      - **public int baseXpReward**
      - **public float xpPerLevel**
      - **public int baseLevelUpXp**
      - **public int levelCurveFactor**
    - **public struct EconomySettings**: Aggregates economy-related settings.
      - **public CurrencySettings currencies**
      - **public XpSettings xp**

# Key Behavior & Side Effects
- Provides structured settings for various game balance aspects, including loot, enemies, skills, and economy.
- The `GetMultiplier` and `GetScaling` methods return specific multipliers and scaling based on the provided enemy rank.

# Constraints & Failure Modes
- No explicit guards or null handling noted.
- Assumes valid `EnemyRank` and `SkillRange` values are provided to methods.

# Example
```csharp
GameBalanceConfig config = ScriptableObject.CreateInstance<GameBalanceConfig>();
float meleeRange = config.GetRangeValue(SkillRange.Melee);
```

# Unknowns
- Specific behavior of `EnemyRank` and `SkillRange` types cannot be determined from this file.

