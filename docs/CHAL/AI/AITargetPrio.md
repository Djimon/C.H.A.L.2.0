# CHAL.AI.AITargetPrio

_Automatically generated/updated from `Assets/src/Systems/Unit/AiTargetSelector.cs`._

Purpose
- Defines AITargetSelector and AITargetPrio within the CHAL.AI namespace.
- Exposes public fields to hold target state: currentTarget (EffectReceiver), prioMode (AITargetPrio), sightRange (float).
- Declares public methods to manage targeting: EnsureTarget() and InvalidateTarget().

Public API
- Namespace/module: CHAL.AI
- Types
  - public class AITargetSelector [no inheritance]
    - Public fields
      - public EffectReceiver currentTarget
      - public AITargetPrio prioMode
      - public float sightRange
    - Public methods
      - public void EnsureTarget()
      - public void InvalidateTarget()
  - public enum AITargetPrio
    - Nearest
    - HighestHP
    - LowestHP

Key Behavior & Side Effects
- EnsureTarget(): intended to lock onto a target until the target is dead or out of range/sight (as described by comment in code).
- InvalidateTarget(): intended to reset currentTarget when the target is lost or dead (as described by comment in code).
- No concrete implementation present; behavior is described only via comments.

Constraints & Failure Modes
- No implemented logic; none of the methods contain executable code beyond comments.
- currentTarget may be null if not set externally; no guards, validation, or initialization shown.
- No threading, async, or performance considerations evident from this file.
- Serialization/Unity-specific behavior not defined (no attributes, no MonoBehaviour, etc.).

Example
- Minimal usage illustrating surface shape and intended calls:
```csharp
var selector = new CHAL.AI.AITargetSelector();
selector.prioMode = CHAL.AI.AITargetPrio.Nearest;
selector.sightRange = 50f;
selector.EnsureTarget();

// Later, when target is lost or dead:
selector.InvalidateTarget();
```

Unknowns
- How currentTarget is assigned or retrieved in practice.
- What constitutes “dead” or “out of range” in this context beyond the comments.
- Default values for fields if not explicitly set.
- How this class interacts with the broader AI/world system (threading, update loops, etc.).
