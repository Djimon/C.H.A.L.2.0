# CHAL.AI.AITargetSelector

_Automatically generated/updated from `Assets/src/Systems/Unit/AiTargetSelector.cs`._

```text
1) Purpose
- Defines AITargetSelector class and AITargetPrio enum within the CHAL.AI namespace.
- Exposes public fields:
  - EffectReceiver currentTarget
  - AITargetPrio prioMode
  - float sightRange
- Declares two lifecycle-like methods with placeholder behavior:
  - void EnsureTarget()
  - void InvalidateTarget()

2) Public API
- Namespace/module
  - CHAL.AI

- Types
  - public class AITargetSelector
    - Public fields
      - public EffectReceiver currentTarget
        - Target currently tracked/selected by AI (type from CHAL.Systems.Unit)
      - public AITargetPrio prioMode
        - Priority mode for target selection
      - public float sightRange
        - Maximum distance for target visibility
    - Public methods
      - public void EnsureTarget()
        - No parameters; intended to lock onto a target
        - Side effect described in code comment: "Lockin on target until dead or out of range/sight"
      - public void InvalidateTarget()
        - No parameters; intended to clear current target
        - Side effect described in code comment: "reset currenttarget if Lost/dead"

  - public enum AITargetPrio
    - Nearest
    - HighestHP
    - LowestHP

3) Key Behavior & Side Effects
- EnsureTarget()
  - Intended to lock onto a target and maintain it until the target dies or is out of range/sight (per inline comment).
- InvalidateTarget()
  - Intended to reset currentTarget if the target is lost or dead (per inline comment).
- No concrete implementation or runtime flows are provided in this file.
- No explicit return values or exceptions are defined.

4) Constraints & Failure Modes
- No guards, null handling, or error handling implemented.
- No threading or asynchronous considerations present.
- No performance or allocation hints provided beyond the public fields.

5) Example
- None derivable from the code as there is no implementation or usage example.

6) Unknowns
- How EffectReceiver is defined and how it interacts with AITargetSelector.
- How currentTarget is assigned or updated in practice.
- How prioMode and sightRange influence target selection decisions.
- Any Unity-specific lifecycle or update integration (not present in this file).
- Any threading/async behavior related to target selection.
```
