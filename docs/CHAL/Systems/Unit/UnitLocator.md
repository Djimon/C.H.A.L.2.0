# Assets/src/Systems/Unit/UnitLocator.cs

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
      - `public void Register(HeroController hero)` - Registers a hero with the locator.
      - `public void Unregister(HeroController hero)` - Unregisters a hero from the locator.
      - `public void Register(EnemyController enemy)` - Registers an enemy with the locator.
      - `public void Unregister(EnemyController enemy)` - Unregisters an enemy from the locator.
      - `public Transform GetNearestEnemy(Vector3 origin, UnitTeam myTeam, float sightRange)` - Returns the nearest enemy within sight range.
      - `public Transform GetHighestHPEnemy(Vector3 origin, UnitTeam myTeam, float sightRange)` - Returns the enemy with the highest HP within sight range.

# Key Behavior & Side Effects
- The `Awake` method initializes the singleton instance and logs readiness.
- The `OnDestroy` method cleans up the instance and clears the collections of heroes and enemies.
- The `Register` and `Unregister` methods log the addition/removal of heroes and enemies.
- The `GetNearestEnemy` and `GetHighestHPEnemy` methods clean up dead units before searching for valid targets.

# Constraints & Failure Modes
- Methods `Register` and `Unregister` ignore null inputs.
- The `GetNearestEnemy` and `GetHighestHPEnemy` methods ensure that only alive units within the specified sight range are considered.
- Cleanup methods remove null or dead entries to prevent null reference exceptions.

# Example
```csharp
UnitLocator.Instance.Register(heroController);
Transform nearestEnemy = UnitLocator.Instance.GetNearestEnemy(playerPosition, UnitTeam.Player, sightRange);
```

# Unknowns
- None.

