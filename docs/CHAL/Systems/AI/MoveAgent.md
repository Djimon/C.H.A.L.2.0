# CHAL.Systems.AI.MoveAgent

_Automatically generated/updated from `Assets/src/Systems/Unit/MoveAgent.cs`._

Purpose
- Wraps a NavMeshAgent on the same GameObject to manage movement, stopping, and speed.
- Exposes speed and stopping distance control (BaseSpeed, CurrentSpeed, StoppingDistance) with caching for null-agent scenarios.
- Provides Init and destination control methods (SetDestination, StopOrHold, ClearPathHard, IsInStoppingRange) for runtime AI behavior.

```

```text
Public API
- Namespace/module
  - CHAL.Systems.AI

- Types
  - public sealed class MoveAgent : MonoBehaviour

  - Public fields/properties
    - public float BaseSpeed { get; private set; }
      - Base movement speed baseline (non-negative).
    - public float CurrentSpeed { get; private set; }
      - Current effective speed after runtime adjustments.
    - public float StoppingDistance
      - Getter: if _agent exists, returns _agent.stoppingDistance; else returns _stoppingDistanceCache.
      - Setter: stores value in _stoppingDistanceCache; if _agent exists, sets _agent.stoppingDistance = Mathf.Max(0f, value).

  - Public methods
    - public void Init(float baseSpeed, bool isHero, float radius = 0.35f, int? overridePriority = null)
      - Configures NavMeshAgent with speed, radius, acceleration, angularSpeed, autoBraking, obstacle avoidance, and priority.
      - Sets BaseSpeed = max(0.1, baseSpeed); CurrentSpeed = BaseSpeed.
      - Ensures _agent exists; assigns radius, speed, acceleration, angularSpeed, autoBraking, obstacleAvoidanceType.
      - Sets avoidancePriority to overridePriority ?? (isHero ? 10 : 50).
      - Applies stopping distance from _stoppingDistanceCache.
      - Side effects: mutates NavMeshAgent properties; may initialize _agent if null.
    - public void ApplyRuntimeSpeed(float speedMultiplier)
      - Clamps speedMultiplier to >= 0.
      - Updates CurrentSpeed = BaseSpeed * speedMultiplier.
      - If _agent available, sets _agent.speed = CurrentSpeed.
      - Side effects: changes agent speed.
    - public void SetDestination(Vector3 worldPos)
      - If _agent is null, returns.
      - If a path exists, only re-set destination if the delta to the new position exceeds _destinationEpsilon.
      - Sets _agent.isStopped = false and calls _agent.SetDestination(worldPos).
      - Side effects: may skip updates to avoid destination spam; triggers NavMesh computation when actually setting.
    - public void StopOrHold()
      - If _agent is null, returns.
      - Sets _agent.isStopped = true.
      - Comment: does not reset path, to keep local position held.
    - public void ClearPathHard()
      - If _agent is null, returns.
      - Sets _agent.isStopped = true; calls _agent.ResetPath().
      - Side effects: clears current path and stops movement.
    - public bool IsInStoppingRange(Vector3 targetPos)
      - If _agent is null, returns true.
      - If path is pending, fallback to distance check against StoppingDistance.
      - If no path, check distance against StoppingDistance.
      - If a path exists, uses remainingDistance <= max(StoppingDistance, 0.01f) as success.
      - Returns whether target is within stopping range.
```

```text
Key Behavior & Side Effects
- Awake
  - Finds NavMeshAgent if not assigned.
  - Configures agent: updateRotation, updateUpAxis, autoBraking = false, obstacleAvoidanceType = HighQualityObstacleAvoidance.
- Init
  - Establishes or updates NavMeshAgent properties: radius, speed, acceleration, angularSpeed, autoBraking, obstacleAvoidanceType, and avoidancePriority based on isHero and overridePriority.
  - Applies cached stopping distance.
  - Ensures agent reference exists.
- SetDestination
  - Avoids frequent re-pathing by skipping if destination change is insignificant (within epsilon).
  - Always un-stops agent before setting a new destination.
- StopOrHold
  - Pauses movement while preserving current path (no ResetPath), preventing jitter at the target.
- ClearPathHard
  - Fully stops and resets the current path for a complete halt.
- IsInStoppingRange
  - Robust handling for various NavMeshAgent states:
    - pathPending: uses distance check while waiting.
    - no path: uses distance check.
    - hasPath: uses remainingDistance with a tolerance based on StoppingDistance.
```

```text
Constraints & Failure Modes
- _agent may be null until Awake/Init; methods guard against null _agent.
- _stoppingDistanceCache stores a non-negative value; StoppingDistance getter relies on _stoppingDistanceCache if agent is null.
- _destinationEpsilon defaults to 0.05; SetDestination avoids minor updates to reduce path computation.
- Init clamps baseSpeed to at least 0.1; accelerations, radii, and other agent properties are clamped to sensible minimums.
- IsInStoppingRange returns true if the agent is null, which treats missing agent as already in range.
- Requires a NavMeshAgent component (enforced by [RequireComponent]).
- No explicit thread/async concerns; all NavMesh operations occur on main thread as per Unity.
```

```text
Example
```csharp
// Minimal usage example (derivable from this file)
var agent = gameObject.GetComponent<MoveAgent>();
agent.Init(baseSpeed: 5f, isHero: true);
agent.SetDestination(new Vector3(10f, 0f, 5f));
```
```

```text
Unknowns
- Behavior beyond the provided Init defaults (e.g., interactions with other movement systems) is not specified.
- Exact NavMeshAgent behavior depends on Unity version and scene NavMesh setup.
- No event hooks or callbacks are defined for destination reach or path changes.

