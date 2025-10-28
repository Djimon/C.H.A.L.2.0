# CHAL.Systems.AI.MoveAgent

_Automatically generated/updated from `Assets/src/Systems/Unit/MoveAgent.cs`._

# Purpose
- Defines a `MoveAgent` class that wraps around `NavMeshAgent` for handling movement and stopping behavior.

# Public API
- Namespace: `CHAL.Systems.AI`
- Types
  - `public sealed class MoveAgent : MonoBehaviour`
    - Public fields/properties:
      - `float BaseSpeed { get; private set; }` - Base movement speed.
      - `float CurrentSpeed { get; private set; }` - Current movement speed.
      - `float StoppingDistance { get; set; }` - Distance at which the agent stops.
    - Public methods:
      - `void Init(float baseSpeed, bool isHero, float radius = 0.35f, int? overridePriority = null)` - Initializes the agent with speed and other parameters.
      - `void ApplyRuntimeSpeed(float speedMultiplier)` - Adjusts the current speed based on a multiplier.
      - `void SetDestination(Vector3 worldPos)` - Sets the destination for the agent, avoiding unnecessary updates.
      - `void StopOrHold()` - Stops the agent while maintaining its position.
      - `void ClearPathHard()` - Stops the agent and clears its path.
      - `bool IsInStoppingRange(Vector3 targetPos)` - Checks if the agent is within stopping distance of a target position.

# Key Behavior & Side Effects
- `Awake`: Initializes the `NavMeshAgent` component and sets default properties.
- `Init`: Configures the agent's speed, radius, and obstacle avoidance settings.
- `SetDestination`: Updates the agent's destination only if the new position is significantly different from the current one.
- `StopOrHold`: Stops the agent without resetting its path.
- `ClearPathHard`: Stops the agent and resets its path.
- `IsInStoppingRange`: Determines if the agent is close enough to the target position based on its stopping distance.

# Constraints & Failure Modes
- Requires a `NavMeshAgent` component (enforced by `[RequireComponent(typeof(NavMeshAgent))]`).
- Handles null checks for `_agent` to prevent null reference exceptions.
- Uses `Mathf.Max` to ensure non-negative values for speed and radius settings.

# Example
```csharp
MoveAgent moveAgent = gameObject.AddComponent<MoveAgent>();
moveAgent.Init(5.0f, true);
moveAgent.SetDestination(new Vector3(10, 0, 10));
```

# Unknowns
- None.

