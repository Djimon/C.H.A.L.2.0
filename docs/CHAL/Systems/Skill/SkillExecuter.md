# CHAL.Systems.Skill.SkillExecuter

_Automatically generated/updated from `Assets/src/Systems/Skills/SkillExecuter.cs`._

1) Purpose
- Defines a static SkillExecutor to run skill logic end-to-end.
- Handles cast-time signaling, per-skill-type behavior, and on-hit damage application.
- Spawns projectile-based skills and applies on-hit impact effects.

2) Public API
- Namespace/module
  - CHAL.Systems.Skill

- Types
  - public static class SkillExecutor
    - Public methods
      - public static void ExecuteSkill(SkillInstance inst, EffectReceiver source, Transform sourceTr, EffectReceiver target, Transform targetTr)
        - Validates inputs; logs start; applies on-cast effects; simulates cast time; executes skill by type.
      - public static void ExecuteSkill(SkillInstance inst, EffectReceiver source, EffectReceiver target)
        - Overload delegating to ExecuteSkill(inst, source, null, target, null)

3) Key Behavior & Side Effects
- ExecuteSkill(inst, source, sourceTr, targetTr)
  - If inst == null or source == null: logs error and returns.
  - Logs casting start: “[SkillExecutor] {source} starts casting {inst.Data.DisplayName}”.
  - Do_OnCastImpactEffects(inst, source): applies OnCastImpactEffects if defined.
  - Handle_CastTimeHook(inst, source): if CastTime > 0, logs cast time; no wait implemented.
  - HandleSkillByType(inst, source, sourceTr, target, targetTr): routes to type-specific handlers.
- HandleSkillByType
  - Melee: ApplyMelee(inst, source, target)
  - Projectile: SpawnProjectile(inst, source, sourceTr, target, targetTr)
  - Spell: ApplySpell(inst, source, target, targetTr)
  - Summon: ApplySummon(inst, source)
- Do_OnCastImpactEffects
  - Iterates inst.Data.OnCastImpactEffects and applies each effect (self-target for buffs if provided).
- SpawnProjectile
  - Logs launch; requires non-null sourceTr; otherwise logs warning and returns.
  - Computes startPos and direction via ComputeSpawnAndDirection(sourceTr, targetTr, out startPos, out dir).
  - Creates projectile via CreateProjectile(inst, source, target, startPos, dir).
- CreateProjectile
  - Creates a new GameObject, adds SphereCollider (isTrigger), Rigidbody (isKinematic), ProjectileController.
  - Initializes projectile with Init(inst, source, target, dir, speed, life).
  - Logs spawned projectile details.
- ComputeSpawnAndDirection
  - startPos = sourceTr.position
  - dir = targetTr.position - sourceTr.position if targetTr provided; otherwise sourceTr.forward
  - Normalizes dir; if too small, uses sourceTr.forward
- ApplyOnHit
  - Validates skill, skill.Data, and target; logs and returns if any are null.
  - DoOnHitImpactEffects(skill, source, target)
  - baseDmg = max(0, skill.Data.BaseDamage)
  - DmgEntries = skill.Data.DamageTypes
  - If DmgEntries is null or empty: FallbackDamage(skill, target, baseDmg, DmgEntries)
  - ApplyCompleteDamage(skill, target, baseDmg, DmgEntries)
- ApplyCompleteDamage
  - For each entry in DmgEntries
    - m = max(0, e.DmgMultiplier); skip if m <= 0
    - dmg = baseDmg * m; type = e.DmgType
    - target.TakeDamage(dmg, type); log per hit
- FallbackDamage
  - Target takes baseDmg Physical
  - Logs fallback damage
- DoOnHitImpactEffects
  - If skill.Data.OnHitImpactEffects exists with items, applies each effect(skill, source, target)

4) Constraints & Failure Modes
- Null guards
  - ExecuteSkill: aborts if inst or source is null.
  - ExecuteSkill (overload): delegates to main method.
  - SpawnProjectile: warns and aborts if sourceTr is null.
  - ApplyOnHit: aborts if skill, skill.Data, or target is null.
- Damage handling
  - If no DamageTypes, falls back to base damage as Physical type.
- Casting
  - CastTime is simulated only via logging; no asynchronous wait or animation hook implemented here.
- OnHit/OnCast effects
  - Only applies if effect collections are non-null; null-safe.
- ValidateFastReturnRules
  - Called in melee/spell paths; has no effect on control flow (return/guard is internal to method, not exposed to caller).

5) Example
```csharp
// Example: simple melee skill hit from caster to target
SkillExecutor.ExecuteSkill(skillInstance, caster, casterTransform, target, targetTransform);

// Example: use overload without transforms
SkillExecutor.ExecuteSkill(skillInstance, caster, target);
```

6) Unknowns
- Definitions and behavior of SkillInstance, SkillData, EffectReceiver, DamageEntry, and DamageType beyond usage here.
- Details of OnCastImpactEffects and OnHitImpactEffects implementations.
- Behavior of BalanceManager and its config regarding friendly fire (beyond usage in code).
- ProjectileController implementation and its collision/impact handling.
- Any threading/async expectations or integration with animation systems beyond logging placeholders.
- Exact structures of related types (e.g., SkillType, ProjectileSpeed, Range, DisplayName) are not defined in this file.

