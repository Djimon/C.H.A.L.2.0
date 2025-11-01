# CHAL.Data.LootRankMultipliers

_Automatically generated/updated from `Assets/src/Data/Config/GameBalanceConfig.cs`._

```csharp
// Documentation for: Assets/src/Data/Config/GameBalanceConfig.cs
```

1) Purpose
- Defines a Unity ScriptableObject GameBalanceConfig that holds game balance data (loot, enemies, skills, economy).
- Provides a CreateAssetMenu entry for Editor creation of a GameBalanceConfig asset.
- Encapsulates multiple serializable, nested settings structs to configure game systems.

2) Public API
- Namespace/module
  - CHAL.Data

- Types
  - public class GameBalanceConfig : ScriptableObject
    - Public fields
      - LootSettings loot
      - EnemySettings enemies
      - SkillRanges skillRanges
      - bool AllowFriendlyFire = false
      - EconomySettings economy
    - Public methods
      - float GetRangeValue(SkillRange range)
        - Returns the configured range value for the given SkillRange

  - Nested public structs (all marked [System.Serializable])
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
        - switch on rank:
          - EnemyRank.Spawn => spawn
          - EnemyRank.Normal => normal
          - EnemyRank.Magic => magic
          - EnemyRank.Elite => elite
          - EnemyRank.Boss => boss
          - EnemyRank.Champion => champion
          - default => 1
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
        - switch on rank:
          - EnemyRank.Spawn => spawn
          - EnemyRank.Normal => normal
          - EnemyRank.Magic => magic
          - EnemyRank.Elite => elite
          - EnemyRank.Boss => boss
          - EnemyRank.Champion => champion
          - default => normal
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
      - [Range(1, 10)] public int levelCurveFactor
    - EconomySettings
      - public CurrencySettings currencies
      - public XpSettings xp

3) Key Behavior & Side Effects
- GetRangeValue(SkillRange range)
  - Returns the corresponding value from skillRanges:
    - SkillRange.Self -> skillRanges.selfRange
    - SkillRange.Melee -> skillRanges.meleeRange
    - SkillRange.Reach -> skillRanges.reachRange
    - SkillRange.MidDistance -> skillRanges.midDistanceRange
    - SkillRange.FarDistance -> skillRanges.farDistanceRange
    - default -> skillRanges.meleeRange
- LootRankMultipliers.GetMultiplier(EnemyRank rank)
  - Maps EnemyRank to the configured multiplier; returns 1 for unknown rank
- EnemyRankSettings.GetScaling(EnemyRank rank)
  - Returns the RankScaling instance corresponding to the given rank
  - Default fallback is normal if rank is not matched
- The asset is a data container; no runtime side effects beyond value lookups. The methods are pure accessors.

4) Constraints & Failure Modes
- Inspector-imposed ranges:
  - LootTrimSettings and related fields use [Range(0, 1)] to constrain 0–1 values
  - XpSettings.levelCurveFactor uses [Range(1, 10)]
- Dependency on external enums:
  - EnemyRank and SkillRange are referenced but definitions are not in this file
  - GetMultiplier and GetScaling rely on those enums
- Nullability note:
  - magicTagPool is a List<string>; nullability depends on Unity serialization and asset state
- Runtime behavior:
  - No explicit initialization or validation code; behavior inferred from usage in other systems

5) Example
- Minimal usage example (assuming a loaded asset):
```csharp
// Assuming `config` is a valid GameBalanceConfig instance
float selfRange = config.GetRangeValue(SkillRange.Self);
```

6) Unknowns
- Definitions of EnemyRank and SkillRange enums (not included in this file)
- How these settings are consumed at runtime (calculation algorithms are not implemented here)
- Any defaults or validation applied at runtime when loading assets (not shown)

Notes
- This file is Unity Editor-friendly via [CreateAssetMenu] and [System.Serializable] nested types.
- Public surface includes the GameBalanceConfig class, its public fields, and all nested types and their public members.
