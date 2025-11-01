# CHAL.Systems.Skill.StackingMode

_Automatically generated/updated from `Assets/src/Systems/Skills/ActiveStatusEffect.cs`._

```text
1) Purpose
- Defines a serializable data container ActiveStatusEffect representing an active status effect instance with identity, duration, timing, and modifiers.
  - Public fields: EffectId (string), Kind (StatusType), source (EffectReceiver), target (EffectReceiver), BaseDuration (float), RemainingTime (float), Modifier (ModifierData).
- Defines stacking behavior options via public enum StackingMode.
  - Members: RefreshDuration, AddStacks, IgnoreIfActive, Replace.
- Defines status categorization via public enum StatusType.
  - Members: DoT, Buff, Debuff, Aura.

```

2) Public API
- Namespace/module: CHAL.Systems.Skill
- Types
  - public class ActiveStatusEffect [Serializable]
    - Public fields
      - string EffectId
      - StatusType Kind
      - EffectReceiver source
      - EffectReceiver target
      - float BaseDuration
      - float RemainingTime
      - ModifierData Modifier
  - public enum StackingMode
    - RefreshDuration
    - AddStacks
    - IgnoreIfActive
    - Replace
  - public enum StatusType
    - DoT
    - Buff
    - Debuff
    - Aura

3) Key Behavior & Side Effects
- No methods or runtime logic are defined in this file.
- All behavior related to how ActiveStatusEffect is updated, stacked, or consumed must be implemented elsewhere; this file only defines data structures and enums.

4) Constraints & Failure Modes
- No validation or guards are defined; fields are public and can be set arbitrarily.
- The class is marked [Serializable], implying it is intended for serialization (e.g., Unity serialization).

5) Example
- Not derivable from this file (no usage/example code provided).

6) Unknowns
- Definitions and semantics of:
  - EffectReceiver (type of source/target) and ModifierData (modifier details) are not present in this file.
  - The exact behavior implied by each StackingMode value (how stacking is applied) is not defined here.
- How ActiveStatusEffect instances are created, updated each frame, or integrated into systems is not shown.
- Any Unity-specific serialization nuances beyond [Serializable] are not specified in this file.
