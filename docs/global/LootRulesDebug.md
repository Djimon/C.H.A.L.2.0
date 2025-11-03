# global.LootRulesDebug

_Automatically generated/updated from `Assets/src/Systems/_test/dbg_LootRules.cs`._

1) Purpose
- Unity test/demo MonoBehaviour LootRulesDebug that exercises loot rule merging, secret drops, budget calculation, unlucky protection, and budget modulation, with runtime logs.
- Exposes serialized test parameters for loot and budget scenarios:
  - enemyTags: string[] to filter/select loot rules by enemy tags
  - level: int for level-dependent budget/modulator behavior
  - difficulty: MapDifficulty for budget calculation context
  - spawns, normals, magics, elites, bosses, champions: int counts for enemy composition
  - Budget, U_budget_Used, vi_item_Value: int values used by the modulator/budget logic
- On Start, wires up systems, emits detailed Debug.Log output for merged drops, secret drops, wave budget, unlucky-protection runs, and final modulator result.

2) Public API
- Namespace/module: (none)
- Types
  - public class LootRulesDebug : MonoBehaviour
    - Public fields
      - string[] enemyTags
        - Tags used to influence loot rule merging
      - int level
        - Level used in budget calculation
      - MapDifficulty difficulty
        - Map difficulty for budget calculation
      - int spawns
        - Number of spawns in the wave
      - int normals
        - Count of normal enemies
      - int magics
        - Count of magic enemies
      - int elites
        - Count of elites
      - int bosses
        - Count of bosses
      - int champions
        - Count of champions
      - int Budget
        - Budget value (for debugging/modulator)
      - int U_budget_Used
        - Budget already used (for modulator)
      - int vi_item_Value
        - Item value used by modulator
    - Public methods: none
      - Start() is present but not public; it is invoked by Unity as a lifecycle method.

3) Key Behavior & Side Effects
- Start method behavior (major flows)
  - ItemRegistry.Instance.Reload() to ensure items are loaded
  - LootRulesService svc = new LootRulesService(); svc.LoadAll()
  - merged = svc.GetMergedForTags(enemyTags)
    - Logs per-drop details: itemId, rarity, lootValue, chance (either single value or array), quantity
  - Logs merged.rarityGuarantees entries: key and value
  - tags = new[] { "insect", "lvl3", "swarm" }
  - extras = svc.GetSecretDrops(tags)
  - Logs extras count and per-item: itemId, chance, quantity
  - B = LootBudgetCalculator.CalculateBudget(spawns, normals, magics, elites, bosses, champions, level, difficulty)
  - Logs Wave Budget with level and difficulty
  - manager = new UnluckyProtection()
  - For i from 0 to 9:
    - mult = manager.GetMultiplier(Rarity.Rare)
    - pEff = pBase * mult
    - Logs fail index, multiplier, and effective chance
    - manager.OnFail(Rarity.Rare)
    - Logs manager.DebugInfo()
  - manager.OnDrop(Rarity.Rare)
  - Logs DebugInfo after drop
  - M = LootBudgetModulator.GetModifier(U_budget_Used, vi_item_Value, B, rarity)
  - Logs final modifier with B, Used, Item, and Rarity

4) Constraints & Failure Modes
- No null checks shown; assumes LootRulesService, ItemRegistry, and related data sources are available and return valid data
- All behavior executes in Start (Unity main thread); no asynchronous handling
- Debug.Log is used for output; performance/logging is intended for debugging/demo purposes
- Public fields are serialized/test-configured; changes affect the emitted debug output and computed values

6) Unknowns
- Definitions and behavior of external types and methods not defined in this file (e.g., MapDifficulty, Rarity, LootRulesService, LootBudgetCalculator, UnluckyProtection, LootBudgetModulator)
- Exact structure of merged.drop items and merged.drops elements (fields like itemId, rarity, lootValue, chance, quantity, chancesArray)
- Semantics of GetSecretDrops and GetMergedForTags results
- Any side effects of ItemRegistry.Reload(), or LoadAll() beyond what is observable via Debug.Log
- Any behavior of the modifier calculation beyond the single usage shown

