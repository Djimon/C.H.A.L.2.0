# CHAL.Systems.Skill.DebuffStatusEffect

_Automatically generated/updated from `Assets/src/Systems/Skills/DebuffStatusEffect.cs`._

# Purpose
- Defines a `DebuffStatusEffect` class representing a runtime debuff on a unit.
- Provides functionality to manage debuff stacks and duration through the `TryAddStack` method.

# Public API
- Namespace: `CHAL.Systems.Skill`
- Types
  - **public class DebuffStatusEffect** [extends ActiveStatusEffect]
    - Public fields/properties:
      - `DebuffSettings Settings`: Configuration settings for the debuff.
      - `int CurrentStacks`: Current number of stacks of the debuff.
      - `StackingMode Stacking`: Mode of stacking for the debuff.
      - `bool modifierApplied`: Indicates if the modifier has been applied.
    - Public methods:
      - `void TryAddStack(EffectReceiver source)`: Attempts to add a stack or refresh duration based on stacking mode.

  - **[System.Serializable] public class DebuffSettings**
    - Public fields/properties:
      - `string EffectId`: Identifier for the debuff effect.
      - `ModifierData Modifier`: Modifier associated with the debuff.
      - `float BaseDuration`: Base duration of the debuff.
      - `int BaseMaxStacks`: Maximum number of stacks for the debuff.
      - `StackingMode Stacking`: Stacking behavior for the debuff.

# Key Behavior & Side Effects
- The `TryAddStack` method modifies `CurrentStacks` and `RemainingTime` based on the stacking mode.
- If `Stacking` is `AddStacks`, it increments `CurrentStacks` until it reaches `_currentMaxStacks` and refreshes the duration.
- If `Stacking` is `RefreshDuration`, it resets `RemainingTime` to `BaseDuration`.

# Constraints & Failure Modes
- `BaseDuration` and `BaseMaxStacks` must be non-negative; defaults to 1 if not set.
- The `EffectId` is derived from `Modifier` if not explicitly provided.
- The method does not handle null checks for `source` in `TryAddStack`.

# Example
```csharp
DebuffSettings settings = new DebuffSettings {
    EffectId = "Slow",
    Modifier = someModifierData,
    BaseDuration = 10f,
    BaseMaxStacks = 3,
    Stacking = StackingMode.AddStacks
};

DebuffStatusEffect debuff = new DebuffStatusEffect(settings);
debuff.TryAddStack(someEffectReceiver);
```

# Unknowns
- The behavior of `EffectReceiver` and how it interacts with `DebuffStatusEffect` is not defined in this file.
- The implementation details of `ModifierData` and `StackingMode` are not provided.

