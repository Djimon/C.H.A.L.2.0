# CHAL.Systems.Unit.UnitLocator

_Automatically generated/updated from `Assets/src/Systems/Unit/UnitLocator.cs`._

# Purpose
- Defines a scene-scoped locator for active units (heroes and enemies) in the game.

# Public API
- Namespace: `CHAL.Systems.Unit`
- Types
  - `public sealed class UnitLocator : MonoBehaviour`
    - Public fields/properties:
      - `static UnitLocator Instance { get; private set; }` - Singleton instance of the UnitLocator.
      - `IReadOnlyCollection<HeroController> ActiveHeroes` - Collection of currently active heroes.
      - `IReadOnlyCollection<EnemyController> ActiveEnemies` - Collection of currently active enemies.
    - Public methods:
      - `public void Register(HeroController hero)` - Registers a hero controller.
      - `public void Unregister(HeroController hero)` - Unregisters a hero controller.
      - `public void Register(EnemyController enemy)` - Registers an enemy controller.
      - `public void Unregister(EnemyController enemy)` - Unregisters an enemy controller.
      - `public Transform GetNearestEnemy(Vector3 origin, UnitTeam myTeam, float sightRange)` - Returns the nearest enemy within sight range.
      - `public Transform GetHighestHPEnemy(Vector3 origin, UnitTeam myTeam, float sightRange)` - Returns the enemy with the highest HP within sight range.

# Key Behavior & Side Effects
- `Awake`: Initializes the singleton instance and logs readiness.
- `OnDestroy`: Cleans up the instance and clears the collections of heroes and enemies.
- Registration methods log the addition/removal of heroes and enemies.
- Query methods clean up dead units before performing their searches.

# Constraints & Failure Modes
- Registration methods ignore null inputs.
- Query methods ignore units that are not alive or out of sight range.
- Uses `HashSet` for efficient O(1) removal of units.

# Example
```csharp
UnitLocator.Instance.Register(heroController);
Transform nearestEnemy = UnitLocator.Instance.GetNearestEnemy(playerPosition, UnitTeam.Player, sightRange);
```

# Unknowns
- The implementation details of `HeroController` and `EnemyController`, including their properties and methods.
- The behavior of `DebugManager` and its logging levels.

