# CHAL.Systems.Skill.DamageImpact

_Automatically generated/updated from `Assets/src/Systems/Skills/Effekte/DamageImpact.cs`._

# Purpose
- Defines the `DamageImpact` class, which applies damage effects in a skill system.

# Public API
- Namespace: `CHAL.Systems.Skill`
- Types
  - public class `DamageImpact` [extends `SkillImpactBase`]
    - Public fields/properties:
      - `List<DamageEntry> Damages`: Damage entries applied by this effect (elemental/physical).
    - Public methods:
      - `void Apply(SkillInstance skill, EffectReceiver source, EffectReceiver target)`: Applies damage to the target based on the skill and damage entries.

# Key Behavior & Side Effects
- Iterates through `Damages` and calculates final damage using `skill.Damage` and `damage.DmgMultiplier`.
- Logs the damage dealt to the target using `DebugManager.Log`.
- Calls `target.TakeDamage(finalDamage, dmgType)` to apply the damage.

# Constraints & Failure Modes
- Assumes `Damages` is not null or empty; behavior on null/empty is not handled.
- No threading or async considerations evident.
- Performance implications not specified.

# Example
```csharp
var damageImpact = new DamageImpact();
damageImpact.Damages = new List<DamageEntry> { /* populate with DamageEntry instances */ };
damageImpact.Apply(skillInstance, sourceReceiver, targetReceiver);
```

# Unknowns
- Details of `DamageEntry`, `SkillInstance`, and `EffectReceiver` types cannot be determined from this file.
- Behavior of `TakeDamage` method is not defined in this file.

