# CHAL.Core.GameState

_Automatically generated/updated from `Assets/src/Core/GameManager.cs`._

1) Purpose
- Defines GameState enum and GameManager singleton that coordinates global game flow, state transitions, scene loading, and persistence.
- Bridges between domain models (inventory, profile, map, research) and Unity lifecycle, including inventory routing, map loading, and research initialization.
- Provides public surface to access and manipulate core systems (inventory, profile, research) and to drive high-level game actions (start, continue, save, quit).

2) Public API
- Namespace/Module
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
      - public GameBalanceConfig Config { get; }
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

    - Public methods
      - public InventoryDef GetTemplate(PlayerInventoryType typeId)
      - public InventoryInstance EnsureInstance(string instanceId, PlayerInventoryType templateTypeId)
      - public void MapDomainToProfile()
      - public void MapProfileToDomain()
      - public string InstanceIdFor(PlayerInventoryType t)
      - public void InitResearch(bool loadExisting)
      - public void SaveGame()
      - public void ResetProfile()
      - public void GoToMainMenu()
      - public void ExitToHideout()
      - public void ContinueGame()
      - public void TestInitInventory()
      - public void StartMap(string sceneName, MapDef selectedMap)
      - public void StartNewGame(PlayerProfile profile) [internal]
      - public InventoryInstance? (via EnsureInstance) and related inventory surface are exposed through EnsureInstance
      (Note: methods marked internal/private in code are omitted from the public API section.)

3) Key Behavior & Side Effects
- Lifecycle and initialization
  - Awake: enforces singleton, persists GameObject, loads profile (SaveSystem.Load), logs XP per level, ensures InputManager exists (creates if missing), initializes UnluckyProtection.
  - Start: preloads item registries via ItemRegistry.Instance.TriggerInstance().
- Saving and profile management
  - SaveGame: maps domain inventories to profile, then saves profile via SaveSystem.Save.
  - ResetProfile: preserves old name/colors, resets Profile to a new PlayerProfile and reinitializes basic attributes.
  - Quit path (static): saves profile, then exits play mode (Editor) or quits application.
- State and scene management
  - SetState(GameState): updates CurrentState and logs transition.
  - GoToMainMenu / ExitToHideout / ContinueGame / StartMap: perform state updates and load corresponding scenes.
  - OnApplicationQuit: persists profile and research snapshot if present.
- Inventory management
  - BuildPlayerInventoriesFromFolder: loads all InventoryDef assets from data/Inventory, creates and registers per-type instances (excluding type all).
  - GetTemplate: lazy-loads templates from resources into _inventoryTemplates.
  - EnsureInstance: creates and registers an inventory instance if missing, using a template def.
  - MapDomainToProfile / MapProfileToDomain: convert between Profile inventories and InventoryDomain, using instance IDs (player_<type>).
  - ReadDomainAsDict / TryFillDomainFrom: helpers to convert between domain stacks and dictionary representations; supports on-demand instance creation if needed.
  - BuildInventoryRoutingMaps: builds maps between prefix -> type and type -> instanceId for routing.
  - TryResolveByItemId: resolves an itemId like "<prefix>:<id>" to a type and instanceId.
  - InstanceIdFor: returns or computes the instanceId for a given inventory type.
- Inventory/Domain synchronization flows
  - Construct inventories from folder, then map to domain, then mark InventoryReady when setup complete.
  - During mapping, inventory slots are cleared and refilled from source dictionaries.
- Research initialization
  - InitResearch(loadExisting): ensures runtime container, creates services (ResearchService, ResearchUnlockRegistry, ResearchEventBridge), loads existing snapshot or resets progress, initializes tree/registry, and wires an event to persist research on node completion.
  - EnsureResearchDefsLoaded: loads research tree and nodes from Resources if not already loaded.
- Risky/exception paths
  - GetTemplate/Get/EnsureInstance guard against nulls; logs errors when templates not found or instance creation fails.
  - TryFillDomainFrom may auto-create an instance if slots are missing, parsing suffix from instanceId to derive inventory type.
  - Preprocessor guard for Quit: uses Editor quit path in Unity Editor.
  - Dependencies on Resources.Load/LoadAll; missing assets lead to null returns or empty collections.

- Unity lifecycle (explicit in this file)
  - Awake: singleton enforcement, initialization, and object setup.
  - Start: preloads registries.

4) Constraints & Failure Modes
- Null and existence guards
  - Many methods early-return if Inventory or Profile is null.
  - GetTemplate/EnsureInstance guard against missing data and log errors.
- Resource loading
  - Uses Resources.Load and Resources.LoadAll; absence leads to nulls or empty collections; explicit fallbacks/logs provided.
- Threading/async
  - No asynchronous operations in this file.
- On-demand creation
  - On missing slots, TryFillDomainFrom may attempt to create an instance on the fly using enum parsing; relies on naming convention and enum values.
- Editor vs runtime behavior
  - Quit path uses UnityEditor.EditorApplication.isPlaying in the editor vs Application.Quit at runtime.
- Performance hints
  - BuildPlayerInventoriesFromFolder uses Resources.LoadAll; may incur startup cost.
- Side effects
  - StartNewGame triggers scene load to Hideout and saves progress; ContinueGame includes saving starter state and unlocking starter hero.

5) Example
- Not derivable from this file in a self-contained minimal example without broader project context.

6) Unknowns
- Exact implementations and behaviors of InventoryDomain, InventoryDef, InventoryInstance, ItemStack, SaveSystem, Profile structures, and the specific contents of data/Inventory and data/Research assets.
- Details of scene content (03_Hideout, 01_MainMenu) and exact UI interactions.
- How StarterHero unlocking affects gameplay beyond the surface calls (e.g., race conditions, ordering with inventory/research).
