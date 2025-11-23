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
- The `Apply` method checks for null values in `skill`, `skill.Data`, and `target`, returning early if any are null.
- If no damage entries are configured, it falls back to applying full damage as Physical damage.
- The method logs the damage dealt to the target for each damage entry.

4) Constraints & Failure Modes
- The method does not apply damage if `baseDamage` is less than or equal to 0.
- If no valid damage entries are found, it defaults to Physical damage.
- Negative multipliers are ignored, and no damage is applied in such cases.

5) Example
```csharp
DamageImpact damageImpact = new DamageImpact();
damageImpact.Damages = new List<DamageEntry> { /* populate with DamageEntry instances */ };
damageImpact.Apply(skillInstance, sourceReceiver, targetReceiver);
```

6) Unknowns
- The structure and properties of `DamageEntry`, `SkillInstance`, and `EffectReceiver` are not defined in this file.
