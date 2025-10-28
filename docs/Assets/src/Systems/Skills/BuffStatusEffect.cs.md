# Assets/src/Systems/Skills/BuffStatusEffect.cs

_Automatic generated/updated._

```markdown
## Purpose
- Defines the `BuffStatusEffect` class for managing buff effects in a game.
- Provides a `BuffSettings` class for configuring buff parameters.

## Public API
- Namespace: `CHAL.Systems.Skill`
- Types
  - `public class BuffStatusEffect : ActiveStatusEffect`
    - Public fields/properties:
      - `BuffSettings Settings`: Configuration settings for the buff.
      - `int CurrentStacks`: Current number of active stacks of the buff.
      - `StackingMode Stacking`: Mode of stacking for the buff.
      - `bool modifierApplied`: Indicates if the modifier has been applied.
    - Public methods:
      - `public void TryAddStack(EffectReceiver source)`: Attempts to add a stack to the buff, refreshing duration or increasing stacks based on the stacking mode.

  - `public class BuffSettings`
    - Public fields/properties:
      - `string EffectId`: Identifier for the buff effect.
      - `ModifierData Modifier`: Data for stat changes during the buff's duration.
      - `float BaseDuration`: Duration of the buff effect.
      - `int BaseMaxStacks`: Maximum number of stacks for the buff.
      - `StackingMode Stacking`: Stacking behavior for the buff.

## Key Behavior & Side Effects
- `TryAddStack` method modifies `CurrentStacks` and `RemainingTime` based on the stacking mode.
- If `Stacking` is `AddStacks`, it increments `CurrentStacks` if below `CurrentMaxStacks` and refreshes `RemainingTime`.
- If `Stacking` is `RefreshDuration`, it simply refreshes `RemainingTime`.

## Constraints & Failure Modes
- `CurrentStacks` is clamped to `CurrentMaxStacks` using `Mathf.Min`.
- The `CurrentMaxStacks` is initialized to at least 1 based on `settings.BaseMaxStacks`.

## Example
```csharp
BuffSettings settings = new BuffSettings();
BuffStatusEffect buffEffect = new BuffStatusEffect(settings);
buffEffect.TryAddStack(source);
```

## Unknowns
- The behavior of `EffectReceiver` and how it interacts with `BuffStatusEffect` is not defined in this file.
- The implementation details of `ModifierData` and `StackingMode` are not provided.
```
