# CHAL.Data.EconomySettings

_Automatically generated/updated from `Assets/src/Data/Config/GameBalanceConfig.cs`._

1) Purpose
- Define a Unity ScriptableObject GameBalanceConfig that groups game balance settings.
- Expose many serializable structs for configuring loot, waves, enemies, skills, and economy.
- Provide small helper methods to compute range values, enemy multipliers, and rank scaling at runtime.

2) Public API
- Namespace/Module
  - CHAL.Data

- Types

  public class GameBalanceConfig : ScriptableObject
  - Public fields
    - LootSettings loot
    - EnemySettings enemies
    - SkillRanges skillRanges
    - bool AllowFriendlyFire
    - EconomySettings economy
  - Public methods
    - public float GetRangeValue(SkillRange range)
      - Returns the configured range value for the given SkillRange.

  Nested public structs

  public struct LootBudgetSettings
  - Public fields
    - public float levelFactor
    - public float budgetVariance
    - public float beta

  public struct LootFloorSettings
  - Public fields
    - [Range(0, 1)] public float rare
    - [Range(0, 1)] public float epic
    - [Range(0, 1)] public float legendary
    - [Range(0, 1)] public float specials

  public struct LootUnluckySettings
  - Public fields
    - public float alphaRare
    - public float alphaEpic
    - public float alphaLegendary
    - public float alphaSpecials

  public struct LootTrimSettings
  - Public fields
    - [Range(0, 1)] public float common
    - [Range(0, 1)] public float uncommon
    - [Range(0, 1)] public float rare
    - [Range(0, 1)] public float epic
    - [Range(0, 1)] public float legendary

  public struct LootRankMultipliers
  - Public fields
    - public int spawn
    - public int normal
    - public int magic
    - public int elite
    - public int boss
    - public int champion
  - Public methods
    - public int GetMultiplier(EnemyRank rank)
      - Returns multiplier for given EnemyRank; maps rank to corresponding field or 1 by default.

  public struct LootSettings
  - Public fields
    - public LootBudgetSettings budget
    - public LootFloorSettings floors
    - public LootUnluckySettings unlucky
    - public LootTrimSettings trim
    - public LootRankMultipliers rankMultipliers

  public struct EnemyBudget
  - Public fields
    - public int spawn
    - public int normal
    - public int magic
    - public int elite
    - public int boss
    - public int champion

  public struct EnemyScaling
  - Public fields
    - public float hpPerLevel
    - public float dmgPerLevel

  public struct WaveSettings
  - Public fields
    - public EnemyBudget budgetPoints
    - public EnemyScaling scaling

  public struct RankScaling
  - Public fields
    - public float hpMultiplier
    - public float dmgMultiplier
    - public float xpMultiplier

  public struct EnemyRankSettings
  - Public fields
    - public RankScaling spawn
    - public RankScaling normal
    - public RankScaling magic
    - public RankScaling elite
    - public RankScaling boss
    - public RankScaling champion
  - Public methods
    - public RankScaling GetScaling(EnemyRank rank)
      - Returns RankScaling for the given EnemyRank; defaults to normal.

  public struct EnemySettings
  - Public fields
    - public EnemyBudget budgetPoints
    - public EnemyScaling scaling
    - public EnemyRankSettings rankScaling
    - public List<string> magicTagPool
    - public int minEliteTags

  public struct SkillRanges
  - Public fields
    - public float selfRange
    - public float meleeRange
    - public float reachRange
    - public float midDistanceRange
    - public float farDistanceRange

  public struct CurrencySettings
  - Public fields
    - public int baseGoldReward
    - public float goldPerLevel

  public struct XpSettings
  - Public fields
    - public int baseXpReward
    - public float xpPerLevel
    - public int baseLevelUpXp
    - [Range(1,10)] public int levelCurveFactor

  public struct EconomySettings
  - Public fields
    - public CurrencySettings currencies
    - public XpSettings xp

3) Key Behavior & Side Effects
- GetRangeValue(SkillRange) selects one of skillRanges values based on SkillRange:
  - Self -> selfRange
  - Melee -> meleeRange
  - Reach -> reachRange
  - MidDistance -> midDistanceRange
  - FarDistance -> farDistanceRange
  - Default -> meleeRange
- GetMultiplier(EnemyRank) returns the corresponding multiplier from LootRankMultipliers:
  - Switch on rank: Spawn, Normal, Magic, Elite, Boss, Champion
  - Default -> 1
- GetScaling(EnemyRank) returns the RankScaling for the corresponding rank:
  - Switch on rank: Spawn, Normal, Magic, Elite, Boss, Champion
  - Default -> normal
- GetRangeValue relies on the serialized SkillRanges data; no runtime side effects beyond reading fields.

4) Constraints & Failure Modes
- Range attributes enforce [0,1] for several float fields (LootFloorSettings, LootTrimSettings) and [Range(1,10)] for levelCurveFactor.
- List<string> magicTagPool can be null if not assigned in editor; no internal null-checks shown.
- No explicit guards in the helper methods; behavior relies on provided enum values (EnemyRank, SkillRange) defined elsewhere.
- This is a ScriptableObject; asset creation is via CreateAssetMenu attribute; runtime instantiation details are not defined here.

5) Example
- Example usage (assumes 'config' is a loaded GameBalanceConfig instance):
```csharp
// Example usage
float selfRange = config.GetRangeValue(SkillRange.Self);
```

6) Unknowns
- Definitions of EnemyRank and SkillRange enums (and their exact members) are not in this file.
- How GameBalanceConfig assets are loaded/assigned at runtime or via Unity editor workflow is not shown.
- Default serialized values for all fields depend on editor data; no explicit defaults beyond AllowFriendlyFire = false.
- Any external behavior or consumers of these settings outside this file are not specified.

