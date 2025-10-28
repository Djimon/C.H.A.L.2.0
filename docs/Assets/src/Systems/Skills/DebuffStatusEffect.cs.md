# Assets/src/Systems/Skills/DebuffStatusEffect.cs

_Automatic generated/updated._

```markdown
## Purpose
- Defines a runtime debuff status effect on a unit, managing negative modifiers.
- Provides functionality to handle stacking and duration of debuffs.

## Public API
- Namespace: `CHAL.Systems.Skill`
- Types
  - **public class DebuffStatusEffect** [extends ActiveStatusEffect]
    - Public fields/properties:
      - `DebuffSettings Settings`: Configuration for the debuff.
      - `int CurrentStacks`: Current number of stacks of the debuff.
      - `StackingMode Stacking`: Mode of stacking behavior.
      - `bool modifierApplied`: Indicates if the modifier has been applied.
    - Public methods:
      - `DebuffStatusEffect(DebuffSettings settings)`: Constructor that initializes the debuff with settings.
      - `void TryAddStack(EffectReceiver source)`: Attempts to add a stack or refresh duration based on stacking mode.

  - **[System.Serializable] public class DebuffSettings**
    - Public fields/properties:
      - `string EffectId`: Identifier for the debuff effect.
      - `ModifierData Modifier`: Data related to the modifier.
      - `float BaseDuration`: Base duration of the debuff.
      - `int BaseMaxStacks`: Maximum number of stacks allowed.
      - `StackingMode Stacking`: Defines how stacking behaves.

## Key Behavior & Side Effects
- The constructor initializes the debuff's settings, including effect ID, duration, and stacking behavior.
- `TryAddStack` method modifies the current stack count or refreshes the duration based on the stacking mode.

## Constraints & Failure Modes
- The `BaseDuration` and `BaseMaxStacks` are clamped to ensure they are not less than zero or one, respectively.
- The method `TryAddStack` handles stack addition and duration refresh based on the defined stacking mode.

## Example
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

## Unknowns
- The behavior of `EffectReceiver` and how it interacts with `DebuffStatusEffect` is not defined in this file.
- The implementation details of `ModifierData` and `StackingMode` are not provided.
```
