# CHAL.Data.LootFloorSettings

_Automatically generated/updated from `Assets/src/Data/Config/GameBalanceConfig.cs`._

1) Purpose
- Define a Unity ScriptableObject GameBalanceConfig with nested serializable structs for game balance data (loot, waves, enemies, skills, economy).
- Expose public fields for inspector-driven configuration (LootSettings, EnemySettings, SkillRanges, EconomySettings, etc.).
- Provide small surface API methods to query configured values (range lookup, rank scaling, multiplier lookup).

2) Public API
- Namespace/Module
  - CHAL.Data

- public class GameBalanceConfig : ScriptableObject [CreateAssetMenu(fileName = "GameBalanceConfig", menuName = "Config/GameBalanceConfig")]
  - Public fields
    - [Header("Loot Settings")] public LootSettings loot
    - [Header("Enemy Settings")] public EnemySettings enemies
    - [Header("Skill Settings")] public SkillRanges skillRanges; public bool AllowFriendlyFire = false
    - [Header("Economy Settings")] public EconomySettings economy
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
- GetRangeValue(SkillRange range)
  - Returns the numeric value from skillRanges corresponding to the given range.
- LootRankMultipliers.GetMultiplier(EnemyRank rank)
  - Maps rank to the configured multiplier (spawn/normal/magic/elite/boss/champion); default 1 for unlisted ranks.
- EnemyRankSettings.GetScaling(EnemyRank rank)
  - Returns the RankScaling for the given rank (spawn/normal/magic/elite/boss/champion).
- Data-only configuration; no runtime state changes beyond value lookups.

4) Constraints & Failure Modes
- Range attributes constrain UI in the inspector for the corresponding fields.
- List<string> magicTagPool may be null if not initialized; code consuming it should guard nulls.
- Enum-based fallbacks in GetMultiplier/GetScaling use sensible defaults (1 or normal) when rank is unrecognized.
- No explicit threading/async semantics; all fields are simple data types intended for inspector-driven configuration.

5) Example
- Not derivable from file alone; no usage example included.

6) Unknowns
- Definition of EnemyRank enum (and its possible values) is not in this file.
- Definition of SkillRange enum (and its values) is not in this file.
- Exact runtime semantics of the configured values (balance formulas) are not specified here.
