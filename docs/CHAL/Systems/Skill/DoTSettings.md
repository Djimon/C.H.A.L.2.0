# CHAL.Systems.Skill.DoTSettings

_Automatically generated/updated from `Assets/src/Systems/Skills/DoTStatusEffect.cs`._

1) Purpose
- Defines a Damage-over-Time (DoT) status effect (DoTStatusEffect) with stacking behavior.
- Provides configuration for DoT via DoTSettings (serializable, public fields).
- Implements stacking and duration refresh logic in TryAddStack.

2) Public API
- Namespace/module: CHAL.Systems.Skill

- public class DoTStatusEffect : ActiveStatusEffect
  - Public fields
    - public DoTSettings DoTsettings
    - public int CurrentStacks
    - private int CurrentMaxStacks
    - public float internalTickTimer
    - public StackingMode Stacking
  - Public constructors
    - public DoTStatusEffect(DoTSettings settings)
  - Public methods
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
- DoTStatusEffect constructor
  - Sets DoTsettings
  - EffectId = settings.EffectId
  - RemainingTime = settings.BaseDuration
  - CurrentMaxStacks = settings.BaseMaxStacks
  - internalTickTimer = settings.TickInterval
- TryAddStack(EffectReceiver source)
  - bonusStacks = source.ActiveModifiers.Apply(ModifierTarget.DoTMaxStacks, 0, [SkillTag.DoT])
  - CurrentMaxStacks = DoTsettings.BaseMaxStacks + bonusStacks
  - CurrentStacks = min(CurrentStacks, CurrentMaxStacks)
  - If CurrentStacks < CurrentMaxStacks -> CurrentStacks++
  - Else -> RemainingTime = BaseDuration

4) Constraints & Failure Modes
- No explicit null checks in TryAddStack; relies on non-null source and source.ActiveModifiers.
- Public fields are mutable; behavior depends on base class (ActiveStatusEffect) for fields like EffectId, RemainingTime, BaseDuration.

5) Example
- Minimal usage example (illustrative; assumes a valid source):
```csharp
var settings = new DoTSettings
{
  EffectId = "DefaultDoT",
  DamageType = DamageType.Poison,
  DamagePerTick = 1f,
  TickInterval = 1f,
  BaseDuration = 5f,
  BaseMaxStacks = 1,
  Stacking = StackingMode.AddStacks
};

var dotEffect = new DoTStatusEffect(settings);

// Assuming 'source' is a valid EffectReceiver with ActiveModifiers
dotEffect.TryAddStack(source);
```

6) Unknowns
- Details of ActiveStatusEffect base class (fields like EffectId, RemainingTime, BaseDuration) and how they are used at runtime.
- Definitions and behavior of:
  - EffectReceiver
  - ActiveModifiers and Apply(...) semantics
  - ModifierTarget.DoTMaxStacks
  - SkillTag.DoT and StackingMode semantics
  - Damage application per tick (how TickInterval, DamagePerTick are applied over time)
- DoT tick progression, timing, and interaction with other systems beyond this file.

