# CHAL.Systems.Map.MapManager

_Automatically generated/updated from `Assets/src/Systems/Map/MapManager.cs`._

# Purpose
- Manages the map and its related functionalities in the game.

# Public API
- Namespace: `CHAL.Systems.Map`
- Types
  - public class `MapManager` [extends `MonoBehaviour`]
    - Public fields/properties
      - `MapDef CurrentMap`: Current map definition.
      - `GameObject waveRewardUI`: UI for wave rewards.
      - `GameObject mapRewardUI`: UI for map rewards.
      - `GameObject selectHeroUI`: UI for hero selection.
      - `bool AutoStartAllWaves`: Indicates if all waves should start automatically.
      - `int CurrentWave`: Current wave number.
      - `int MaxWaves`: Maximum number of waves in the current map.
    - Public methods
      - `void HideUI()`: Hides the user interface elements for the current wave.
      - `void PrepareMap()`: Prepares the game map for the current wave.
      - `void ResetWave()`: Resets the current wave to the first wave.
      - `void StartWave()`: Starts a new wave in the game.
      - `bool HasNextWave()`: Checks if there is a next wave available; returns true if there is.
      - `void OnWaveCompleted(bool success, WaveRewards rewards)`: Handles wave completion, success, and rewards.
      - `void SetAutoStartAllWaves(bool enabled)`: Sets whether all waves should start automatically.
      - `void NextWave()`: Advances to the next wave in the game.
      - `internal void SetSelectedHeroes(List<string> heroIds)`: Sets the selected heroes for the current wave.

# Key Behavior & Side Effects
- `Awake()`: Hides UI elements when the game starts.
- `Start()`: Prepares the map for the current wave.
- `HideUI()`: Hides wave and map reward UI elements.
- `PrepareMap()`: Initializes the map and instantiates necessary prefabs; resets wave and auto-start settings.
- `StartWave()`: Initializes the wave manager, resets heroes, and starts the wave.
- `OnWaveCompleted()`: Updates game state based on wave success and displays appropriate rewards.

# Constraints & Failure Modes
- If `CurrentMap.mapPrefab` is null in `PrepareMap()`, a warning is logged.
- If `WaveManager` is not found in `StartWave()`, an error is logged and the game exits to hideout.
- `ResetHeroesForNewWave()`: Only destroys heroes belonging to the active map instance.
- `SpawnSelectedHeroesAtSlots()`: Handles null or empty hero IDs and spawn slots gracefully.

# Example
```csharp
MapManager mapManager = new MapManager();
mapManager.PrepareMap();
mapManager.StartWave();
```

# Unknowns
- The implementation details of `GameManager`, `WaveManager`, `HeroDef`, and `WaveRewards` cannot be determined from this file.

