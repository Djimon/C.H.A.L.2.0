# Assets/src/Systems/Skills/ActiveStatusEffect.cs

_Automatic generated/updated._

```markdown
# Purpose
- Defines the `ActiveStatusEffect` class representing a status effect applied to a target.
- Provides enumerations for `StackingMode` and `StatusType` to categorize effects.

# Public API
- Namespace: `CHAL.Systems.Skill`
- Types
  - **public class ActiveStatusEffect**
    - `public string EffectId` - Identifier for the effect.
    - `public StatusType Kind` - Type of the status effect.
    - `public EffectReceiver source` - The source of the effect.
    - `public EffectReceiver target` - The target of the effect.
    - `public float BaseDuration` - Initial duration of the effect.
    - `public float RemainingTime` - Time remaining for the effect.
    - `public ModifierData Modifier` - Data related to the effect's modifiers.
  - **public enum StackingMode**
    - `RefreshDuration` - Refresh duration without increasing stacks.
    - `AddStacks` - Increase stacks up to a maximum and refresh duration.
    - `IgnoreIfActive` - Ignore if the effect is already active.
    - `Replace` - Replace the existing effect.
  - **public enum StatusType**
    - `DoT` - Damage over time.
    - `Buff` - Positive effect.
    - `Debuff` - Negative effect.
    - `Aura` - Area effect.

# Key Behavior & Side Effects
- The `ActiveStatusEffect` class encapsulates the properties and behaviors of a status effect, including its duration and modifiers.

# Constraints & Failure Modes
- No explicit guards or null/empty handling noted.
- No threading or async considerations present.

# Example
```csharp
ActiveStatusEffect effect = new ActiveStatusEffect
{
    EffectId = "burn",
    Kind = StatusType.DoT,
    BaseDuration = 5.0f,
    RemainingTime = 5.0f,
    Modifier = new ModifierData() // Assuming ModifierData is defined elsewhere
};
```

# Unknowns
- The definition and structure of `EffectReceiver` and `ModifierData` are not provided in this file.
```
