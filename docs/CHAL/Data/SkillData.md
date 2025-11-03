# CHAL.Data.SkillData

_Automatically generated/updated from `Assets/src/Data/Defs/SkillData.cs`._

Purpose
- Defines SkillData as a Unity ScriptableObject in CHAL.Data to configure skills.
- Groups identity, type, composition, effects, metadata, and presentation fields for a skill.
- Exposes a Unity Editor asset menu entry via CreateAssetMenu.

Public API
- Namespace/Module: CHAL.Data
- Type
  - public class SkillData : ScriptableObject
    - public string SkillId; (identity)
    - public string DisplayName; (display name)
    - public float BaseDamage; (base damage; default 1)
    - public List<DamageEntry> DamageTypes; (damage composition by type)
    - public float CastTime; (time in seconds to cast; 0 = instant; default 0)
    - public float Cooldown; (cooldown in seconds; default 2)

    - public SkillType SkillType; (main behavior: Melee, Projectile, Spell, or Summon)
    - public bool isProjectile; (flag; default false)
    - public bool isAoE; (flag; default false)
    - public bool hasDuration; (flag; default false)

    - public SkillRange Range; (range category; default SkillRange.Melee)
    - public float Duration; (duration for buffs/debuffs/DoTs; default 0)
    - public float ProjectileSpeed; (speed of projectile; default 0)
    - public int ProjectileCount; (projectile count; default 0)
    - public float AoERadius; (AoE radius; default 0)

    - public List<SkillImpactBase> OnCastImpactEffects; (immediate effects on cast)
    - public List<SkillImpactBase> OnHitImpactEffects; (effects on hit)

    - public List<SkillTag> Tags; (tags like Projectile, Fire, DoT, Buff, etc.)

    - public GameObject vfxPrefab; (prefab spawned when skill is triggered)
    - public AnimationType animationType; (animation type used when performing the skill)

    - Attributes:
      - [CreateAssetMenu(fileName = "SkilData", menuName = "Data/SkillData")]

Key Behavior & Side Effects
- None defined in this file. No methods or runtime logic.
- Data container used by other systems to drive skill behavior.

Constraints & Failure Modes
- Defaults present for some fields:
  - BaseDamage = 1
  - CastTime = 0f
  - Cooldown = 2f
  - Range = SkillRange.Melee
  - Duration = 0f
  - ProjectileSpeed = 0f
  - ProjectileCount = 0
  - AoERadius = 0f
- Lists (DamageTypes, OnCastImpactEffects, OnHitImpactEffects, Tags) are public but not initialized in code; may be null if not assigned in Inspector or via code.
- Asset creation is via Unity Editor; fileName for asset menu is "SkilData" (potential typo).

Unknowns
- Definitions/behaviors of DamageEntry, SkillImpactBase, SkillTag, SkillType, SkillRange, AnimationType are not in this file.
- How SkillData is consumed by runtime systems or which components rely on which fields.
- Exact null-handling, serialization details, or default initialization behavior beyond explicit defaults.

