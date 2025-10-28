# Assets/src/Systems/Unit/MoveAgent.cs

_Automatic generated/updated._

```markdown
## Purpose
- Defines a `MoveAgent` class that wraps around `NavMeshAgent` for handling movement and stopping behavior.

## Public API
- Namespace: `CHAL.Systems.AI`
- Types
  - `public sealed class MoveAgent : MonoBehaviour`
    - Public fields/properties:
      - `float BaseSpeed { get; private set; }` - Base movement speed.
      - `float CurrentSpeed { get; private set; }` - Current movement speed.
      - `float StoppingDistance { get; set; }` - Distance at which the agent stops.
    - Public methods:
      - `void Init(float baseSpeed, bool isHero, float radius = 0.35f, int? overridePriority = null)` - Initializes the agent with speed and other parameters.
      - `void ApplyRuntimeSpeed(float speedMultiplier)` - Applies a multiplier to the current speed.
      - `void SetDestination(Vector3 worldPos)` - Sets the destination for the agent, avoiding unnecessary updates.
      - `void StopOrHold()` - Stops the agent and holds its position.
      - `void ClearPathHard()` - Clears the agent's path and stops it.
      - `bool IsInStoppingRange(Vector3 targetPos)` - Checks if the agent is within stopping range of the target position.

## Key Behavior & Side Effects
- `Awake`: Initializes the `NavMeshAgent` component and sets default properties.
- `Init`: Configures the agent's speed, radius, and obstacle avoidance settings.
- `SetDestination`: Updates the destination only if the new position is significantly different from the current one.
- `StopOrHold`: Stops the agent without resetting its path.
- `ClearPathHard`: Stops the agent and resets its path.
- `IsInStoppingRange`: Determines if the agent is close enough to the target position based on the stopping distance.

## Constraints & Failure Modes
- Requires a `NavMeshAgent` component due to the `[RequireComponent(typeof(NavMeshAgent))]` attribute.
- Handles null checks for `_agent` to prevent null reference exceptions.
- Uses squared magnitude for distance checks to avoid unnecessary square root calculations.

## Example
```csharp
MoveAgent moveAgent = gameObject.AddComponent<MoveAgent>();
moveAgent.Init(5f, true);
moveAgent.SetDestination(new Vector3(10, 0, 10));
```

## Unknowns
- None.
```
