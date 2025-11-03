# CHAL.AI.AITargetPrio

_Automatically generated/updated from `Assets/src/Systems/Unit/AiTargetSelector.cs`._

1) Purpose
- Defines CHAL.AI.AITargetSelector: a placeholder target-selection helper for AI.
- Stores currentTarget, priority mode, and sight range for targeting decisions.
- Declares EnsureTarget() and InvalidateTarget() as public surface.
- Defines CHAL.AI.AITargetPrio with Nearest, HighestHP, LowestHP.

```

```text
2) Public API
- Namespace/module
  - CHAL.AI
- Types
  - public class AITargetSelector
    - Public fields
      - public EffectReceiver currentTarget
        - currently targeted entity (role: target reference)
      - public AITargetPrio prioMode
        - target selection priority mode
      - public float sightRange
        - detection/engagement range
    - Public methods
      - public void EnsureTarget()
        - No implementation provided; intended to lock onto a target until dead or out of range/sight (per code comment)
      - public void InvalidateTarget()
        - No implementation provided; intended to reset currentTarget if Lost/dead (per code comment)
  - public enum AITargetPrio
    - Nearest
    - HighestHP
    - LowestHP

```

```text
3) Key Behavior & Side Effects
- EnsureTarget()
  - Intended behavior: lock onto a target until dead or out of range/sight (per comment).
- InvalidateTarget()
  - Intended behavior: reset currentTarget if Lost/dead (per comment).
- Public surface only; no concrete logic implemented in this file.
- currentTarget holds the currently selected EffectReceiver reference; updated by methods (implementation not provided).

```

```text
4) Constraints & Failure Modes
- No explicit null checks, validations, or error handling in this file.
- No threading, async, or performance/debug notes.
- All fields are public; no encapsulation or invariants documented.
- No default values or constructor logic shown.

```

```text
6) Unknowns
- How EnsureTarget selects a target based on prioMode (algorithm not implemented).
- How EffectReceiver defines dead state or range/sight checks.
- How this integrates with Unity lifecycle (MonoBehaviour/ScriptableObject) since not specified.
- Any side effects beyond updating currentTarget are not defined.

