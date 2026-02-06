# Assets/src/Core/GameManager.cs

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
      - `public StatisticsService Stats { get; private set; }`
      - `public UnluckyProtection Unlucky { get; private set; }`
      - `public MapDef pendingMap { get; private set; }`
      - `public InventoryDomain Inventory { get; private set; }`
      - `public bool InventoryReady { get; private set; }`
      - `public CodexService codexService { get; private set; }`
      - `public CodexUnlockRegistry codexUnlocks { get; private set; }`
      - `public GearModRegistry gearModRegistry;`
      - `public GearRoller gearRoller { get; private set; }`
      - `public GameBalanceConfig BalanceConfig { get; }`
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
      - `public void InitCodex(bool loadExisting)`

# Key Behavior & Side Effects
- Singleton pattern ensures only one instance of `GameManager` exists.
- Game state transitions are logged and can trigger UI updates.
- Game data is saved on quitting and when transitioning between states.
- Inventory and research systems are initialized and managed within the game lifecycle.
- Player inventories are built from resources and mapped to the profile.
- Gear instances and skill module instances are registered and managed.

# Constraints & Failure Modes
- If `Profile` is null, certain operations (like continuing a game) will not proceed.
- Inventory and research systems require proper initialization to function correctly.
- Error handling is present for missing inventory definitions and instance creation failures.
- If `instanceId` is null or empty in `EnsureInstance`, an error is logged, and the method returns null.
- If `gear` or `inst` is null in registration methods, a warning is logged.

# Example
```csharp
GameManager.Instance.GoToMainMenu();
```

# Unknowns
- The exact structure and contents of `PlayerProfile`, `InventoryDomain`, `ResearchService`, and other referenced types are not defined in this file.
