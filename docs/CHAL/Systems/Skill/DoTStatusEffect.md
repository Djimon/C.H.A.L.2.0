# Assets/src/Systems/Skills/DoTStatusEffect.cs

_Automatically generated/updated from `Assets/src/Systems/Skills/DoTStatusEffect.cs`._

# Purpose
- Defines a damage-over-time (DoT) status effect that can stack over time.
- Inherits from `ActiveStatusEffect` and manages its own stacking behavior.

# Public API
- Namespace: `CHAL.Systems.Skill`
- Types
  - **public class DoTStatusEffect** [extends ActiveStatusEffect]
    - **public DoTSettings DoTsettings**: Configuration settings for the DoT effect.
    - **public int CurrentStacks**: Current number of stacks of the DoT effect.
    - **private int CurrentMaxStacks**: Maximum number of stacks allowed.
    - **public float internalTickTimer**: Timer for the internal tick interval.
    - **public StackingMode Stacking**: Defines the stacking behavior.
    - **public DoTStatusEffect(DoTSettings settings)**: Constructor that initializes the DoT effect with settings.
    - **public void TryAddStack(EffectReceiver source)**: Attempts to add a stack or refresh duration based on the source's modifiers.

  - **[System.Serializable] public class DoTSettings**
    - **public string EffectId**: Identifier for the effect.
    - **public DamageType DamageType**: Type of damage inflicted by the effect.
    - **public float DamagePerTick**: Amount of damage dealt per tick.
    - **public float TickInterval**: Time interval between ticks.
    - **public float BaseDuration**: Duration of the effect.
    - **public int BaseMaxStacks**: Maximum number of stacks allowed by default.
    - **public StackingMode Stacking**: Defines the stacking behavior.

# Key Behavior & Side Effects
- The `TryAddStack` method recalculates the maximum stacks based on active modifiers and either increases the current stacks or refreshes the duration if the maximum is reached.

# Constraints & Failure Modes
- The maximum stacks are recalculated based on modifiers from the `EffectReceiver`.
- The current stacks are capped at the maximum stacks during the addition process.

# Example
```csharp
DoTSettings settings = new DoTSettings();
DoTStatusEffect dotEffect = new DoTStatusEffect(settings);
dotEffect.TryAddStack(source);
```

# Unknowns
- None.
