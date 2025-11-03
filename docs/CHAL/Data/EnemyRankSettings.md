# CHAL.Data.EnemyRankSettings

_Automatically generated/updated from `Assets/src/Data/Config/GameBalanceConfig.cs`._

1) Purpose
- Defines GameBalanceConfig as a Unity ScriptableObject for game balance data.
- Groups balance data into nested, serializable structs for Loot, Waves, Enemies, Skills, and Economy.
- Exposes a CreateAssetMenu asset type for easy creation in the editor.

2) Public API
- Namespace/module
  - CHAL.Data
- Types
  - public class GameBalanceConfig : ScriptableObject
    - Public fields
      - public LootSettings loot
      - public EnemySettings enemies
      - public SkillRanges skillRanges
      - public bool AllowFriendlyFire = false
      - public EconomySettings economy
    - Public methods
      - public float GetRangeValue(SkillRange range)
    - Nested public types (with public members)
      - public struct LootBudgetSettings
        - public float levelFactor
        - public float budgetVariance
        - public float beta
      - public struct LootFloorSettings
        - [Range(0, 1)] public float rare
        - [Range(0, 1)] public float epic
        - [Range(0, 1)] public float legendary
        - [Range(0, 1)] public float specials
      - public struct LootUnluckySettings
        - public float alphaRare
        - public float alphaEpic
        - public float alphaLegendary
        - public float alphaSpecials
      - public struct LootTrimSettings
        - [Range(0, 1)] public float common
        - [Range(0, 1)] public float uncommon
        - [Range(0, 1)] public float rare
        - [Range(0, 1)] public float epic
        - [Range(0, 1)] public float legendary
      - public struct LootRankMultipliers
        - public int spawn
        - public int normal
        - public int magic
        - public int elite
        - public int boss
        - public int champion
        - public int GetMultiplier(EnemyRank rank)
      - public struct LootSettings
        - public LootBudgetSettings budget
        - public LootFloorSettings floors
        - public LootUnluckySettings unlucky
        - public LootTrimSettings trim
        - public LootRankMultipliers rankMultipliers
      - public struct EnemyBudget
        - public int spawn
        - public int normal
        - public int magic
        - public int elite
        - public int boss
        - public int champion
      - public struct EnemyScaling
        - public float hpPerLevel
        - public float dmgPerLevel
      - public struct WaveSettings
        - public EnemyBudget budgetPoints
        - public EnemyScaling scaling
      - public struct RankScaling
        - public float hpMultiplier
        - public float dmgMultiplier
        - public float xpMultiplier
      - public struct EnemyRankSettings
        - public RankScaling spawn
        - public RankScaling normal
        - public RankScaling magic
        - public RankScaling elite
        - public RankScaling boss
        - public RankScaling champion
        - public RankScaling GetScaling(EnemyRank rank)
      - public struct EnemySettings
        - public EnemyBudget budgetPoints
        - public EnemyScaling scaling
        - public EnemyRankSettings rankScaling
        - public List<string> magicTagPool
        - public int minEliteTags
      - public struct SkillRanges
        - public float selfRange
        - public float meleeRange
        - public float reachRange
        - public float midDistanceRange
        - public float farDistanceRange
      - public struct CurrencySettings
        - public int baseGoldReward
        - public float goldPerLevel
      - public struct XpSettings
        - public int baseXpReward
        - public float xpPerLevel
        - public int baseLevelUpXp
        - [Range(1,10)] public int levelCurveFactor
      - public struct EconomySettings
        - public CurrencySettings currencies
        - public XpSettings xp

3) Key Behavior & Side Effects
- GetRangeValue maps a SkillRange to the corresponding value in skillRanges:
  - Self -> skillRanges.selfRange
  - Melee -> skillRanges.meleeRange
  - Reach -> skillRanges.reachRange
  - MidDistance -> skillRanges.midDistanceRange
  - FarDistance -> skillRanges.farDistanceRange
  - default -> skillRanges.meleeRange
- LootRankMultipliers.GetMultiplier returns an int multiplier based on EnemyRank, with a default of 1.
- EnemyRankSettings.GetScaling returns the RankScaling for the given EnemyRank, with a default of normal.
- The class is a data container; no runtime state changes beyond field access/modification.

4) Constraints & Failure Modes
- Inspector ranges:
  - LootFloorSettings.rare, epic, legendary, specials are annotated with [Range(0,1)].
  - SkillRanges fields selfRange, meleeRange, reachRange, midDistanceRange, farDistanceRange are not individually constrained here (inspector may reflect exact types).
  - XpSettings.levelCurveFactor is constrained to [1,10] via [Range(1,10)].
- Nullability:
  - magicTagPool is a public List<string>; nullability is not enforced in code.
- Default fallbacks in switches:
  - LootRankMultipliers.GetMultiplier: _ => 1
  - EnemyRankSettings.GetScaling: _ => normal
  - GameBalanceConfig.GetRangeValue: default -> skillRanges.meleeRange
- No explicit threading/async behavior; this is a data asset only.
- No validation beyond inspector attributes; runtime validation is not present in this file.

5) Example
- Not derivable from this file alone; omitted.

6) Unknowns
- Definitions of EnemyRank and SkillRange (used in signatures) are not declared in this file.
- How these balance settings are consumed (e.g., how waves spawn or how loot is actually computed) is external to this file.
- Any runtime validation or initialization logic outside this file (e.g., defaults if asset is missing) is not shown here.

