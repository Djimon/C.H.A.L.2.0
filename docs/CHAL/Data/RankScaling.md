# CHAL.Data.RankScaling

_Automatically generated/updated from `Assets/src/Data/Config/GameBalanceConfig.cs`._

```text
1) Purpose
- Defines a Unity ScriptableObject GameBalanceConfig that centralizes game balance data (loot, waves/enemies, skills, economy).
- Exposes a hierarchy of serializable structs for configuring loot, enemy waves, enemy stats, skills, and economy.
- Provides small inline accessors (GetRangeValue, GetMultiplier, GetScaling) used by other systems to read configuration values.

2) Public API
- Namespace: CHAL.Data

- public class GameBalanceConfig : ScriptableObject
  - [CreateAssetMenu] metadata allows creating an asset named "GameBalanceConfig" under Config/GameBalanceConfig
  - Public fields:
    - public LootSettings loot
    - public EnemySettings enemies
    - public SkillRanges skillRanges
    - public bool AllowFriendlyFire = false
    - public EconomySettings economy
  - Public method:
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
    - returns corresponding multiplier for the given rank; if none matches, returns 1

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
    - returns corresponding RankScaling for the given rank; defaults to normal

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
- CreateAssetMenu attribute enables creation of a GameBalanceConfig asset in the Unity Editor; no runtime side effects by itself.
- GetRangeValue(SkillRange range) selects and returns a value from skillRanges based on the provided SkillRange; falls back to meleeRange for unknown values.
- LootRankMultipliers.GetMultiplier(EnemyRank rank) maps EnemyRank to the configured multiplier; defaults to 1 if rank is not matched.
- EnemyRankSettings.GetScaling(EnemyRank rank) selects the RankScaling for the given rank; defaults to normal if not matched.
- Public fields are intended as configuration data read by game systems; no mutation logic is defined here beyond field access.

4) Constraints & Failure Modes
- Range and switch fallbacks:
  - GetRangeValue: default to skillRanges.meleeRange if range is unrecognized.
  - GetMultiplier: default to 1 if rank is unrecognized.
  - GetScaling: default to normal if rank is unrecognized.
- Public List<string> magicTagPool may be null at runtime; no null-checks in this file.
- Serialized fields assume Unity serialization; complex types rely on Unity editor to populate.

5) Example
- Not provided in this file.

6) Unknowns
- Definitions of SkillRange and EnemyRank enums are not in this file.
- Behavior or calculation specifics for how these config values interact with gameplay systems are not defined here.
- Any external constraints or validations beyond [Range] attributes are not specified in this file.
```
