# Assets/src/Systems/Unit/UnitRegistry.cs

_Automatically generated/updated from `Assets/src/Systems/Unit/UnitRegistry.cs`._

# Purpose
- Defines a singleton registry for managing hero and enemy definitions in the game.

# Public API
- Namespace: `CHAL.Systems.Unit`
- Types
  - `public sealed class UnitRegistry : ScriptableObject`
    - Public properties:
      - `static UnitRegistry Instance`: Provides access to the singleton instance of `UnitRegistry`.
    - Public methods:
      - `public void Reload()`: Reloads hero and enemy data from resources.
      - `public HeroDef GetHeroById(string id)`: Retrieves a hero definition by its unique identifier; returns null if not found.
      - `public EnemyDef GetEnemyByID(string id)`: Retrieves an enemy definition by its unique identifier; returns null if not found.
      - `public IEnumerable<string> GetAllHeroIds()`: Retrieves all hero IDs from the collection.
      - `public IEnumerable<string> GetAllEnemyIds()`: Retrieves all enemy IDs from the collection.
      - `public IEnumerable<HeroDef> GetAllHeroes()`: Retrieves all hero definitions from the collection.
      - `public IEnumerable<EnemyDef> GetAllEnemies()`: Retrieves all enemy definitions from the collection.

# Key Behavior & Side Effects
- The `Reload` method clears existing hero and enemy definitions and loads new definitions from the specified resource paths.
- Warnings are logged for invalid or duplicate IDs during the loading process.
- The `EditorAutoReload` method automatically reloads data in the editor when not in play mode.

# Constraints & Failure Modes
- The `Reload` method does not handle exceptions from resource loading; it assumes resources are available.
- The `GetHeroById` and `GetEnemyByID` methods return null if the provided ID does not exist in the registry.

# Example
```csharp
var hero = UnitRegistry.Instance.GetHeroById("hero123");
var enemies = UnitRegistry.Instance.GetAllEnemies();
```

# Unknowns
- The structure and properties of `HeroDef` and `EnemyDef` are not defined in this file.

