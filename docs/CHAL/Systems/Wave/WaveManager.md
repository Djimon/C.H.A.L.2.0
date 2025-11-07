# CHAL.Systems.Wave.WaveManager

_Automatically generated/updated from `Assets/src/Systems/Waves/WaveManager.cs`._

```csharp
1) Purpose
- Defines WaveManager, a Unity MonoBehaviour that orchestrates waves and subwaves, spawning enemies and handling wave end/loot flows.
- Keeps per-wave state (spawn plan, alive enemies, rewards) and integrates loot/reward paths with the player profile.
- Exposes public API to start a wave, collect loot, and simulate stats; includes a WaveRewards data container for per-wave loot.

2) Public API
- Namespace/module
  - CHAL.Systems.Wave

- Types
  - public class WaveManager : MonoBehaviour
    - Public fields
      - public WaveDef waveDef
        - Wave definition for the manager (overall composition hints)
      - public List<Transform> EnemySpawnPoints
        - Spawn points used for enemies
      - public List<Transform> HeroSpawns
        - Spawn points for heroes (unused directly in this file, stored)
      - [SerializeField] private MapDef debugMap
        - Debug map definition for testing purposes
      - [SerializeField] private int debugWaveIndex
        - Debug wave index for testing purposes
      - public GameObject enemyFallbackPrefab
        - Fallback prefab if an enemy baseDef has no prefab
      - public GameObject lootPrefab
        - Loot object prefab used when dropping loot on enemy death
      - public WaveRewards waveRewards
        - Collects loot/rewards for the current wave
    - Public methods
      - public void StartWave(MapDef mapDef, int waveIndex, MapManager _ref)
        - Initializes and starts a wave on the given map definition and wave index; assigns MapManager reference; prepares subwave plan; runs RunWaveRoutine
      - public void CollectRemainingLoot()
        - Auto-collects any remaining LootCube objects of layer "Loot" in the scene into waveRewards
      - public void CollectLoot(string itemId, int quantity)
        - Adds collected loot to waveRewards
      - public void SimulateWaveStats(MapDef mapDef, int waveIndex)
        - Runs a simulation setup for a given map/wave (loosely connected to RunStats; code shows placeholder)
      - public static Vector3 SelectSpawnpoint(List<Transform> spawnPoints)
        - Chooses a random spawn position from the provided list
  - public class WaveRewards
    - Public fields
      - public Dictionary<string, int> Items
        - itemId → count collected for this wave
      - public Dictionary<string, int> Currencies
        - currencyId → amount collected for this wave
      - public int XP
        - XP amount awarded for this wave
    - Public methods
      - public void AddItem(string itemId, int count = 1)
        - Increments item count in Items
      - public void AddCurrency(string currencyId, int amount)
        - Increments currency amount in Currencies
      - public void AddXP(int amount)
        - Increments XP and logs debug

3) Key Behavior & Side Effects
- Lifecycle hooks
  - Awake
    - Initializes LootRulesService, loads rules
    - Sets _unlucky via GameManager.Instance
    - Creates LootRoller using rules and unlucky protection
    - Subscribes to EnemyController.OnEnemyKilled and LootCube.OnLootCollected
  - OnDestroy
    - Unsubscribes from EnemyController.OnEnemyKilled and LootCube.OnLootCollected
- Wave startup flow (StartWave)
  - Validates inputs (mapDef non-null, waveIndex within range)
  - Creates a fresh WaveRewards container
  - Builds wave composition (BuildWaveComposition) for loot/XP context
  - Creates _waveCtx (WaveLootContext) from the composition
  - Clears _aliveEnemies and resets _allSubWavesSpawned
  - Prepares subwave distribution (PrepareSubWaveDistribution)
  - Stops any existing coroutines, then starts RunWaveRoutine(mapDef, wDef)
- Subwave execution
  - RunWaveRoutine
    - Determines subWave count S and inter-subwave delay
    - Applies optional cap on concurrent enemies
    - Spawns each subwave via RunSubWaveRoutine
    - Waits inter-subwave delay between subwaves
    - After all subwaves spawned, marks _allSubWavesSpawned and calls TryEndWave
  - RunSubWaveRoutine
    - Spawns enemies in round-robin by rank (Spawn, Normal, Magic, Elite, Boss, Champion)
    - Enforces a 0.2s delay between individual spawns
- Spawning
  - SpawnOne
    - Gets candidate EnemyDef for the given rank (GetCandidatesForRank)
    - If none, logs a warning and skips
    - Chooses a baseDef, upgrades rank (UpgradeRank)
    - Determines prefab (GetEnemyPrefab) with fallback
    - Instantiates at a random sub-spawn point (SelectSpawnpoint)
    - Initializes EnemyController with the EnemyStruct instance
    - Tracks alive enemies in _aliveEnemies
- Loot & rewards
  - HandleEnemyKilled
    - Removes the killed enemy from _aliveEnemies
    - Grants currency and XP via _roller and wave context
    - Rolls loot for the monster, spawns loot lootPrefabs at position
    - Calls TryEndWave
  - TryEndWave
    - If all subwaves spawned and no alive enemies remain, finalizes wave via LootRoller and triggers EndWave(true)
  - EndWave
    - If success: CollectRemainingLoot, TransferRewardsToProfile, log, and notify MapManager
    - If failed: log
    - Calls _MapMangerRef.OnWaveCompleted(success, waveRewards) and resets waveRewards
  - CollectRemainingLoot
    - Finds Loot cubes on the Loot layer, auto-collects them by calling CollectLoot and destroying the objects
  - TransferRewardsToProfile
    - Transfers wave rewards into the player's inventory/profile
    - Resolves item IDs to inventory instances, ensures instances exist, adds to domain and logs issues
    - Adds currencies and XP to profile
    - Updates map progress if on final wave and saves game
- Wave composition & helpers
  - BuildWaveComposition
    - Builds a WaveComposition with Level/Difficulty and Monster list using AddEnemies per rank
  - UpgradeRank
    - Returns an EnemyStruct with rank upgrades
    - Adds magic tag for Magic rank, and elite modifiers if applicable
  - GetCandidatesForRank
    - Chooses eligible EnemyDef entries for a given rank from mapDef.allowedEnemies
  - AddEnemies
    - Adds a number of enemies of a given rank to a WaveComposition
  - PrepareSubWaveDistribution
    - Constructs a per-subwave plan (spawn/normal/magic/elite/boss/champion)
    - Uses backloaded deltas with alpha to shape distribution
    - Balances per-subwave counts to meet total required per rank
    - Performs safety check and logs mismatches
  - BuildBackloadedDeltas
    - Builds per-subwave deltas based on a geometric-like weight (alpha)
  - RemainingForRank
    - Helper to compute remaining capacity for a rank in subwave k
- Misc
  - SelectSpawnpoint
    - Returns a random valid spawn location or Vector3.zero with a warning if none
  - Debug helpers
    - DebugStartWave / DebugSimulateWaveStats via ContextMenu
- Events
  - Subscribes to EnemyController.OnEnemyKilled and LootCube.OnLootCollected
  - On wave end, triggers MapManager.OnWaveCompleted

4) Constraints & Failure Modes
- Input validation
  - StartWave guards against null mapDef and invalid waveIndex
- Spawn candidates
  - SpawnOne logs a warning and skips spawn if no candidates for the given rank
- Loot/spawn dependencies
  - GetEnemyPrefab falls back to enemyFallbackPrefab if baseDef.prefab is null
- Loot collection
  - CollectRemainingLoot relies on Loot layer existing; warns if not found
- Item/identity resolution
  - TransferRewardsToProfile uses GameManager.TryResolveByItemId; if unknown, logs and continues
  - EnsureInstance is called before adding items to inventory
- Wave progression
  - TryEndWave only ends when all subwaves spawned and no alive enemies remain
  - EndWave calls MapManager.OnWaveCompleted and resets waveRewards
- Async flows
  - Uses coroutines; StopAllCoroutines is called before starting a new wave to avoid overlapping
- Null references
  - Some flows assume _MapMangerRef is set by StartWave; methods referencing it may crash if used incorrectly
- Performance
  - Spawning is sequential with 0.2s micro-delays; large waves may incur noticeable delays in RunSubWaveRoutine

5) Example
- Minimal usage snippet (illustrative, requires existing map/manager context)
```csharp
// Example: start a wave from code
WaveManager wm = someGameObject.GetComponent<WaveManager>();
MapDef mapDef = someMapDef;          // provided by game logic
MapManager mapManager = someMapManager; // contextual manager
wm.StartWave(mapDef, 1, mapManager);
```

6) Unknowns
- Definitions/structure of several types not defined in this file (MapDef, WaveDef, WaveComposition, EnemyDef, EnemyStruct, EnemyRank, LootRoller, LootRulesService, MapManager, DebugManager, BalanceManager, etc.)
- Exact behavior of LootRoller.RollGoldForMonster, RollXPForMonster, RollLootForMonster
- Details of WaveLootContext, and how the loot context interacts with LootCube/Inventory beyond what is shown
- The contents and format of WaveDef.backload and how backloading alpha is configured
- Any side effects of GameManager.TryResolveByItemId beyond what’s visible here
```
