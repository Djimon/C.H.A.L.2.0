# global.DebugEnemySpawner

_Automatically generated/updated from `Assets/src/Systems/_test/DebugEnemySpawner.cs`._

Purpose
- MonoBehaviour that spawns a configured enemy on Start().
- Looks up an enemy definition by ID via UnitRegistry; logs an error and aborts if not found.
- Instantiates the provided enemy prefab at the spawn point and initializes its EnemyController with a constructed EnemyStruct.
- Default enemyId = "debug_rat"; prefab is expected to contain an EnemyController.

Public API
- Namespace/module: none (global)
- Types
  - public class DebugEnemySpawner : MonoBehaviour
    - Public fields
      - GameObject enemyPrefab; // Prefab mit EnemyController
      - Transform spawnPoint;
      - string enemyId = "debug_rat";
    - Public methods
      - private void Start()
        - Flow: look up enemy def; on null, log error and return; otherwise construct EnemyStruct, instantiate prefab at spawnPoint, get EnemyController, call Init(data).

Key Behavior & Side Effects
- Start() flow
  - def = UnitRegistry.Instance.GetEnemyByID(enemyId)
  - if def == null -> DebugManager.Error($"EnemyDef {enemyId} not found!"); return
  - data = new EnemyStruct
    - EnemyId = enemyId
    - Count = 10
    - bonusTags = ["swarm"]
    - Rank = EnemyRank.Normal
  - go = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity)
  - ctrl = go.GetComponent<EnemyController>()
  - ctrl.Init(data)
- Side effects
  - Potential in-scene instantiation of a new enemy instance
  - Logging via DebugManager when enemy def is not found
  - EnemyController initialization with the provided data

Constraints & Failure Modes
- Guard
  - If enemy definition is not found, logs error and does not spawn.
- Potential null references (not guarded in code)
  - enemyPrefab, spawnPoint, or the EnemyController component on the instantiated object may be null, which could lead to exceptions at runtime (e.g., ctrl being null or calling Init on a null reference).
- Assumptions
  - enemyPrefab contains an EnemyController component (since GetComponent<EnemyController>() is used).
  - Spawn uses spawnPoint.position and Quaternion.identity (no rotation offset).
  - The code relies on external types (EnemyStruct, EnemyRank, EnemyController, UnitRegistry, DebugManager) whose definitions are not in this file.

Example
```csharp
// Example usage in Unity
// Attach DebugEnemySpawner to a scene GameObject.
// Assign a prefab that has an EnemyController component to 'enemyPrefab'.
// Assign a Transform in the scene to 'spawnPoint'.
// Optionally set 'enemyId' to a different value (default "debug_rat").
```

Unknowns
- Definitions of EnemyStruct, EnemyRank, and their exact fields beyond those set here.
- Behavior of UnitRegistry.Instance.GetEnemyByID and what IDs are valid beyond this example.
- Implementation details of DebugManager.Error.
- Exact expectations for the EnemyController.Init(data) method and how data is consumed.
- Whether enemyPrefab is guaranteed to include an EnemyController component; no null checks for the component are present in this file.
