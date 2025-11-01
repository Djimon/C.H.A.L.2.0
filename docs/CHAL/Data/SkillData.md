# CHAL.Data.SkillData

_Automatically generated/updated from `Assets/src/Data/Defs/SkillData.cs`._

```csharp
// (No code changes; documentation only)
```

Purpose
- Defines a data asset type SkillData as a ScriptableObject for skill configuration.
- Encapsulates identity, behavior, composition, effects, and presentation data for a skill.
- Exposes editor-generated asset creation via CreateAssetMenu with fileName "SkilData" and menu path "Data/SkillData".

Public API
- Namespace/Module: CHAL.Data
- Types
  - public class SkillData : ScriptableObject
    - Public fields
      - string SkillId
        - Identity key for the skill
      - string DisplayName
        - UI name for display
      - float BaseDamage
        - Base damage value (default 1)
      - List<DamageEntry> DamageTypes
        - Damage breakdown/types used by the skill
      - float CastTime
        - Time in seconds to cast; 0 means instant (tooltip: "Time in seconds to cast this skill. 0 = instant.")
      - float Cooldown
        - Cooldown in seconds before this skill can be used again (tooltip: "Cooldown in seconds before this skill can be used again.")
      - SkillType SkillType
        - Main behavior category (e.g., Melee, Projectile, Spell, or Summon)
      - bool isProjectile
        - Indicates if the skill uses a projectile
      - bool isAoE
        - Indicates if the skill is area-of-effect
      - bool hasDuration
        - Indicates if the skill has a duration
      - SkillRange Range
        - Range category (default: SkillRange.Melee)
      - float Duration
        - Duration in seconds for effects like buffs/debuffs/DoTs
      - float ProjectileSpeed
        - Speed of projectile, if applicable
      - int ProjectileCount
        - Number of projectiles spawned, if applicable
      - float AoERadius
        - Radius for AoE effects, if applicable
      - List<SkillImpactBase> OnCastImpactEffects
        - Effects applied immediately when the skill is cast
      - List<SkillImpactBase> OnHitImpactEffects
        - Effects applied when the skill hits a target
      - List<SkillTag> Tags
        - Tags describing the skill (e.g., Projectile, Fire, DoT, Buff)
      - GameObject vfxPrefab
        - Prefab spawned when the skill effect is triggered
      - AnimationType animationType
        - Animation type used when performing this skill

Key Behavior & Side Effects
- No methods or runtime logic defined in this file; acts as a data container.
- Asset is serialized by Unity; default field values are as specified in the code.
- CreateAssetMenu annotation enables editor-based asset creation with the given fileName and menuPath.

Constraints & Failure Modes
- Nullability: List fields (DamageTypes, OnCastImpactEffects, OnHitImpactEffects, Tags) may be null if not assigned in the inspector.
- Editor/runtime separation: Asset creation is editor-supported via CreateAssetMenu; runtime instantiation via ScriptableObject.CreateInstance is possible but assets from disk are separate.
- External types: Depends on types defined elsewhere (DamageEntry, SkillImpactBase, SkillTag, SkillType, SkillRange, AnimationType, etc.).
- Typo in asset name: fileName is "SkilData" (as defined) – note for asset creation/UI consistency.

Example
- Minimal runtime instantiation (not loading an asset from disk)
```csharp
// Runtime example: create a SkillData instance programmatically
var skill = ScriptableObject.CreateInstance<CHAL.Data.SkillData>();
skill.SkillId = "example_skill";
skill.DisplayName = "Example Skill";
```

Unknowns
- Definitions and semantics of external types: DamageEntry, SkillImpactBase, SkillTag, SkillType, SkillRange, AnimationType.
- How SkillData is consumed by other systems (combat, animation, etc.) beyond this file.
- Whether any fields are mutually exclusive or require validation beyond what is shown here.
