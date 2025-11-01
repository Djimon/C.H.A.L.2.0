# CHAL.Core.GameManager

_Automatically generated/updated from `Assets/src/Core/GameManager.cs`._

```csharp
// Documentation generated from Assets/src/Core/GameManager.cs
```

1) Purpose
- Defines GameState and GameManager, centralizing game orchestration (profile, inventory, research, scene transitions).
- Ensures a singleton GameManager instance across scenes and handles persistence/loading of profile and research state.
- Provides public API for game flow (start new game, continue, navigate menus, start maps) and inventory/research helpers.

2) Public API
- Namespace/module
  - CHAL.Core

- Types
  - public enum GameState
    - MainMenu
    - MapPhase      // Spieler kämpft auf einer Map
    - WaveReward    // kleiner Reward-Screen
    - MapReward     // großer Reward-Screen
    - Hideout

  - public class GameManager : MonoBehaviour
    - Public properties
      - public HeroDef starterHero { get; private set; } [SerializeField]
      - public HeroCatalog HeroCatalogue => heroCatalog;
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
      - private void EnsureResearchDefsLoaded() // not public; internal/public listed above as applicable

4) Key Behavior & Side Effects
- Awake
  - Enforces singleton; persists GameObject across scenes.
  - Loads profile via SaveSystem.Load() (Profile may be null).
  - Finds or creates an InputManager (auto-instantiates if missing) and parents it under the GameManager.
  - Initializes UnluckyProtection if not already set.
- Start
  - Preloads item/registry data via ItemRegistry.Instance.TriggerInstance().
- StartNewGame(profile)
  - Sets Profile to the provided profile.
  - Ensures Inventory domain exists.
  - Builds player inventories from data folder, builds routing maps, maps profile to domain, marks InventoryReady.
  - Initializes research (loadExisting=false).
  - Saves game state, switches to Hideout, loads 03_Hideout.
- GoToMainMenu
  - Saves game, sets state to MainMenu, loads 01_MainMenu.
- ExitToHideout
  - Sets state to Hideout, loads 03_Hideout.
- ContinueGame
  - Validates Profile exists; builds inventories, routing maps, maps profile to domain, InventoryReady.
  - Initializes research with loadExisting=true.
  - Ensures starter hero is unlocked (starterHero or default TestHero).
  - Switches to Hideout and loads 03_Hideout.
- Quit
  - Saves profile; depending on environment, either triggers Unity Editor stop or Application.Quit().
- StartMap(sceneName, selectedMap)
  - Stores pendingMap, updates state to MapPhase, loads the specified scene.
- OnApplicationQuit
  - Saves profile; if research runtime exists, saves a research snapshot.
- Inventory/Profile mapping
  - BuildPlayerInventoriesFromFolder loads InventoryDef assets under data/Inventory and creates InventoryInstance objects per type (excluding type all).
  - GetTemplate(typeId) caches and returns the InventoryDef for a given type.
  - EnsureInstance(instanceId, templateTypeId) creates and registers an InventoryInstance if missing.
  - MapDomainToProfile reads domain inventories, converts to profile inventories by dictionary mapping, and logs results.
  - MapProfileToDomain converts profile inventories to domain inventories; uses ReadDomainAsDict and TryFillDomainFrom for synchronization.
  - TryResolveByItemId parses item IDs to determine inventory type and instanceId via prefix mapping.
  - InstanceIdFor(t) resolves or builds a canonical instanceId for a given type.
  - InitResearch(loadExisting) wires up research services/registries, loads or resets snapshots, and subscribes to node-completion to persist snapshots.
  - EnsureResearchDefsLoaded loads the research tree and node definitions from Resources.
- Private helpers (flow, guards)
  - BuildInventoryRoutingMaps builds mapping between inventory type prefixes and instance IDs.
  - ReadDomainAsDict aggregates domain items into a dictionary by itemID -> count.
  - TryFillDomainFrom clears and refills domain slots from a source dictionary, creating instances on-demand if needed.

5) Example
- Not included (no minimal example derivable directly from file without external context).

6) Unknowns
- Exact shapes of InventoryDomain, InventoryDef, InventoryInstance, and how they behave beyond this file.
- How HeroDef starterHero interacts with the rest of the game (beyond being stored and used for unlocking starter hero).
- Behavior of other systems (Map, Loot, LootRewards, ResearchTree) beyond their usage here.
- Any runtime implications of scene load order and how camera/player state is restored when loading 03_Hideout or 01_MainMenu.
- Details of SaveSystem, Profile persistence format, and the exact data layout of inventories in data/Inventory.
- Any threading concerns or asynchronous loading behavior not evident in this file.
