# CHAL.Core.GameManager

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
      - `internal void StartNewGame(PlayerProfile profile)`
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
- Singleton pattern ensures only one instance of `GameManager` exists.
- Loads player profile from persistent storage on `Awake`.
- Initializes input manager if not present.
- Saves game state on quitting and when transitioning between states.
- Transitions between game states and loads corresponding scenes.

# Constraints & Failure Modes
- If `Profile` is null, methods that depend on it (e.g., `ContinueGame`) will log a warning and return early.
- Inventory and research systems require proper initialization; failure to do so may lead to null references.
- Scene loading is dependent on valid scene names.

# Example
```csharp
GameManager.Instance.GoToMainMenu();
```

# Unknowns
- The exact structure and contents of `PlayerProfile`, `InventoryDomain`, `ResearchTreeDef`, and other referenced types cannot be determined from this file.

