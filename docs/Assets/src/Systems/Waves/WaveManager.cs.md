# Assets/src/Systems/Waves/WaveManager.cs

_Automatic generated/updated._

```markdown
# WaveManager.cs Documentation

## Purpose
- Defines the `WaveManager` class for managing enemy waves in a game.
- Provides functionality for starting waves, spawning enemies, and handling rewards.

## Public API
- Namespace: `CHAL.Systems.Wave`
- Types:
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
        - Starts a wave based on the provided map definition and wave index.
      - `public void CollectLoot(string itemId, int quantity);`
        - Collects loot and adds it to the wave rewards.
      - `public void CollectRemainingLoot();`
        - Collects any remaining loot at the end of a wave.
      - `public void SimulateWaveStats(MapDef mapDef, int waveIndex);`
        - Simulates wave statistics for a given map and wave index.

## Key Behavior & Side Effects
- `StartWave` initializes the wave, validates input, and starts the coroutine to run the wave.
- `RunWaveRoutine` manages the spawning of sub-waves and waits for enemy counts to drop below a cap.
- `HandleEnemyKilled` processes enemy deaths, updates rewards, and checks if the wave can end.
- `EndWave` finalizes the wave, transfers rewards, and resets the wave rewards.

## Constraints & Failure Modes
- Guards against invalid `mapDef` or `waveIndex` in `StartWave`.
- Handles cases where no candidates for enemy ranks are found during spawning.
- Uses coroutines for wave management, which may affect performance if not managed properly.

## Example
```csharp
WaveManager waveManager = new WaveManager();
waveManager.StartWave(mapDef, 1, mapManagerRef);
```

## Unknowns
- Specific implementations of `MapDef`, `WaveDef`, `EnemyController`, and other referenced classes are not defined in this file.
- The behavior of external dependencies like `DebugManager` and `GameManager` cannot be determined from this file.
```
