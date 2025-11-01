# CHAL.Systems.Skill.BuffStatusEffect

_Automatically generated/updated from `Assets/src/Systems/Skills/BuffStatusEffect.cs`._

```text
1) Purpose
- Defines BuffStatusEffect, a status effect representing a buff with stacking and duration logic.
- Defines BuffSettings (serializable) as configuration data for BuffStatusEffect.
- Initializes effect identifiers, duration, modifier, and stacking behavior from BuffSettings; marks the effect as a Buff.

2) Public API
- Namespace/module
  - CHAL.Systems.Skill

- Types
  - public class BuffStatusEffect : ActiveStatusEffect
    - Public fields
      - BuffSettings Settings — source configuration for the buff
      - int CurrentStacks — current number of active stacks
      - int CurrentMaxStacks (private) — maximum allowed stacks (at least 1)
      - StackingMode Stacking — stacking behavior (default: RefreshDuration)
      - bool modifierApplied — flag (purpose not fully defined in this file)
    - Public methods
      - BuffStatusEffect(BuffSettings settings)
        - Initializes Settings, EffectId, BaseDuration, RemainingTime, Modifier, CurrentMaxStacks, Stacking, and Kind (StatusType.Buff)
      - void TryAddStack(EffectReceiver source)
        - Reapplies or adds a stack according to Stacking mode; refreshes RemainingTime to BaseDuration under specified rules
        - Notes: contains logic to cap CurrentStacks to CurrentMaxStacks and to refresh duration when stacking
        - Comment indicates additional behavior (IgnoreIfActive / Replace) is handled centrally in EffectReceiver.ApplyEffect

  - public class BuffSettings
    - Public fields
      - string EffectId — identifier for the buff (default "DefaultBuff")
      - ModifierData Modifier — stat modification applied during duration
      - float BaseDuration — base duration in seconds
      - int BaseMaxStacks — maximum number of stacks
      - StackingMode Stacking — stacking behavior (e.g., RefreshDuration)

3) Key Behavior & Side Effects
- Initialization flow (BuffStatusEffect constructor)
  - Settings = settings
  - EffectId = settings.EffectId
  - BaseDuration = settings.BaseDuration
  - RemainingTime = settings.BaseDuration
  - Modifier = settings.Modifier
  - CurrentMaxStacks = Mathf.Max(1, settings.BaseMaxStacks)
  - Stacking = settings.Stacking
  - Kind = StatusType.Buff

- TryAddStack(source) flow
  - CurrentStacks = Mathf.Min(CurrentStacks, CurrentMaxStacks)
  - If Stacking == AddStacks
    - If CurrentStacks < CurrentMaxStacks, CurrentStacks++
    - RemainingTime = BaseDuration (refresh on new stack or cap)
  - Else if Stacking == RefreshDuration
    - RemainingTime = BaseDuration
  - Note: Further handling (IgnoreIfActive / Replace) is expected to occur in EffectReceiver.ApplyEffect

- Dynamic max-stacks potential (commented out)
  - There is commented code suggesting dynamic calculation of CurrentMaxStacks from modifiers (mod-driven buff max stacks)

4) Constraints & Failure Modes
- CurrentMaxStacks is constrained to be at least 1 via Mathf.Max(1, settings.BaseMaxStacks)
- No null-checks shown for settings in constructor (assumes non-null BuffSettings)
- Serialization: BuffSettings is marked [System.Serializable], enabling Unity serialization
- Threading: no explicit threading or async behavior
- Side effects: constructor and TryAddStack mutate state (CurrentStacks, RemainingTime, etc.)

5) Example
- Minimal usage example (assuming ModifierData and related types are constructible in the project)
```csharp
// Example usage
BuffSettings settings = new BuffSettings
{
    EffectId = "PowerUp",
    BaseDuration = 8f,
    BaseMaxStacks = 3,
    Stacking = StackingMode.AddStacks,
    Modifier = new ModifierData()
};

BuffStatusEffect buff = new BuffStatusEffect(settings);

// buff is now active with configured duration, stacking, and modifier
```

6) Unknowns
- Definitions and members of ActiveStatusEffect (base class)
- Details of StatusType, StackingMode, and EffectReceiver (types/enums used but not defined in this file)
- The exact structure and construction of ModifierData
- Behavior of IgnoreIfActive / Replace as implemented in EffectReceiver.ApplyEffect
- Any additional side effects or lifecycle management beyond this file (e.g., update loops, expiration handling)
```
