# CHAL.AI.AITargetSelector

_Automatically generated/updated from `Assets/src/Systems/Unit/AiTargetSelector.cs`._

```text
1) Purpose
- Defines a simple AI target selector with:
  - Public fields: currentTarget (EffectReceiver), prioMode (AITargetPrio), sightRange (float)
  - Public methods: EnsureTarget(), InvalidateTarget()
- Defines AITargetPrio enum used to select target prioritization:
  - Nearest, HighestHP, LowestHP
```

```text
2) Public API
- Namespace/module
  - CHAL.AI

- Types
  - public class AITargetSelector
    - Public fields
      - public EffectReceiver currentTarget
        - current target reference
      - public AITargetPrio prioMode
        - target prioritization mode
      - public float sightRange
        - sight detection range
    - Public methods
      - public void EnsureTarget()
        - no explicit behavior implemented; intended to "Lockin on target until dead or out of range/sight" (per comment)
      - public void InvalidateTarget()
        - no explicit behavior implemented; intended to "reset currentTarget if Lost/dead" (per comment)
  - public enum AITargetPrio
    - Nearest
    - HighestHP
    - LowestHP
```

```text
3) Key Behavior & Side Effects
- EnsureTarget
  - Intended effect: lock onto a target and retain it until the target is dead or out of range/sight (as per in-code comment)
- InvalidateTarget
  - Intended effect: reset currentTarget when the target is lost or dead (as per in-code comment)
- Note: No implementation details are present beyond the comments; no automatic state changes are performed in this file.
```

```text
4) Constraints & Failure Modes
- No explicit guards, validation, or initialization in this file
- Fields are public; no synchronization or threading notes
- No runtime error handling or async behavior defined
- EffectReceiver type is referenced but defined elsewhere (CHAL.Systems.Unit)
```

```text
5) Example
- Minimal usage (illustrative; no behavior guarantees since methods are unimplemented in this file)
```csharp
using CHAL.AI;

var selector = new AITargetSelector
{
    sightRange = 50f,
    prioMode = AITargetPrio.Nearest
};

// Adjust target according to its intended lifecycle (not implemented here)
selector.EnsureTarget();
```
```

```text
6) Unknowns
- Exact implementation details of EnsureTarget/InvalidateTarget (selection logic, target assignment, and release conditions)
- How sightRange and prioMode influence target choice in practice
- How EffectReceiver represents target status (alive/dead) and integration with this class
- Whether AITargetSelector is instantiated via MonoBehaviour/ScriptableObject or another pattern
```
