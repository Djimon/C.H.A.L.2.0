# CHAL.Systems.Wave.WaveManager

_Automatically generated/updated from `Assets/src/Systems/Waves/WaveManager.cs`._

# WaveManager.cs

## Purpose
- Manages the spawning and lifecycle of waves of enemies in a game.
- Handles loot collection and reward distribution upon wave completion.

## Public API
- Namespace: `CHAL.Systems.Wave`
- Types
  - `public class WaveManager : MonoBehaviour`
    - Public fields/properties:
      - `public WaveDef waveDef;`
      - `public List<Transform> EnemySpawnPoints;`
      - `public List<Transform> HeroSpawns;`
      - `public GameObject enemyFallbackPrefab;`
      - `public GameObject lootPrefab;`
      - `public WaveRewards waveRewards;`
    - Public methods:
      - `public void StartWave(MapDef mapDef, int waveIndex, MapManager _ref);`
      - `public void CollectLoot(string itemId, int quantity);`
      - `public void CollectRemainingLoot();`
      - `public void SimulateWaveStats(MapDef mapDef, int waveIndex);`

  - `public class WaveRewards`
    - Public fields/properties:
      - `public Dictionary<string, int> Items;`
      - `public Dictionary<string, int> Currencies;`
      - `public int XP;`
    - Public methods:
      - `public void AddItem(string itemId, int count = 1);`
      - `public void AddCurrency(string currencyId, int amount);`
      - `public void AddXP(int amount);`

## Key Behavior & Side Effects
- `StartWave`: Initializes a new wave, validates input, and starts the wave routine.
- `RunWaveRoutine`: Manages the spawning of sub-waves and waits for enemy counts to drop below a cap.
- `HandleEnemyKilled`: Updates rewards and handles loot spawning when an enemy is killed.
- `EndWave`: Finalizes the wave, transfers rewards, and resets the wave rewards.

## Constraints & Failure Modes
- Guards against invalid wave indices in `StartWave`.
- Handles null or empty spawn points in `SelectSpawnpoint`.
- Uses coroutines for wave management, which may affect performance if not managed properly.

## Example
```csharp
WaveManager waveManager = new WaveManager();
waveManager.StartWave(mapDef, 1, mapManagerRef);
```

## Unknowns
- The behavior of external classes and methods (e.g., `LootRulesService`, `LootRoller`, `EnemyController`) cannot be determined from this file.
- The structure and contents of `MapDef`, `WaveDef`, and `EnemyDef` are not defined in this file.

