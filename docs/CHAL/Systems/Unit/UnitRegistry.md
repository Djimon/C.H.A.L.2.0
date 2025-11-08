# CHAL.Systems.Unit.UnitRegistry

_Automatically generated/updated from `Assets/src/Systems/Unit/UnitRegistry.cs`._

# Purpose
- Defines the `UnitRegistry` class, a singleton that manages hero and enemy definitions.

# Public API
- Namespace: `CHAL.Systems.Unit`
- Types
  - `public sealed class UnitRegistry : ScriptableObject`
    - Public fields/properties:
      - `public static UnitRegistry Instance`: Singleton instance of `UnitRegistry`.
    - Public methods:
      - `public void Reload()`: Reloads hero and enemy data from resources.
      - `public HeroDef GetHeroById(string id)`: Retrieves a hero definition by its unique identifier; returns null if not found.
      - `public EnemyDef GetEnemyByID(string id)`: Retrieves an enemy definition by its unique identifier; returns null if not found.
      - `public IEnumerable<string> GetAllHeroIds()`: Returns an enumerable collection of hero IDs.
      - `public IEnumerable<string> GetAllEnemyIds()`: Returns an enumerable collection of enemy IDs.
      - `public IEnumerable<HeroDef> GetAllHeroes()`: Returns an enumerable collection of hero definitions.
      - `public IEnumerable<EnemyDef> GetAllEnemies()`: Returns an enumerable collection of enemy definitions.

# Key Behavior & Side Effects
- The `Reload` method clears existing hero and enemy definitions and loads new definitions from the `Resources` folder.
- Logs warnings for invalid or duplicate IDs during loading.
- The `Instance` property initializes the singleton instance and calls `Reload` if it is null.

# Constraints & Failure Modes
- The `Reload` method does not handle exceptions from resource loading.
- IDs must not be null or whitespace; invalid IDs are logged and skipped.
- Duplicate IDs are logged as warnings and not added to the registry.

# Example
```csharp
var hero = UnitRegistry.Instance.GetHeroById("hero_id");
var enemies = UnitRegistry.Instance.GetAllEnemies();
```

# Unknowns
- The structure and properties of `HeroDef` and `EnemyDef` are not defined in this file.

