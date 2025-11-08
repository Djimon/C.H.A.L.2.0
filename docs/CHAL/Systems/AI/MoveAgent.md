# CHAL.Systems.AI.MoveAgent

_Automatically generated/updated from `Assets/src/Systems/Unit/MoveAgent.cs`._

# Purpose
- Defines a `MoveAgent` class that wraps around Unity's `NavMeshAgent` to handle movement and stopping behavior.

# Public API
- Namespace: `CHAL.Systems.AI`
- Types
  - **sealed class** `MoveAgent` [extends `MonoBehaviour`]
    - Public fields/properties:
      - `float BaseSpeed`: The base speed for the agent.
      - `float CurrentSpeed`: The current speed of the agent.
      - `float StoppingDistance`: The distance at which the agent stops, can be set and retrieved.
    - Public methods:
      - `void Init(float baseSpeed, bool isHero, float radius = 0.35f, int? overridePriority = null)`: Initializes the agent's speed and settings.
      - `void ApplyRuntimeSpeed(float speedMultiplier)`: Adjusts the current speed based on a multiplier.
      - `void SetDestination(Vector3 worldPos)`: Sets the destination for the agent, avoiding unnecessary updates.
      - `void StopOrHold()`: Stops the agent without resetting its path.
      - `void ClearPathHard()`: Stops the agent and clears its path.
      - `bool IsInStoppingRange(Vector3 targetPos)`: Checks if the agent is within stopping range of a target position.

# Key Behavior & Side Effects
- The `Awake` method initializes the `NavMeshAgent` and sets default properties.
- The `Init` method configures the agent's speed, radius, and avoidance priority.
- The `SetDestination` method prevents excessive path recalculations by checking if the new destination is significantly different.
- The `StopOrHold` method stops the agent while maintaining its current position.
- The `ClearPathHard` method stops the agent and resets its path.
- The `IsInStoppingRange` method determines if the agent has reached its destination based on path status or distance.

# Constraints & Failure Modes
- The `NavMeshAgent` component is required; if not present, methods that rely on it will not function.
- The `StoppingDistance` property ensures non-negative values are set.
- The `SetDestination` method checks for path validity and only updates the destination if necessary.
- The `IsInStoppingRange` method handles cases where the agent has no valid path or is still calculating one.

# Example
```csharp
MoveAgent moveAgent = GetComponent<MoveAgent>();
moveAgent.Init(5f, true);
moveAgent.SetDestination(new Vector3(10, 0, 10));
```

# Unknowns
- None.

