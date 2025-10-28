# Assets/src/Data/Config/GameBalanceConfig.cs

_Automatic generated/updated._

```markdown
# Purpose
- Defines a configuration asset for game balance settings, including loot, waves, enemies, skills, and economy.

# Public API
- Namespace: `CHAL.Data`
- Types
  - `public class GameBalanceConfig : ScriptableObject`
    - Public fields/properties:
      - `public LootSettings loot` - Configuration for loot settings.
      - `public EnemySettings enemies` - Configuration for enemy settings.
      - `public SkillRanges skillRanges` - Configuration for skill ranges.
      - `public bool AllowFriendlyFire` - Flag to allow friendly fire.
      - `public EconomySettings economy` - Configuration for economy settings.
    - Public methods:
      - `public float GetRangeValue(SkillRange range)` - Returns the range value for the specified skill range.

  - `public struct LootBudgetSettings`
    - Public fields:
      - `public float levelFactor`
      - `public float budgetVariance`
      - `public float beta`

  - `public struct LootFloorSettings`
    - Public fields:
      - `public float rare`
      - `public float epic`
      - `public float legendary`
      - `public float specials`

  - `public struct LootUnluckySettings`
    - Public fields:
      - `public float alphaRare`
      - `public float alphaEpic`
      - `public float alphaLegendary`
      - `public float alphaSpecials`

  - `public struct LootTrimSettings`
    - Public fields:
      - `public float common`
      - `public float uncommon`
      - `public float rare`
      - `public float epic`
      - `public float legendary`

  - `public struct LootRankMultipliers`
    - Public fields:
      - `public int spawn`
      - `public int normal`
      - `public int magic`
      - `public int elite`
      - `public int boss`
      - `public int champion`
    - Public methods:
      - `public int GetMultiplier(EnemyRank rank)`

  - `public struct LootSettings`
    - Public fields:
      - `public LootBudgetSettings budget`
      - `public LootFloorSettings floors`
      - `public LootUnluckySettings unlucky`
      - `public LootTrimSettings trim`
      - `public LootRankMultipliers rankMultipliers`

  - `public struct EnemyBudget`
    - Public fields:
      - `public int spawn`
      - `public int normal`
      - `public int magic`
      - `public int elite`
      - `public int boss`
      - `public int champion`

  - `public struct EnemyScaling`
    - Public fields:
      - `public float hpPerLevel`
      - `public float dmgPerLevel`

  - `public struct WaveSettings`
    - Public fields:
      - `public EnemyBudget budgetPoints`
      - `public EnemyScaling scaling`

  - `public struct RankScaling`
    - Public fields:
      - `public float hpMultiplier`
      - `public float dmgMultiplier`
      - `public float xpMultiplier`

  - `public struct EnemyRankSettings`
    - Public fields:
      - `public RankScaling spawn`
      - `public RankScaling normal`
      - `public RankScaling magic`
      - `public RankScaling elite`
      - `public RankScaling boss`
      - `public RankScaling champion`
    - Public methods:
      - `public RankScaling GetScaling(EnemyRank rank)`

  - `public struct EnemySettings`
    - Public fields:
      - `public EnemyBudget budgetPoints`
      - `public EnemyScaling scaling`
      - `public EnemyRankSettings rankScaling`
      - `public List<string> magicTagPool`
      - `public int minEliteTags`

  - `public struct SkillRanges`
    - Public fields:
      - `public float selfRange`
      - `public float meleeRange`
      - `public float reachRange`
      - `public float midDistanceRange`
      - `public float farDistanceRange`

  - `public struct CurrencySettings`
    - Public fields:
      - `public int baseGoldReward`
      - `public float goldPerLevel`

  - `public struct XpSettings`
    - Public fields:
      - `public int baseXpReward`
      - `public float xpPerLevel`
      - `public int baseLevelUpXp`
      - `public int levelCurveFactor`

  - `public struct EconomySettings`
    - Public fields:
      - `public CurrencySettings currencies`
      - `public XpSettings xp`

# Key Behavior & Side Effects
- Provides structured settings for various game balance aspects, allowing for easy configuration and adjustment of gameplay parameters.

# Constraints & Failure Modes
- None explicitly defined in the file.

# Example
```csharp
GameBalanceConfig config = ScriptableObject.CreateInstance<GameBalanceConfig>();
config.AllowFriendlyFire = true;
float meleeRange = config.GetRangeValue(SkillRange.Melee);
```

# Unknowns
- No information on the `EnemyRank` or `SkillRange` types, as they are not defined in this file.
```
