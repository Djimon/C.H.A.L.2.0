# CHAL.Systems.Skill.StackingMode

_Automatically generated/updated from `Assets/src/Systems/Skills/ActiveStatusEffect.cs`._

1) Purpose
- Defines a serializable data container for an active status effect (ActiveStatusEffect).
- Defines stacking behavior (StackingMode) for status effects.
- Defines status categorization (StatusType) for status effects.

2) Public API
- Namespace/module: CHAL.Systems.Skill
- Types
  - public class ActiveStatusEffect [Serializable]
    - Public fields
      - string EffectId — identifier of the effect
      - StatusType Kind — category of the status (DoT/Buff/Debuff/Aura)
      - EffectReceiver source — origin of the effect
      - EffectReceiver target — recipient of the effect
      - float BaseDuration — base duration of the effect
      - float RemainingTime — remaining time of the effect
      - ModifierData Modifier — modifier data applied by this effect
  - public enum StackingMode
    - RefreshDuration — Dauer erneuern, keine Stacks erhhen
    - AddStacks — Stack++ bis MaxStacks, Dauer erneuern
    - IgnoreIfActive — wenn vorhanden -> ignorieren
    - Replace — vorhandenen Effekt ersetzen
  - public enum StatusType
    - DoT
    - Buff
    - Debuff
    - Aura

3) Key Behavior & Side Effects
- No behavior or logic is implemented; this file contains only data definitions with no methods.
- All fields are public and have no constructors defined; default values apply.

4) Constraints & Failure Modes
- Public reference fields (source, target, Modifier) default to null if not assigned.
- Floating fields (BaseDuration, RemainingTime) default to 0f if not assigned.
- The class is marked [Serializable], indicating intended serialization support.

5) Example
- Minimal usage example (instantiation of the data container):

```csharp
var effect = new CHAL.Systems.Skill.ActiveStatusEffect
{
    EffectId = "burn",
    Kind = CHAL.Systems.Skill.StatusType.DoT,
    BaseDuration = 5f,
    RemainingTime = 5f
    // source/target/Modifier can be assigned as available
};
```

6) Unknowns
- What EffectReceiver exactamente represents (definitions located elsewhere).
- What ModifierData contains or how it applies (definitions located elsewhere).
- How ActiveStatusEffect is consumed by systems (flows, updates, or persistence) beyond this file.
- Any runtime validation or lifecycle management (constructors, methods) not present in this file.

