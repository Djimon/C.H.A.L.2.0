# CHAL.Core.GameState

_Automatically generated/updated from `Assets/src/Core/GameManager.cs`._

```text
1) Purpose
- Defines the GameState enum and a GameManager MonoBehaviour that coordinates game flow, inventory, and research, plus scene transitions and saving/loading.
- Centralizes access via a singleton (Instance) and exposes public surface for inventory, profile, and research subsystems.

2) Public API
- Namespace: CHAL.Core
- Types
  - public enum GameState
    - MainMenu
    - MapPhase
    - WaveReward
    - MapReward
    - Hideout
  - public class GameManager : MonoBehaviour
    - public static GameManager Instance { get; private set; }
    - public PlayerProfile Profile { get; private set; }
    - public UnluckyProtection Unlucky { get; private set; }
    - public MapDef pendingMap { get; private set; }
    - public InventoryDomain Inventory { get; private set; }
    - public bool InventoryReady { get; private set; }
    - public HeroDef starterHero { get; private set; }
    - public HeroCatalog HeroCatalogue => heroCatalog;
    - public ResearchService researchService { get; private set; }
    - public ResearchUnlockRegistry ResearchUnlocks { get; private set; }
    - public ResearchEventBridge ResearchBridge { get; private set; }
    - public GameBalanceConfig Config { get; }
    - public GameState CurrentState { get; private set; }

- Public/visible methods and surface
  - internal void StartNewGame(PlayerProfile profile)
  - public void GoToMainMenu()
  - public void ExitToHideout()
  - internal void ContinueGame()
  - internal static void Quit()
  - public void TestInitInventory()
  - internal void StartMap(string sceneName, MapDef selectedMap)

  - public InventoryDef GetTemplate(PlayerInventoryType typeId)
  - public InventoryInstance EnsureInstance(string instanceId, PlayerInventoryType templateTypeId)
  - public void MapDomainToProfile()
  - public void MapProfileToDomain()
  - public bool TryResolveByItemId(string itemId, out PlayerInventoryType type, out string instanceId)
  - public string InstanceIdFor(PlayerInventoryType t)
  - public void InitResearch(bool loadExisting)

Notes:
- GetTemplate, EnsureInstance, TryResolveByItemId, InstanceIdFor, InitResearch are part of the public API surface.
- The class contains internal/private helpers not listed here (e.g., BuildPlayerInventoriesFromFolder, ReadDomainAsDict, TryFillDomainFrom, BuildInventoryRoutingMaps, EnsureResearchDefsLoaded, etc.).

3) Key Behavior & Side Effects
- Awake
  - Enforces singleton; persists GameObject across scenes.
  - Loads save profile via SaveSystem.Load() into Profile.
  - Locates or creates an InputManager in the scene.
  - Initializes UnluckyProtection if not present.

- Start
  - Preloads registries via ItemRegistry.Instance.TriggerInstance().

- StartNewGame(profile)
  - Sets Profile to provided profile.
  - Creates InventoryDomain if needed.
  - Builds player inventories from data folder and routing maps.
  - Maps profile inventories to domain and marks InventoryReady = true.
  - Initializes research (loadExisting = false).
  - Saves game and transitions to Hideout scene.

- GoToMainMenu
  - Saves game, switches state to MainMenu, loads 01_MainMenu.

- ExitToHideout
  - Switches to Hideout state and loads 03_Hideout.

- ContinueGame
  - Validates existing Profile.
  - Builds inventories and routing maps, maps profile to domain, InventoryReady = true.
  - Initializes research (loadExisting = true).
  - Ensures starter hero is unlocked and transitions to Hideout.

- InitResearch(loadExisting)
  - Ensures research defs loaded.
  - Ensures Profile.ResearchRuntime exists.
  - Creates service/registry/bridge instances as needed.
  - If loading existing, restores from saved snapshot; otherwise resets runtime state and saves a fresh snapshot.
  - Initializes service from tree and rebuilds unlocks.
  - Subscribes to OnNodeCompleted to apply unlocks and persist snapshots.

- MapDomainToProfile / MapProfileToDomain
  - Synchronizes inventories between in-game domain and saved profile inventories.
  - ReadDomainAsDict builds a dictionary of item counts per instance.
  - TryFillDomainFrom fills domain from a saved dictionary; can auto-create instances on demand.

- OnApplicationQuit
  - Persists profile and, if present, a research snapshot.

- StartMap(sceneName, selectedMap)
  - Sets pendingMap, transitions to MapPhase, loads the specified scene.

- Inventory handling
  - BuildPlayerInventoriesFromFolder loads InventoryDef assets under data/Inventory, creates per-type instances, and registers them.
  - GetTemplate loads and caches InventoryDef templates; returns null with error if not found.
  - EnsureInstance creates or returns an existing InventoryInstance for a given id and template type.
  - TryResolveByItemId parses item IDs to determine the inventory type and concrete instanceId via routing maps.
  - InstanceIdFor resolves or builds the instanceId for a given type.
  - BuildInventoryRoutingMaps populates mapping between prefixes, types, and instance IDs for runtime resolution.

- Debug/logging
  - Uses DebugManager for diagnostic messages (Dev level and other categories).

4) Constraints & Failure Modes
- Singleton enforcement in Awake: duplicates are destroyed to preserve a single instance.
- Profile loading may result in null; code guards against nulls in many paths.
- GetTemplate/logging: if no matching InventoryDef is found, returns null and logs an error.
- EnsureInstance/Fill logic: handles missing InventoryDomain or empty instance IDs with early returns.
- StartNewGame continues even if some subsystems are uninitialized, but assumes resources exist.
- InitResearch defers to EnsureResearchDefsLoaded; loads resources from Resources folder when missing.
- Quit uses UnityEditor path when in editor; otherwise calls Application.Quit.
- OnApplicationQuit attempts to save profile and research safely; exceptions are not surfaced.

5) Example
- Not applicable (no explicit minimal usage example derivable from this file alone).

6) Unknowns
- Detailed behavior of external types (InventoryDomain, SaveSystem, ItemRegistry, Profile types, ResearchTreeDef, etc.) is not defined here.
- Exact structure of data under data/Inventory and data/Research, and the contents of HeroCatalog/StarterHero, are not shown.
- The behavior of DebugManager, Health/UnluckyProtection, and the specifics of scene setup (03_Hideout, 01_MainMenu) are outside this file.
- Threading/async implications beyond Unity main thread expectations are not specified.
- Any side effects from serializable field initialization or Unity serialization order are not detailed beyond code usage.
