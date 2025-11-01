# CHAL.Core.GameManager

_Automatically generated/updated from `Assets/src/Core/GameManager.cs`._

```text
Purpose
- Defines GameState enum and a singleton GameManager that coordinates initialization, state transitions, persistence, and scene navigation.
- Manages inventory routing/templating and maps between in-game inventory domain and persistent player profile.
- Integrates research systems (ResearchService, ResearchUnlockRegistry, ResearchEventBridge) and provides access to game config.

```

```csharp
// Public API surface (from this file)
```

Public API
- Namespace
  - CHAL.Core

- Types
  - public enum GameState
    - MainMenu
    - MapPhase
    - WaveReward
    - MapReward
    - Hideout

  - public class GameManager : MonoBehaviour
    - Public fields/properties
      - [SerializeField] public HeroDef starterHero { get; private set; }
      - [SerializeField] private HeroCatalog heroCatalog;
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
      - public GameState CurrentState { get; private set; } = GameState.MainMenu;

    - Public methods
      - public void SaveGame()
      - public void ResetProfile()
      - public void SetState(GameState newState)
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
      - public string InstanceIdFor(PlayerInventoryType t)
      - public bool TryResolveByItemId(string itemId, out PlayerInventoryType type, out string instanceId)

```

```text
Key Behavior & Side Effects
- Singleton and persistence
  - Ensures a single GameManager instance; persists via DontDestroyOnLoad.
  - Loads Profile via SaveSystem.Load() in Awake; creates UnluckyProtection if missing.

- Startup flow
  - Start(): preloads all item registries (ItemRegistry.Instance.TriggerInstance()).

- Game lifecycle
  - StartNewGame(profile):
    - Assigns Profile; creates InventoryDomain if null; builds inventories and routing maps; maps profile to domain; marks InventoryReady; initializes research (loadExisting: false); saves; transitions to Hideout; loads 03_Hideout.
  - ContinueGame():
    - Validates Profile; builds inventories/maps; maps profile to domain; InventoryReady; initializes research (loadExisting: true); ensures starter hero unlocked; transitions to Hideout; loads 03_Hideout.
  - GoToMainMenu()/ExitToHideout(): save/persist, switch state, load respective scenes.

- Inventory management
  - BuildPlayerInventoriesFromFolder(): loads InventoryDef assets from Resources/data/Inventory and registers instances per type (excluding type all).
  - GetTemplate(typeId): loads and caches InventoryDef templates from Resources/data/Inventory.
  - EnsureInstance(instanceId, type): creates and registers an InventoryInstance if missing; returns null on error.
  - MapDomainToProfile()/MapProfileToDomain(): serialize/deserialize inventories between in-game domain and PlayerProfile.
  - ReadDomainAsDict(instanceId)/TryFillDomainFrom(source, instanceId): internal helpers for Inventory domain <-> profile translation.
  - StartMap(sceneName, selectedMap): records pendingMap, sets MapPhase, loads scene.

- Inventory routing
  - BuildInventoryRoutingMaps(): builds mappings between string prefixes, enum types, and instance IDs for inventories.
  - TryResolveByItemId(itemId, out type, out instanceId): resolves itemId prefix to inventory type and instanceId; creates mapping if missing.
  - InstanceIdFor(type): returns or builds the instanceId for a given type.

- Research integration
  - InitResearch(loadExisting): ensures runtime state, creates services/registry/bridge, loads existing snapshot or initializes fresh state, wires OnNodeCompleted to save progress.
  - EnsureResearchDefsLoaded(): loads research tree/nodes from Resources if not set.

- Lifecycle/utility
  - OnApplicationQuit(): saves profile and research snapshot.
  - Quit(): saves profile and exits (Editor vs runtime).

- Helpers and guards
  - GetTemplate/GetInstance/Map* methods include null/empty checks and log warnings on missing data.
  - ReadDomainAsDict/TryFillDomainFrom guard against null Inventory, empty instance IDs, and zero-slot inventories.

```

```text
Constraints & Failure Modes
- Null checks and guards
  - Many public methods return early if Inventory/Profile are null (e.g., MapDomainToProfile, MapProfileToDomain).
  - EnsureInstance returns null if instanceId is empty or InventoryDomain missing.

- Resource loading
  - GetTemplate/InitResearch rely on Resources.Load*; missing assets will log errors or fall back (e.g., GetTemplate logs if no matching InventoryDef).

- On-demand creation
  - If an inventory slot set is requested but the instance has no slots, TryFillDomainFrom will attempt to create the instance on-demand by parsing the suffix from the instanceId.

- Scene management
  - StartMap/StartNewGame/ContinueGame load scenes by name; failures rely on Unity scene lifecycle (no explicit fallbacks shown here).

- Editor vs runtime behavior
  - Quit method uses UNITY_EDITOR conditional compilation to stop Play mode in editor; otherwise Application.Quit().

- Side effects
  - StartNewGame/ContinueGame perform multiple state mutations (Profile, Inventory, mapping, research) and save at key points.
  - InitResearch wires an event to persist research snapshots on node completion.

- Performance hints
  - GetTemplate caches templates in _inventoryTemplates after first load per typeId.
  - BuildInventoryRoutingMaps enumerates all PlayerInventoryType values and creates mappings upfront.

- Unknown external behavior
  - Details of InventoryDomain, InventoryInstance, ResearchService, and related types are not defined in this file; their behaviors and thread-safety are not specified here.

```

```text
Example
- Minimal usage example (derived from file’s public surface)

```csharp
// Start a new game with an existing profile
GameManager.Instance.StartNewGame(existingProfile);
```

```

```text
Unknowns
- Exact implementations and contracts of:
  - InventoryDomain, InventoryDef, InventoryInstance, ItemStack, Read/Write semantics in MapDomainToProfile/MapProfileToDomain
  - ResearchService, ResearchUnlockRegistry, ResearchEventBridge, and how research state is structured
  - Profile and how inventories are represented (Profile.Inventories, inv.invID, FromDictionary/ToDictionary)
  - ItemRegistry, UnluckyProtection, InputManager, HeroDef, HeroCatalog, MapDef, and related resources

- Runtime behavior not visible here:
  - Threading/async concerns, if any, within GoToMainMenu, StartNewGame, etc.
  - Exact scene setup/names beyond those referenced (01_MainMenu, 03_Hideout)
  - Data formats stored in SaveSystem for profiles and research
```
