# CHAL.Systems.Skill.BuffStatusEffect

_Automatically generated/updated from `Assets/src/Systems/Skills/BuffStatusEffect.cs`._

# Purpose
- Defines the `BuffStatusEffect` class for managing buff effects in a skill system.
- Provides a `BuffSettings` class for configuring buff parameters.

# Public API
- Namespace: `CHAL.Systems.Skill`
- Types
  - **public class BuffStatusEffect** [extends ActiveStatusEffect]
    - Public fields/properties:
      - `BuffSettings Settings`: Configuration settings for the buff.
      - `int CurrentStacks`: Current number of active stacks of the buff.
      - `StackingMode Stacking`: Mode for how stacks are handled.
      - `bool modifierApplied`: Indicates if the modifier has been applied.
    - Public methods:
      - `void TryAddStack(EffectReceiver source)`: Attempts to add a stack to the buff, refreshing duration or increasing stacks based on the stacking mode.

  - **[System.Serializable] public class BuffSettings**
    - Public fields/properties:
      - `string EffectId`: Identifier for the buff effect.
      - `ModifierData Modifier`: Data for stat changes during the buff's duration.
      - `float BaseDuration`: Duration of the buff effect.
      - `int BaseMaxStacks`: Maximum number of stacks for the buff.
      - `StackingMode Stacking`: Mode for how stacks are handled.

# Key Behavior & Side Effects
- `TryAddStack` method modifies `CurrentStacks` and `RemainingTime` based on the stacking mode.
- If `Stacking` is `AddStacks`, it increments `CurrentStacks` and refreshes `RemainingTime` if below `CurrentMaxStacks`.
- If `Stacking` is `RefreshDuration`, it simply refreshes `RemainingTime`.

# Constraints & Failure Modes
- `CurrentStacks` is capped at `CurrentMaxStacks`.
- `CurrentMaxStacks` is initialized to the maximum of 1 and `settings.BaseMaxStacks`.

# Example
```csharp
BuffSettings settings = new BuffSettings();
settings.EffectId = "SpeedBoost";
settings.BaseDuration = 10f;
settings.BaseMaxStacks = 3;

BuffStatusEffect buff = new BuffStatusEffect(settings);
buff.TryAddStack(source);
```

# Unknowns
- The behavior of `EffectReceiver` and how it interacts with `BuffStatusEffect` is not defined in this file.
- The implementation details of `ModifierData` and `StackingMode` are not provided.

