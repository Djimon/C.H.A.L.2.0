# Assets/src/Systems/Skills/SkillExecuter.cs

_Automatic generated/updated._

```markdown
# Purpose
- Defines the `SkillExecutor` class for executing skills in a game system.

# Public API
- Namespace: `CHAL.Systems.Skill`
- Types
  - **public static class** `SkillExecutor`
    - **public static void** `ExecuteSkill(SkillInstance inst, EffectReceiver source, Transform sourceTr, EffectReceiver target, Transform targetTr)`
      - Executes a skill with specified source and target transforms.
    - **public static void** `ExecuteSkill(SkillInstance inst, EffectReceiver source, EffectReceiver target)`
      - Executes a skill with specified source and target without transforms.

# Key Behavior & Side Effects
- Validates input parameters and logs errors if `inst` or `source` is null.
- Applies on-cast effects, handles cast time, and executes skill based on its type (Melee, Projectile, Spell, Summon).
- Logs various actions taken during skill execution, including casting and damage application.
- Handles friendly fire rules based on team configuration.

# Constraints & Failure Modes
- Returns early if `target` is null or if `source` and `target` are the same when friendly fire is not allowed.
- Logs warnings if the source transform is not provided when spawning a projectile.
- Ensures that damage multipliers are non-negative and applies fallback damage if no damage entries are present.

# Example
```csharp
SkillExecutor.ExecuteSkill(skillInstance, effectReceiverSource, effectReceiverTarget);
```

# Unknowns
- The implementation details of `EffectReceiver`, `SkillInstance`, and `DamageEntry` are not provided in this file.
- The behavior of `DebugManager` and its logging levels are not defined in this file.
```
