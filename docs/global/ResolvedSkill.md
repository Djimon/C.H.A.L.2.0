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
      - float Range: Maximum range of the skill.
      - IReadOnlyList<SkillDeliveryTag> Tags: Final set of tags associated with the skill.

    - Public methods:
      - ResolvedSkill(string skillId, string familyId, string moduleId, string coreId, string archetypeId, float damage, float radius, float duration, float cooldown, float castTime, float projectileSpeed, float range, IReadOnlyList<SkillDeliveryTag> tags): Constructor that initializes a new instance of `ResolvedSkill`.

# Key Behavior & Side Effects
- The constructor initializes all properties of the `ResolvedSkill` class with provided values.

# Constraints & Failure Modes
- No explicit guards or error handling are present in the constructor.
- Assumes valid input for all parameters; no null checks are implemented.

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
    100.0f,
    new List<SkillDeliveryTag> { /* tags */ }
);
```

# Unknowns
- The definition and purpose of `SkillDeliveryTag` are not provided in this file.

