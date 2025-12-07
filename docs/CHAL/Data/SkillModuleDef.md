# Assets/src/Data/Defs/SkillModuleDef.cs

_Automatically generated/updated from `Assets/src/Data/Defs/SkillModuleDef.cs`._

# Purpose
- Defines a ScriptableObject for skill data, including identity, damage, casting properties, and effects.

# Public API
- Namespace: `CHAL.Data`
- Types
  - public class `SkillModuleDef` [extends ScriptableObject]
    - Public fields/properties:
      - `string SkillId`: Unique identifier for the skill.
      - `string DisplayName`: Name displayed for the skill.
      - `HeroAttribs AttributeAffinity`: Attribute affinity for the skill (default is STR).
      - `float BaseDamage`: Base damage dealt by the skill (default is 1).
      - `DamageType BaseDamageType`: Type of damage (default is Physical).
      - `SkillFamilyDef skillFamily`: Family definition for the skill.
      - `float CastTime`: Time in seconds to cast the skill (default is 0).
      - `float Cooldown`: Cooldown time in seconds before the skill can be reused (default is 2).
      - `SkillType SkillType`: Main behavior of the skill (default is Melee).
      - `bool isProjectile`: Indicates if the skill is a projectile.
      - `bool isAoE`: Indicates if the skill is an area of effect skill.
      - `bool hasDuration`: Indicates if the skill has a duration.
      - `SkillRange Range`: Range of the skill (default is MeleeRange).
      - `float Duration`: Duration in seconds for effects like buffs or debuffs.
      - `float ProjectileSpeed`: Speed of the projectile.
      - `int ProjectileCount`: Number of projectiles.
      - `float AoERadius`: Radius for area of effect.
      - `float damageAttributeScalingFactor`: Scaling factor for damage based on attributes (default is 1.0).
      - `List<SkillImpactBase> OnCastImpact`: Effects applied immediately when the skill is cast.
      - `List<SkillImpactBase> OnHitImpact`: Effects applied when the skill hits a target.
      - `List<SkillImpactBase> OnEndImpact`: Effects applied when the skill ends.
      - `List<SkillDeliveryTag> DeliveryTags`: Tags for skill delivery (e.g., Projectile, Fire).
      - `List<SkillMechanicTag> MechanicTags`: Tags for skill mechanics.
      - `GameObject vfxPrefab`: Prefab for visual effects triggered by the skill.
      - `AnimationType animationType`: Type of animation used for the skill.

# Key Behavior & Side Effects
- Represents various properties and effects associated with a skill, including casting time, cooldown, and impact effects.

# Constraints & Failure Modes
- No explicit guards or null handling noted.
- Assumes valid references for lists and GameObject fields.

# Example
```csharp
SkillModuleDef skill = ScriptableObject.CreateInstance<SkillModuleDef>();
skill.SkillId = "Fireball";
skill.DisplayName = "Fireball";
skill.BaseDamage = 50;
skill.CastTime = 1.5f;
skill.Cooldown = 3f;
skill.SkillType = SkillType.Spell;
```

# Unknowns
- None.

