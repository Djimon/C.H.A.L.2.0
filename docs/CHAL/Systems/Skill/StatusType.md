# CHAL.Systems.Skill.StatusType

_Automatically generated/updated from `Assets/src/Systems/Skills/ActiveStatusEffect.cs`._

# Purpose
- Defines the `ActiveStatusEffect` class representing an active status effect applied to a target.
- Contains information about the effect's duration, source, and target.
- Provides enumerations for stacking modes and status types.

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
      - `float RemainingTime`: The remaining time before the effect expires.
      - `ModifierData Modifier`: Data related to the effect's modifiers.
  - public enum `StackingMode`
    - Values:
      - `RefreshDuration`: Refreshes duration without increasing stacks.
      - `AddStacks`: Increases stacks up to a maximum while refreshing duration.
      - `IgnoreIfActive`: Ignores if the effect is already active.
      - `Replace`: Replaces the existing effect.
  - public enum `StatusType`
    - Values:
      - `DoT`: Damage over time.
      - `Buff`: Positive effect.
      - `Debuff`: Negative effect.
      - `Aura`: Area effect.

# Key Behavior & Side Effects
- The `ActiveStatusEffect` class encapsulates the properties necessary to manage an active effect, including its duration and source/target relationships.

# Constraints & Failure Modes
- No explicit guards or null handling are defined in the provided code.
- No threading or async considerations are evident.

# Example
```csharp
ActiveStatusEffect effect = new ActiveStatusEffect
{
    EffectId = "FireBurn",
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
