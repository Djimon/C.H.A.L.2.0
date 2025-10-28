# Assets/src/Systems/Map/MapManager.cs

_Automatic generated/updated._

```markdown
## Purpose
- Defines the `MapManager` class for managing map-related functionality in the game.

## Public API
- Namespace: `CHAL.Systems.Map`
- Types
  - `public class MapManager : MonoBehaviour`
    - Public fields/properties:
      - `MapDef CurrentMap { get; private set; }` - Current map definition.
      - `int CurrentWave { get; private set; }` - Current wave number.
      - `int MaxWaves => CurrentMap != null ? CurrentMap.maxWaves : 0` - Maximum number of waves in the current map.
    - Public methods:
      - `void HideUI()` - Hides the wave and map reward UI.
      - `void PrepareMap()` - Prepares the current map and initializes UI.
      - `void ResetWave()` - Resets the current wave to 1 and starts the wave.
      - `void StartWave()` - Starts the current wave, hides UI, and spawns heroes.
      - `void OnWaveCompleted(bool success, WaveRewards rewards)` - Handles the completion of a wave and updates UI based on success.
      - `void NextWave()` - Advances to the next wave and starts it.

## Key Behavior & Side Effects
- `Awake()`: Initializes the manager and hides UI elements.
- `Start()`: Prepares the map at the start of the game.
- `PrepareMap()`: Instantiates the map prefab and initializes hero selection UI.
- `StartWave()`: Hides UI, retrieves the `WaveManager`, spawns selected heroes, and starts the wave.
- `OnWaveCompleted()`: Updates game state and UI based on the success of the wave.

## Constraints & Failure Modes
- `PrepareMap()`: Checks for the existence of the map prefab and logs a warning if missing.
- `StartWave()`: Logs an error and exits to hideout if `WaveManager` is not found.
- `SpawnSelectedHeroesAtSlots()`: Handles null or empty hero IDs and spawns only if valid slots are available.

## Example
```csharp
MapManager mapManager = new MapManager();
mapManager.PrepareMap();
mapManager.StartWave();
```

## Unknowns
- The exact structure of `MapDef`, `HeroDef`, and `WaveRewards` types cannot be determined from this file.
- The implementation details of `GameManager`, `WaveRewardUI`, and `MapRewardUI` are not provided.
```
