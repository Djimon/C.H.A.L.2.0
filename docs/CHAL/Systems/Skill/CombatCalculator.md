# Assets/src/Systems/Skills/CombatCalculator.cs

_Automatically generated/updated from `Assets/src/Systems/Skills/CombatCalculator.cs`._

# Purpose
- Defines a static class `CombatCalculator` for resolving combat mechanics in a skill-based system.

# Public API
- Namespace: `CHAL.Systems.Skill`
- Types
  - **public static class** `CombatCalculator`
    - **public static HitResult** `ResolveHit(SkillInstance skill, EffectReceiver attacker, EffectReceiver defender)` 
      - Resolves a hit based on the attacker, defender, and skill.
    - **public static float** `ComputeFinalDamageScalar(SkillInstance skill, HitResult hit)` 
      - Computes the final damage scalar based on the skill's damage and hit result.
    - **public static DamagePacket** `BuildDamagePacket(SkillInstance skill, EffectReceiver attacker, EffectReceiver defender, HitResult hit)` 
      - Builds a damage packet containing damage information based on the skill and hit result.
    - **public static HitResult** `Resolve(EffectReceiver attacker, EffectReceiver defender, SkillInstance skill)` 
      - Resolves the hit result based on the attacker, defender, and skill.

# Key Behavior & Side Effects
- `ResolveHit` delegates hit resolution to the `Resolve` method.
- `ComputeFinalDamageScalar` returns 0 if the skill's damage is null or empty or if the hit is not successful.
- `BuildDamagePacket` creates a `DamagePacket` and adds damage entries based on the skill's damage and hit result.

# Constraints & Failure Modes
- Methods handle null or empty skill damage lists by returning default values (0 or empty packets).
- The `Resolve` method currently assumes hits always succeed and crits never occur until further implementation is added.

# Example
```csharp
var hitResult = CombatCalculator.ResolveHit(skillInstance, attacker, defender);
float damageScalar = CombatCalculator.ComputeFinalDamageScalar(skillInstance, hitResult);
var damagePacket = CombatCalculator.BuildDamagePacket(skillInstance, attacker, defender, hitResult);
```

# Unknowns
- The implementation details for accuracy, evasion, crit chance, and crit multiplier are placeholders and not yet defined.
