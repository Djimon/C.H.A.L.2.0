# CHAL.Data.WaveSettings

_Automatically generated/updated from `Assets/src/Data/Config/GameBalanceConfig.cs`._

# Purpose
- Defines a configuration asset for game balance settings, including loot, waves, enemies, skills, and economy.

# Public API
- Namespace: `CHAL.Data`
- Types
  - `public class GameBalanceConfig : ScriptableObject`
    - Public fields/properties:
      - `LootSettings loot` - Configuration for loot settings.
      - `EnemySettings enemies` - Configuration for enemy settings.
      - `SkillRanges skillRanges` - Configuration for skill ranges.
      - `bool AllowFriendlyFire` - Indicates if friendly fire is allowed.
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
- The `GetRangeValue(SkillRange range)` method retrieves the range value based on the provided `SkillRange`.
- The `GetMultiplier(EnemyRank rank)` method retrieves the multiplier for the specified enemy rank.
- The `GetScaling(EnemyRank rank)` method retrieves the rank scaling for the specified enemy rank.

# Constraints & Failure Modes
- No explicit guards or null handling noted.
- The `GetMultiplier` and `GetScaling` methods return a default value (1 or normal scaling) if an unknown rank is provided.

# Example
```csharp
GameBalanceConfig config = ScriptableObject.CreateInstance<GameBalanceConfig>();
float meleeRange = config.GetRangeValue(SkillRange.Melee);
```

# Unknowns
- The definitions and values for `EnemyRank` and `SkillRange` are not provided in this file.

