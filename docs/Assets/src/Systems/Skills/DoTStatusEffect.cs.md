# Assets/src/Systems/Skills/DoTStatusEffect.cs

_Automatic generated/updated._

```markdown
# Purpose
- Defines the `DoTStatusEffect` class for managing damage-over-time (DoT) effects in a game.
- Provides `DoTSettings` for configuring the properties of the DoT effect.

# Public API
- Namespace: `CHAL.Systems.Skill`
- Types
  - `public class DoTStatusEffect : ActiveStatusEffect`
    - Public fields/properties:
      - `DoTSettings DoTsettings`: Configuration settings for the DoT effect.
      - `int CurrentStacks`: Current number of stacks of the DoT effect.
      - `float internalTickTimer`: Timer for managing tick intervals.
      - `StackingMode Stacking`: Mode for stacking behavior.
    - Public methods:
      - `public void TryAddStack(EffectReceiver source)`: Attempts to add a stack or refresh duration based on the source.

  - `public class DoTSettings`
    - Public fields/properties:
      - `string EffectId`: Identifier for the DoT effect.
      - `DamageType DamageType`: Type of damage inflicted by the DoT.
      - `float DamagePerTick`: Amount of damage dealt per tick.
      - `float TickInterval`: Time interval between ticks.
      - `float BaseDuration`: Duration of the DoT effect.
      - `int BaseMaxStacks`: Maximum number of stacks for the DoT effect.
      - `StackingMode Stacking`: Stacking behavior configuration.

# Key Behavior & Side Effects
- `TryAddStack` method recalculates the maximum stacks based on active modifiers and either increases the current stacks or refreshes the duration of the effect.

# Constraints & Failure Modes
- The maximum number of stacks is recalculated based on modifiers applied to the `EffectReceiver`.
- The current stacks are capped at the maximum stacks.

# Example
```csharp
DoTSettings settings = new DoTSettings();
DoTStatusEffect dotEffect = new DoTStatusEffect(settings);
dotEffect.TryAddStack(source);
```

# Unknowns
- The behavior of `ActiveStatusEffect` and `EffectReceiver` is not defined in this file.
- The implementation details of `ModifierTarget` and `SkillTag` are not provided.
```
