# Assets/src/Systems/Skills/SkillExecuter.cs

_Automatically generated/updated from `Assets/src/Systems/Skills/SkillExecuter.cs`._

# Purpose
- Defines the `SkillExecutor` class for executing skills from a source to a target, applying effects based on the skill instance.

# Public API
- Namespace: `CHAL.Systems.Skill`
- Types
  - `public static class SkillExecutor`
    - Public methods:
      - `public static void ExecuteSkill(SkillInstance inst, EffectReceiver source, Transform sourceTr, EffectReceiver target, Transform targetTr)`
        - Executes a skill with specified source and target transforms.
      - `public static void ExecuteSkill(SkillInstance inst, EffectReceiver source, EffectReceiver target)`
        - Executes a skill without transforms, using default values.
      - `internal static void ApplyOnHit(SkillInstance skill, EffectReceiver source, EffectReceiver target)`
        - Applies damage and effects when a skill hits a target.
      - `internal static void ApplyOnHit(SkillInstance skill, EffectReceiver source, EffectReceiver target, HitResult hit)`
        - Applies damage and effects when a skill hits a target, considering the hit result.

# Key Behavior & Side Effects
- Validates skill and source before execution; logs errors if invalid.
- Applies on-cast effects and handles cast time.
- Executes skill effects based on skill type (Melee, Projectile, Spell, Summon).
- Handles damage application and effects upon hitting a target.

# Constraints & Failure Modes
- Checks for null values in skill and source; returns early if invalid.
- Prevents friendly fire based on configuration.
- Logs warnings if the source transform is not provided for projectiles.

# Example
```csharp
SkillExecutor.ExecuteSkill(skillInstance, sourceReceiver, targetReceiver);
```

# Unknowns
- None.
