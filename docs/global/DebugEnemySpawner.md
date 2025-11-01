# global.DebugEnemySpawner

_Automatically generated/updated from `Assets/src/Systems/_test/DebugEnemySpawner.cs`._

1) Purpose
- MonoBehaviour that spawns a debug enemy on Start using a configured prefab and spawn point.
- Constructs an EnemyStruct with preset values and initializes the spawned EnemyController.
- Logs an error and aborts if the requested enemy definition cannot be found in UnitRegistry.

2) Public API
- Class: public class DebugEnemySpawner : MonoBehaviour
  - Public fields:
    - GameObject enemyPrefab — Prefab containing EnemyController
    - Transform spawnPoint
    - string enemyId — ID used to lookup enemy definition; default "debug_rat"

3) Key Behavior & Side Effects
- Start workflow:
  - var def = UnitRegistry.Instance.GetEnemyByID(enemyId)
  - if (def == null) DebugManager.Error($"EnemyDef {enemyId} not found!"); return;
  - EnemyStruct data = new EnemyStruct
    - EnemyId = enemyId
    - Count = 10
    - bonusTags = new System.Collections.Generic.List<string> { "swarm" }
    - Rank = EnemyRank.Normal
  - var go = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity)
  - var ctrl = go.GetComponent<EnemyController>()
  - ctrl.Init(data)

4) Constraints & Failure Modes
- Guards:
  - If enemy definition is not found, logs error and aborts.
- Potential issues not guarded in code:
  - If enemyPrefab or spawnPoint is null, Instantiate may fail.
  - If the instantiated prefab lacks an EnemyController component, ctrl will be null and ctrl.Init(data) may throw.
- Assumes existence of enemyId in UnitRegistry and a valid EnemyController on the prefab.

5) Example
- Not applicable (no derivable code example beyond the file itself).

6) Unknowns
- Exact structures and behavior of:
  - EnemyStruct, EnemyRank, EnemyController.Init
  - UnitRegistry.GetEnemyByID
  - DebugManager.Error
- Any runtime constraints (e.g., multiple spawns, threading) beyond this file.
