# CHAL.Data.LootSettings

_Automatically generated/updated from `Assets/src/Data/Config/GameBalanceConfig.cs`._

1) Purpose
- Define CHAL.Data.GameBalanceConfig as a ScriptableObject that holds game balance parameters for loot, waves/enemies, skills, and economy.
- Declare multiple public, serializable structs to organize related settings (loot, enemy waves, enemy scaling, skills, economy, etc.) for editor-driven configuration.
- Provide a small set of read-only/type-lookup helpers (GetRangeValue, GetMultiplier, GetScaling) that map between enum ranks/ranges and their corresponding configured values.

2) Public API
- Namespace/module: CHAL.Data
- Types

public class GameBalanceConfig : ScriptableObject
- Public fields
  - LootSettings loot
  - EnemySettings enemies
  - SkillRanges skillRanges
  - bool AllowFriendlyFire
  - EconomySettings economy
- Public methods
  - float GetRangeValue(SkillRange range)

[System.Serializable] public struct LootBudgetSettings
- public float levelFactor
- public float budgetVariance
- public float beta

[System.Serializable] public struct LootFloorSettings
- [Range(0, 1)] public float rare
- [Range(0, 1)] public float epic
- [Range(0, 1)] public float legendary
- [Range(0, 1)] public float specials

[System.Serializable] public struct LootUnluckySettings
- public float alphaRare
- public float alphaEpic
- public float alphaLegendary
- public float alphaSpecials

[System.Serializable] public struct LootTrimSettings
- [Range(0, 1)] public float common
- [Range(0, 1)] public float uncommon
- [Range(0, 1)] public float rare
- [Range(0, 1)] public float epic
- [Range(0, 1)] public float legendary

[System.Serializable] public struct LootRankMultipliers
- public int spawn
- public int normal
- public int magic
- public int elite
- public int boss
- public int champion
- public int GetMultiplier(EnemyRank rank)
  - Returns corresponding multiplier for rank via switch:
    - EnemyRank.Spawn => spawn
    - EnemyRank.Normal => normal
    - EnemyRank.Magic => magic
    - EnemyRank.Elite => elite
    - EnemyRank.Boss => boss
    - EnemyRank.Champion => champion
    - _ => 1

[System.Serializable] public struct LootSettings
- public LootBudgetSettings budget
- public LootFloorSettings floors
- public LootUnluckySettings unlucky
- public LootTrimSettings trim
- public LootRankMultipliers rankMultipliers

[System.Serializable] public struct EnemyBudget
- public int spawn
- public int normal
- public int magic
- public int elite
- public int boss
- public int champion

[System.Serializable] public struct EnemyScaling
- public float hpPerLevel
- public float dmgPerLevel

[System.Serializable] public struct WaveSettings
- public EnemyBudget budgetPoints
- public EnemyScaling scaling

[System.Serializable] public struct RankScaling
- public float hpMultiplier
- public float dmgMultiplier
- public float xpMultiplier

[System.Serializable] public struct EnemyRankSettings
- public RankScaling spawn
- public RankScaling normal
- public RankScaling magic
- public RankScaling elite
- public RankScaling boss
- public RankScaling champion
- public RankScaling GetScaling(EnemyRank rank)
  - Returns corresponding RankScaling for rank via switch:
    - EnemyRank.Spawn => spawn
    - EnemyRank.Normal => normal
    - EnemyRank.Magic => magic
    - EnemyRank.Elite => elite
    - EnemyRank.Boss => boss
    - EnemyRank.Champion => champion
    - _ => normal

[System.Serializable] public struct EnemySettings
- public EnemyBudget budgetPoints
- public EnemyScaling scaling
- public EnemyRankSettings rankScaling
- public List<string> magicTagPool
- public int minEliteTags

[System.Serializable] public struct SkillRanges
- public float selfRange
- public float meleeRange
- public float reachRange
- public float midDistanceRange
- public float farDistanceRange

[System.Serializable] public struct CurrencySettings
- public int baseGoldReward
- public float goldPerLevel

[System.Serializable] public struct XpSettings
- public int baseXpReward
- public float xpPerLevel
- public int baseLevelUpXp
- [Range(1,10)] public int levelCurveFactor

[System.Serializable] public struct EconomySettings
- public CurrencySettings currencies
- public XpSettings xp

3) Key Behavior & Side Effects
- Asset creation: [CreateAssetMenu] enables creating a GameBalanceConfig asset via Unity Editor.
- GetRangeValue(SkillRange range)
  - Returns the configured range value corresponding to the provided SkillRange.
  - Defaults to skillRanges.meleeRange if the range is unrecognized.
- LootRankMultipliers.GetMultiplier(EnemyRank rank)
  - Returns the configured multiplier for the given rank.
  - Falls back to 1 if rank is not matched by the defined cases.
- EnemyRankSettings.GetScaling(EnemyRank rank)
  - Returns the RankScaling corresponding to the given rank.
  - Falls back to normal if rank is not matched.
- Getters are pure lookups; no side effects or mutations occur.

4) Constraints & Failure Modes
- Range constraints: certain fields annotated with [Range(0, 1)] constrain values in the editor to [0,1].
- External enums: EnemyRank, SkillRange referenced but not defined in this file; rely on external definitions.
- Potential nulls: public List<string> magicTagPool is not initialized here; may need editor population to avoid null references at runtime.
- No explicit constructors; Unity serialization handles initialization for scriptable assets.
- Default fallbacks in switch expressions:
  - GetMultiplier: default 1
  - GetScaling: default normal
  - GetRangeValue: default meleeRange

5) Example
- Not included (no derivable minimal usage example beyond standard Unity ScriptableObject usage in editor).

6) Unknowns
- Definitions and members of EnemyRank, SkillRange, and EnemyRank enum usage beyond this file.
- The exact runtime semantics of these settings in gameplay (how each field is consumed by other systems).
- Default values assigned in the editor for each field are not deduced from code alone.
- Any serialization behavior or asset management beyond CreateAssetMenu is not specified here.
