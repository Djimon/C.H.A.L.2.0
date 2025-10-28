# Assets/src/Core/GameManager.cs

_Automatic generated/updated._

```markdown
## Purpose
- Defines the `GameManager` class for managing game state, player profiles, and inventory systems.

## Public API
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

## Key Behavior & Side Effects
- Singleton pattern for `GameManager` ensures only one instance exists.
- Game state transitions managed via `SetState(GameState newState)`.
- Player profile loading and saving handled in `Awake()`, `SaveGame()`, and `OnApplicationQuit()`.
- Inventory and research systems initialized in `StartNewGame()` and `ContinueGame()`.
- Scene management through `SceneManager.LoadScene()` for different game states.

## Constraints & Failure Modes
- Handles null checks for `Profile` and `Inventory` in various methods.
- Logs warnings/errors for missing data or invalid operations.
- Uses `DontDestroyOnLoad(gameObject)` to persist the `GameManager` across scenes.

## Example
```csharp
GameManager.Instance.StartNewGame(new PlayerProfile());
```

## Unknowns
- Specific implementations of `SaveSystem`, `InventoryDomain`, `ResearchService`, and other referenced classes are not defined in this file.
```
