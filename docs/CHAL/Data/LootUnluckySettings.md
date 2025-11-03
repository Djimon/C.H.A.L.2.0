# CHAL.Data.LootUnluckySettings

_Automatically generated/updated from `Assets/src/Data/Config/GameBalanceConfig.cs`._

1) Purpose
- Defines a Unity ScriptableObject: CHAL.Data.GameBalanceConfig
- Groups game balance configuration into Loot, Waves/Enemies, Skills, and Economy via serializable structs
- Exposes a CreateAssetMenu entry for editor creation

2) Public API
- Namespace: CHAL.Data

- public class GameBalanceConfig : ScriptableObject
  - public LootSettings loot
  - public EnemySettings enemies
  - public SkillRanges skillRanges
  - public bool AllowFriendlyFire = false
  - public EconomySettings economy
  - public float GetRangeValue(SkillRange range) : float

- Nested public types

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
    - public int GetMultiplier(EnemyRank rank) : int

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
    - public RankScaling GetScaling(EnemyRank rank) : RankScaling

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
- GetRangeValue(SkillRange range) selects and returns the corresponding value from skillRanges based on SkillRange
  - Self -> skillRanges.selfRange
  - Melee -> skillRanges.meleeRange
  - Reach -> skillRanges.reachRange
  - MidDistance -> skillRanges.midDistanceRange
  - FarDistance -> skillRanges.farDistanceRange
  - default -> skillRanges.meleeRange
- LootRankMultipliers.GetMultiplier(EnemyRank rank) returns the multiplier corresponding to the rank, with a default of 1
- EnemyRankSettings.GetScaling(EnemyRank rank) returns the RankScaling for the given rank, with a default of normal
- The class is marked with CreateAssetMenu, enabling creation of assets via Unity editor
- Editor grouping via [Header] attributes for Loot, Skill, Economy, and Enemy sections

4) Constraints & Failure Modes
- Range attributes constrain certain fields in the editor:
  - LootFloorSettings.rare/epic/legendary/specials: [Range(0,1)]
  - LootTrimSettings.common/uncommon/rare/epic/legendary: [Range(0,1)]
  - SkillRanges values have no explicit range constraints in code (editor may apply elsewhere)
  - XpSettings.levelCurveFactor: [Range(1,10)]
- List<string> magicTagPool in EnemySettings; no validation here
- No runtime validation beyond the editor attributes and switch defaults in methods
- Unknowns: behavior and expectations of EnemyRank, SkillRange, and how these assets are consumed elsewhere are not defined in this file

5) Example
- Minimal usage (assuming an asset reference exists or you create one at runtime):
```csharp
// Assuming 'config' is a reference to a GameBalanceConfig asset
float selfRange = config.GetRangeValue(SkillRange.Self);
```
- Or creating a transient instance (for testing):
```csharp
var config = ScriptableObject.CreateInstance<GameBalanceConfig>();
float val = config.GetRangeValue(SkillRange.Self);
```

6) Unknowns
- Definitions and values for EnemyRank, SkillRange, and how they map across the rest of the codebase are not included here
- How these settings are consumed by actual game systems (combat, loot drops, wave spawning) is not shown
- Any default initialization behavior for the nested structs is not specified beyond field defaults in code

