# CHAL.AI.AITargetPrio

_Automatically generated/updated from `Assets/src/Systems/Unit/AiTargetSelector.cs`._

# Purpose
- Defines the `AITargetSelector` class for managing AI targets in gameplay interactions.
- Provides an enumeration `AITargetPrio` for target selection priorities.

# Public API
- Namespace: `CHAL.AI`
- Types
  - public class `AITargetSelector`
    - Public fields/properties:
      - `EffectReceiver currentTarget`: The current target for the AI.
      - `AITargetPrio prioMode`: The priority mode for selecting targets.
      - `float sightRange`: The range within which targets can be detected.
    - Public methods:
      - `void EnsureTarget()`: Validates if the current target is still valid and within range.
      - `void InvalidateTarget()`: Resets the current target if it is lost or dead.
  - public enum `AITargetPrio`
    - Values:
      - `Nearest`: Selects the nearest target.
      - `HighestHP`: Selects the target with the highest health points.
      - `LowestHP`: Selects the target with the lowest health points.

# Key Behavior & Side Effects
- `EnsureTarget()`: Ensures the target remains valid and within the defined sight range.
- `InvalidateTarget()`: Resets the current target if it is determined to be lost or dead.

# Constraints & Failure Modes
- No explicit guards or null handling are defined in the provided code.
- No threading or async considerations are evident.

# Example
```csharp
AITargetSelector targetSelector = new AITargetSelector();
targetSelector.sightRange = 10f;
targetSelector.prioMode = AITargetPrio.Nearest;
targetSelector.EnsureTarget();
```

# Unknowns
- The implementation details of how targets are validated or reset are not provided.
