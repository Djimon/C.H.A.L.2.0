# CHAL.Systems.Map.MapManager

_Automatically generated/updated from `Assets/src/Systems/Map/MapManager.cs`._

# Purpose
- Defines the `MapManager` class for managing map-related functionalities in the game.

# Public API
- Namespace: `CHAL.Systems.Map`
- Types
  - `public class MapManager : MonoBehaviour`
    - Public fields/properties:
      - `MapDef CurrentMap { get; private set; }` - Current map definition.
      - `int CurrentWave { get; private set; }` - Current wave number.
      - `int MaxWaves => CurrentMap != null ? CurrentMap.maxWaves : 0` - Maximum number of waves in the current map.
    - Public methods:
      - `void HideUI()` - Hides the wave and map reward UI.
      - `void PrepareMap()` - Prepares the map for the current game session.
      - `void ResetWave()` - Resets the current wave to 1 and starts it.
      - `void StartWave()` - Starts the current wave, hides UI, and spawns heroes.
      - `void SetSelectedHeroes(List<string> heroIds)` - Sets the list of selected heroes.
      - `void OnWaveCompleted(bool success, WaveRewards rewards)` - Handles actions upon wave completion.
      - `void NextWave()` - Advances to the next wave.

# Key Behavior & Side Effects
- `Awake()`: Initializes UI and prepares the map.
- `Start()`: Calls `PrepareMap()` to set up the current map.
- `HideUI()`: Hides the UI elements for rewards.
- `PrepareMap()`: Instantiates the map prefab and initializes hero selection UI.
- `StartWave()`: Hides UI, retrieves the `WaveManager`, and spawns selected heroes.
- `OnWaveCompleted()`: Updates game state based on wave success and shows appropriate reward UI.

# Constraints & Failure Modes
- `PrepareMap()`: Logs a warning if the map prefab is missing.
- `StartWave()`: Exits to hideout if `WaveManager` is not found.
- `SpawnSelectedHeroesAtSlots()`: Safely handles null or empty hero IDs and spawn slots.

# Example
```csharp
MapManager mapManager = new MapManager();
mapManager.SetSelectedHeroes(new List<string> { "hero1", "hero2" });
mapManager.PrepareMap();
mapManager.StartWave();
```

# Unknowns
- The exact structure of `MapDef`, `HeroDef`, and `WaveRewards` types.
- The behavior of `GameManager.Instance` and its methods.

