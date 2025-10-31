# CHAL.Data.LootUnluckySettings

_Automatically generated/updated from `Assets/src/Data/Config/GameBalanceConfig.cs`._

# Purpose
- Defines game balance configuration settings for loot, waves, enemies, skills, and economy.

# Public API
- Namespace: `CHAL.Data`
- Types:
  - **GameBalanceConfig** [extends ScriptableObject]
    - Public fields/properties:
      - `LootSettings loot` - Configuration for loot settings.
      - `EnemySettings enemies` - Configuration for enemy settings.
      - `SkillRanges skillRanges` - Configuration for skill ranges.
      - `bool AllowFriendlyFire` - Flag to allow friendly fire.
      - `EconomySettings economy` - Configuration for economy settings.
    - Public methods:
      - `float GetRangeValue(SkillRange range)` - Returns the range value for the specified skill range.

  - **LootBudgetSettings** [struct]
    - Public fields:
      - `float levelFactor`
      - `float budgetVariance`
      - `float beta`

  - **LootFloorSettings** [struct]
    - Public fields:
      - `float rare`
      - `float epic`
      - `float legendary`
      - `float specials`

  - **LootUnluckySettings** [struct]
    - Public fields:
      - `float alphaRare`
      - `float alphaEpic`
      - `float alphaLegendary`
      - `float alphaSpecials`

  - **LootTrimSettings** [struct]
    - Public fields:
      - `float common`
      - `float uncommon`
      - `float rare`
      - `float epic`
      - `float legendary`

  - **LootRankMultipliers** [struct]
    - Public fields:
      - `int spawn`
      - `int normal`
      - `int magic`
      - `int elite`
      - `int boss`
      - `int champion`
    - Public methods:
      - `int GetMultiplier(EnemyRank rank)` - Returns the multiplier for the specified enemy rank.

  - **LootSettings** [struct]
    - Public fields:
      - `LootBudgetSettings budget`
      - `LootFloorSettings floors`
      - `LootUnluckySettings unlucky`
      - `LootTrimSettings trim`
      - `LootRankMultipliers rankMultipliers`

  - **EnemyBudget** [struct]
    - Public fields:
      - `int spawn`
      - `int normal`
      - `int magic`
      - `int elite`
      - `int boss`
      - `int champion`

  - **EnemyScaling** [struct]
    - Public fields:
      - `float hpPerLevel`
      - `float dmgPerLevel`

  - **WaveSettings** [struct]
    - Public fields:
      - `EnemyBudget budgetPoints`
      - `EnemyScaling scaling`

  - **RankScaling** [struct]
    - Public fields:
      - `float hpMultiplier`
      - `float dmgMultiplier`
      - `float xpMultiplier`

  - **EnemyRankSettings** [struct]
    - Public fields:
      - `RankScaling spawn`
      - `RankScaling normal`
      - `RankScaling magic`
      - `RankScaling elite`
      - `RankScaling boss`
      - `RankScaling champion`
    - Public methods:
      - `RankScaling GetScaling(EnemyRank rank)` - Returns the scaling for the specified enemy rank.

  - **EnemySettings** [struct]
    - Public fields:
      - `EnemyBudget budgetPoints`
      - `EnemyScaling scaling`
      - `EnemyRankSettings rankScaling`
      - `List<string> magicTagPool`
      - `int minEliteTags`

  - **SkillRanges** [struct]
    - Public fields:
      - `float selfRange`
      - `float meleeRange`
      - `float reachRange`
      - `float midDistanceRange`
      - `float farDistanceRange`

  - **CurrencySettings** [struct]
    - Public fields:
      - `int baseGoldReward`
      - `float goldPerLevel`

  - **XpSettings** [struct]
    - Public fields:
      - `int baseXpReward`
      - `float xpPerLevel`
      - `int baseLevelUpXp`
      - `int levelCurveFactor`

  - **EconomySettings** [struct]
    - Public fields:
      - `CurrencySettings currencies`
      - `XpSettings xp`

# Key Behavior & Side Effects
- Provides structured settings for game balance, including loot generation, enemy scaling, skill ranges, and economy management.
- The `GetMultiplier` and `GetScaling` methods return specific values based on the provided enemy rank.

# Constraints & Failure Modes
- No explicit guards or null handling noted.
- Assumes valid `EnemyRank` and `SkillRange` inputs for methods.

# Example
```csharp
GameBalanceConfig config = ScriptableObject.CreateInstance<GameBalanceConfig>();
float meleeRange = config.GetRangeValue(SkillRange.Melee);
```

# Unknowns
- No information on the `EnemyRank` and `SkillRange` types or their definitions.

