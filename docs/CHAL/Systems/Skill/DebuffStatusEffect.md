# CHAL.Systems.Skill.DebuffStatusEffect

_Automatically generated/updated from `Assets/src/Systems/Skills/DebuffStatusEffect.cs`._

```text
1) Purpose
- Runtime DEBUFF on a unit (negative modifier). Modifier add/remove is handled centrally by EffectReceiver (on apply/expire).
- Defines DebuffStatusEffect, a stackable status effect with configurable duration and max stacks.
- Defines DebuffSettings as a serializable config for a debuff (modifier, duration, stacking).

2) Public API
- Namespace/module
  - CHAL.Systems.Skill
- Types
  - public class DebuffStatusEffect : ActiveStatusEffect
    - Public fields/properties
      - public DebuffSettings Settings
        - Debuff configuration (modifier, duration, stacking)
      - public int CurrentStacks
        - Current number of active stacks (default 1)
      - private int _currentMaxStacks
        - Maximum stacks allowed (non-public)
      - public StackingMode Stacking
        - Stacking behavior (e.g., AddStacks, RefreshDuration)
      - public bool modifierApplied
        - Flag (usage not shown in this file)
    - Public methods
      - public DebuffStatusEffect(DebuffSettings settings)
        - Constructor: initializes fields based on settings
        - Sets EffectId, Modifier, BaseDuration, RemainingTime, _currentMaxStacks, CurrentStacks, Stacking, and Kind
      - public void TryAddStack(EffectReceiver source)
        - Reapply handling: increase stacks or refresh duration depending on Stacking
        - If Stacking == StackingMode.AddStacks
          - If CurrentStacks < _currentMaxStacks → CurrentStacks++
          - RemainingTime = BaseDuration (refresh on reapply)
        - Else if Stacking == StackingMode.RefreshDuration
          - RemainingTime = BaseDuration (refresh on reapply)
        - Note: IgnoreIfActive / Replace behavior is decided centrally in EffectReceiver.ApplyEffect(...)
  - [System.Serializable] public class DebuffSettings
    - public string EffectId = "Debuff_Default"
    - public ModifierData Modifier
    - public float BaseDuration = 5f
    - public int BaseMaxStacks = 1
    - public StackingMode Stacking = StackingMode.RefreshDuration
      - Stacking semantics align with DoT

3) Key Behavior & Side Effects
- DebuffStatusEffect constructor
  - EffectId is chosen as:
    - If settings?.EffectId is null/empty: use settings?.Modifier?.Id if available, else "Debuff"
    - Otherwise use settings.EffectId
  - Modifier is set from settings.Modifier
  - BaseDuration is set to max(0, settings.BaseDuration)
  - RemainingTime is initialized to BaseDuration
  - _currentMaxStacks is max(1, settings.BaseMaxStacks)
  - CurrentStacks initialized to 1
  - Stacking set from settings.Stacking
  - Kind set to StatusType.Debuff
- TryAddStack flow
  - If StackingMode.AddStacks
    - Increment CurrentStacks up to _currentMaxStacks
    - Reset RemainingTime to BaseDuration
  - Else if StackingMode.RefreshDuration
    - Reset RemainingTime to BaseDuration
  - Central decision about apply/expire is handled by EffectReceiver

4) Constraints & Failure Modes
- Null handling
  - Settings may be null; defaults apply:
    - EffectId → "Debuff" or Modifier.Id if available
    - Modifier → null
    - BaseDuration → 0f
    - BaseMaxStacks → 1
- Durations
  - BaseDuration is clamped to be >= 0 via Mathf.Max
- Stacking
  - _currentMaxStacks defaults to at least 1
  - CurrentStacks starts at 1
- Unknown runtime decisions
  - Actual application/removal timing and central decision logic live in EffectReceiver
  - modifierApplied usage is not defined in this file

5) Example
- Minimal instantiation (C#)
  - var debuff = new DebuffStatusEffect(new DebuffSettings
    {
      EffectId = "Stunned",
      Modifier = someModifierData,      // ModifierData instance
      BaseDuration = 6f,
      BaseMaxStacks = 3,
      Stacking = StackingMode.AddStacks
    });
- Note: To apply to a unit, coordinate with EffectReceiver per project usage.

6) Unknowns
- Definitions and behavior of:
  - ActiveStatusEffect (base class)
  - StackingMode enum
  - ModifierData
  - EffectReceiver
  - StatusType
- Exact lifecycle and interaction details of modifierApplied
- How this DebuffStatusEffect interacts with other status effects (e.g., Buffs/DoTs) beyond shared lifecycle concepts
```
