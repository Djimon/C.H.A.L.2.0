# CHAL.Systems.Skill.DebuffStatusEffect

_Automatically generated/updated from `Assets/src/Systems/Skills/DebuffStatusEffect.cs`._

```text
1) Purpose
- Runtime DEBUFF on a unit (negative modifier).
- Modifier add/remove is handled centrally by EffectReceiver (on apply/expire).
- DebuffLifecycle mirrors Buffs for modifier lifecycle (Add/Remove in receiver).

2) Public API

- Namespace/module: CHAL.Systems.Skill

- Types
  - public class DebuffStatusEffect : ActiveStatusEffect
    - Public fields
      - public DebuffSettings Settings
        - Debuff configuration used to initialize the effect
      - public int CurrentStacks
        - current stack count
      - public StackingMode Stacking
        - stacking behavior (e.g., AddStacks, RefreshDuration)
      - public bool modifierApplied
        - flag (purpose not detailed in this file)
    - Public methods
      - public DebuffStatusEffect(DebuffSettings settings)
        - constructor; initializes fields from settings and sets lifecycle/state
      - public void TryAddStack(EffectReceiver source)
        - reapply/stack handling; updates CurrentStacks and RemainingTime based on Stacking

  - [System.Serializable] public class DebuffSettings
    - Public fields
      - public string EffectId
        - identifier for the effect; defaults to "Debuff_Default" when not provided
      - public ModifierData Modifier
        - modifier data applied by this debuff
      - public float BaseDuration
        - base duration in seconds (clamped to >= 0)
      - public int BaseMaxStacks
        - maximum number of stacks (>= 1)
      - public StackingMode Stacking
        - stacking semantics (same as DoT)
        
Note: All types referenced but not defined here (ActiveStatusEffect, EffectReceiver, ModifierData, StackingMode, StatusType) are assumed to be defined elsewhere in the project.

3) Key Behavior & Side Effects
- Initialization (DebuffStatusEffect constructor)
  - EffectId = ifSettingsEffectIdEmptyOrNull ? (Modifier exists ? Modifier.Id : "Debuff") : Settings.EffectId
  - Modifier = settings?.Modifier
  - BaseDuration = max(0, settings?.BaseDuration ?? 0)
  - RemainingTime = BaseDuration
  - _currentMaxStacks = max(1, settings?.BaseMaxStacks ?? 1)
  - CurrentStacks = 1
  - Stacking = (settings != null) ? settings.Stacking : StackingMode.RefreshDuration
  - Kind = StatusType.Debuff
- Stacking behavior (TryAddStack)
  - If Stacking == AddStacks:
    - If CurrentStacks < _currentMaxStacks, then CurrentStacks++
    - RemainingTime = BaseDuration (refresh on reapply)
  - Else if Stacking == RefreshDuration:
    - RemainingTime = BaseDuration
  - Other policies (IgnoreIfActive/Replace) are handled centrally by EffectReceiver.ApplyEffect(...)
- Side effects
  - Modifies CurrentStacks and RemainingTime
  - Sets internal Kind to Debuff to align lifecycle with modifier system

4) Constraints & Failure Modes
- Null/empty handling
  - Settings may be null; code guards with null-conditional operators
  - EffectId falls back to Modifier.Id when available, otherwise "Debuff" or explicit Settings.EffectId
  - BaseDuration is clamped to >= 0
  - _currentMaxStacks defaults to at least 1 when Settings is null
- Threading/async: Not specified; behavior implied to run within Unity main thread lifecycle
- Performance/allocations: No heavy allocations in this file beyond standard field initializations

5) Example
```csharp
// Minimal example: create a debuff with stacking
var ds = new DebuffSettings
{
  EffectId = "Debuff_Slow",
  Modifier = new ModifierData(), // placeholder; actual structure defined elsewhere
  BaseDuration = 6f,
  BaseMaxStacks = 3,
  Stacking = StackingMode.AddStacks
};

var debuff = new DebuffStatusEffect(ds);
```

6) Unknowns
- Definitions and behavior of ActiveStatusEffect, EffectReceiver, ModifierData, StackingMode, StatusType
- How Debuff interacts with other systems at apply/expire in runtime
- Exact semantics of Modifier.Id and Modifier lifecycle in this context
- Any threading considerations or lifecycle hooks not shown in this file
```
