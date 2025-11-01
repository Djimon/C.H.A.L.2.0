# CHAL.Systems.Unit.UnitRegistry

_Automatically generated/updated from `Assets/src/Systems/Unit/UnitRegistry.cs`._

1) Purpose
- Defines a singleton-like registry (UnitRegistry) as a ScriptableObject that caches HeroDef and EnemyDef lookups loaded from Unity Resources.
- Provides public queries to retrieve by id and to enumerate all heroes/enemies and their ids.
- Loads data from Resources/data/Heroes and Resources/data/Enemies via Reload() and exposes them through internal dictionaries.

2) Public API
- Namespace/Module: CHAL.Systems.Unit

- Types
  - public sealed class UnitRegistry : ScriptableObject
    - Public fields/properties
      - public static UnitRegistry Instance { get; }
        - Lazy-initializes a UnitRegistry instance and calls Reload().
    - Public methods
      - public void Reload()
        - Clears internal caches; loads HeroDef and EnemyDef assets; validates IDs; logs warnings on invalid/duplicate IDs; populates caches.
      - public HeroDef GetHeroById(string id)
        - Returns the HeroDef for the given id or null if not found.
      - public EnemyDef GetEnemyByID(string id)
        - Returns the EnemyDef for the given id or null if not found.
      - public IEnumerable<string> GetAllHeroIds()
        - Returns all stored hero IDs.
      - public IEnumerable<string> GetAllEnemyIds()
        - Returns all stored enemy IDs.
      - public IEnumerable<HeroDef> GetAllHeroes()
        - Returns all stored HeroDef values.
      - public IEnumerable<EnemyDef> GetAllEnemies()
        - Returns all stored EnemyDef values.

3) Key Behavior & Side Effects
- Lazy singleton access
  - Accessing UnitRegistry.Instance creates a new UnitRegistry via CreateInstance<UnitRegistry>() and immediately calls Reload().
- Data loading and caching (Reload)
  - Clears _HeroById and _EnemyById.
  - Loads all HeroDef assets from Resources/data/Heroes.
  - For each HeroDef:
    - Skips if HeroId is null/empty/whitespace with a warning.
    - Skips if HeroId is a duplicate with a warning.
    - Logs loading of the hero and stores in _HeroById[HeroId].
  - Logs total loaded heroes.
  - Loads all EnemyDef assets from Resources/data/Enemies.
  - For each EnemyDef:
    - Skips if enemyId is null/empty/whitespace with a warning.
    - Skips if enemyId is a duplicate with a warning.
    - Logs loading of the enemy and stores in _EnemyById[enemyId].
  - Logs total loaded enemies and the count (enemycount).
- Lookup behavior
  - GetHeroById returns the matching HeroDef or null if not present.
  - GetEnemyByID returns the matching EnemyDef or null if not present.
- Enumeration behavior
  - GetAllHeroIds/GetAllEnemyIds return current dictionaries’ keys.
  - GetAllHeroes/GetAllEnemies return current dictionaries’ values.
- Editor auto reload (Unity Editor)
  - On load (InitializeOnLoadMethod), if not playing, triggers Instance.Reload() to refresh caches.

4) Constraints & Failure Modes
- ID validation
  - Hero ids and enemy ids must be non-empty; otherwise entries are skipped with warnings.
- Duplicates
  - Duplicate IDs are ignored with warnings to avoid overwriting existing entries.
- Resource dependency
  - If no assets exist under data/Heroes or data/Enemies, corresponding caches simply remain empty.
- Null handling
  - Lookups return null when not found (no exception).
- Threading
  - No explicit thread-safety guarantees; Unity typically calls on main thread.
- Logging
  - Uses DebugManager for diagnostic messages; log levels and categories are as coded.

5) Example
- Minimal usage example:
```csharp
var registry = CHAL.Systems.Unit.UnitRegistry.Instance;
var hero = registry.GetHeroById("hero1");
```

6) Unknowns
- Structure/details of HeroDef and EnemyDef beyond the used HeroId/enemyId fields.
- Exact behavior of DebugManager (levels, routing) outside the shown usage.
- Whether concurrent access could race during Instance creation (single-threaded Unity main thread typically avoids this).
- The commented-out CreateAssetMenu attribute functionality (currently not active).
