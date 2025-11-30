# Assets/src/Systems/Skills/SkillImpactBase.cs

_Automatically generated/updated from `Assets/src/Systems/Skills/SkillImpactBase.cs`._

# Purpose
- Defines an abstract base class for skill impact effects in a game system.

# Public API
- Namespace: `CHAL.Systems.Skill`
- Types
  - **public abstract class SkillImpactBase : ScriptableObject**
    - **Public fields/properties**
      - `string EffectId`: Optional unique identifier for debugging or balancing.
    - **Public methods**
      - `abstract void Apply(SkillInstance skill, EffectReceiver source, EffectReceiver target)`: Executes the effect from source to target.
      - `virtual void Apply(SkillInstance skill, EffectReceiver source, EffectReceiver target, HitResult hit)`: Executes the effect from source to target, with an additional hit result parameter.

# Key Behavior & Side Effects
- The `Apply` method is intended to be overridden in derived classes to implement specific skill effects.
- The overloaded `Apply` method calls the primary `Apply` method, allowing for additional behavior based on the `HitResult`.

# Constraints & Failure Modes
- No explicit guards or null handling are defined in this file.

# Example
```csharp
public class FireballImpact : SkillImpactBase
{
    public override void Apply(SkillInstance skill, EffectReceiver source, EffectReceiver target)
    {
        // Implementation of fireball effect
    }
}
```

# Unknowns
- No information on derived classes or specific implementations of `Apply`.
