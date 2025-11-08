# CHAL.Data.SkillData

_Automatically generated/updated from `Assets/src/Data/Defs/SkillData.cs`._

# Purpose
- Defines a `SkillData` class that represents data for a skill, including its identity, damage, and casting properties.

# Public API
- Namespace: `CHAL.Data`
- Types
  - public class `SkillData` [extends `ScriptableObject`]
    - Public fields/properties:
      - `string SkillId` - Unique identifier for the skill.
      - `string DisplayName` - Name displayed for the skill.
      - `float BaseDamage` - Base damage dealt by the skill (default is 1).
      - `List<DamageEntry> DamageTypes` - Types of damage associated with the skill.
      - `float CastTime` - Time in seconds to cast the skill (default is 0).
      - `float Cooldown` - Cooldown time in seconds before the skill can be reused (default is 2).
      - `SkillType SkillType` - Main behavior of the skill (e.g., Melee, Projectile).
      - `bool isProjectile` - Indicates if the skill is a projectile.
      - `bool isAoE` - Indicates if the skill is an area of effect (AoE) skill.
      - `bool hasDuration` - Indicates if the skill has a duration.
      - `SkillRange Range` - Range of the skill (default is Melee).
      - `float Duration` - Duration in seconds for effects like buffs or debuffs.
      - `float ProjectileSpeed` - Speed of the projectile (if applicable).
      - `int ProjectileCount` - Number of projectiles (if applicable).
      - `float AoERadius` - Radius of the area of effect (if applicable).
      - `List<SkillImpactBase> OnCastImpactEffects` - Effects applied immediately when the skill is cast.
      - `List<SkillImpactBase> OnHitImpactEffects` - Effects applied when the skill successfully hits a target.
      - `List<SkillTag> Tags` - Tags associated with the skill (e.g., Projectile, Fire).
      - `GameObject vfxPrefab` - Prefab spawned when the skill effect is triggered.
      - `AnimationType animationType` - Animation type used when performing the skill.

# Key Behavior & Side Effects
- The `SkillData` class is used to define various properties of a skill, which can affect gameplay mechanics such as damage, casting time, and visual effects.

# Constraints & Failure Modes
- No explicit guards or null/empty handling are defined in the provided code.
- Assumes that all referenced types (e.g., `DamageEntry`, `SkillType`, `SkillImpactBase`, `SkillTag`, `SkillRange`, `AnimationType`) are defined elsewhere in the project.

# Example
```csharp
SkillData skill = ScriptableObject.CreateInstance<SkillData>();
skill.SkillId = "Fireball";
skill.DisplayName = "Fireball";
skill.BaseDamage = 50;
skill.CastTime = 1.5f;
skill.Cooldown = 5f;
skill.SkillType = SkillType.Spell;
```

# Unknowns
- The definitions and behaviors of `DamageEntry`, `SkillType`, `SkillImpactBase`, `SkillTag`, `SkillRange`, and `AnimationType` cannot be determined from this file.
