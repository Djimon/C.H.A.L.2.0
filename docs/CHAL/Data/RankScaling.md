# CHAL.Data.RankScaling

_Automatically generated/updated from `Assets/src/Data/Config/GameBalanceConfig.cs`._

# Purpose
- Defines the `GameBalanceConfig` ScriptableObject for configuring game balance settings.

# Public API
- Namespace: `CHAL.Data`
- Types
  - public class `GameBalanceConfig` [extends `ScriptableObject`]
    - Public fields/properties:
      - `LootSettings loot` - Configuration for loot settings.
      - `EnemySettings enemies` - Configuration for enemy settings.
      - `SkillRanges skillRanges` - Configuration for skill ranges.
      - `bool AllowFriendlyFire` - Flag to allow friendly fire.
      - `EconomySettings economy` - Configuration for economy settings.
    - Public methods:
      - `float GetRangeValue(SkillRange range)` - Returns the range value for the specified skill range.

  - public struct `LootBudgetSettings`
    - Public fields:
      - `float levelFactor`
      - `float budgetVariance`
      - `float beta`

  - public struct `LootFloorSettings`
    - Public fields:
      - `float rare`
      - `float epic`
      - `float legendary`
      - `float specials`

  - public struct `LootUnluckySettings`
    - Public fields:
      - `float alphaRare`
      - `float alphaEpic`
      - `float alphaLegendary`
      - `float alphaSpecials`

  - public struct `LootTrimSettings`
    - Public fields:
      - `float common`
      - `float uncommon`
      - `float rare`
      - `float epic`
      - `float legendary`

  - public struct `LootRankMultipliers`
    - Public fields:
      - `int spawn`
      - `int normal`
      - `int magic`
      - `int elite`
      - `int boss`
      - `int champion`
    - Public methods:
      - `int GetMultiplier(EnemyRank rank)`

  - public struct `LootSettings`
    - Public fields:
      - `LootBudgetSettings budget`
      - `LootFloorSettings floors`
      - `LootUnluckySettings unlucky`
      - `LootTrimSettings trim`
      - `LootRankMultipliers rankMultipliers`

  - public struct `EnemyBudget`
    - Public fields:
      - `int spawn`
      - `int normal`
      - `int magic`
      - `int elite`
      - `int boss`
      - `int champion`

  - public struct `EnemyScaling`
    - Public fields:
      - `float hpPerLevel`
      - `float dmgPerLevel`

  - public struct `WaveSettings`
    - Public fields:
      - `EnemyBudget budgetPoints`
      - `EnemyScaling scaling`

  - public struct `RankScaling`
    - Public fields:
      - `float hpMultiplier`
      - `float dmgMultiplier`
      - `float xpMultiplier`

  - public struct `EnemyRankSettings`
    - Public fields:
      - `RankScaling spawn`
      - `RankScaling normal`
      - `RankScaling magic`
      - `RankScaling elite`
      - `RankScaling boss`
      - `RankScaling champion`
    - Public methods:
      - `RankScaling GetScaling(EnemyRank rank)`

  - public struct `EnemySettings`
    - Public fields:
      - `EnemyBudget budgetPoints`
      - `EnemyScaling scaling`
      - `EnemyRankSettings rankScaling`
      - `List<string> magicTagPool`
      - `int minEliteTags`

  - public struct `CurrencySettings`
    - Public fields:
      - `int baseGoldReward`
      - `float goldPerLevel`

  - public struct `XpSettings`
    - Public fields:
      - `int baseXpReward`
      - `float xpPerLevel`
      - `int baseLevelUpXp`
      - `int levelCurveFactor`

  - public struct `EconomySettings`
    - Public fields:
      - `CurrencySettings currencies`
      - `XpSettings xp`

# Key Behavior & Side Effects
- Provides structured settings for loot, enemies, skills, and economy.
- Allows retrieval of specific range values based on `SkillRange`.

# Constraints & Failure Modes
- No explicit guards or null handling noted.
- Assumes valid `SkillRange` values are provided to `GetRangeValue`.

# Example
```csharp
GameBalanceConfig config = ScriptableObject.CreateInstance<GameBalanceConfig>();
float meleeRange = config.GetRangeValue(SkillRange.Melee);
```

# Unknowns
- No information on the `EnemyRank` and `SkillRange` types.

