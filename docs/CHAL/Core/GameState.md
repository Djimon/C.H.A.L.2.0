# CHAL.Core.GameState

_Automatically generated/updated from `Assets/src/Core/GameManager.cs`._

# Purpose
- Manages the game state and handles game-related logic.

# Public API
- Namespace: `CHAL.Core`
- Types
  - `public class GameManager : MonoBehaviour`
    - Public fields/properties:
      - `public HeroDef starterHero { get; private set; }`
      - `public HeroCatalog HeroCatalogue => heroCatalog;`
      - `public static GameManager Instance { get; private set; }`
      - `public PlayerProfile Profile { get; private set; }`
      - `public UnluckyProtection Unlucky { get; private set; }`
      - `public MapDef pendingMap { get; private set; }`
      - `public InventoryDomain Inventory { get; private set; }`
      - `public bool InventoryReady { get; private set; }`
      - `public ResearchService researchService { get; private set; }`
      - `public ResearchUnlockRegistry ResearchUnlocks { get; private set; }`
      - `public ResearchEventBridge ResearchBridge { get; private set; }`
      - `public GameBalanceConfig Config { get; }`
      - `public GameState CurrentState { get; private set; }`
    - Public methods:
      - `public void SaveGame()`
      - `public void ResetProfile()`
      - `public void SetState(GameState newState)`
      - `public void GoToMainMenu()`
      - `public void ExitToHideout()`
      - `internal void ContinueGame()`
      - `internal static void Quit()`
      - `public void TestInitInventory()`
      - `internal void StartMap(string sceneName, MapDef selectedMap)`
      - `public InventoryDef GetTemplate(PlayerInventoryType typeId)`
      - `public InventoryInstance EnsureInstance(string instanceId, PlayerInventoryType templateTypeId)`
      - `public void MapDomainToProfile()`
      - `public void MapProfileToDomain()`
      - `public bool TryResolveByItemId(string itemId, out PlayerInventoryType type, out string instanceId)`
      - `public string InstanceIdFor(PlayerInventoryType t)`
      - `public void InitResearch(bool loadExisting)`

# Key Behavior & Side Effects
- Singleton pattern for `GameManager` ensures only one instance exists.
- Loads player profile from persistent storage on `Awake`.
- Initializes input manager if not present.
- Saves game state on quitting and transitions between game states.
- Handles inventory and research initialization and mapping between domain and profile.

# Constraints & Failure Modes
- If `Profile` is null, methods that depend on it will not execute properly.
- Inventory and research systems require proper initialization to function.
- Error handling is minimal; some methods log errors but do not throw exceptions.

# Example
```csharp
GameManager.Instance.GoToMainMenu();
```

# Unknowns
- The exact structure and contents of `PlayerProfile`, `InventoryDomain`, `ResearchService`, and other referenced types are not defined in this file.

