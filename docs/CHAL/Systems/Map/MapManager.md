# CHAL.Systems.Map.MapManager

_Automatically generated/updated from `Assets/src/Systems/Map/MapManager.cs`._

1) Purpose
- MapManager is a Unity MonoBehaviour that orchestrates map lifecycle: load current map, instantiate its prefab, manage waves, spawn selected heroes, and coordinate reward UI.
- Exposes map and wave state (CurrentMap, CurrentWave, MaxWaves) and UI references (waveRewardUI, mapRewardUI, selectHeroUI) for wiring in the scene.
- Coordinates with GameManager, HeroCatalogue, WaveManager, and UI components to drive map progression and rewards.

2) Public API
- Namespace/module: CHAL.Systems.Map
- Type: public class MapManager : MonoBehaviour
- Public properties
  - public MapDef CurrentMap { get; private set; }
  - public int CurrentWave { get; private set; } = 1;
  - public int MaxWaves => CurrentMap != null ? CurrentMap.maxWaves : 0;
- Public fields
  - public GameObject waveRewardUI
  - public GameObject mapRewardUI
  - public GameObject selectHeroUI
- Public methods
  - public void HideUI()
  - public void PrepareMap()
  - public void ResetWave()
  - public void StartWave()
  - public void OnWaveCompleted(bool success, WaveRewards rewards)
  - public void NextWave()
- Internal methods (visibility in codebase)
  - internal void SetSelectedHeroes(List<string> heroIds)
- Note: There are also private methods (not part of the public API) used internally:
  - SpawnSelectedHeroesAtSlots(List<string> heroIds, WaveManager waveMgr)
  - ResetHeroesForNewWave(WaveManager waveMgr)
  - ResolveHeroDef(string heroId)
  - GetHeroPrefab(HeroDef def)

3) Key Behavior & Side Effects
- Awake
  - Calls HideUI() to hide reward UI at startup.
- Start
  - Calls PrepareMap() to initialize the current map and select hero flow.
- HideUI
  - waveRewardUI.GetComponent<WaveRewardUI>().Show(false)
  - mapRewardUI.GetComponent<MapRewardUI>().Show(false)
- PrepareMap
  - CurrentMap = GameManager.Instance.pendingMap
  - CurrentWave = 1
  - Logs map start details
  - Destroys existing _mapInstancedPrefab if present
  - Instantiates CurrentMap.mapPrefab if available; otherwise logs a warning
  - Initializes and shows HeroSelectionUI via selectHeroUI
- ResetWave
  - Sets CurrentWave to 1 and starts the wave via StartWave()
- StartWave
  - Hides UI
  - Locates WaveManager in the instantiated map
  - If WaveManager not found: logs error, exits to hideout
  - Calls ResetHeroesForNewWave to clear existing heroes
  - Spawns selected heroes at slots using _pendingSelectedHeroes
  - Logs wave start
  - Calls _waveManager.StartWave(CurrentMap, CurrentWave, this)
- ResetHeroesForNewWave
  - Destroys existing hero instances belonging to the active map instance
  - Logs the number of cleared hero instances
  - Calls SpawnSelectedHeroesAtSlots to spawn new heroes
- SpawnSelectedHeroesAtSlots
  - Guards against null waveMgr
  - Retrieves spawns from WaveManager.HeroSpawns
  - Returns early if spawns null/empty
  - Spawns up to min(spawns.Count, heroIds.Count)
  - For each heroId:
    - Resolves HeroDef via ResolveHeroDef
    - Gets prefab via GetHeroPrefab(def); uses heroFallbackPrefab if needed
    - Instantiates at corresponding spawn position/rotation
    - If spawned object has HeroController, calls Init(def)
    - Logs warning if no HeroController on the spawned hero
- ResolveHeroDef
  - Returns GameManager.Instance.HeroCatalogue.GetById(heroId) or null
- GetHeroPrefab
  - Uses def.Prefab if def != null; otherwise falls back to heroFallbackPrefab
- SetSelectedHeroes
  - Stores a copy of the provided list into _pendingSelectedHeroes (or null if input is null)
- OnWaveCompleted
  - If success is false:
    - Set game state to WaveReward
    - Show waveRewardUI and call populateText(success)
  - If success and CurrentWave < MaxWaves:
    - Set state to WaveReward
    - Show wave reward UI and populate text
  - If success and on last wave:
    - Set state to MapReward
    - Show mapRewardUI and populate text
- NextWave
  - Increments CurrentWave
  - Sets game state to MapPhase
  - Calls StartWave()

4) Constraints & Failure Modes
- Potential null access in StartWave if _mapInstancedPrefab is not set (no explicit null check before GetComponentInChildren)
- StartWave assumes Current map prefab was instantiated; otherwise _mapInstancedPrefab could be null
- HideUI and OnWaveCompleted unconditionally access waveRewardUI/mapRewardUI components; missing references could cause NREs
- GetHeroPrefab may return null if both def.Prefab and heroFallbackPrefab are null; SpawnSelectedHeroesAtSlots handles this by logging a warning and skipping
- MaxWaves returns 0 if CurrentMap is null; rely on external code to initialize CurrentMap
- SpawnSelectedHeroesAtSlots depends on WaveManager.HeroSpawns being non-null; otherwise no spawning
- OnWaveCompleted uses rewards parameter but only passes success to populateText; details depend on UI implementation

5) Example
- Not provided (no self-contained usage example derivable without surrounding game context)

6) Unknowns
- Exact definitions and members of external types (MapDef, HeroDef, WaveManager, WaveRewards, GameManager, HeroCatalogue, DebugManager, HeroSelectionUI, WaveRewardUI, MapRewardUI, HeroController)
- How pendingMap is populated in GameManager, and when PrepareMap is typically called relative to Start
- Behavior of WaveManager.StartWave and HeroController.Init
- Serialized field expectations for heroFallbackPrefab.Prefab naming and availability
- Threading model, if any, for these calls (Unity main thread assumed)

