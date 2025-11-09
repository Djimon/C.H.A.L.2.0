# Assets/src/Systems/Skills/BuffStatusEffect.cs

_Automatically generated/updated from `Assets/src/Systems/Skills/BuffStatusEffect.cs`._

# Purpose
- Defines a `BuffStatusEffect` class that represents a status effect applying buffs to entities.
- Provides a `BuffSettings` class for configuring buff properties.

# Public API
- Namespace: `CHAL.Systems.Skill`
- Types
  - **public class BuffStatusEffect** [extends ActiveStatusEffect]
    - Public fields/properties:
      - `BuffSettings Settings`: Configuration settings for the buff.
      - `int CurrentStacks`: Current number of stacks of the buff.
      - `StackingMode Stacking`: Mode of stacking for the buff.
      - `bool modifierApplied`: Indicates if the modifier has been applied.
    - Public methods:
      - `void TryAddStack(EffectReceiver source)`: Attempts to add a stack of the buff or refresh its duration.

  - **[System.Serializable] public class BuffSettings**
    - Public fields/properties:
      - `string EffectId`: Identifier for the buff effect.
      - `ModifierData Modifier`: Data for stat changes during the buff's duration.
      - `float BaseDuration`: Duration of the buff.
      - `int BaseMaxStacks`: Maximum number of stacks for the buff.
      - `StackingMode Stacking`: Stacking behavior for the buff.

# Key Behavior & Side Effects
- `TryAddStack` method increases the `CurrentStacks` up to `CurrentMaxStacks` or refreshes the duration based on the stacking mode.
- If `Stacking` is `StackingMode.AddStacks`, it increments `CurrentStacks` and refreshes the duration.
- If `Stacking` is `StackingMode.RefreshDuration`, it simply refreshes the duration.

# Constraints & Failure Modes
- `CurrentStacks` is clamped to `CurrentMaxStacks` to prevent exceeding the maximum.
- The `CurrentMaxStacks` is initialized to the maximum of 1 and `settings.BaseMaxStacks`.

# Example
```csharp
BuffSettings settings = new BuffSettings();
BuffStatusEffect buffEffect = new BuffStatusEffect(settings);
buffEffect.TryAddStack(source);
```

# Unknowns
- The behavior of `EffectReceiver` and how it interacts with `modifierApplied` is not defined in this file.
