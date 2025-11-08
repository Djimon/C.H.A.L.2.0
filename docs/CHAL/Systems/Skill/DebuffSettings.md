# CHAL.Systems.Skill.DebuffSettings

_Automatically generated/updated from `Assets/src/Systems/Skills/DebuffStatusEffect.cs`._

# Purpose
- Defines a debuff status effect that can be applied to units, modifying their attributes negatively.

# Public API
- Namespace: `CHAL.Systems.Skill`
- Types
  - **public class DebuffStatusEffect : ActiveStatusEffect**
    - Public fields/properties:
      - `DebuffSettings Settings`: Configuration settings for the debuff.
      - `int CurrentStacks`: Current number of stacks of the debuff.
      - `StackingMode Stacking`: Mode of stacking for the debuff.
      - `bool modifierApplied`: Indicates if the modifier has been applied.
    - Public methods:
      - `DebuffStatusEffect(DebuffSettings settings)`: Constructor that initializes the debuff with settings.
      - `void TryAddStack(EffectReceiver source)`: Attempts to add a stack of the debuff or refresh its duration.

  - **[System.Serializable] public class DebuffSettings**
    - Public fields/properties:
      - `string EffectId`: Identifier for the debuff effect.
      - `ModifierData Modifier`: The modifier associated with the debuff.
      - `float BaseDuration`: Base duration of the debuff effect.
      - `int BaseMaxStacks`: Maximum number of stacks for the debuff.
      - `StackingMode Stacking`: Stacking behavior for the debuff.

# Key Behavior & Side Effects
- The `TryAddStack` method modifies the `CurrentStacks` and `RemainingTime` based on the stacking mode.
- If `Stacking` is set to `AddStacks`, it increments `CurrentStacks` until it reaches `BaseMaxStacks` and refreshes the duration.
- If `Stacking` is set to `RefreshDuration`, it resets the `RemainingTime` to `BaseDuration`.

# Constraints & Failure Modes
- The `BaseDuration` and `BaseMaxStacks` are clamped to a minimum of 0 and 1, respectively.
- The `EffectId` defaults to "Debuff" if not provided in `DebuffSettings`.

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
```

# Unknowns
- The behavior of `EffectReceiver` and how it interacts with `DebuffStatusEffect` is not defined in this file.
- The implementation details of `ModifierData` and `StackingMode` are not provided.

