# CHAL.Core.GameState

_Automatically generated/updated from `Assets/src/Core/GameManager.cs`._

1) Purpose
- Defines GameState enum and a central GameManager (Unity MonoBehaviour) that coordinates core game systems (inventory, profile, scene flow, and research).
- Provides helper APIs for inventory templates/instances, profile-domain mapping, and starting/continuing games.
- Bridges scene navigation with game state, save/load, and runtime research setup.

2) Public API
- Namespace: CHAL.Core
- Types
  - public enum GameState
    - MainMenu
    - MapPhase     // Player fights on a map
    - WaveReward   // small reward screen
    - MapReward    // large reward screen
    - Hideout
  - public class GameManager : MonoBehaviour
    - Public surface
      - public static GameManager Instance { get; private set; }
      - public PlayerProfile Profile { get; private set; }
      - public UnluckyProtection Unlucky { get; private set; }
      - public MapDef pendingMap { get; private set; }
      - public InventoryDomain Inventory { get; private set; }
      - public bool InventoryReady { get; private set; }
      - public HeroCatalog HeroCatalogue => heroCatalog;
      - public HeroDef starterHero { get; private set; }
      - public ResearchService researchService { get; private set; }
      - public ResearchUnlockRegistry ResearchUnlocks { get; private set; }
      - public ResearchEventBridge ResearchBridge { get; private set; }
      - public GameBalanceConfig Config
        - Getter loads from Resources if needed; returns the config
      - public GameState CurrentState { get; private set; } // initial value: GameState.MainMenu
    - Internal/public methods
      - internal void StartNewGame(PlayerProfile profile)
      - internal void StartMap(string sceneName, MapDef selectedMap)
      - internal void ContinueGame()
      - internal static void Quit()
      - internal GameManager() (implicit constructor; not shown)
      - public void SaveGame()
      - public void ResetProfile()
      - public void SetState(GameState newState)
      - public void GoToMainMenu()
      - public void ExitToHideout()
      - public void TestInitInventory()
      - public InventoryDef GetTemplate(PlayerInventoryType typeId)
      - public InventoryInstance EnsureInstance(string instanceId, PlayerInventoryType templateTypeId)
      - public void MapDomainToProfile()
      - public void MapProfileToDomain()
      - public bool TryResolveByItemId(string itemId, out PlayerInventoryType type, out string instanceId)
      - public string InstanceIdFor(PlayerInventoryType t)
      - public void InitResearch(bool loadExisting)
    - Unity lifecycle (non-public)
      - private void Awake() // singleton setup, profile load, input manager creation, Unlucky protection
      - private void Start() // preload registries
      - private void OnApplicationQuit() // persist profile and research snapshot
  - Notes
    - The file uses a number of internal/private helpers (not exposed in the Public API section) for inventory and research wiring.

3) Key Behavior & Side Effects
- Awake
  - Enforces singleton: destroys duplicates, assigns Instance, marks DontDestroyOnLoad.
  - Loads PlayerProfile save if available.
  - Logs XP-per-level from Config.
  - Ensures an InputManager exists (searches for one; creates if missing).
  - Initializes UnluckyProtection if not present.
- Start
  - Triggers ItemRegistry to preload.
- StartNewGame(profile)
  - Sets Profile to the provided profile.
  - Ensures Inventory domain exists.
  - Builds inventories from folder resources, builds routing maps, maps profile to domain.
  - Sets InventoryReady = true.
  - Initializes research (loadExisting = false).
  - Saves game, sets state to Hideout, loads the Hideout scene.
- ContinueGame()
  - Requires a non-null Profile; otherwise logs a warning and aborts.
  - Rebuilds inventories from folder, routing maps; maps profile to domain.
  - Sets InventoryReady = true.
  - Initializes research (loadExisting = true).
  - Ensures starter hero is unlocked, then goes to Hideout scene.
- SaveGame()
  - Persists domain inventory state to the profile, then saves the profile.
- MapDomainToProfile()
  - Transfers current domain inventories into the profile by reading domain slots and converting to a dictionary per inventory.
- MapProfileToDomain()
  - Applies profile inventories into the domain by converting profile data to dictionaries and filling domain state.
- GetTemplate(typeId)
  - Retrieves InventoryDef template for a given typeId; caches results; loads from Resources if needed.
- EnsureInstance(instanceId, templateTypeId)
  - Creates an InventoryInstance on demand if missing, using the template; returns null on invalid inputs.
- TryResolveByItemId(itemId, out type, out instanceId)
  - Parses a colon-delimited itemId; resolves a prefix to a PlayerInventoryType and computes the corresponding instanceId.
- InstanceIdFor(t)
  - Returns the instanceId for a given inventory type, creating it if missing.
- InitResearch(loadExisting)
  - Ensures research defs are loaded; ensures a ResearchRuntime exists; instantiates services/bridges; loads existing snapshot or resets runtime state; wires events for unlocks and saving.
- StartMap(sceneName, selectedMap)
  - Sets pendingMap, updates state to MapPhase, and loads the specified scene.
- OnApplicationQuit
  - Saves profile; if research runtime exists, builds and saves a research snapshot.
- BuildPlayerInventoriesFromFolder
  - Loads InventoryDef assets from data/Inventory, creates per-type instances (excluding type all), and registers them into the Inventory domain.
- BuildInventoryRoutingMaps
  - Populates mappings from inventory type prefixes (lowercase) to enum values and from types to default instanceIds ("player_<type>").
- ReadDomainAsDict(instanceId)
  - Reads current domain slots for an instance and builds a dictionary of itemId -> count for non-empty stacks.
- TryFillDomainFrom(source, instanceId)
  - Clears existing domain slots; ensures an instance exists (creating on-demand if possible); refills from source using Inventory.TryAdd.
- StartMap
  - See StartMap above (internal).
- Other side effects
  - Debug/Logging calls throughout for state transitions, inventory actions, and research events.
  - SceneManagement.LoadScene calls trigger navigation between MainMenu, Hideout, MapPhase, etc.

4) Constraints & Failure Modes
- Null checks and guards:
  - Profile can be null in several flows (ContinueGame warns and aborts).
  - GetTemplate/logging when template not found; returns null.
  - EnsureInstance returns null on invalid input or missing domain.
  - TryResolveByItemId returns false for empty/invalid IDs.
- Resource dependency:
  - Config and templates loaded from Resources; missing assets may cause nulls or errors.
- Threading/async:
  - No explicit async/threading; operations are synchronous in Unity main thread.
- Scene loading:
  - Scene names are hard-coded (e.g., "03_Hideout", "01_MainMenu"); no error handling shown if scene is missing.
- Persistence:
  - Quit path and OnApplicationQuit attempt to save; exceptions in Quit are caught in the public Quit path, but not everywhere.
- Performance:
  - On-demand instance creation via EnsureInstance and TryFillDomainFrom may trigger runtime template lookups.

5) Example
- Not derivable from this file in a minimal, self-contained example. (No explicit code example provided.)

6) Unknowns
- External types and their behavior (e.g., InventoryDomain, InventoryDef, InventoryInstance, PlayerInventoryType, MapDef, ResearchTreeDef, ResearchNodeDef, etc.) are referenced but not defined in this file.
- Details of SaveSystem, DebugManager, ItemStack, and the various manager/services (ResearchService, ResearchUnlockRegistry, ResearchEventBridge) are not shown here.
- Exact serialization format, resource layout, and the full lifecycle of inventory/research data beyond what is shown.
- Any side effects from other components not visible in this file (e.g., UI updates, event bus behavior, or additional scene initialization).

