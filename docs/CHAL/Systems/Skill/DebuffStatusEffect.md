# Assets/src/Systems/Skills/DebuffStatusEffect.cs

_Automatically generated/updated from `Assets/src/Systems/Skills/DebuffStatusEffect.cs`._

# Purpose
- Defines a runtime debuff status effect on a unit, managing negative modifiers.

# Public API
- Namespace: `CHAL.Systems.Skill`
- Types
  - public class `DebuffStatusEffect` [extends `ActiveStatusEffect`]
    - Public fields/properties:
      - `DebuffSettings Settings`: Configuration settings for the debuff.
      - `int CurrentStacks`: Current number of stacks of the debuff.
      - `StackingMode Stacking`: Mode of stacking for the debuff.
      - `bool modifierApplied`: Indicates if the modifier has been applied.
    - Public methods:
      - `DebuffStatusEffect(DebuffSettings settings)`: Constructor that initializes the debuff with settings.
      - `void TryAddStack(EffectReceiver source)`: Attempts to add a stack or refresh duration of the debuff.

  - public class `DebuffSettings`
    - Public fields/properties:
      - `string EffectId`: Identifier for the debuff effect.
      - `ModifierData Modifier`: The modifier associated with the debuff.
      - `float BaseDuration`: Base duration of the debuff.
      - `int BaseMaxStacks`: Maximum number of stacks for the debuff.
      - `StackingMode Stacking`: Stacking behavior for the debuff.

# Key Behavior & Side Effects
- The `DebuffStatusEffect` constructor initializes the debuff with settings, setting the effect ID, duration, and maximum stacks.
- The `TryAddStack` method modifies the current stacks or refreshes the duration based on the stacking mode.

# Constraints & Failure Modes
- The `BaseDuration` and `BaseMaxStacks` are set to a minimum of 0 and 1 respectively.
- The `CurrentStacks` is initialized to 1 and can only increase up to `_currentMaxStacks`.

# Example
```csharp
DebuffSettings settings = new DebuffSettings
{
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

