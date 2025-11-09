# Assets/src/Systems/Map/MapManager.cs

_Automatically generated/updated from `Assets/src/Systems/Map/MapManager.cs`._

# Purpose
- Manages the map and its related functionalities in the game.

# Public API
- Namespace: `CHAL.Systems.Map`
- Types
  - `public class MapManager : MonoBehaviour`
    - Public fields/properties:
      - `MapDef CurrentMap { get; private set; }` - Current map definition.
      - `GameObject waveRewardUI` - UI for wave rewards.
      - `GameObject mapRewardUI` - UI for map rewards.
      - `GameObject selectHeroUI` - UI for hero selection.
      - `bool AutoStartAllWaves { get; }` - Indicates if all waves should start automatically.
      - `int CurrentWave { get; private set; }` - Current wave number.
      - `int MaxWaves { get; }` - Maximum number of waves in the current map.
    - Public methods:
      - `void HideUI()` - Hides the user interface elements for the current wave.
      - `void PrepareMap()` - Prepares the game map for the current wave.
      - `void ResetWave()` - Resets the current wave to the first wave.
      - `void StartWave()` - Starts a new wave in the game.
      - `bool HasNextWave()` - Checks if there is a next wave available.
      - `void OnWaveCompleted(bool success, WaveRewards rewards)` - Handles wave completion and rewards.
      - `void SetAutoStartAllWaves(bool enabled)` - Sets whether all waves should start automatically.
      - `void NextWave()` - Advances to the next wave in the game.
      - `internal void SetSelectedHeroes(List<string> heroIds)` - Sets the selected heroes for the wave.

# Key Behavior & Side Effects
- `Awake()`: Hides UI elements at the start.
- `Start()`: Prepares the map for the current wave.
- `HideUI()`: Hides reward UI elements.
- `PrepareMap()`: Initializes the map and instantiates prefabs; resets wave and auto-start settings.
- `StartWave()`: Initializes the wave manager, resets heroes, and starts the wave.
- `OnWaveCompleted()`: Updates game state and shows appropriate reward UI based on wave success.

# Constraints & Failure Modes
- If `CurrentMap.mapPrefab` is null, a warning is logged.
- If `WaveManager` is not found in the instantiated map prefab, an error is logged and the game exits to hideout.
- Handles null or empty hero IDs gracefully in `SpawnSelectedHeroesAtSlots()` and `ResetHeroesForNewWave()`.

# Example
```csharp
MapManager mapManager = new MapManager();
mapManager.PrepareMap();
mapManager.StartWave();
```

# Unknowns
- The behavior of `GameManager.Instance` and its methods cannot be determined from this file.
- The structure of `HeroDef` and `WaveRewards` is not defined in this file.

