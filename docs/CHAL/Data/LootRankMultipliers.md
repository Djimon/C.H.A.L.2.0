# CHAL.Data.LootRankMultipliers

_Automatically generated/updated from `Assets/src/Data/Config/GameBalanceConfig.cs`._

# Purpose
- Defines game balance configuration settings for loot, waves, enemies, skills, and economy.

# Public API
- Namespace: `CHAL.Data`
- Types
  - **public class** `GameBalanceConfig` [extends `ScriptableObject`]
    - **public LootSettings** `loot` - Configuration for loot settings.
    - **public EnemySettings** `enemies` - Configuration for enemy settings.
    - **public SkillRanges** `skillRanges` - Configuration for skill ranges.
    - **public bool** `AllowFriendlyFire` - Indicates if friendly fire is allowed.
    - **public EconomySettings** `economy` - Configuration for economy settings.
    - **public float** `GetRangeValue(SkillRange range)` - Returns the range value for the specified skill range.

  - **public struct** `LootBudgetSettings`
    - **public float** `levelFactor`
    - **public float** `budgetVariance`
    - **public float** `beta`

  - **public struct** `LootFloorSettings`
    - **public float** `rare`
    - **public float** `epic`
    - **public float** `legendary`
    - **public float** `specials`

  - **public struct** `LootUnluckySettings`
    - **public float** `alphaRare`
    - **public float** `alphaEpic`
    - **public float** `alphaLegendary`
    - **public float** `alphaSpecials`

  - **public struct** `LootTrimSettings`
    - **public float** `common`
    - **public float** `uncommon`
    - **public float** `rare`
    - **public float** `epic`
    - **public float** `legendary`

  - **public struct** `LootRankMultipliers`
    - **public int** `spawn`
    - **public int** `normal`
    - **public int** `magic`
    - **public int** `elite`
    - **public int** `boss`
    - **public int** `champion`
    - **public int** `GetMultiplier(EnemyRank rank)`

  - **public struct** `LootSettings`
    - **public LootBudgetSettings** `budget`
    - **public LootFloorSettings** `floors`
    - **public LootUnluckySettings** `unlucky`
    - **public LootTrimSettings** `trim`
    - **public LootRankMultipliers** `rankMultipliers`

  - **public struct** `EnemyBudget`
    - **public int** `spawn`
    - **public int** `normal`
    - **public int** `magic`
    - **public int** `elite`
    - **public int** `boss`
    - **public int** `champion`

  - **public struct** `EnemyScaling`
    - **public float** `hpPerLevel`
    - **public float** `dmgPerLevel`

  - **public struct** `WaveSettings`
    - **public EnemyBudget** `budgetPoints`
    - **public EnemyScaling** `scaling`

  - **public struct** `RankScaling`
    - **public float** `hpMultiplier`
    - **public float** `dmgMultiplier`
    - **public float** `xpMultiplier`

  - **public struct** `EnemyRankSettings`
    - **public RankScaling** `spawn`
    - **public RankScaling** `normal`
    - **public RankScaling** `magic`
    - **public RankScaling** `elite`
    - **public RankScaling** `boss`
    - **public RankScaling** `champion`
    - **public RankScaling** `GetScaling(EnemyRank rank)`

  - **public struct** `EnemySettings`
    - **public EnemyBudget** `budgetPoints`
    - **public EnemyScaling** `scaling`
    - **public EnemyRankSettings** `rankScaling`
    - **public List<string>** `magicTagPool`
    - **public int** `minEliteTags`

  - **public struct** `SkillRanges`
    - **public float** `selfRange`
    - **public float** `meleeRange`
    - **public float** `reachRange`
    - **public float** `midDistanceRange`
    - **public float** `farDistanceRange`

  - **public struct** `CurrencySettings`
    - **public int** `baseGoldReward`
    - **public float** `goldPerLevel`

  - **public struct** `XpSettings`
    - **public int** `baseXpReward`
    - **public float** `xpPerLevel`
    - **public int** `baseLevelUpXp`
    - **public int** `levelCurveFactor`

  - **public struct** `EconomySettings`
    - **public CurrencySettings** `currencies`
    - **public XpSettings** `xp`

# Key Behavior & Side Effects
- Provides structured settings for various game balance aspects including loot, enemies, skills, and economy.
- The `GetMultiplier` method in `LootRankMultipliers` and `GetScaling` method in `EnemyRankSettings` return specific multipliers based on the provided rank.

# Constraints & Failure Modes
- No explicit guards or null handling noted.
- Assumes valid `EnemyRank` and `SkillRange` values are provided to methods.

# Example
```csharp
GameBalanceConfig config = ScriptableObject.CreateInstance<GameBalanceConfig>();
float meleeRange = config.GetRangeValue(SkillRange.Melee);
```

# Unknowns
- Specific behavior of `EnemyRank` and `SkillRange` types is not defined in this file.

