# Assets/src/Systems/Skills/ResolvedSkill.cs

_Automatically generated/updated from `Assets/src/Systems/Skills/ResolvedSkill.cs`._

# Purpose
- Defines the `ResolvedSkill` class, which encapsulates skill metadata and runtime values for a skill in a game.

# Public API
- Namespace/module: None

- Types
  - public sealed class ResolvedSkill
    - Public fields/properties:
      - string SkillId: Unique identifier for the skill.
      - string FamilyId: Identifier for the skill family.
      - string ModuleId: Identifier for the skill module.
      - string CoreId: Identifier for the core skill.
      - string ArchetypeId: Identifier for the archetype.
      - float Damage: Damage value of the skill.
      - float Radius: Effective radius of the skill.
      - float Duration: Duration of the skill's effect.
      - float Cooldown: Cooldown time before the skill can be used again.
      - float CastTime: Time taken to cast the skill.
      - float ProjectileSpeed: Speed of the skill's projectile.
      - SkillRange Range: Type of range for the skill (e.g., MeleeRange).
      - float AoERadius: Radius for area of effect.
      - int ProjectileCount: Number of projectiles fired by the skill.
      - IReadOnlyList<SkillDeliveryTag> DeliveryTags: Final set of delivery tags associated with the skill.
      - IReadOnlyList<SkillMechanicTag> MechanicTags: Final set of mechanic tags associated with the skill.
      - DamageType? DamageType: Type of damage associated with the skill.
      - TagContext tagContext: Context containing tags related to the skill.

    - Public methods:
      - ResolvedSkill(string skillId, string familyId, string moduleId, string coreId, string archetypeId, float damage, float radius, float duration, float cooldown, float castTime, float projectileSpeed, SkillRange range, float aoeRadius, int projectileCount, List<DamageEntry> damageEntries, TagContext tags): Constructor that initializes a new instance of `ResolvedSkill`.
      - void UpdateRuntimeValues(float damage, float radius, float duration, float cooldown, float castTime, float projectileSpeed, SkillRange range, float aoeRadius, int projectileCount): Updates runtime values of the skill.
      - float TotalDamage: Calculates and returns the total damage based on damage entries.
      - void AddOrReplaceDamageEntries(List<DamageEntry> entries): Replaces the current damage entries with the provided list.

# Key Behavior & Side Effects
- The constructor initializes all properties of the `ResolvedSkill` class with provided values.
- The `UpdateRuntimeValues` method allows for updating the skill's runtime values.
- The `TotalDamage` property calculates total damage based on the `DamageEntries`, returning the base damage if no entries are present.
- The `AddOrReplaceDamageEntries` method replaces the existing damage entries with a new list.

# Constraints & Failure Modes
- No explicit guards or error handling are present in the constructor.
- Assumes valid input for all parameters; no null checks are implemented.
- The `TotalDamage` property handles null or empty `DamageEntries` by returning the base `Damage`.

# Example
```csharp
var skill = new ResolvedSkill(
    "skill_001",
    "family_001",
    "module_001",
    "core_001",
    "archetype_001",
    50.0f,
    10.0f,
    5.0f,
    2.0f,
    1.0f,
    20.0f,
    SkillRange.MeleeRange,
    5.0f,
    1,
    new List<DamageEntry> { /* damage entries */ },
    new TagContext() /* tags */
);
```

# Unknowns
- The definition and purpose of `SkillDeliveryTag`, `SkillMechanicTag`, `DamageEntry`, `DamageType`, and `TagContext` are not provided in this file.
