# CHAL.AI.AITargetSelector

_Automatically generated/updated from `Assets/src/Systems/Unit/AiTargetSelector.cs`._

# Purpose
- Selects and manages AI targets for gameplay interactions.

# Public API
- Namespace: `CHAL.AI`
- Types
  - public class `AITargetSelector`
    - Public fields/properties:
      - `EffectReceiver currentTarget`: The current target for the AI.
      - `AITargetPrio prioMode`: The priority mode for selecting targets.
      - `float sightRange`: The range within which targets can be detected.
    - Public methods:
      - `void EnsureTarget()`: Ensures the target is still valid and within range.
      - `void InvalidateTarget()`: Resets the current target if it is lost or dead.
  - public enum `AITargetPrio`
    - Values:
      - `Nearest`
      - `HighestHP`
      - `LowestHP`

# Key Behavior & Side Effects
- `EnsureTarget()`: Locks onto the target until it is dead or out of range/sight.
- `InvalidateTarget()`: Resets the current target if it is lost or dead.

# Constraints & Failure Modes
- None explicitly defined in the code.

# Example
```csharp
AITargetSelector targetSelector = new AITargetSelector();
targetSelector.sightRange = 10.0f;
targetSelector.prioMode = AITargetPrio.Nearest;
targetSelector.EnsureTarget();
```

# Unknowns
- None.
