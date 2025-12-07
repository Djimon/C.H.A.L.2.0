# Assets/src/Systems/Unit/MoveAgent.cs

_Automatically generated/updated from `Assets/src/Systems/Unit/MoveAgent.cs`._

# Purpose
- Defines a `MoveAgent` class that wraps around `NavMeshAgent` to handle movement and stopping behavior.

# Public API
- Namespace: `CHAL.Systems.AI`
- Types
  - **public sealed class MoveAgent : MonoBehaviour**
    - Public fields/properties:
      - `float BaseSpeed { get; private set; }` - The base speed for the agent.
      - `float CurrentSpeed { get; private set; }` - The current speed of the agent.
      - `float StoppingDistance` - The distance at which the agent stops.
    - Public methods:
      - `void Init(float baseSpeed, bool isHero, float radius = 0.35f, int? overridePriority = null)` - Initializes the agent's speed and settings.
      - `void ApplyRuntimeSpeed(float speedMultiplier)` - Adjusts the current speed based on a multiplier.
      - `void SetDestination(Vector3 worldPos)` - Sets the destination for the agent.
      - `void StopOrHold()` - Stops the agent and holds its position.
      - `void ClearPathHard()` - Clears the agent's path and stops it.
      - `bool IsInStoppingRange(Vector3 targetPos)` - Checks if the agent is within stopping range of the target position.

# Key Behavior & Side Effects
- The `Awake` method initializes the `NavMeshAgent` and sets default properties.
- The `Init` method configures the agent's speed, radius, and avoidance settings.
- The `SetDestination` method prevents unnecessary path recalculations by checking if the new destination is significantly different.
- The `StopOrHold` method stops the agent without resetting its path.
- The `ClearPathHard` method stops the agent and resets its path.
- The `IsInStoppingRange` method determines if the agent has reached its destination based on remaining distance or position.

# Constraints & Failure Modes
- The agent requires a `NavMeshAgent` component, enforced by the `[RequireComponent(typeof(NavMeshAgent))]` attribute.
- Methods return early if the `_agent` is null, preventing null reference exceptions.
- The `StoppingDistance` property ensures non-negative values are set for the agent's stopping distance.
- The `SetDestination` method uses a configurable epsilon value (`_destinationEpsilon`) to determine significant changes in destination.

# Example
```csharp
MoveAgent moveAgent = gameObject.AddComponent<MoveAgent>();
moveAgent.Init(5.0f, true);
moveAgent.SetDestination(new Vector3(10, 0, 10));
```

# Unknowns
- None.
