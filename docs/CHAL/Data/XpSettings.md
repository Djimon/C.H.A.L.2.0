# CHAL.Data.XpSettings

_Automatically generated/updated from `Assets/src/Data/Config/GameBalanceConfig.cs`._

# Purpose
- Defines game balance configuration settings for loot, waves, enemies, skills, and economy.

# Public API
- Namespace: `CHAL.Data`
- Types
  - `public class GameBalanceConfig : ScriptableObject`
    - Public fields/properties:
      - `LootSettings loot` - Configuration for loot settings.
      - `EnemySettings enemies` - Configuration for enemy settings.
      - `SkillRanges skillRanges` - Configuration for skill ranges.
      - `bool AllowFriendlyFire` - Flag to allow friendly fire.
      - `EconomySettings economy` - Configuration for economy settings.
    - Public methods:
      - `float GetRangeValue(SkillRange range)` - Returns the range value for the specified skill range.

  - `public struct LootBudgetSettings`
    - Public fields:
      - `float levelFactor`
      - `float budgetVariance`
      - `float beta`

  - `public struct LootFloorSettings`
    - Public fields:
      - `float rare`
      - `float epic`
      - `float legendary`
      - `float specials`

  - `public struct LootUnluckySettings`
    - Public fields:
      - `float alphaRare`
      - `float alphaEpic`
      - `float alphaLegendary`
      - `float alphaSpecials`

  - `public struct LootTrimSettings`
    - Public fields:
      - `float common`
      - `float uncommon`
      - `float rare`
      - `float epic`
      - `float legendary`

  - `public struct LootRankMultipliers`
    - Public fields:
      - `int spawn`
      - `int normal`
      - `int magic`
      - `int elite`
      - `int boss`
      - `int champion`
    - Public methods:
      - `int GetMultiplier(EnemyRank rank)` - Returns the multiplier for the specified enemy rank.

  - `public struct LootSettings`
    - Public fields:
      - `LootBudgetSettings budget`
      - `LootFloorSettings floors`
      - `LootUnluckySettings unlucky`
      - `LootTrimSettings trim`
      - `LootRankMultipliers rankMultipliers`

  - `public struct EnemyBudget`
    - Public fields:
      - `int spawn`
      - `int normal`
      - `int magic`
      - `int elite`
      - `int boss`
      - `int champion`

  - `public struct EnemyScaling`
    - Public fields:
      - `float hpPerLevel`
      - `float dmgPerLevel`

  - `public struct WaveSettings`
    - Public fields:
      - `EnemyBudget budgetPoints`
      - `EnemyScaling scaling`

  - `public struct RankScaling`
    - Public fields:
      - `float hpMultiplier`
      - `float dmgMultiplier`
      - `float xpMultiplier`

  - `public struct EnemyRankSettings`
    - Public fields:
      - `RankScaling spawn`
      - `RankScaling normal`
      - `RankScaling magic`
      - `RankScaling elite`
      - `RankScaling boss`
      - `RankScaling champion`
    - Public methods:
      - `RankScaling GetScaling(EnemyRank rank)` - Returns the scaling for the specified enemy rank.

  - `public struct EnemySettings`
    - Public fields:
      - `EnemyBudget budgetPoints`
      - `EnemyScaling scaling`
      - `EnemyRankSettings rankScaling`
      - `List<string> magicTagPool`
      - `int minEliteTags`

  - `public struct SkillRanges`
    - Public fields:
      - `float selfRange`
      - `float meleeRange`
      - `float reachRange`
      - `float midDistanceRange`
      - `float farDistanceRange`

  - `public struct CurrencySettings`
    - Public fields:
      - `int baseGoldReward`
      - `float goldPerLevel`

  - `public struct XpSettings`
    - Public fields:
      - `int baseXpReward`
      - `float xpPerLevel`
      - `int baseLevelUpXp`
      - `int levelCurveFactor`

  - `public struct EconomySettings`
    - Public fields:
      - `CurrencySettings currencies`
      - `XpSettings xp`

# Key Behavior & Side Effects
- Provides structured settings for various game balance aspects including loot, enemies, skills, and economy.
- Methods like `GetMultiplier` and `GetScaling` return specific configurations based on the provided rank.

# Constraints & Failure Modes
- No explicit guards or null handling noted.
- Assumes valid `EnemyRank` and `SkillRange` values are provided to methods.

# Example
```csharp
GameBalanceConfig config = ScriptableObject.CreateInstance<GameBalanceConfig>();
float meleeRange = config.GetRangeValue(SkillRange.Melee);
```

# Unknowns
- No information on how this configuration is utilized in the game logic.
- No details on the `EnemyRank` and `SkillRange` enumerations.

