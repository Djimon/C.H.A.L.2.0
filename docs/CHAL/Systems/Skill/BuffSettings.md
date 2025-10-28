# CHAL.Systems.Skill.BuffSettings

_Automatically generated/updated from `Assets/src/Systems/Skills/BuffStatusEffect.cs`._

# Purpose
- Defines the `BuffStatusEffect` class for managing buff effects in a game.
- Provides a `BuffSettings` class for configuring buff parameters.

# Public API
- Namespace: `CHAL.Systems.Skill`
- Types
  - **public class** `BuffStatusEffect` [extends `ActiveStatusEffect`]
    - **public BuffSettings** `Settings` - Configuration settings for the buff.
    - **public int** `CurrentStacks` - Current number of active stacks of the buff.
    - **private int** `CurrentMaxStacks` - Maximum number of stacks allowed.
    - **public StackingMode** `Stacking` - Mode of stacking for the buff.
    - **public bool** `modifierApplied` - Indicates if the modifier has been applied.
    - **public BuffStatusEffect(BuffSettings settings)** - Constructor that initializes the buff with settings.
    - **public void** `TryAddStack(EffectReceiver source)` - Attempts to add a stack to the buff, refreshing duration or increasing stacks based on the stacking mode.

  - **[System.Serializable]**
    - **public class** `BuffSettings`
      - **public string** `EffectId` - Identifier for the buff effect.
      - **public ModifierData** `Modifier` - Data for stat changes during the buff's duration.
      - **public float** `BaseDuration` - Duration of the buff effect.
      - **public int** `BaseMaxStacks` - Maximum number of stacks for the buff.
      - **public StackingMode** `Stacking` - Stacking behavior for the buff.

# Key Behavior & Side Effects
- `TryAddStack` modifies `CurrentStacks` and `RemainingTime` based on the stacking mode.
- If `Stacking` is `AddStacks`, it increments `CurrentStacks` until `CurrentMaxStacks` is reached and refreshes `RemainingTime`.
- If `Stacking` is `RefreshDuration`, it simply refreshes `RemainingTime`.

# Constraints & Failure Modes
- `CurrentStacks` is clamped to `CurrentMaxStacks` to prevent exceeding the maximum.
- The constructor initializes `CurrentMaxStacks` to at least 1 based on `BaseMaxStacks`.

# Example
```csharp
BuffSettings settings = new BuffSettings();
settings.EffectId = "SpeedBoost";
settings.BaseDuration = 10f;
settings.BaseMaxStacks = 3;
BuffStatusEffect buff = new BuffStatusEffect(settings);
buff.TryAddStack(source);
```

# Unknowns
- The behavior of `EffectReceiver` and how it interacts with `BuffStatusEffect` is not defined in this file.
- The implementation details of `ModifierData` and `StackingMode` are not provided.

