# CHAL.Systems.Map.MapManager

_Automatically generated/updated from `Assets/src/Systems/Map/MapManager.cs`._

1) Purpose
- Defines MapManager (MonoBehaviour) responsible for map setup, wave progression, and hero spawning coordination.
- Holds references to runtime UI elements (wave rewards, map rewards, hero selection) and the current map state.
- Orchestrates map instantiation, hero spawning at spawn slots, wave start/completion, and reward UI transitions.

2) Public API
- Namespace/module: CHAL.Systems.Map
- Types
  - public class MapManager : MonoBehaviour
    - Public fields/properties
      - public MapDef CurrentMap { get; private set; }
      - public GameObject waveRewardUI;
      - public GameObject mapRewardUI;
      - public GameObject selectHeroUI;
      - public int CurrentWave { get; private set; } = 1;
      - public int MaxWaves => CurrentMap != null ? CurrentMap.maxWaves : 0;
    - Public methods
      - public void HideUI()
      - public void PrepareMap()
      - public void ResetWave()
      - [ContextMenu("Debug/StartWave")]
        - public void StartWave()
      - public void OnWaveCompleted(bool success, WaveRewards rewards)
      - [ContextMenu("Debug/Start Next Wave")]
        - public void NextWave()
    - Internal methods
      - internal void SetSelectedHeroes(List<string> heroIds)
    - Private methods (surface not exposed publicly)
      - private void SpawnSelectedHeroesAtSlots(List<string> heroIds, WaveManager waveMgr)
      - private HeroDef ResolveHeroDef(string heroId)
      - private GameObject GetHeroPrefab(HeroDef def)
    - Private/Serialized fields (surface not exposed publicly)
      - [SerializeField] private GameObject heroFallbackPrefab
      - private Dictionary<string, HeroDef> _heroById
      - private List<string> _pendingSelectedHeroes
      - private GameObject _mapInstancedPrefab
      - private WaveManager _waveManager

3) Key Behavior & Side Effects
- Awake
  - Hides all UI via HideUI().
- Start
  - Calls PrepareMap() to initialize current map and setup.
- HideUI
  - Disables visibility of WaveRewardUI and MapRewardUI components.
- PrepareMap
  - Reads CurrentMap from GameManager.Instance.pendingMap and resets CurrentWave to 1.
  - Destroys existing _mapInstancedPrefab if present.
  - Instantiates CurrentMap.mapPrefab if provided; logs a warning if missing.
  - Initializes and shows the hero selection UI (selectHeroUI) with this MapManager as context.
- ResetWave
  - Sets CurrentWave to 1 and starts the wave via StartWave().
- StartWave
  - Hides UI.
  - Locates WaveManager via _mapInstancedPrefab.GetComponentInChildren<WaveManager>().
  - If WaveManager not found, logs error, and exits to hero-hideout via GameManager.
  - Spawns selected heroes at slots using _pendingSelectedHeroes via SpawnSelectedHeroesAtSlots.
  - Logs the wave start and delegates to _waveManager.StartWave(CurrentMap, CurrentWave, this).
- SpawnSelectedHeroesAtSlots
  - If waveMgr is null, returns.
  - Reads spawns from waveMgr.HeroSpawns; returns if null/empty.
  - Spawns up to min(spawns.Count, heroIds.Count) heroes.
  - For each heroId: resolves HeroDef, selects prefab (def.Prefab or fallback), logs warning if missing, instantiates at corresponding spawn point, initializes HeroController if present.
- ResolveHeroDef
  - Returns HeroDef from GameManager.Instance.HeroCatalogue.GetById(heroId) if catalogue exists.
- GetHeroPrefab
  - Returns def.Prefab if available; otherwise returns heroFallbackPrefab.
- SetSelectedHeroes
  - Stores a copy of provided heroIds into _pendingSelectedHeroes (null-safe).
- OnWaveCompleted
  - If not success: set state to WaveReward, show and populate wave reward UI, return.
  - If success and CurrentWave < MaxWaves: show WaveReward UI and populate; otherwise switch to MapReward and show/populate map reward UI.
- NextWave
  - Increments CurrentWave, sets GameState to MapPhase, and starts the next wave via StartWave().

4) Constraints & Failure Modes
- Precondition dependencies (must be set externally)
  - CurrentMap is sourced from GameManager.Instance.pendingMap (not null-guarded here).
  - waveRewardUI, mapRewardUI, selectHeroUI must be assigned to avoid null reference on UI calls.
  - _mapInstancedPrefab must be non-null when StartWave runs; otherwise GetComponentInChildren<WaveManager>() may fail.
  - CurrentMap.mapPrefab may be null; code logs a warning but continues.
- Potential null-ref / missing data risks
  - HideUI assumes waveRewardUI/mapRewardUI are non-null.
  - StartWave assumes _mapInstancedPrefab exists; no null-check before retrieving WaveManager.
  - GetHeroPrefab may return null if both def.Prefab and heroFallbackPrefab are null; spawning will skip with a warning.
  - OnWaveCompleted accesses rewards without null checks beyond the provided parameters; assumes rewards object is valid when called.
- Performance/allocations
  - SpawnSelectedHeroesAtSlots creates heroes iteratively; relies on provided spawn list and hero IDs.

5) Example
- Not applicable: no derivable minimal usage snippet due to missing public singleton access and runtime setup details in this file.

6) Unknowns
- Exact structure of MapDef, HeroDef, WaveManager, WaveRewards, MapRewardUI, WaveRewardUI beyond usage here.
- How HeroDef.Prefab is defined (field name and type) beyond its usage.
- Exact lifecycle guarantees for when PrepareMap is called relative to GameManager state.
- How _heroById is populated (declared but unused in this file).
- Whether heroFallbackPrefab is always assigned in the editor or potentially null.

Note: This documentation reflects the public surface and explicit behavior visible in the provided MapManager.cs.
