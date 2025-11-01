# CHAL.Data.WaveSettings

_Automatically generated/updated from `Assets/src/Data/Config/GameBalanceConfig.cs`._

1) Purpose
- Defines a Unity ScriptableObject (GameBalanceConfig) in the CHAL.Data namespace for configuring game balance.
- Provides a set of serializable configuration structs for Loot, Waves, Enemies, Skills, and Economy.
- Offers helper methods to derive values from config (GetMultiplier on LootRankMultipliers; GetRangeValue on GameBalanceConfig).

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
      - Returns the corresponding multiplier for the given rank; falls back to 1 for unknown

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
      - Returns the RankScaling for the given rank; defaults to normal for unknown

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
- GetMultiplier(EnemyRank rank)
  - Maps rank to corresponding multiplier using a switch on EnemyRank
  - Unknown ranks default to 1 (no change)
- GetScaling(EnemyRank rank) (via EnemyRankSettings)
  - Returns the RankScaling for the specified rank using a switch
  - Unknown ranks default to normal scaling
- GetRangeValue(SkillRange range)
  - Returns the appropriate range value from skillRanges based on the SkillRange
  - Unknown ranges default to meleeRange
- Asset semantics
  - This is a ScriptableObject intended to be created via the Unity asset menu (CreateAssetMenu with fileName "GameBalanceConfig" and menuName "Config/GameBalanceConfig")

4) Constraints & Failure Modes
- Editor-only constraints
  - Several fields use [Range(0, 1)] to constrain values in the editor
  - [Range(1,10)] constrains levelCurveFactor in the editor
- No explicit runtime validation or error handling present
- Public fields imply that values should be set by editors or code; null/reference checks are not defined here
- Behavior depends on external types (EnemyRank, SkillRange) defined elsewhere; not provided in this file

5) Example
- Not included (not clearly derivable from this file alone)

6) Unknowns
- Definitions and values of EnemyRank and SkillRange (enums) are not defined in this file
- How these config values are consumed by gameplay systems is not defined here
- Any runtime defaults beyond the code-provided defaults are not specified
- Any additional side effects from Unity serialization or asset creation beyond CreateAssetMenu are not detailed here
