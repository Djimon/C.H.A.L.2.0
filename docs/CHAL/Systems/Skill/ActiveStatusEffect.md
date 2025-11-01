# CHAL.Systems.Skill.ActiveStatusEffect

_Automatically generated/updated from `Assets/src/Systems/Skills/ActiveStatusEffect.cs`._

```text
1) Purpose
- Defines a serializable ActiveStatusEffect data container representing an active status effect on a unit.
- Defines stacking and type enums used by the status system (StackingMode, StatusType).
- Located in CHAL.Systems.Skill; relies on CHAL.Data and CHAL.Systems.Unit types.

```

```text
2) Public API
- Namespace/module
  - CHAL.Systems.Skill

- Types
  - public class ActiveStatusEffect [Serializable]
    - Public fields
      - string EffectId;                  // Identifier for the effect
      - StatusType Kind;                   // Type of the effect (See StatusType)
      - EffectReceiver source;             // Originator of the effect
      - EffectReceiver target;             // Recipient of the effect
      - float BaseDuration;                // Base duration of the effect
      - float RemainingTime;               // Remaining duration of the effect
      - ModifierData Modifier;             // Modifier data applied by the effect

  - public enum StackingMode
    - RefreshDuration       // Dauer erneuern, keine Stacks erhöhen
    - AddStacks             // Stack++ bis MaxStacks, Dauer erneuern
    - IgnoreIfActive        // wenn vorhanden -> ignorieren
    - Replace               // vorhandenen Effekt ersetzen

  - public enum StatusType
    - DoT
    - Buff
    - Debuff
    - Aura

```

```text
3) Key Behavior & Side Effects
- No methods or behavior are implemented in this file; it only defines data structures.
- Descriptions of stacking semantics are provided via comments on StackingMode; actual logic is not present here.
- The class is marked Serializable, indicating it is intended for Unity serialization.

```

```text
4) Constraints & Failure Modes
- No validation logic; public fields can be null or default values.
- BaseDuration and RemainingTime are plain floats; no automatic normalization or checks are defined here.
- Serialization implications depend on Unity/CHAL usage (fields must be serializable types).

```

```text
5) Example
```csharp
using CHAL.Systems.Skill;

// Minimal example: construct an active DoT with basic fields
var nowEffect = new ActiveStatusEffect
{
    EffectId = "fire-doT",
    Kind = StatusType.DoT,
    BaseDuration = 6f,
    RemainingTime = 6f,
    source = null,   // to be set by caller
    target = null,   // to be set by caller
    Modifier = null    // to be set by caller
};
```

```

```text
6) Unknowns
- Definitions of EffectReceiver and ModifierData (types used by this file) are not shown here.
- How ActiveStatusEffect instances are created, updated, or removed is not defined in this file.
- Any runtime constraints (e.g., max stacks, interaction with other systems) are not specified.
- Any default values or serialization behavior beyond [Serializable] are not specified.
```
