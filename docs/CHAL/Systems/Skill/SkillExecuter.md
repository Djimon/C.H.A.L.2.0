# CHAL.Systems.Skill.SkillExecuter

_Automatically generated/updated from `Assets/src/Systems/Skills/SkillExecuter.cs`._

# Purpose
- Defines the `SkillExecutor` class for executing skills in a game system.

# Public API
- Namespace: `CHAL.Systems.Skill`
- Types
  - `public static class SkillExecutor`
    - Public methods:
      - `public static void ExecuteSkill(SkillInstance inst, EffectReceiver source, Transform sourceTr, EffectReceiver target, Transform targetTr)`
        - Executes a skill with specified source and target transforms.
      - `public static void ExecuteSkill(SkillInstance inst, EffectReceiver source, EffectReceiver target)`
        - Executes a skill with specified source and target without transforms.

# Key Behavior & Side Effects
- Validates input parameters; logs errors if `inst` or `source` is null.
- Handles skill execution based on type (Melee, Projectile, Spell, Summon).
- Applies on-cast effects and manages cast time.
- Logs various actions and outcomes during skill execution.
- Applies damage and effects upon hitting a target.

# Constraints & Failure Modes
- Returns early if `inst` or `source` is null in `ExecuteSkill`.
- Validates that the source and target are not the same and checks for friendly fire rules.
- Handles null or empty damage entries by applying fallback damage.

# Example
```csharp
SkillInstance skillInstance = ...; // Assume this is initialized
EffectReceiver source = ...; // Assume this is initialized
EffectReceiver target = ...; // Assume this is initialized
SkillExecutor.ExecuteSkill(skillInstance, source, target);
```

# Unknowns
- The implementation details of `SkillInstance`, `EffectReceiver`, and `DamageEntry` are not provided.
- The behavior of `DebugManager` and its logging levels are not defined in this file.

