# CHAL.Systems.Wave.WaveRewards

_Automatically generated/updated from `Assets/src/Systems/Waves/WaveManager.cs`._

Purpose
- Defines WaveManager: orchestrates subwave planning, spawning, and end-of-wave handling for waves.
- Defines WaveRewards: lightweight container for earned items, currencies, and XP with helper methods.
- Encapsulates public API for starting a wave, loot collection, and reward transfer.

Public API
- Namespace: CHAL.Systems.Wave

- Types
  - public class WaveManager : MonoBehaviour
    - Public fields
      - WaveDef waveDef
      - List<Transform> EnemySpawnPoints
      - List<Transform> HeroSpawns
      - GameObject enemyFallbackPrefab
      - GameObject lootPrefab
      - WaveRewards waveRewards
    - Public methods
      - void StartWave(MapDef mapDef, int waveIndex, MapManager _ref)
      - void CollectRemainingLoot()
      - void CollectLoot(string itemId, int quantity)
      - void TransferRewardsToProfile(WaveRewards rewards)
      - static Vector3 SelectSpawnpoint(List<Transform> spawnPoints)
      - void SimulateWaveStats(MapDef mapDef, int waveIndex)

  - public class WaveRewards
    - Public fields
      - Dictionary<string, int> Items
      - Dictionary<string, int> Currencies
      - int XP
    - Public methods
      - void AddItem(string itemId, int count = 1)
      - void AddCurrency(string currencyId, int amount)
      - void AddXP(int amount)

Key Behavior & Side Effects
- Initialization and event wiring
  - Awake: initializes LootRulesService, LootRoller; subscribes to EnemyController.OnEnemyKilled and LootCube.OnLootCollected.
  - OnDestroy: unsubscribes from events.
- Wave start flow
  - StartWave(mapDef, waveIndex, ref) validates input; creates waveRewards; builds wave composition; initializes _waveCtx; clears alive enemies; prepares SubWavePlan; restarts RunWaveRoutine coroutine.
- Subwave execution
  - RunWaveRoutine: computes S (subWaveCount), inter-subwave delay, and optional cap; spawns S subwaves with backloading logic; waits between subwaves; marks end of subwaves and calls TryEndWave.
  - RunSubWaveRoutine: spawns in rank order (Spawn, Normal, Magic, Elite, Boss, Champion) with 0.2s inter-spawn delay per item.
- Spawning
  - SpawnOne: selects candidate defs for rank, upgrades rank, picks prefab, instantiates at a spawn point, initializes EnemyController, tracks in _aliveEnemies.
  - GetEnemyPrefab: uses baseDef.prefab or falls back to enemyFallbackPrefab.
  - SelectSpawnpoint: chooses a random point from EnemySpawnPoints; logs warning if none.
- Loot and rewards on enemy death
  - HandleEnemyKilled: removes from _aliveEnemies; adds gold/XP to waveRewards; rolls loot and spawns LootCube items; calls TryEndWave.
  - CollectLoot: adds collected loot to waveRewards and logs.
- Wave completion
  - TryEndWave: if all subwaves spawned and no alive enemies, finalizes loot rolls and ends wave.
  - EndWave: on success, collects remaining loot, transfers rewards to profile, logs, and notifies MapManager; resets waveRewards. On failure, logs and notifies MapManager.
- Reward/loot transfer
  - TransferRewardsToProfile: resolves item IDs to inventory types, ensures instances, adds items to domain, adds currencies and XP, updates map progress if on final wave, saves game.
  - CollectRemainingLoot: auto-collects remaining Loot objects on the map by layer, logs actions, and destroys picked loot.
- Misc helpers and stats
  - BuildWaveComposition / AddEnemies: build a data-driven representation of the wave for loot/stats context; select candidates per rank and instantiate EnemyStruct entries.
  - BuildBackloadedDeltas / RemainingForRank: compute and balance deltas per subwave with backloading; guard against non-positive totals.
- Debug utilities
  - ContextMenu entries to start a wave or simulate stats from the Inspector (private methods).

Constraints & Failure Modes
- StartWave guards
  - If mapDef is null, waveIndex out of range, or mapDef.waveDefs invalid, logs error and aborts.
- Spawning guards
  - SpawnOne: if no candidate enemies for rank, logs warning and skips.
  - GetEnemyPrefab: falls back to enemyFallbackPrefab if baseDef.prefab is null.
- Loot and inventory guards
  - CollectRemainingLoot: warns if Loot layer not found; otherwise collects known loot objects.
  - TransferRewardsToProfile: gracefully handles null profile/domain/rewards; unknown item IDs are logged and counted as unknown; ensures inventory instance before adding.
  - On adding to inventory: logs if Add fails.
- Wave progression guards
  - RunWaveRoutine respects cap (if configured) by yielding until under cap.
  - TryEndWave only triggers when all subwaves spawned and no alive enemies.
- General robustness
  - SelectSpawnpoint returns Vector3.zero if no spawn points and logs a warning.
  - BuildBackloadedDeltas and related helpers guard against zero/negative totals.
- Concurrency and persistence
  - Awake/OnDestroy manage event subscriptions; EndWave calls GameManager.Save if rewards are transferred.
  - EndWave notifies MapManager using OnWaveCompleted(success, waveRewards) and resets waveRewards.
- Debug
  - Debug menu actions rely on inspector-provided debugMap/debugWaveIndex; null checks present.

Unknowns
- Details of types not defined in this file (e.g., WaveDef, WaveComposition, EnemyDef, EnemyStruct, MapDef, BalanceManager, DebugManager, LootRulesService, LootRoller, UnluckyProtection, MapManager, GameManager, Inventory domain, etc.).
- Exact behavior of external systems (loot dropping rules, currency/XP scaling, map progression persistence, and UI feedback) beyond what this file directly implements.
- Runtime characteristics of the Loot/Inventory pipeline (threading, async behavior, or side effects outside this file).

