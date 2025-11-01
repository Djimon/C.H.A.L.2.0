# CHAL.Data.XpSettings

_Automatically generated/updated from `Assets/src/Data/Config/GameBalanceConfig.cs`._

Purpose
- Defines game balance as a ScriptableObject asset (CHAL.Data.GameBalanceConfig).
- Groups configuration for Loot, Waves, Enemies, Skills, and Economy.
- Provides small helper methods to map ranks to multipliers and to lookup ranges.

Public API

Namespace/Module
- CHAL.Data

Types

- public class GameBalanceConfig : ScriptableObject
  - Public fields
    - public LootSettings loot
    - public EnemySettings enemies
    - public SkillRanges skillRanges
    - public bool AllowFriendlyFire = false
    - public EconomySettings economy
  - Public methods
    - public float GetRangeValue(SkillRange range)
      - Returns the corresponding range from skillRanges based on the SkillRange value
      - Fallback: skillRanges.meleeRange
- public struct LootBudgetSettings
  - Public fields
    - public float levelFactor
    - public float budgetVariance
    - public float beta
- public struct LootFloorSettings
  - Public fields
    - [Range(0,1)] public float rare
    - [Range(0,1)] public float epic
    - [Range(0,1)] public float legendary
    - [Range(0,1)] public float specials
- public struct LootUnluckySettings
  - Public fields
    - public float alphaRare
    - public float alphaEpic
    - public float alphaLegendary
    - public float alphaSpecials
- public struct LootTrimSettings
  - Public fields
    - [Range(0,1)] public float common
    - [Range(0,1)] public float uncommon
    - [Range(0,1)] public float rare
    - [Range(0,1)] public float epic
    - [Range(0,1)] public float legendary
- public struct LootRankMultipliers
  - Public fields
    - public int spawn
    - public int normal
    - public int magic
    - public int elite
    - public int boss
    - public int champion
  - Public methods
    - public int GetMultiplier(EnemyRank rank)
      - Maps EnemyRank to corresponding multiplier field; default: 1
- public struct LootSettings
  - Public fields
    - public LootBudgetSettings budget
    - public LootFloorSettings floors
    - public LootUnluckySettings unlucky
    - public LootTrimSettings trim
    - public LootRankMultipliers rankMultipliers
- public struct EnemyBudget
  - Public fields
    - public int spawn
    - public int normal
    - public int magic
    - public int elite
    - public int boss
    - public int champion
- public struct EnemyScaling
  - Public fields
    - public float hpPerLevel
    - public float dmgPerLevel
- public struct WaveSettings
  - Public fields
    - public EnemyBudget budgetPoints
    - public EnemyScaling scaling
- public struct RankScaling
  - Public fields
    - public float hpMultiplier
    - public float dmgMultiplier
    - public float xpMultiplier
- public struct EnemyRankSettings
  - Public fields
    - public RankScaling spawn
    - public RankScaling normal
    - public RankScaling magic
    - public RankScaling elite
    - public RankScaling boss
    - public RankScaling champion
  - Public methods
    - public RankScaling GetScaling(EnemyRank rank)
      - Maps EnemyRank to the corresponding RankScaling; default: normal
- public struct EnemySettings
  - Public fields
    - public EnemyBudget budgetPoints
    - public EnemyScaling scaling
    - public EnemyRankSettings rankScaling
    - public List<string> magicTagPool
    - public int minEliteTags
- public struct SkillRanges
  - Public fields
    - public float selfRange
    - public float meleeRange
    - public float reachRange
    - public float midDistanceRange
    - public float farDistanceRange
- public struct CurrencySettings
  - Public fields
    - public int baseGoldReward
    - public float goldPerLevel
- public struct XpSettings
  - Public fields
    - public int baseXpReward
    - public float xpPerLevel
    - public int baseLevelUpXp
    - [Range(1,10)] public int levelCurveFactor
- public struct EconomySettings
  - Public fields
    - public CurrencySettings currencies
    - public XpSettings xp

Notes on attributes
- [CreateAssetMenu] on GameBalanceConfig enables asset creation via Unity menu.
- [Header(...)] groupings present for Loot Settings, Enemy Settings, and Economy Settings.
- [Range(0,1)] applied to several probability-like fields.
- [Range(1,10)] applied to levelCurveFactor.

Key Behavior & Side Effects
- GetMultiplier(EnemyRank rank) in LootRankMultipliers
  - Returns the corresponding multiplier based on rank:
    - Spawn -> spawn, Normal -> normal, Magic -> magic, Elite -> elite, Boss -> boss, Champion -> champion, else 1
- GetScaling(EnemyRank rank) in EnemyRankSettings
  - Returns the RankScaling for the given rank:
    - Spawn -> spawn, Normal -> normal, Magic -> magic, Elite -> elite, Boss -> boss, Champion -> champion, else normal
- GetRangeValue(SkillRange range) in GameBalanceConfig
  - Returns the value from skillRanges matching the provided SkillRange:
    - Self -> selfRange
    - Melee -> meleeRange
    - Reach -> reachRange
    - MidDistance -> midDistanceRange
    - FarDistance -> farDistanceRange
  - Default: meleeRange
- This file is a data container (ScriptableObject) with no external side effects beyond data accessors.

Constraints & Failure Modes
- Many fields are serialized with Unity attributes; nullability is not explicitly enforced (e.g., List<string> magicTagPool could be null if not initialized).
- Range attributes imply valid ranges for UI when editing in Unity; no runtime validation shown.
- Unknown external types referenced (EnemyRank, SkillRange) are not defined in this file; their definitions live elsewhere.

Unknowns
- Definitions and values of:
  - EnemyRank (enum) and SkillRange (enum)
- How these configurations are intended to be loaded/validated at runtime.
- Exact gameplay semantics tied to each numeric setting beyond what is visible here.
