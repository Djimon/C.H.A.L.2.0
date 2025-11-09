# Assets/src/Systems/_test/DebugEnemySpawner.cs

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
      - string enemyId = "debug_rat"; // ID of the enemy to spawn
    - Public methods:
      - void Start(); // Spawns enemies at the start of the game

# Key Behavior & Side Effects
- On Start, retrieves enemy definition by ID; logs an error if not found.
- Instantiates an enemy prefab at the specified spawn point and initializes it with a debug enemy struct.

# Constraints & Failure Modes
- Requires a valid enemy prefab and spawn point to function correctly.
- Logs an error if the enemy definition is not found in the UnitRegistry.

# Example
```csharp
public class ExampleUsage : MonoBehaviour
{
    public DebugEnemySpawner spawner;

    private void Start()
    {
        spawner.enemyId = "debug_rat"; // Set the enemy ID
        spawner.Start(); // Manually trigger enemy spawning
    }
}
```

# Unknowns
- None.
