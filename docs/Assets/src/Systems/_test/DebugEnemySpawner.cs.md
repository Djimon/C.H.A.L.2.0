# Assets/src/Systems/_test/DebugEnemySpawner.cs

_Automatic generated/updated._

```markdown
## Purpose
- Defines a `DebugEnemySpawner` class for spawning enemy game objects in Unity.

## Public API
- Namespace: None
- Types
  - public class DebugEnemySpawner : MonoBehaviour
    - Public fields/properties:
      - GameObject enemyPrefab: Prefab with `EnemyController`.
      - Transform spawnPoint: Location to spawn the enemy.
      - string enemyId: Identifier for the enemy type (default is "debug_rat").
    - Public methods:
      - void Start(): Initializes the enemy spawn process and logs an error if the enemy definition is not found.

## Key Behavior & Side Effects
- On `Start`, retrieves enemy definition by `enemyId`.
- Logs an error if the enemy definition is not found.
- Instantiates an enemy prefab at the specified spawn point and initializes it with an `EnemyStruct`.

## Constraints & Failure Modes
- Requires `enemyPrefab` to be assigned in the Unity Inspector.
- Requires `spawnPoint` to be assigned in the Unity Inspector.
- Assumes `UnitRegistry.Instance.GetEnemyByID` returns null if the enemy ID is invalid.

## Example
```csharp
public class ExampleUsage : MonoBehaviour
{
    public DebugEnemySpawner spawner;

    void Start()
    {
        spawner.enemyId = "debug_rat"; // Set enemy ID
        spawner.Start(); // Trigger enemy spawning
    }
}
```

## Unknowns
- The implementation details of `UnitRegistry` and `EnemyController`.
- The structure and properties of `EnemyStruct`.
```
