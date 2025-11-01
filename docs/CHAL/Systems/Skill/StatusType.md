# CHAL.Systems.Skill.StatusType

_Automatically generated/updated from `Assets/src/Systems/Skills/ActiveStatusEffect.cs`._

```text
1) Purpose
- Serializable data container representing an active status effect (identification, timing, source/target, and modifier).
- Defines stacking behavior options via StackingMode.
- Defines status categories via StatusType.

2) Public API
- Namespace/module: CHAL.Systems.Skill

- Types
  - public class ActiveStatusEffect [Serializable]
    - public string EffectId
      - Identifier for the effect
    - public StatusType Kind
      - Category of the status (DoT, Buff, Debuff, Aura)
    - public EffectReceiver source
      - Origin of the effect
    - public EffectReceiver target
      - Recipient of the effect
    - public float BaseDuration
      - Base duration before modifiers
    - public float RemainingTime
      - Time remaining before expiration
    - public ModifierData Modifier
      - Modifier data applied by the effect

  - public enum StackingMode
    - RefreshDuration
      - Dauer erneuern, keine Stacks erhohen
    - AddStacks
      - Stack++ bis MaxStacks, Dauer erneuern
    - IgnoreIfActive
      - wenn vorhanden -> ignorieren
    - Replace
      - vorhandenen Effekt ersetzen

  - public enum StatusType
    - DoT
    - Buff
    - Debuff
    - Aura

3) Key Behavior & Side Effects
- No methods or runtime logic defined.
- This file only provides data structures; stacking behavior semantics are described by enums, but no implementation is included here.

4) Constraints & Failure Modes
- No guards/validation present.
- Public reference fields (source/target) may be null unless enforced elsewhere.
- Serializable attribute indicates intended for serialization (e.g., Unity).

5) Example
- (none derivable from this file)

6) Unknowns
- What EffectReceiver and ModifierData contain or how they behave.
- How ActiveStatusEffect is instantiated, updated, or removed in practice.
- Exactly how StackingMode values are applied in runtime logic.
- Any threading, async, or lifecycle implications beyond public fields.
