# CHAL.Systems.Skill.StackingMode

_Automatically generated/updated from `Assets/src/Systems/Skills/ActiveStatusEffect.cs`._

# Purpose
- Defines the `ActiveStatusEffect` class representing an active status effect applied to a target, including its duration, source, and target.
- Provides enumerations for `StackingMode` and `StatusType` to categorize effects.

# Public API
- Namespace: `CHAL.Systems.Skill`
- Types
  - public class `ActiveStatusEffect`
    - Public fields/properties:
      - `string EffectId`: Identifier for the effect.
      - `StatusType Kind`: Type of the status effect.
      - `EffectReceiver source`: The source of the effect.
      - `EffectReceiver target`: The target of the effect.
      - `float BaseDuration`: The initial duration of the effect.
      - `float RemainingTime`: The remaining time for the effect.
      - `ModifierData Modifier`: Data related to the effect's modifiers.
  - public enum `StackingMode`
    - Values:
      - `RefreshDuration`: Refresh duration without increasing stacks.
      - `AddStacks`: Increase stacks up to maximum and refresh duration.
      - `IgnoreIfActive`: Ignore if the effect is already active.
      - `Replace`: Replace the existing effect.
  - public enum `StatusType`
    - Values:
      - `DoT`: Damage over Time.
      - `Buff`: Positive effect.
      - `Debuff`: Negative effect.
      - `Aura`: Area effect.

# Key Behavior & Side Effects
- Represents the state of an active status effect, including its duration and the entities involved (source and target).

# Constraints & Failure Modes
- No explicit guards or null handling noted in the code.
- No threading or async considerations present.

# Example
```csharp
ActiveStatusEffect effect = new ActiveStatusEffect
{
    EffectId = "burn",
    Kind = StatusType.DoT,
    BaseDuration = 5.0f,
    RemainingTime = 5.0f,
    source = someEffectReceiver,
    target = someTargetReceiver,
    Modifier = someModifierData
};
```

# Unknowns
- The implementation details of `EffectReceiver` and `ModifierData` are not provided in this file.

