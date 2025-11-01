# CHAL.Systems.Skill.ActiveStatusEffect

_Automatically generated/updated from `Assets/src/Systems/Skills/ActiveStatusEffect.cs`._

1) Purpose
- Defines a serializable data container ActiveStatusEffect representing an active status effect on a unit (identity, duration, source/target, modifier).
- Defines StackingMode to describe how repeated applications should interact (with behavior described in in-code comments).
- Defines StatusType to categorize effect kinds (DoT, Buff, Debuff, Aura).

2) Public API
- Namespace/module: CHAL.Systems.Skill

- Types
  - public class ActiveStatusEffect [Serializable]
    - public string EffectId
      - identifier for the effect
    - public StatusType Kind
      - category of the status (e.g., DoT, Buff)
    - public EffectReceiver source
      - originator of the effect
    - public EffectReceiver target
      - recipient of the effect
    - public float BaseDuration
      - base duration of the effect
    - public float RemainingTime
      - remaining duration of the effect
    - public ModifierData Modifier
      - modifier data describing the effect

  - public enum StackingMode
    - RefreshDuration
      - Dauer erneuern, keine Stacks erhhen
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
- No methods or executable logic are defined in this file; only data structures.
- StackingMode values include in-code comments describing intended runtime behavior (e.g., whether to refresh duration, add stacks, ignore, or replace), but no implementation is provided here.

4) Constraints & Failure Modes
- ActiveStatusEffect is marked [Serializable], indicating it is intended for serialization (e.g., Unity).
- All fields are public; there is no validation or guards in this file.
- Types EffectReceiver and ModifierData are referenced but defined elsewhere; their nullability/validation cannot be inferred from this file alone.
- No threading, asynchronous behavior, or performance guarantees are expressed.

5) Example
- Not provided, as no concrete usage or constructor logic is defined here.

6) Unknowns
- Definitions and contracts of EffectReceiver and ModifierData.
- How ActiveStatusEffect instances are created, updated, or removed in practice.
- Any max stack counts or explicit stacking rules beyond the comments in StackingMode.
- Any integration points with systems that manage lifetimes, updating RemainingTime, or applying modifiers.
