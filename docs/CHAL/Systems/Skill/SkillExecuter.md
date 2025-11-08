# CHAL.Systems.Skill.SkillExecuter

_Automatically generated/updated from `Assets/src/Systems/Skills/SkillExecuter.cs`._

# Purpose
- Defines the `SkillExecutor` class for executing skills from a source to a target, applying effects based on the skill instance.

# Public API
- Namespace: `CHAL.Systems.Skill`
- Types
  - **static class** `SkillExecutor`
    - **Public methods**
      - `static void ExecuteSkill(SkillInstance inst, EffectReceiver source, Transform sourceTr, EffectReceiver target, Transform targetTr)`
        - Executes a skill with a specified source and target, applying effects based on the skill instance.
      - `static void ExecuteSkill(SkillInstance inst, EffectReceiver source, EffectReceiver target)`
        - Executes a skill on a target from a specified source.

# Key Behavior & Side Effects
- Validates skill and source before execution; logs errors if invalid.
- Applies on-cast effects, handles cast time, and executes skill effects based on skill type (Melee, Projectile, Spell, Summon).
- Logs various actions and outcomes during skill execution, including casting, hitting, and spawning projectiles.

# Constraints & Failure Modes
- Returns early if `inst` or `source` is null in `ExecuteSkill`.
- Handles friendly fire rules based on team configuration.
- Ensures that damage is only applied if valid damage entries exist; falls back to physical damage if none are present.

# Example
```csharp
SkillExecutor.ExecuteSkill(skillInstance, effectReceiverSource, effectReceiverTarget);
```

# Unknowns
- None.

