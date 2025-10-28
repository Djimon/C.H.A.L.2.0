# Assets/src/Systems/Unit/UnitLocator.cs

_Automatic generated/updated._

```markdown
## Purpose
- Defines the `UnitLocator` class for managing active units (heroes and enemies) in a scene.

## Public API
- Namespace: `CHAL.Systems.Unit`
- Types:
  - `public sealed class UnitLocator : MonoBehaviour`
    - Public fields/properties:
      - `static UnitLocator Instance { get; private set; }` - Singleton instance of `UnitLocator`.
      - `IReadOnlyCollection<HeroController> ActiveHeroes` - Collection of active heroes.
      - `IReadOnlyCollection<EnemyController> ActiveEnemies` - Collection of active enemies.
    - Public methods:
      - `void Register(HeroController hero)` - Registers a hero.
      - `void Unregister(HeroController hero)` - Unregisters a hero.
      - `void Register(EnemyController enemy)` - Registers an enemy.
      - `void Unregister(EnemyController enemy)` - Unregisters an enemy.
      - `Transform GetNearestEnemy(Vector3 origin, UnitTeam myTeam, float sightRange)` - Returns the nearest enemy within sight range.
      - `Transform GetHighestHPEnemy(Vector3 origin, UnitTeam myTeam, float sightRange)` - Returns the enemy with the highest HP within sight range.

## Key Behavior & Side Effects
- `Awake`: Initializes the singleton instance and logs readiness.
- `OnDestroy`: Cleans up the instance and clears the collections of heroes and enemies.
- `Register` and `Unregister`: Add or remove heroes/enemies from the respective collections and log the changes.
- `GetNearestEnemy` and `GetHighestHPEnemy`: Search through active units, applying validity checks and distance calculations.

## Constraints & Failure Modes
- Null checks are performed in `Register` and `Unregister` methods to prevent adding/removing null references.
- Cleanup methods remove dead units to reduce the risk of null reference exceptions.
- The class is designed to be attached to the same GameObject as the `MapManager`.

## Example
```csharp
UnitLocator.Instance.Register(hero);
Transform nearestEnemy = UnitLocator.Instance.GetNearestEnemy(playerPosition, UnitTeam.Player, sightRange);
```

## Unknowns
- The behavior of `DebugManager` and its logging levels is not defined in this file.
- The implementation details of `HeroController` and `EnemyController` are not provided.
```
