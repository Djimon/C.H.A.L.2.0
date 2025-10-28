# Assets/src/Systems/Skills/Effekte/DamageImpact.cs

_Automatic generated/updated._

```markdown
## Purpose
- Defines a `DamageImpact` class that applies damage effects in a skill system.

## Public API
- Namespace: `CHAL.Systems.Skill`
- Types
  - **public class DamageImpact** [extends `SkillImpactBase`]
    - **public List<DamageEntry> Damages**: List of damage entries applied by this effect (elemental/physical).
    - **public override void Apply(SkillInstance skill, EffectReceiver source, EffectReceiver target)**: Applies damage to the target based on the skill and damage entries.

## Key Behavior & Side Effects
- Iterates through `Damages` to calculate and apply damage to the target.
- Logs the damage dealt using `DebugManager.Log`.

## Constraints & Failure Modes
- Assumes `Damages` is not null; behavior on null is not handled.
- Requires `EffectReceiver` to implement `TakeDamage` method.

## Example
```csharp
var damageImpact = new DamageImpact();
damageImpact.Damages = new List<DamageEntry> { /* populate with DamageEntry instances */ };
damageImpact.Apply(skillInstance, sourceReceiver, targetReceiver);
```

## Unknowns
- Details of `DamageEntry`, `SkillInstance`, and `EffectReceiver` types cannot be determined from this file.
```
