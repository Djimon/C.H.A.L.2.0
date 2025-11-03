# CHAL.Systems.Skill.DoTStatusEffect

_Automatically generated/updated from `Assets/src/Systems/Skills/DoTStatusEffect.cs`._

1) Purpose
- Defines DoTStatusEffect, a status effect that applies damage over time with stacking behavior.
- Defines DoTSettings, a serializable data container for DoT configuration (damage type, tick, duration, stacks, etc.).

2) Public API
- Namespace/module
  - CHAL.Systems.Skill

- Types
  - public class DoTStatusEffect : ActiveStatusEffect
    - Public fields/properties
      - public DoTSettings DoTsettings
      - public int CurrentStacks
      - private int CurrentMaxStacks
      - public float internalTickTimer
      - public StackingMode Stacking
    - Public methods
      - public DoTStatusEffect(DoTSettings settings)
      - public void TryAddStack(EffectReceiver source)

  - public class DoTSettings
    - [System.Serializable]
    - Public fields
      - public string EffectId
      - public DamageType DamageType
      - public float DamagePerTick
      - public float TickInterval
      - public float BaseDuration
      - public int BaseMaxStacks
      - public StackingMode Stacking

3) Key Behavior & Side Effects
- DoTStatusEffect constructor (DoTSettings settings)
  - Initializes: DoTsettings = settings
  - Sets EffectId = settings.EffectId
  - Sets RemainingTime = settings.BaseDuration
  - Sets CurrentMaxStacks = settings.BaseMaxStacks
  - Sets internalTickTimer = settings.TickInterval
- TryAddStack(EffectReceiver source)
  - Recalculates bonusStacks via source.ActiveModifiers.Apply with:
    - ModifierTarget.DoTMaxStacks
    - 0
    - List<SkillTag> { SkillTag.DoT }
  - CurrentMaxStacks = DoTsettings.BaseMaxStacks + bonusStacks
  - CurrentStacks = Min(CurrentStacks, CurrentMaxStacks)
  - If CurrentStacks < CurrentMaxStacks: CurrentStacks++
  - Else: RemainingTime = BaseDuration (refresh duration when max stacks reached)
- DoTSettings fields (defaults shown in code)
  - EffectId default: "DefaultDoT"
  - DamageType default: DamageType.Poison
  - DamagePerTick default: 1f
  - TickInterval default: 1f
  - BaseDuration default: 5f
  - BaseMaxStacks default: 1
  - Stacking default: StackingMode.AddStacks

4) Constraints & Failure Modes
- Null handling not explicit in TryAddStack; passing a null source would trigger a runtime error when accessing source.ActiveModifiers.
- Reliant on external types not defined in this file: ActiveStatusEffect, EffectReceiver, ModifierTarget, SkillTag, DamageType, StackingMode, etc.
- Behavior assumes DoTTick/damage application occurs elsewhere (not in this file); only stacking and timing-related fields are managed here.

5) Example
- Minimal usage example (instantiation and a stack try):

```csharp
var settings = new DoTSettings
{
    EffectId = "IceDoT",
    DamageType = DamageType.Poison,
    DamagePerTick = 2f,
    TickInterval = 1f,
    BaseDuration = 5f,
    BaseMaxStacks = 3,
    Stacking = StackingMode.AddStacks
};

var dotStatus = new DoTStatusEffect(settings);

// To add a stack, provide a valid EffectReceiver source with configured ActiveModifiers
// dotStatus.TryAddStack(source);
```

6) Unknowns
- Details of ActiveStatusEffect base class (properties like EffectId, RemainingTime, BaseDuration) are not in this file.
- Definitions/semantics of EffectReceiver, ModifierTarget, SkillTag, DamageType, StackingMode, and how DoT damage is actually applied over time.
- How internalTickTimer is used beyond initialization (no tick handling shown here).
- Threading or synchronization considerations are not specified.

