# global.AITargetSelector

_Automatically generated/updated from `Assets/src/Systems/Unit/AiTargetSelector.cs`._

# Purpose
- Defines the `AITargetSelector` class for managing AI target selection.
- Provides methods to ensure and invalidate targets based on specific conditions.

# Public API
- Namespace/module: None
- Types
  - public class AITargetSelector
    - Public fields/properties:
      - EffectReceiver currentTarget: The currently selected target.
      - AITargetPrio prioMode: The priority mode for target selection.
      - float sightRange: The range within which targets can be detected.
    - Public methods:
      - void EnsureTarget(): Locks onto the current target until it is dead or out of range/sight.
      - void InvalidateTarget(): Resets the current target if it is lost or dead.

  - public enum AITargetPrio
    - Nearest: Selects the nearest target.
    - HighestHP: Selects the target with the highest health points.
    - LowestHP: Selects the target with the lowest health points.

# Key Behavior & Side Effects
- `EnsureTarget`: Maintains the current target until it is no longer valid.
- `InvalidateTarget`: Clears the current target when it is no longer valid.

# Constraints & Failure Modes
- No explicit guards or null/empty handling noted.
- No threading or async considerations present.
- Performance/allocation hints are not evident.

# Example
```csharp
AITargetSelector targetSelector = new AITargetSelector();
targetSelector.sightRange = 50f;
targetSelector.prioMode = AITargetPrio.Nearest;
targetSelector.EnsureTarget();
```

# Unknowns
- No information on how `EffectReceiver` is defined or its role.
- No details on how targets are selected or updated in practice.

