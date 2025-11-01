# CHAL.Systems.Skill.BuffStatusEffect

_Automatically generated/updated from `Assets/src/Systems/Skills/BuffStatusEffect.cs`._

```text
1) Purpose
- Defines BuffStatusEffect, a concrete ActiveStatusEffect for buff-type status effects with stack and duration handling.
- Defines BuffSettings, a serializable configuration object for BuffStatusEffect (id, duration, max stacks, stacking mode, modifier).
- Provides basic stacking semantics via TryAddStack and constructor-driven initialization.

2) Public API
- Namespace/module
  - CHAL.Systems.Skill

- Types
  - public class BuffStatusEffect : ActiveStatusEffect
    - Public fields
      - public BuffSettings Settings
      - public int CurrentStacks
      - public bool modifierApplied
    - Public methods
      - public BuffStatusEffect(BuffSettings settings)
        - ctor that initializes effect from BuffSettings
      - public void TryAddStack(EffectReceiver source)
        - Reapplies/updates stacks and duration based on Stacking mode
    - (Private/internal fields)
      - private int CurrentMaxStacks = 1
    - Inferred/public state setup in ctor
      - EffectId, BaseDuration, RemainingTime, Modifier, Stacking, Kind are assigned from Settings or defaults

  - public class BuffSettings
    - Public fields (serializable config)
      - public string EffectId = "DefaultBuff"
      - public ModifierData Modifier
      - public float BaseDuration = 5f
      - public int BaseMaxStacks = 1
      - public StackingMode Stacking = StackingMode.RefreshDuration

3) Key Behavior & Side Effects
- Construction behavior (BuffStatusEffect ctor)
  - Sets Settings
  - Sets EffectId from Settings.EffectId
  - Sets BaseDuration and RemainingTime from Settings.BaseDuration
  - Sets Modifier from Settings.Modifier
  - Sets CurrentMaxStacks to max(1, Settings.BaseMaxStacks)
  - Sets Stacking from Settings.Stacking
  - Sets Kind to StatusType.Buff
- TryAddStack(source) behavior
  - Clamps CurrentStacks to CurrentMaxStacks: CurrentStacks = Min(CurrentStacks, CurrentMaxStacks)
  - If Stacking == StackingMode.AddStacks
    - If CurrentStacks < CurrentMaxStacks, increment CurrentStacks
    - Always refresh duration: RemainingTime = BaseDuration
  - Else if Stacking == StackingMode.RefreshDuration
    - Refresh duration: RemainingTime = BaseDuration
  - Commented note: dynamic max stacks from modifiers is not implemented here; central handling occurs in EffectReceiver.ApplyEffect
- Observed interactions
  - Central effect application logic likely handles IgnoreIfActive/Replace (not defined in this file)

4) Constraints & Failure Modes
- CurrentMaxStacks is at least 1 (via Mathf.Max)
- CurrentStacks is clamped to CurrentMaxStacks before stacking logic
- Uses UnityEngine Mathf and Unity serialization attribute
- Public API depends on external types: ActiveStatusEffect, EffectReceiver, ModifierData, StackingMode, StatusType, and other project-specific systems
- Null handling for BuffSettings/Modifier or source is not explicit; passing null may lead to runtime errors

5) Example
- Minimal usage (illustrative; adjust to project types)
```csharp
// Build buff settings
var settings = new BuffStatusEffect.BuffSettings
{
    EffectId = "SpeedBoost",
    BaseDuration = 8f,
    BaseMaxStacks = 3,
    Stacking = StackingMode.AddStacks,
    Modifier = new ModifierData { /* ... */ }
};

// Create buff and apply a stack
var buff = new BuffStatusEffect(settings);
EffectReceiver source = /* obtain from game logic */;
buff.TryAddStack(source);
```

6) Unknowns
- Details of ActiveStatusEffect base class (available properties/methods)
- The exact behavior/structure of EffectReceiver
- What ModifierData contains and how it modifies stats
- The definitions and values of StackingMode and StatusType outside this file
- How this integrates with broader status-effect application flow (e.g., EffectReceiver.ApplyEffect)
- Any runtime behavior beyond what is explicit in this file (e.g., expiration handling, removal triggers)
```
