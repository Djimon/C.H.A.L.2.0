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
      - `void Apply(SkillInstance skill, EffectReceiver source, EffectReceiver target, HitResult hit)`: Applies the skill effect to the target, dealing damage based on the skill and damage multipliers.
      - `void Apply(SkillInstance skill, EffectReceiver source, EffectReceiver target)`: Applies the skill effect from the source to the target, resolving the hit result internally.

3) Key Behavior & Side Effects
- The `Apply(SkillInstance skill, EffectReceiver source, EffectReceiver target, HitResult hit)` method checks for null values in `skill`, `skill.skillData`, and `target`, returning early if any are null.
- If no damage entries are configured, it returns early without applying damage.
- The method logs the damage dealt to the target for each damage entry using `DebugManager.Log`.
- The method calls `target.TakeDamage(packet)` to apply the damage to the target.
- The `Apply(SkillInstance skill, EffectReceiver source, EffectReceiver target)` method resolves the hit result using `CombatCalculator.Resolve` before calling the other `Apply` method.

4) Constraints & Failure Modes
- The method does not apply damage if `packet.DamagePerType` is empty.
- If no valid damage entries are found, it returns early without applying damage.
- Negative multipliers are ignored, and no damage is applied in such cases.

5) Example
```csharp
DamageImpact damageImpact = new DamageImpact();
damageImpact.Damages = new List<DamageEntry> { /* populate with DamageEntry instances */ };
damageImpact.Apply(skillInstance, sourceReceiver, targetReceiver, hitResult);
damageImpact.Apply(skillInstance, sourceReceiver, targetReceiver); // uses internal hit resolution
```

6) Unknowns
- The structure and properties of `DamageEntry`, `SkillInstance`, `EffectReceiver`, and `HitResult` are not defined in this file.
