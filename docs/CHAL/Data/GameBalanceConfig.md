# CHAL.Data.GameBalanceConfig

_Automatically generated/updated from `Assets/src/Data/Config/GameBalanceConfig.cs`._

```text
1) Purpose
- Defines a Unity ScriptableObject asset (GameBalanceConfig) containing game balance configuration.
- Organizes configuration into sections: Loot, Waves, Enemies, Skills, Economy.
- Exposes a CreateAssetMenu entry for easy creation in the Unity Editor.

2) Public API
- Namespace/Module: CHAL.Data
- Types
  - public class GameBalanceConfig : ScriptableObject
    - Public fields
      - LootSettings loot
      - SkillRanges skillRanges
      - bool AllowFriendlyFire
      - EconomySettings economy
    - Public methods
      - float GetRangeValue(SkillRange range)
        - Returns the configured range value for the given SkillRange
  - public struct LootBudgetSettings
    - Public fields
      - float levelFactor
      - float budgetVariance
      - float beta
  - public struct LootFloorSettings
    - Public fields
      - [Range(0,1)] float rare
      - [Range(0,1)] float epic
      - [Range(0,1)] float legendary
      - [Range(0,1)] float specials
  - public struct LootUnluckySettings
    - Public fields
      - float alphaRare
      - float alphaEpic
      - float alphaLegendary
      - float alphaSpecials
  - public struct LootTrimSettings
    - Public fields
      - [Range(0,1)] float common
      - [Range(0,1)] float uncommon
      - [Range(0,1)] float rare
      - [Range(0,1)] float epic
      - [Range(0,1)] float legendary
  - public struct LootRankMultipliers
    - Public fields
      - int spawn
      - int normal
      - int magic
      - int elite
      - int boss
      - int champion
    - Public method
      - int GetMultiplier(EnemyRank rank)
        - Maps EnemyRank to the corresponding multiplier; default 1 if not matched
  - public struct LootSettings
    - Public fields
      - LootBudgetSettings budget
      - LootFloorSettings floors
      - LootUnluckySettings unlucky
      - LootTrimSettings trim
      - LootRankMultipliers rankMultipliers
  - public struct EnemyBudget
    - Public fields
      - int spawn
      - int normal
      - int magic
      - int elite
      - int boss
      - int champion
  - public struct EnemyScaling
    - Public fields
      - float hpPerLevel
      - float dmgPerLevel
  - public struct WaveSettings
    - Public fields
      - EnemyBudget budgetPoints
      - EnemyScaling scaling
  - public struct RankScaling
    - Public fields
      - float hpMultiplier
      - float dmgMultiplier
      - float xpMultiplier
  - public struct EnemyRankSettings
    - Public fields
      - RankScaling spawn
      - RankScaling normal
      - RankScaling magic
      - RankScaling elite
      - RankScaling boss
      - RankScaling champion
    - Public method
      - RankScaling GetScaling(EnemyRank rank)
        - Maps EnemyRank to the corresponding RankScaling; default normal
  - public struct EnemySettings
    - Public fields
      - EnemyBudget budgetPoints
      - EnemyScaling scaling
      - RankSettings rankScaling
      - List<string> magicTagPool
      - int minEliteTags
  - public struct SkillRanges
    - Public fields
      - float selfRange
      - float meleeRange
      - float reachRange
      - float midDistanceRange
      - float farDistanceRange
  - public struct CurrencySettings
    - Public fields
      - int baseGoldReward
      - float goldPerLevel
  - public struct XpSettings
    - Public fields
      - int baseXpReward
      - float xpPerLevel
      - int baseLevelUpXp
      - [Range(1,10)] int levelCurveFactor
  - public struct EconomySettings
    - Public fields
      - CurrencySettings currencies
      - XpSettings xp

3) Key Behavior & Side Effects
- GetRangeValue maps a SkillRange to the corresponding field in skillRanges using a switch expression.
- GetMultiplier maps an EnemyRank to the corresponding multiplier in LootRankMultipliers via a switch; default is 1.
- GetScaling maps an EnemyRank to the appropriate RankScaling via a switch; default is normal.
- The asset is intended to be used as a data/config source in the Unity Editor; sections are organized with Headers for Inspector clarity.

4) Constraints & Failure Modes
- Default cases exist in switch expressions:
  - LootRankMultipliers.GetMultiplier: unknown rank → 1
  - EnemyRankSettings.GetScaling: unknown rank → normal
  - GameBalanceConfig.GetRangeValue: unknown range → skillRanges.meleeRange
- Public fields imply no internal validation in this file; relies on Unity Inspector for ranges (where applicable).
- List<string> magicTagPool may be null if not initialized by the editor or code.

5) Example
```csharp
// Example: read a range value for a specific skill
GameBalanceConfig config = /* assigned in inspector or loaded as asset */;
float selfRange = config.GetRangeValue(SkillRange.Self);
```

6) Unknowns
- Definitions of EnemyRank and SkillRange (assumed to be defined elsewhere).
- Exact runtime loading/usage patterns (e.g., Resources.Load vs. addressable) are not specified here.
- Semantics of the numeric fields (e.g., how levelFactor or beta are applied) are not defined in this file.
- Any validation or dependencies between fields are not enforced within this file.
