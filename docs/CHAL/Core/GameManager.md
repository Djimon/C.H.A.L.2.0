# CHAL.Core.GameManager

_Automatically generated/updated from `Assets/src/Core/GameManager.cs`._

1) Purpose
- Defines the GameState enum and the GameManager singleton that coordinates core game lifecycle in CHAL.Core.
- Manages scene navigation, player profile persistence, inventory routing/mapping, and research system integration.
- Exposes public API for starting/continuing games, saving, inventory/template resolution, and simple game-flow controls.

2) Public API
- Namespace: CHAL.Core

- Types
  - Public enum GameState
    - MainMenu
    - MapPhase     // Spieler kämpft auf einer Map
    - WaveReward   // kleiner Reward-Screen
    - MapReward    // großer Reward-Screen
    - Hideout

  - Public class GameManager : MonoBehaviour
    - Public properties/fields
      - public HeroDef starterHero { get; private set; } // serialized
      - public HeroCatalog HeroCatalogue => heroCatalog
      - public static GameManager Instance { get; private set; }
      - public PlayerProfile Profile { get; private set; }
      - public UnluckyProtection Unlucky { get; private set; }
      - public MapDef pendingMap { get; private set; }
      - public InventoryDomain Inventory { get; private set; }
      - public bool InventoryReady { get; private set; }
      - public ResearchService researchService { get; private set; }
      - public ResearchUnlockRegistry ResearchUnlocks { get; private set; }
      - public ResearchEventBridge ResearchBridge { get; private set; }
      - public GameBalanceConfig Config { get; }
      - public GameState CurrentState { get; private set; } // initial MainMenu

    - Public methods
      - public void SaveGame()
      - public void ResetProfile()
      - public InventoryInstance EnsureInstance(string instanceId, PlayerInventoryType templateTypeId)
      - public void MapDomainToProfile()
      - public void MapProfileToDomain()
      - public bool TryResolveByItemId(string itemId, out PlayerInventoryType type, out string instanceId)
      - public string InstanceIdFor(PlayerInventoryType t)
      - public InventoryDef GetTemplate(PlayerInventoryType typeId)
      - public void GoToMainMenu()
      - public void ExitToHideout()
      - public void TestInitInventory()
      - public void InitResearch(bool loadExisting)
      - public static void Quit()
      - public InventoryInstance? (no other public overloads listed)

- Notes
  - Config getter lazily loads "Config/GameBalanceConfig" from Resources if needed.
  - Many routing/mapping helpers are public (e.g., MapDomainToProfile/MapProfileToDomain, GetTemplate, EnsureInstance, TryResolveByItemId, InstanceIdFor).

3) Key Behavior & Side Effects
- Startup and singleton
  - Awake enforces singleton (Destroy duplicates); enables DontDestroyOnLoad; loads Profile via SaveSystem.Load(); creates InputManager if missing; initializes UnluckyProtection if not present.
  - Start preloads ItemRegistry.

- Game flow
  - StartNewGame(profile)
    - Sets Profile, ensures Inventory exists, builds inventories from folder, builds routing maps, maps profile to domain, marks InventoryReady, initializes research (loadExisting=false), saves, sets state to Hideout, loads 03_Hideout.
  - ContinueGame()
    - Requires non-null Profile; rebuilds inventories/maps, maps profile to domain, InventoryReady, initializes research (loadExisting=true), ensures starter hero unlocked, sets state to Hideout, loads 03_Hideout.
  - GoToMainMenu()
    - Saves, sets state MainMenu, loads 01_MainMenu.
  - ExitToHideout()
    - Sets state Hideout, loads 03_Hideout.
  - StartMap(sceneName, selectedMap)
    - (internal) sets pendingMap, state MapPhase, loads specified scene.

- Inventory domain & routing
  - BuildPlayerInventoriesFromFolder()
    - Loads InventoryDef assets from data/Inventory; creates InventoryInstance per type except type all; registers in Inventory.
  - GetTemplate(typeId)
    - Caches and returns InventoryDef for given typeId by loading from data/Inventory; logs error and returns null if not found.
  - EnsureInstance(instanceId, templateTypeId)
    - Creates domain instance if missing using corresponding template; returns null on invalid inputs.
  - MapDomainToProfile()/MapProfileToDomain()
    - Syncs inventories between in-game domain and Profile.Inventories; uses instance IDs like "player_<type>".
  - ReadDomainAsDict()/TryFillDomainFrom(...)
    - Helpers to convert domain slot state to/from dictionary form for profile persistence.
  - Init and routing maps
    - BuildInventoryRoutingMaps(): builds mappings between string prefixes and types, and type to instanceId (prefix-based routing).

- Research system
  - InitResearch(loadExisting)
    - Ensures runtime container; creates services (ResearchService, ResearchUnlockRegistry, ResearchEventBridge); loads existing snapshot or initializes new one; wires up event handlers for saving/unlocking; initializes tree and unlocks.
  - EnsureResearchDefsLoaded()
    - Lazy-loads research tree and node definitions from Resources if needed.

- Persistence & lifecycle
  - OnApplicationQuit()
    - Saves Profile; saves research snapshot if present.
  - SaveGame()
    - Maps domain inventories to profile, then saves Profile.

- Misc utilities
  - TryResolveByItemId(itemId, out type, out instanceId)
    - Parses itemId prefix (before colon) to determine inventory type and instanceId; creates mapping if missing.
  - InstanceIdFor(type)
    - Returns or builds the canonical instanceId for a given inventory type.

4) Constraints & Failure Modes
- Null/missing data guards
  - ContinueGame() returns early if Profile is null.
  - EnsureInstance/GetTemplate guard against empty instance IDs and missing domain/templates; logs errors on failure.
  - GetTemplate returns null if no matching InventoryDef found.
  - StartNewGame assumes a valid profile; effects depend on caller.
- Inventory and domain consistency
  - BuildPlayerInventoriesFromFolder relies on Resources data; missing entries are skipped.
  - MapDomainToProfile/MapProfileToDomain skip null entries and empty IDs.
- Scene management
  - SceneManager.LoadScene calls appear in several flows; scene names must exist at runtime.
- Editor vs. runtime
  - Quit() uses UnityEditor.EditorApplication.isPlaying in Editor; otherwise Application.Quit.
- Runtime dependencies
  - Many flows assume InventoryDomain and Profile presence; null checks guard some code paths.
- Performance
  - Lazy loading of config and resources helps avoid upfront costs; caching templates improves repeated lookups.

5) Example
- Not derivable from this file beyond usage implied by public API (no concise, self-contained example to include without external context). Omitted.

6) Unknowns
- Details of InventoryDomain, InventoryInstance, ItemStack, and how exact item IDs map to definitions beyond the high-level routing.
- Behavior of SaveSystem, PlayerProfile structure, and exact shape of inventory dictionaries.
- Exact behaviors of the ResearchTree/ResearchNodeDef implementations and how milestones unlocks interact with UI flows.
- Any side effects from external systems not shown in this file (e.g., event buses, UI bindings, or scene setup steps outside 03_Hideout/01_MainMenu).
- Any concurrency considerations or threading model for save/load operations.

Notes
- Public surface area focuses on types and public members; internal/private details are described only where they impact public behavior or flows.
- Unity lifecycle: Awake handles singleton pattern and core initialization; Start preloads registries.

