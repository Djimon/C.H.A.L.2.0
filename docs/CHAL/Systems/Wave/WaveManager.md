# CHAL.Systems.Wave.WaveManager

_Automatically generated/updated from `Assets/src/Systems/Waves/WaveManager.cs`._

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
- Starts a new wave with `StartWave`, which initializes wave rewards and prepares sub-wave distribution.
- Spawns enemies in sub-waves using `RunWaveRoutine` and `RunSubWaveRoutine`.
- Collects loot when enemies are killed via `HandleEnemyKilled`.
- Ends the wave when all enemies are defeated and loot is collected using `TryEndWave`.

# Constraints & Failure Modes
- Guards against invalid wave indices in `StartWave`.
- Handles null or empty spawn points in `SelectSpawnpoint`.
- Ensures that loot collection does not exceed available loot.

# Example
```csharp
WaveManager waveManager = new WaveManager();
waveManager.StartWave(mapDef, 1, mapManagerRef);
```

# Unknowns
- The implementation details of `LootRulesService`, `LootRoller`, and `UnluckyProtection` are not provided.
- The behavior of `DebugManager` methods is not defined in this file.

