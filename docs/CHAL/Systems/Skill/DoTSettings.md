# CHAL.Systems.Skill.DoTSettings

_Automatically generated/updated from `Assets/src/Systems/Skills/DoTStatusEffect.cs`._

# Purpose
- Defines the `DoTStatusEffect` class for managing damage-over-time (DoT) effects in a game.
- Provides the `DoTSettings` class for configuring DoT effect parameters.

# Public API
- Namespace: `CHAL.Systems.Skill`
- Types
  - `public class DoTStatusEffect` [extends `ActiveStatusEffect`]
    - Public fields/properties:
      - `DoTSettings DoTsettings`: Configuration settings for the DoT effect.
      - `int CurrentStacks`: Current number of stacks of the DoT effect.
      - `float internalTickTimer`: Timer for the internal tick interval.
      - `StackingMode Stacking`: Mode for stacking behavior.
    - Public methods:
      - `public void TryAddStack(EffectReceiver source)`: Attempts to add a stack or refresh duration; modifies `CurrentStacks` and `RemainingTime`.

  - `public class DoTSettings`
    - Public fields/properties:
      - `string EffectId`: Identifier for the DoT effect.
      - `DamageType DamageType`: Type of damage inflicted by the DoT.
      - `float DamagePerTick`: Amount of damage dealt per tick.
      - `float TickInterval`: Time between each tick of damage.
      - `float BaseDuration`: Total duration of the DoT effect.
      - `int BaseMaxStacks`: Maximum number of stacks for the DoT effect.
      - `StackingMode Stacking`: Stacking behavior configuration.

# Key Behavior & Side Effects
- `TryAddStack` method recalculates the maximum stacks based on active modifiers and either increases the current stacks or refreshes the duration of the effect.

# Constraints & Failure Modes
- `CurrentMaxStacks` is recalculated based on active modifiers, ensuring it does not exceed the base maximum stacks.
- If `CurrentStacks` is at maximum, the duration is refreshed instead of increasing stacks.

# Example
```csharp
DoTSettings settings = new DoTSettings();
DoTStatusEffect dotEffect = new DoTStatusEffect(settings);
dotEffect.TryAddStack(source);
```

# Unknowns
- The behavior of `ActiveStatusEffect` and `EffectReceiver` is not defined in this file.
- The implementation details of `ModifierTarget` and `SkillTag` are not provided.

