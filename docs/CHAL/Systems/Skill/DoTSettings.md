# CHAL.Systems.Skill.DoTSettings

_Automatically generated/updated from `Assets/src/Systems/Skills/DoTStatusEffect.cs`._

```text
1) Purpose
- Define a Damage-over-Time (DoT) status effect with stacking behavior.
- Hold DoT configuration (DoTSettings) and runtime state (CurrentStacks, timer, etc.) for the effect.

2) Public API
- Namespace: CHAL.Systems.Skill

- public class DoTStatusEffect : ActiveStatusEffect
  - Public fields/properties
    - public DoTSettings DoTsettings
    - public int CurrentStacks
    - public float internalTickTimer
    - public StackingMode Stacking = StackingMode.AddStacks
  - Public constructors
    - public DoTStatusEffect(DoTSettings settings)
  - Public methods
    - public void TryAddStack(EffectReceiver source)

- [System.Serializable] public class DoTSettings
  - Public fields
    - public string EffectId = "DefaultDoT"
    - public DamageType DamageType = DamageType.Poison
    - public float DamagePerTick = 1f
    - public float TickInterval = 1f
    - public float BaseDuration = 5f
    - public int BaseMaxStacks = 1
    - public StackingMode Stacking = StackingMode.AddStacks

3) Key Behavior & Side Effects
- DoTStatusEffect constructor
  - Sets DoTsettings
  - Sets EffectId from settings
  - Sets RemainingTime from settings.BaseDuration
  - Sets CurrentMaxStacks from settings.BaseMaxStacks
  - Sets internalTickTimer from settings.TickInterval

- TryAddStack(EffectReceiver source)
  - Recalculates bonusStacks via source.ActiveModifiers.Apply(ModifierTarget.DoTMaxStacks, 0, new List<SkillTag> { SkillTag.DoT })
  - CurrentMaxStacks = DoTsettings.BaseMaxStacks + bonusStacks
  - CurrentStacks = Math.Min(CurrentStacks, CurrentMaxStacks)
  - If CurrentStacks < CurrentMaxStacks: CurrentStacks++ (add a stack)
  - Else: RemainingTime = BaseDuration (refresh/reassert duration)

- Note: Stacking field exists (StackingMode) but the logic in this file uses an in-class default of AddStacks; no alternate stacking behavior is applied within TryAddStack.

4) Constraints & Failure Modes
- Null handling: TryAddStack does not guard against null source; potential NullReferenceException if source is null.
- External dependencies: Relies on ActiveStatusEffect, EffectReceiver, ActiveModifiers, ModifierTarget, SkillTag, DamageType, StackingMode (definitions not in this file).
- Serialization: DoTSettings is marked [System.Serializable] for Unity/editor usage.

5) Example
```csharp
using CHAL.Systems.Skill;

public class ExampleUsage {
    public void CreateDoT() {
        var settings = new DoTSettings
        {
            EffectId = "PoisonDoT",
            DamageType = DamageType.Poison,
            DamagePerTick = 2f,
            TickInterval = 1f,
            BaseDuration = 5f,
            BaseMaxStacks = 3,
            Stacking = StackingMode.AddStacks
        };

        var dot = new DoTStatusEffect(settings);
        // dot can later have TryAddStack called with an EffectReceiver
    }
}
```

6) Unknowns
- Details of ActiveStatusEffect (e.g., how RemainingTime/BaseDuration are updated over time).
- The exact implementations/types of EffectReceiver, ActiveModifiers, ModifierTarget, SkillTag, DamageType, StackingMode beyond their usage here.
- How DoT tick application is processed across ticks (not implemented in this snippet).
- Whether and how DoTSettings.Stacking influences behavior beyond being stored.
