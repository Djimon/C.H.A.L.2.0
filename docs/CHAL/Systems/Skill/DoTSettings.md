# CHAL.Systems.Skill.DoTSettings

_Automatically generated/updated from `Assets/src/Systems/Skills/DoTStatusEffect.cs`._

# Purpose
- Defines a damage-over-time (DoT) status effect that can stack over time.
- Inherits from `ActiveStatusEffect` and manages its own stacking behavior.

# Public API
- Namespace: `CHAL.Systems.Skill`
- Types
  - **public class DoTStatusEffect** [extends ActiveStatusEffect]
    - Public fields/properties:
      - `DoTSettings DoTsettings`: Configuration settings for the DoT effect.
      - `int CurrentStacks`: Current number of stacks of the DoT effect.
      - `float internalTickTimer`: Timer for the internal tick interval.
      - `StackingMode Stacking`: Defines the stacking behavior.
    - Public methods:
      - `void TryAddStack(EffectReceiver source)`: Attempts to add a stack or refresh duration based on the current state.

  - **[System.Serializable] public class DoTSettings**
    - Public fields/properties:
      - `string EffectId`: Identifier for the DoT effect.
      - `DamageType DamageType`: Type of damage inflicted by the DoT.
      - `float DamagePerTick`: Amount of damage dealt per tick.
      - `float TickInterval`: Time interval between ticks.
      - `float BaseDuration`: Base duration of the effect.
      - `int BaseMaxStacks`: Maximum number of stacks allowed.
      - `StackingMode Stacking`: Defines the stacking behavior.

# Key Behavior & Side Effects
- The `TryAddStack` method recalculates the maximum stacks based on active modifiers and either increases the current stacks or refreshes the duration if the maximum is reached.

# Constraints & Failure Modes
- The maximum number of stacks is recalculated based on active modifiers.
- The current stacks are capped at the maximum stacks.
- If the current stacks are less than the maximum, a stack is added; otherwise, the duration is refreshed.

# Example
```csharp
DoTSettings settings = new DoTSettings();
DoTStatusEffect dotEffect = new DoTStatusEffect(settings);
dotEffect.TryAddStack(source);
```

# Unknowns
- None.
