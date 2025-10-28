# Assets/src/Systems/Unit/AiTargetSelector.cs

_Automatic generated/updated._

```markdown
# Purpose
- Defines the `AITargetSelector` class for managing AI target selection.
- Provides methods to ensure and invalidate targets based on specific conditions.

# Public API
- **Namespace/module**: None specified.
- **Types**
  - `public class AITargetSelector`
    - `public EffectReceiver currentTarget` - The currently selected target.
    - `public AITargetPrio prioMode` - The priority mode for target selection.
    - `public float sightRange` - The range within which targets can be seen.
    - `public void EnsureTarget()` - Locks onto the current target until it is dead or out of range/sight.
    - `public void InvalidateTarget()` - Resets the current target if it is lost or dead.
  
  - `public enum AITargetPrio`
    - `Nearest`
    - `HighestHP`
    - `LowestHP`

# Key Behavior & Side Effects
- `EnsureTarget`: Locks onto a target until it is either dead or out of sight.
- `InvalidateTarget`: Resets the current target if it is no longer valid.

# Constraints & Failure Modes
- No explicit guards or null handling noted.
- No threading or async considerations present.

# Example
```csharp
AITargetSelector targetSelector = new AITargetSelector();
targetSelector.sightRange = 10f;
targetSelector.prioMode = AITargetPrio.Nearest;
targetSelector.EnsureTarget();
```

# Unknowns
- No information on how `EffectReceiver` is defined or its role.
- No details on how the target selection process is triggered or managed.
```
