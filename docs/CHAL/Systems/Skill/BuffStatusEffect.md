# CHAL.Systems.Skill.BuffStatusEffect

_Automatically generated/updated from `Assets/src/Systems/Skills/BuffStatusEffect.cs`._

# Purpose
- Defines a `BuffStatusEffect` class that represents a status effect applying buffs to entities.
- Provides a `BuffSettings` class for configuring buff properties.

# Public API
- Namespace: `CHAL.Systems.Skill`
- Types
  - **public class BuffStatusEffect : ActiveStatusEffect**
    - Public fields/properties:
      - `BuffSettings Settings`: Configuration settings for the buff.
      - `int CurrentStacks`: Current number of stacks of the buff.
      - `StackingMode Stacking`: Mode of stacking for the buff.
      - `bool modifierApplied`: Indicates if the modifier has been applied.
    - Public methods:
      - `BuffStatusEffect(BuffSettings settings)`: Constructor that initializes the buff with settings.
      - `void TryAddStack(EffectReceiver source)`: Attempts to add a stack to the buff, refreshing duration or increasing stacks based on the stacking mode.

  - **[System.Serializable] public class BuffSettings**
    - Public fields/properties:
      - `string EffectId`: Identifier for the buff effect.
      - `ModifierData Modifier`: Data for stat changes during the buff's duration.
      - `float BaseDuration`: Duration of the buff effect.
      - `int BaseMaxStacks`: Maximum number of stacks for the buff.
      - `StackingMode Stacking`: Stacking behavior for the buff.

# Key Behavior & Side Effects
- `TryAddStack` method modifies `CurrentStacks` and `RemainingTime` based on the stacking mode:
  - If `Stacking` is `AddStacks`, it increases `CurrentStacks` up to `CurrentMaxStacks` and refreshes `RemainingTime`.
  - If `Stacking` is `RefreshDuration`, it simply refreshes `RemainingTime`.

# Constraints & Failure Modes
- `CurrentStacks` is clamped to `CurrentMaxStacks` to prevent exceeding the maximum.
- The `CurrentMaxStacks` is initialized to the maximum of 1 and `settings.BaseMaxStacks`.

# Example
```csharp
BuffSettings settings = new BuffSettings();
settings.EffectId = "SpeedBuff";
settings.BaseDuration = 10f;
settings.BaseMaxStacks = 3;

BuffStatusEffect buff = new BuffStatusEffect(settings);
buff.TryAddStack(source);
```

# Unknowns
- The behavior of `EffectReceiver` and how it interacts with `BuffStatusEffect` is not defined in this file.
