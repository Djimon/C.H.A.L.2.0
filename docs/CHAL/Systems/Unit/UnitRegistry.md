# CHAL.Systems.Unit.UnitRegistry

_Automatically generated/updated from `Assets/src/Systems/Unit/UnitRegistry.cs`._

1) Purpose
- Defines a singleton UnitRegistry (ScriptableObject) that loads unit definitions from Resources and provides lookups by ID.
- Maintains internal dictionaries: HeroDef by HeroId and EnemyDef by enemyId.
- Exposes public API to retrieve heroes/enemies and to enumerate all IDs and collections.  
- Note: [CreateAssetMenu] attribute is present in comments (not active).

2) Public API
- Namespace/module: CHAL.Systems.Unit

- Types
  - public sealed class UnitRegistry : ScriptableObject
    - Public fields/properties
      - public static UnitRegistry Instance
        - Lazy-initializes a runtime instance via CreateInstance<UnitRegistry>() and calls Reload()
    - Private fields
      - private readonly Dictionary<string, HeroDef> _HeroById = new();
      - private readonly Dictionary<string, EnemyDef> _EnemyById = new();
    - Public methods
      - public void Reload()
        - Clears both dictionaries
        - Loads heroes: Resources.LoadAll<HeroDef>("data/Heroes")
          - Skips entries with empty/whitespace HeroId (logs a warning)
          - Skips duplicates (logs a warning)
          - Logs each loaded HeroId and stores in _HeroById
          - Logs total loaded heroes
        - Loads enemies: Resources.LoadAll<EnemyDef>("data/Enemies")
          - Skips entries with empty/whitespace enemyId (logs a warning)
          - Skips duplicates (logs a warning)
          - Logs each loaded enemyId and stores in _EnemyById
          - Logs total loaded enemies
        - Logs total enemy count (enemycount) after loading
      - public HeroDef GetHeroById(string id)
        - Return the corresponding HeroDef or null if not found
      - public EnemyDef GetEnemyByID(string id)
        - Return the corresponding EnemyDef or null if not found
      - public IEnumerable<string> GetAllHeroIds()
        - Returns _HeroById.Keys
      - public IEnumerable<string> GetAllEnemyIds()
        - Returns _EnemyById.Keys
      - public IEnumerable<HeroDef> GetAllHeroes()
        - Returns _HeroById.Values
      - public IEnumerable<EnemyDef> GetAllEnemies()
        - Returns _EnemyById.Values
#if UNITY_EDITOR
        - Editor-only: EditorAutoReload() with [UnityEditor.InitializeOnLoadMethod]
          - On load (not playing), triggers Instance?.Reload()
#endif

3) Key Behavior & Side Effects
- Instance creation
  - First access to UnitRegistry.Instance creates a new runtime instance and immediately calls Reload.
- Reload workflow
  - Clears existing caches
  - Loads HeroDef assets from data/Heroes in Resources
  - Validates HeroDef.HeroId (non-empty) and checks for duplicates
  - Logs per-item loads and a final hero count
  - Loads EnemyDef assets from data/Enemies in Resources
  - Validates EnemyDef.enemyId (non-empty) and checks for duplicates
  - Logs per-item loads and a final enemy count
- Lookup behavior
  - GetHeroById/GetEnemyByID return corresponding definitions or null if not present
- Editor behavior
  - In editor, on load (InitializeOnLoadMethod) and not in play mode, triggers a reload to refresh caches

4) Constraints & Failure Modes
- ID validation
  - Skips invalid IDs (null/whitespace) with warnings
  - Skips duplicates with warnings
- Data source
  - Uses Unity Resources; if paths contain no assets, dictionaries remain empty
- Return semantics
  - Lookup methods return null when not found
- Threading
  - No explicit synchronization; not thread-safe
- Runtime scope
  - Instance is created at runtime via CreateInstance and is not an authored asset
- Editor behavior
  - EditorAutoReload only active in Unity Editor and when not playing

5) Example
- Minimal usage example:
```csharp
// Example: lookup a hero by ID
var registry = CHAL.Systems.Unit.UnitRegistry.Instance;
HeroDef hero = registry.GetHeroById("hero_01");
```

6) Unknowns
- Details of HeroDef/EnemyDef structures beyond using HeroId and enemyId
- Behavior of DebugManager (log levels, output destinations)
- Exact data in Resources (availability, naming, and content of data/Heroes and data/Enemies)
- Whether any other systems rely on UnitRegistry persistence beyond the runtime singleton
- Any side effects of calling Reload multiple times in quick succession (idempotency, performance)
