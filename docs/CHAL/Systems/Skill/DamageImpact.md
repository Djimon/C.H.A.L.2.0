# Assets/src/Systems/Skills/Effekte/DamageImpact.cs

_Automatically generated/updated from `Assets/src/Systems/Skills/Effekte/DamageImpact.cs`._

1) Purpose
- Defines the `DamageImpact` class representing the damage impact of a skill, including various damage entries.

2) Public API
- Namespace: `CHAL.Systems.Skill`
- Types
  - public class `DamageImpact` [extends `SkillImpactBase`]
    - Public fields/properties:
      - `List<DamageEntry> Damages`: Damage entries applied by this effect (elemental/physical).
    - Public methods:
      - `void Apply(SkillInstance skill, EffectReceiver source, EffectReceiver target)`: Applies the skill effect to the target, dealing damage based on the skill and damage multipliers.

3) Key Behavior & Side Effects
- The `Apply` method iterates through each damage entry, calculates the final damage, logs the damage dealt, and applies the damage to the target.

4) Constraints & Failure Modes
- No explicit guards or null handling noted in the provided code.

5) Example
```csharp
DamageImpact damageImpact = new DamageImpact();
damageImpact.Damages = new List<DamageEntry> { /* populate with DamageEntry instances */ };
damageImpact.Apply(skillInstance, sourceReceiver, targetReceiver);
```

6) Unknowns
- The structure and properties of `DamageEntry`, `SkillInstance`, and `EffectReceiver` are not defined in this file.
