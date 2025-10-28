# global.DebugEnemySpawner

_Automatically generated/updated from `Assets/src/Systems/_test/DebugEnemySpawner.cs`._

# Purpose
- Defines a `DebugEnemySpawner` class for spawning enemy game objects in Unity.

# Public API
- Namespace: None
- Types
  - public class DebugEnemySpawner : MonoBehaviour
    - Public fields/properties:
      - GameObject enemyPrefab: Prefab with `EnemyController`.
      - Transform spawnPoint: Location to spawn the enemy.
      - string enemyId: Identifier for the enemy type (default is "debug_rat").
    - Public methods:
      - void Start(): Initializes the enemy spawn process.

# Key Behavior & Side Effects
- On `Start`, retrieves enemy definition by `enemyId`.
- Logs an error if the enemy definition is not found.
- Instantiates an enemy prefab at the specified spawn point.
- Initializes the `EnemyController` with a predefined `EnemyStruct`.

# Constraints & Failure Modes
- Requires `enemyPrefab` to be assigned in the Unity Inspector.
- Requires `spawnPoint` to be assigned in the Unity Inspector.
- Handles null enemy definitions by logging an error and exiting early.

# Example
```csharp
public class ExampleUsage : MonoBehaviour
{
    public DebugEnemySpawner spawner;

    private void Start()
    {
        spawner.Start(); // Triggers enemy spawning.
    }
}
```

# Unknowns
- The implementation details of `UnitRegistry`, `EnemyStruct`, and `EnemyController` are not provided in this file.

