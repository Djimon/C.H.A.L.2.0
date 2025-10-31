# CHAL.Systems.Unit.UnitLocator

_Automatically generated/updated from `Assets/src/Systems/Unit/UnitLocator.cs`._

# Purpose
- Defines a scene-scoped locator for active units (heroes and enemies).
- Provides methods to register/unregister units and query for the nearest or highest HP enemy.

# Public API
- Namespace: `CHAL.Systems.Unit`
- Types:
  - `public sealed class UnitLocator : MonoBehaviour`
    - Public fields/properties:
      - `public static UnitLocator Instance { get; private set; }` - Singleton instance of the locator.
      - `public IReadOnlyCollection<HeroController> ActiveHeroes` - Collection of active heroes.
      - `public IReadOnlyCollection<EnemyController> ActiveEnemies` - Collection of active enemies.
    - Public methods:
      - `public void Register(HeroController hero)` - Registers a hero.
      - `public void Unregister(HeroController hero)` - Unregisters a hero.
      - `public void Register(EnemyController enemy)` - Registers an enemy.
      - `public void Unregister(EnemyController enemy)` - Unregisters an enemy.
      - `public Transform GetNearestEnemy(Vector3 origin, UnitTeam myTeam, float sightRange)` - Returns the nearest enemy's transform within sight range or null.
      - `public Transform GetHighestHPEnemy(Vector3 origin, UnitTeam myTeam, float sightRange)` - Returns the highest HP enemy's transform within sight range or null.

# Key Behavior & Side Effects
- Singleton pattern ensures only one instance exists in the scene.
- Registers and unregisters heroes and enemies, logging actions.
- Cleans up dead units from the collections to prevent null reference exceptions.
- Queries for enemies based on distance or health, returning the appropriate transform.

# Constraints & Failure Modes
- Guards against null references when registering/unregistering units.
- Cleanup methods remove dead units to maintain valid collections.
- Must be attached to the same GameObject as the MapManager.

# Example
```csharp
UnitLocator.Instance.Register(hero);
Transform nearestEnemy = UnitLocator.Instance.GetNearestEnemy(playerPosition, UnitTeam.Player, sightRange);
```

# Unknowns
- None.

