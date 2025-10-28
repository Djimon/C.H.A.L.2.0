# Assets/src/Data/Defs/SkillData.cs

_Automatic generated/updated._

```markdown
## Purpose
- Defines the `SkillData` class as a ScriptableObject for skill configuration in the game.

## Public API
- Namespace: `CHAL.Data`
- Types
  - **public class SkillData : ScriptableObject**
    - Public fields/properties:
      - `public string SkillId` - Unique identifier for the skill.
      - `public string DisplayName` - Name displayed for the skill.
      - `public float BaseDamage` - Base damage dealt by the skill.
      - `public List<DamageEntry> DamageTypes` - Types of damage the skill can deal.
      - `public float CastTime` - Time in seconds to cast the skill (0 = instant).
      - `public float Cooldown` - Cooldown time in seconds before the skill can be reused.
      - `public SkillType SkillType` - Main behavior of the skill (e.g., Melee, Projectile).
      - `public bool isProjectile` - Indicates if the skill is a projectile.
      - `public bool isAoE` - Indicates if the skill is an area of effect skill.
      - `public bool hasDuration` - Indicates if the skill has a duration.
      - `public SkillRange Range` - Range type of the skill (e.g., Melee).
      - `public float Duration` - Duration in seconds for effects like buffs or debuffs.
      - `public float ProjectileSpeed` - Speed of the projectile if applicable.
      - `public int ProjectileCount` - Number of projectiles to be fired.
      - `public float AoERadius` - Radius of effect for area skills.
      - `public List<SkillImpactBase> OnCastImpactEffects` - Effects applied when the skill is cast.
      - `public List<SkillImpactBase> OnHitImpactEffects` - Effects applied on a successful hit.
      - `public List<SkillTag> Tags` - Tags associated with the skill (e.g., Projectile, Fire).
      - `public GameObject vfxPrefab` - Prefab for visual effects triggered by the skill.
      - `public AnimationType animationType` - Type of animation used for the skill.

## Key Behavior & Side Effects
- The `SkillData` class serves as a configuration asset for defining various attributes and behaviors of skills in the game.

## Constraints & Failure Modes
- No explicit guards or null handling noted in the file.
- Assumes valid data is provided for all fields.

## Example
```csharp
SkillData skill = ScriptableObject.CreateInstance<SkillData>();
skill.SkillId = "Fireball";
skill.DisplayName = "Fireball";
skill.BaseDamage = 50;
skill.CastTime = 1.5f;
skill.Cooldown = 5f;
```

## Unknowns
- The behavior of `DamageEntry`, `SkillImpactBase`, `SkillType`, `SkillRange`, `SkillTag`, and `AnimationType` cannot be determined from this file.
```
