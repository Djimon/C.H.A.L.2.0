# CHAL.Systems.Skill.DoTStatusEffect

_Automatically generated/updated from `Assets/src/Systems/Skills/DoTStatusEffect.cs`._

# Purpose
- Defines the `DoTStatusEffect` class for managing damage-over-time (DoT) effects in a game.
- Provides the `DoTSettings` class for configuring DoT effect parameters.

# Public API
- Namespace: `CHAL.Systems.Skill`
- Types
  - **public class DoTStatusEffect** [extends ActiveStatusEffect]
    - **public DoTSettings DoTsettings**: Configuration settings for the DoT effect.
    - **public int CurrentStacks**: Current number of stacks of the DoT effect.
    - **private int CurrentMaxStacks**: Maximum number of stacks allowed.
    - **public float internalTickTimer**: Timer for the internal tick interval.
    - **public StackingMode Stacking**: Mode for stacking behavior.
    - **public DoTStatusEffect(DoTSettings settings)**: Constructor that initializes the DoT effect with settings.
    - **public void TryAddStack(EffectReceiver source)**: Attempts to add a stack to the DoT effect or refresh its duration.

  - **[System.Serializable] public class DoTSettings**
    - **public string EffectId**: Identifier for the DoT effect.
    - **public DamageType DamageType**: Type of damage inflicted by the DoT.
    - **public float DamagePerTick**: Amount of damage dealt per tick.
    - **public float TickInterval**: Time interval between ticks.
    - **public float BaseDuration**: Base duration of the DoT effect.
    - **public int BaseMaxStacks**: Base maximum number of stacks.
    - **public StackingMode Stacking**: Stacking behavior configuration.

# Key Behavior & Side Effects
- The `TryAddStack` method recalculates the maximum stacks based on active modifiers and either increases the current stacks or refreshes the duration if the maximum is reached.

# Constraints & Failure Modes
- The `CurrentMaxStacks` is recalculated based on active modifiers, which may affect the stacking behavior.
- The method ensures that `CurrentStacks` does not exceed `CurrentMaxStacks`.

# Example
```csharp
DoTSettings settings = new DoTSettings();
DoTStatusEffect dotEffect = new DoTStatusEffect(settings);
dotEffect.TryAddStack(source);
```

# Unknowns
- The implementation details of `ActiveStatusEffect`, `EffectReceiver`, and `ModifierTarget` are not provided in this file.
- The behavior of `StackingMode` and `DamageType` is not defined in this file.

