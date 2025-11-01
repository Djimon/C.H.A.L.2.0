# CHAL.Systems.Skill.DoTStatusEffect

_Automatically generated/updated from `Assets/src/Systems/Skills/DoTStatusEffect.cs`._

```csharp
1) Purpose
- Defines DoTStatusEffect: a Damage-over-Time status effect with stacking behavior, based on DoTSettings.
- Defines DoTSettings: serializable configuration for a DoT (ID, damage type, tick, duration, and stacking).

```

```csharp
2) Public API
- Namespace: CHAL.Systems.Skill

- public class DoTStatusEffect : ActiveStatusEffect
  - public DoTSettings DoTsettings
    - configuration for this DoT (see DoTSettings)
  - public int CurrentStacks
    - current number of active stacks
  - private int CurrentMaxStacks = 1
    - maximum allowed stacks for current DoT (derived from settings + bonuses)
  - public float internalTickTimer
    - timer for ticking intervals
  - public StackingMode Stacking = StackingMode.AddStacks
    - stacking behavior (default AddStacks)
  - public DoTStatusEffect(DoTSettings settings)
    - constructor: initializes DoT from settings
  - public void TryAddStack(EffectReceiver source)
    - reapply/stack handling: recalculates max stacks, clamps current, increments or refreshes duration

- public class DoTSettings
  - public string EffectId = "DefaultDoT"
    - identifier for this DoT effect
  - public DamageType DamageType = DamageType.Poison
    - type of damage dealt per tick
  - public float DamagePerTick = 1f
    - damage amount per tick
  - public float TickInterval = 1f
    - interval between ticks
  - public float BaseDuration = 5f
    - duration of the DoT when applied
  - public int BaseMaxStacks = 1
    - base maximum stack count
  - public StackingMode Stacking = StackingMode.AddStacks
    - stacking mode for this DoT
```

```csharp
3) Key Behavior & Side Effects
- DoTStatusEffect constructor(DoTSettings settings)
  - DoTsettings = settings
  - EffectId = settings.EffectId
  - RemainingTime = settings.BaseDuration
  - CurrentMaxStacks = settings.BaseMaxStacks
  - internalTickTimer = settings.TickInterval

- TryAddStack(EffectReceiver source)
  - bonusStacks = source.ActiveModifiers.Apply(
      ModifierTarget.DoTMaxStacks,
      0,
      new List<SkillTag> { SkillTag.DoT }
    )
  - CurrentMaxStacks = DoTsettings.BaseMaxStacks + bonusStacks
  - CurrentStacks = Math.Min(CurrentStacks, CurrentMaxStacks)
  - if (CurrentStacks < CurrentMaxStacks)
      CurrentStacks++
    else
      RemainingTime = BaseDuration
```

```csharp
4) Constraints & Failure Modes
- Null references risk: DoTSettings must be non-null when constructing DoTStatusEffect.
- Dependency assumptions: TryAddStack relies on source.ActiveModifiers and its Apply(...) result; nulls there may cause runtime exceptions.
- Incomplete tick logic: internalTickTimer exists but no per-tick method is defined in this file.
- Serialization: DoTSettings is marked [System.Serializable], but runtime handling of serialization is not shown here.
```

```csharp
6) Unknowns
- Details of ActiveStatusEffect (base class) behavior and fields (e.g., EffectId, RemainingTime, BaseDuration) are not defined in this file.
- Definitions and runtime behavior of ModifierTarget, SkillTag, and EffectReceiver.ActiveModifiers are not shown.
- How and when per-tick damage is applied, and how DoT interacts with other status effects or entities, is not implemented here.
- Whether DoTStatusEffect honors DoTSettings.Stacking field in TryAddStack or elsewhere is not visible in this file.
- Threading considerations and potential concurrency implications are not specified.
```
