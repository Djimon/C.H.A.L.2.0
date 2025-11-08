# global.DebugEnemySpawner

_Automatically generated/updated from `Assets/src/Systems/_test/DebugEnemySpawner.cs`._

# Purpose
- Spawns enemies in the game for debugging purposes.

# Public API
- Namespace: None
- Types
  - public class DebugEnemySpawner : MonoBehaviour
    - Public fields/properties:
      - GameObject enemyPrefab; // Prefab with EnemyController
      - Transform spawnPoint;
      - string enemyId = "debug_rat";
    - Public methods:
      - void Start(); // Initializes enemy spawning process.

# Key Behavior & Side Effects
- On Start, retrieves enemy definition by ID. If not found, logs an error.
- Instantiates an enemy prefab at the specified spawn point and initializes it with a debug enemy struct.

# Constraints & Failure Modes
- If the enemy definition is not found, an error is logged and no enemy is spawned.
- Assumes that enemyPrefab has an EnemyController component.

# Example
```csharp
public class ExampleUsage : MonoBehaviour
{
    public DebugEnemySpawner spawner;

    void Start()
    {
        spawner.Start(); // Triggers enemy spawning for debugging.
    }
}
```

# Unknowns
- None.
