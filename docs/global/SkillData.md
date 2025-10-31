# global.SkillData

_Automatically generated/updated from `Assets/src/Data/Defs/SkillData.cs`._

# SkillData.cs Documentation

## Purpose
- Defines the `SkillData` class as a ScriptableObject for skill configuration in the game.
- Provides fields for skill identity, type, composition, effects, and presentation.

## Public API
- Namespace: None specified
- Types
  - public class SkillData : ScriptableObject
    - Public fields/properties:
      - string SkillId
      - string DisplayName
      - float BaseDamage
      - List<DamageEntry> DamageTypes
      - float CastTime
      - float Cooldown
      - SkillType SkillType
      - bool isProjectile
      - bool isAoE
      - bool hasDuration
      - SkillRange Range
      - float Duration
      - float ProjectileSpeed
      - int ProjectileCount
      - float AoERadius
      - List<SkillImpactBase> OnCastImpactEffects
      - List<SkillImpactBase> OnHitImpactEffects
      - List<SkillTag> Tags
      - GameObject vfxPrefab
      - AnimationType animationType

## Key Behavior & Side Effects
- `SkillData` serves as a configuration asset for defining various skills, including their effects and behaviors when used in the game.

## Constraints & Failure Modes
- No explicit guards or null/empty handling noted.
- Assumes valid data is provided for all fields.

## Example
```csharp
SkillData skill = ScriptableObject.CreateInstance<SkillData>();
skill.SkillId = "Fireball";
skill.DisplayName = "Fireball";
skill.BaseDamage = 50;
skill.CastTime = 1.5f;
skill.Cooldown = 5f;
skill.SkillType = SkillType.Spell;
```

## Unknowns
- No information on the behavior of `DamageEntry`, `SkillImpactBase`, `SkillType`, `SkillRange`, `SkillTag`, or `AnimationType`.
- No details on how `OnCastImpactEffects` and `OnHitImpactEffects` are processed during gameplay.

