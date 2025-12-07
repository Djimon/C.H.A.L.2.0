# Assets/src/Systems/Map/Waves/WaveManager.cs

_Automatically generated/updated from `Assets/src/Systems/Map/Waves/WaveManager.cs`._

# Purpose
- Manages the spawning of waves of enemies in the game.
- Handles enemy spawn points, wave definitions, and rewards.

# Public API
- Namespace: `CHAL.Systems.Wave`
- Types
  - public class `WaveManager` [extends `MonoBehaviour`]
    - Public fields/properties:
      - `WaveDef waveDef`
      - `List<Transform> EnemySpawnPoints`
      - `List<Transform> HeroSpawns`
      - `GameObject enemyFallbackPrefab`
      - `GameObject lootPrefab`
      - `WaveRewards waveRewards`
    - Public methods:
      - `void StartWave(MapDef mapDef, int waveIndex, MapManager _ref)`
      - `void CollectRemainingLoot()`
      - `void CollectLoot(string itemId, int quantity)`
      - `void SimulateWaveStats(MapDef mapDef, int waveIndex)`
      - static `Vector3 SelectSpawnpoint(List<Transform> spawnPoints)`

  - public class `WaveRewards`
    - Public fields/properties:
      - `Dictionary<string, int> Items`
      - `Dictionary<string, int> Currencies`
      - `int XP`
    - Public methods:
      - `void AddItem(string itemId, int count = 1)`
      - `void AddCurrency(string currencyId, int amount)`
      - `void AddXP(int amount)`

# Key Behavior & Side Effects
- `StartWave`: Initializes a new wave, validates input, and starts the wave routine.
- `RunWaveRoutine`: Manages the spawning of sub-waves and waits for all enemies to be defeated.
- `HandleEnemyKilled`: Updates rewards and checks if the wave can end after an enemy is killed.
- `CollectLoot`: Adds loot to rewards and checks if the wave can end after collecting loot.

# Constraints & Failure Modes
- Guards against invalid `mapDef` or `waveIndex` in `StartWave`.
- Handles null or empty spawn points in `SelectSpawnpoint`.
- Uses coroutines for wave management, ensuring proper timing and order of enemy spawns.

# Example
```csharp
WaveManager waveManager = new WaveManager();
waveManager.StartWave(mapDef, 1, mapManagerRef);
```

# Unknowns
- The implementation details of `LootRulesService`, `LootRoller`, and `UnluckyProtection` are not provided in this file.
- The behavior of `DebugManager` and its logging methods is not defined in this file.
