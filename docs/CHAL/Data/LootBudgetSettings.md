# CHAL.Data.LootBudgetSettings

_Automatically generated/updated from `Assets/src/Data/Config/GameBalanceConfig.cs`._

1) Purpose
- Defines a Unity ScriptableObject GameBalanceConfig (namespace CHAL.Data) for game balance configuration.
- Provides a rich, nested set of serializable structs to configure loot, enemies, skills, and economy.
- Exposes public configuration fields and small helper methods to query values at runtime.

2) Public API
- Namespace/module
  - CHAL.Data

- Types

  - public class GameBalanceConfig : ScriptableObject
    - Public fields
      - public LootSettings loot
      - public bool AllowFriendlyFire
      - public SkillRanges skillRanges
      - public EconomySettings economy
      - public EnemySettings enemies
    - Public methods
      - public float GetRangeValue(SkillRange range)
        - Returns the configured value for the given SkillRange; maps range to corresponding skillRanges field; default is SkillRange.Melee

  - LootBudgetSettings
    - public float levelFactor
    - public float budgetVariance
    - public float beta

  - LootFloorSettings
    - [Range(0, 1)] public float rare
    - [Range(0, 1)] public float epic
    - [Range(0, 1)] public float legendary
    - [Range(0, 1)] public float specials

  - LootUnluckySettings
    - public float alphaRare
    - public float alphaEpic
    - public float alphaLegendary
    - public float alphaSpecials

  - LootTrimSettings
    - [Range(0, 1)] public float common
    - [Range(0, 1)] public float uncommon
    - [Range(0, 1)] public float rare
    - [Range(0, 1)] public float epic
    - [Range(0, 1)] public float legendary

  - LootRankMultipliers
    - public int spawn
    - public int normal
    - public int magic
    - public int elite
    - public int boss
    - public int champion
    - public int GetMultiplier(EnemyRank rank)
      - Returns a multiplier based on rank; falls back to 1 for unknown ranks

  - LootSettings
    - public LootBudgetSettings budget
    - public LootFloorSettings floors
    - public LootUnluckySettings unlucky
    - public LootTrimSettings trim
    - public LootRankMultipliers rankMultipliers

  - EnemyBudget
    - public int spawn
    - public int normal
    - public int magic
    - public int elite
    - public int boss
    - public int champion

  - EnemyScaling
    - public float hpPerLevel
    - public float dmgPerLevel

  - WaveSettings
    - public EnemyBudget budgetPoints
    - public EnemyScaling scaling

  - RankScaling
    - public float hpMultiplier
    - public float dmgMultiplier
    - public float xpMultiplier

  - EnemyRankSettings
    - public RankScaling spawn
    - public RankScaling normal
    - public RankScaling magic
    - public RankScaling elite
    - public RankScaling boss
    - public RankScaling champion
    - public RankScaling GetScaling(EnemyRank rank)
      - Returns the RankScaling for the given rank; defaults to normal if unknown

  - EnemySettings
    - public EnemyBudget budgetPoints
    - public EnemyScaling scaling
    - public EnemyRankSettings rankScaling
    - public List<string> magicTagPool
    - public int minEliteTags

  - SkillRanges
    - public float selfRange
    - public float meleeRange
    - public float reachRange
    - public float midDistanceRange
    - public float farDistanceRange

  - CurrencySettings
    - public int baseGoldReward
    - public float goldPerLevel

  - XpSettings
    - public int baseXpReward
    - public float xpPerLevel
    - public int baseLevelUpXp
    - [Range(1,10)] public int levelCurveFactor

  - EconomySettings
    - public CurrencySettings currencies
    - public XpSettings xp

3) Key Behavior & Side Effects
- GetRangeValue maps SkillRange to a corresponding value in skillRanges; no side effects, read-only lookup.
- LootRankMultipliers.GetMultiplier returns an int multiplier for a given EnemyRank; fallback to 1 for unknown ranks.
- EnemyRankSettings.GetScaling returns the RankScaling for a given EnemyRank; fallback to normal for unknown ranks.
- The asset is data-driven configuration; changing fields at edit time affects how loot, enemies, skills, and economy are calculated in the game.

4) Constraints & Failure Modes
- Editor-only constraints:
  - [Range(0,1)] attributes constrain certain fields to [0,1] in the inspector.
  - List<string> magicTagPool may be null if not assigned; runtime code should handle potential nulls.
- Dependencies:
  - Uses EnemyRank and SkillRange enums/types defined elsewhere in the project.
- No runtime guarantees provided in this file beyond value lookup; actual usage is external.

5) Example
- Minimal usage example (assuming a valid GameBalanceConfig instance named config and SkillRange enum defined elsewhere):

```csharp
float meleeRange = config.GetRangeValue(SkillRange.Melee);
```

6) Unknowns
- Definitions and exact values of EnemyRank and SkillRange enums are not in this file.
- How the stored values are interpreted by gameplay systems (loot generation, wave composition, enemy scaling, etc.) is not shown here.
- Any runtime validation or defaults beyond the declared fields are not present in this file.

