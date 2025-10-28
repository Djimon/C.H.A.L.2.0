# Assets/src/Systems/Unit/UnitRegistry.cs

_Automatic generated/updated._

```markdown
## Purpose
- Defines a singleton `UnitRegistry` for managing hero and enemy definitions.
- Provides methods to load, retrieve, and list heroes and enemies from resources.

## Public API
- Namespace: None
- Types
  - `public sealed class UnitRegistry : ScriptableObject`
    - Public fields/properties:
      - `public static UnitRegistry Instance`: Singleton instance of `UnitRegistry`.
    - Public methods:
      - `public void Reload()`: Loads hero and enemy definitions from resources, clearing previous data.
      - `public HeroDef GetHeroById(string id)`: Returns the `HeroDef` associated with the given ID or null if not found.
      - `public EnemyDef GetEnemyByID(string id)`: Returns the `EnemyDef` associated with the given ID or null if not found.
      - `public IEnumerable<string> GetAllHeroIds()`: Returns all hero IDs.
      - `public IEnumerable<string> GetAllEnemyIds()`: Returns all enemy IDs.
      - `public IEnumerable<HeroDef> GetAllHeroes()`: Returns all hero definitions.
      - `public IEnumerable<EnemyDef> GetAllEnemies()`: Returns all enemy definitions.

## Key Behavior & Side Effects
- `Reload()` clears existing hero and enemy dictionaries and loads new definitions from the `Resources` folder.
- Logs warnings for invalid or duplicate IDs during loading.
- Automatically reloads in the editor when not playing.

## Constraints & Failure Modes
- Handles null or empty IDs by skipping the corresponding definitions and logging warnings.
- Uses `TryGetValue` to safely retrieve definitions, returning null if the ID is not found.

## Example
```csharp
var hero = UnitRegistry.Instance.GetHeroById("hero_id");
var enemies = UnitRegistry.Instance.GetAllEnemies();
```

## Unknowns
- The structure and properties of `HeroDef` and `EnemyDef` are not defined in this file.
```
