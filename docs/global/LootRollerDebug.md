# global.LootRollerDebug

_Automatically generated/updated from `Assets/src/Systems/_test/dbg_LootRoller.cs`._

1) Purpose
- Debug Unity MonoBehaviour to exercise LootRoller_old by constructing rules, a wave, and rolling loot; logs results.
- Demonstrates WaveComposition setup (Level, Difficulty, Monsters with EnemyId, Count, bonusTags) and LootRoller_old.RollLoot usage.
- Logs each loot entry via Debug.Log (ItemId, EnemyId, PickedTag).

2) Public API
- Namespace/module: global namespace
- Types
  - public class LootRollerDebug : MonoBehaviour
    - Public fields: none
    - Public methods: none
- Note: private field present
  - private CHAL.Systems.Loot.LootRoller_old roller
    - Not publicly accessible; declared but unused in this file (shadowed by a local variable in Start)

3) Key Behavior & Side Effects
- Start method sequence (implicit Unity lifecycle):
  - Create LootRulesService and call LoadAll()
  - Create UnluckyProtection
  - Create LootRoller_old with rules and unlucky
  - Define wave as CHAL.Data.WaveComposition with:
    - Level = 3
    - Difficulty = MapDifficulty.Stable
    - Monsters: list of CHAL.Data.EnemyStruct entries
      - Entry 1: EnemyId = "Monster1", Count = 10, bonusTags = ["insect", "swarm"]
      - Entry 2: EnemyId = "Monster2", Count = 3, bonusTags = ["beast", "tank"]
      - Entry 3: EnemyId = "Monster3", Count = 1, bonusTags = ["insect", "boss"]
  - Call roller.RollLoot(wave) to obtain loot
  - Iterate loot and log: " - {ItemId} from {EnemyId} via {PickedTag}"
- Side effects:
  - Logs to Unity console via Debug.Log for each loot entry

4) Constraints & Failure Modes
- No explicit error handling; Start assumes successful execution of:
  - rules.LoadAll()
  - roller.RollLoot(wave)
- Potential issues:
  - Null/null-like failures from LoadAll, RollLoot, or wave construction are not guarded
  - Private field roller is unused due to an inner local variable with the same name (shadowing)

5) Example
Example (minimal usage pattern derived from file)
- Demonstrates creating rules, loading, constructing a roller, and rolling loot
```csharp
var rules = new CHAL.Systems.Loot.LootRulesService();
rules.LoadAll();

var unlucky = new CHAL.Systems.Loot.UnluckyProtection();
var roller = new CHAL.Systems.Loot.LootRoller_old(rules, unlucky);

var wave = new CHAL.Data.WaveComposition
{
    Level = 3,
    Difficulty = MapDifficulty.Stable,
    Monsters = new List<CHAL.Data.EnemyStruct>
    {
        new CHAL.Data.EnemyStruct { EnemyId = "Monster1", Count = 1, bonusTags = new List<string>{ "insect" } }
    }
};

var loot = roller.RollLoot(wave);
foreach (var entry in loot)
{
    Debug.Log($" - {entry.ItemId} from {entry.EnemyId} via {entry.PickedTag}");
}
```

6) Unknowns
- Implementations details of:
  - CHAL.Systems.Loot.LootRoller_old
  - CHAL.Systems.Loot.LootRulesService
  - CHAL.Systems.Loot.UnluckyProtection
- Exact structure and additional members of the loot entry objects beyond ItemId, EnemyId, PickedTag
- Behavior of MapDifficulty and the full semantics of WaveComposition, EnemyStruct
- Any runtime behavior beyond the Start method (e.g., editor-time effects, serialization)
