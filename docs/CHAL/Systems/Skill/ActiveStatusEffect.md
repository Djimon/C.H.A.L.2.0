# CHAL.Systems.Skill.ActiveStatusEffect

_Automatically generated/updated from `Assets/src/Systems/Skills/ActiveStatusEffect.cs`._

# Purpose
- Defines the `ActiveStatusEffect` class representing a status effect applied to a target.
- Provides enumerations for `StackingMode` and `StatusType` to categorize effects.

# Public API
- Namespace: `CHAL.Systems.Skill`
- Types
  - **public class ActiveStatusEffect**
    - Public fields/properties:
      - `string EffectId`: Identifier for the effect.
      - `StatusType Kind`: Type of the status effect.
      - `EffectReceiver source`: The source of the effect.
      - `EffectReceiver target`: The target of the effect.
      - `float BaseDuration`: The initial duration of the effect.
      - `float RemainingTime`: The remaining time for the effect.
      - `ModifierData Modifier`: Data related to the effect's modifiers.
  - **public enum StackingMode**
    - Values:
      - `RefreshDuration`: Refresh duration without increasing stacks.
      - `AddStacks`: Increase stacks up to a maximum and refresh duration.
      - `IgnoreIfActive`: Ignore if the effect is already active.
      - `Replace`: Replace the existing effect.
  - **public enum StatusType**
    - Values:
      - `DoT`: Damage over Time.
      - `Buff`: Positive effect.
      - `Debuff`: Negative effect.
      - `Aura`: Area effect.

# Key Behavior & Side Effects
- The `ActiveStatusEffect` class encapsulates the properties and behaviors of status effects applied to targets in the game.

# Constraints & Failure Modes
- No explicit guards or null handling noted in the file.
- No threading or async considerations present.

# Example
```csharp
ActiveStatusEffect effect = new ActiveStatusEffect
{
    EffectId = "burn",
    Kind = StatusType.DoT,
    BaseDuration = 5.0f,
    RemainingTime = 5.0f,
    Modifier = new ModifierData() // Assuming ModifierData is defined elsewhere
};
```

# Unknowns
- The definition and structure of `EffectReceiver` and `ModifierData` are not provided in this file.

