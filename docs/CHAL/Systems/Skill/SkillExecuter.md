# CHAL.Systems.Skill.SkillExecuter

_Automatically generated/updated from `Assets/src/Systems/Skills/SkillExecuter.cs`._

```text
sections: 1-6
```

1) Purpose
- Defines a static helper SkillExecutor to run SkillInstance effects against a source and an optional target.
- Orchestrates cast-time effects, per-skill-type behavior (Melee/Projectile/Spell/Summon), and damage application.
- Exposes two public entry points: ExecuteSkill with optional source/target transforms, and a simpler overload without transforms.

2) Public API
- Namespace/Module
  - CHAL.Systems.Skill

- Types
  - public static class SkillExecutor
    - Public methods
      - public static void ExecuteSkill(SkillInstance inst, EffectReceiver source, Transform sourceTr, EffectReceiver target, Transform targetTr)
        - Executes the given skill instance from source to target, using optional source/target transforms.
        - Side effects: logging, on-cast effects, cast-time hook, type-specific skill handling.
      - public static void ExecuteSkill(SkillInstance inst, EffectReceiver source, EffectReceiver target)
        - Overload; calls ExecuteSkill(inst, source, null, target, null).

3) Key Behavior & Side Effects
- ExecuteSkill(inst, source, sourceTr, targetTr, target)
  - Validates inst and source non-null; logs error and returns otherwise.
  - Logs start of cast: “[SkillExecutor] {source} starts casting {inst.Data.DisplayName}”.
  - Do_OnCastImpactEffects(inst, source)
  - Handle_CastTimeHook(inst, source)
  - HandleSkillByType(inst, source, sourceTr, target, targetTr)

- HandleSkillByType
  - Switches on inst.Data.SkillType:
    - Melee: ApplyMelee(inst, source, target)
    - Projectile: SpawnProjectile(inst, source, sourceTr, target, targetTr)
    - Spell: ApplySpell(inst, source, target, targetTr)
    - Summon: ApplySummon(inst, source)

- Do_OnCastImpactEffects
  - If inst.Data.OnCastImpactEffects != null, applies each effect with (inst, source, source).

- Handle_CastTimeHook
  - Reads inst.CastTime; if > 0, logs a dev message about casting duration.
  - Intended hook for potential animation manager (not implemented here).

- ApplyMelee / ApplySpell
  - Both call ValidateFastReturnRules(source, target) (no-ops in practice)
  - Logs appropriate action (hit or cast) with inst.Data.DisplayName and target/source.
  - Call ApplyOnHit(inst, source, target)

- ApplySummon
  - Logs summoning action; placeholder for future summoning logic.

- SpawnProjectile / ComputeSpawnAndDirection / CreateProjectile
  - SpawnProjectile logs launch; requires sourceTr; warns if sourceTr is null and returns.
  - ComputeSpawnAndDirection derives startPos from sourceTr and dir toward targetTr if available; otherwise uses sourceTr.forward; normalizes with a safe fallback.
  - CreateProjectile constructs a new GameObject named Projectile_{DisplayName}, adds SphereCollider (isTrigger), Rigidbody (isKinematic), ProjectileController; initializes with (inst, source, target, dir, speed, life); logs spawn details.
  - Important note: Do not apply OnHit effects inside projectile creation; OnHit effects are handled when the projectile hits.

- ApplyOnHit
  - Validates skill, skill.Data, and target non-null; logs warning and returns if invalid.
  - DoOnHitImpactEffects(skill, source, target)
  - baseDmg = max(0, skill.Data.BaseDamage)
  - DmgEntries = skill.Data.DamageTypes
  - If DmgEntries null or empty -> FallbackDamage(skill, target, baseDmg, DmgEntries)
  - Otherwise -> ApplyCompleteDamage(skill, target, baseDmg, DmgEntries)

- ApplyCompleteDamage
  - Iterates DmgEntries; for each:
    - m = max(0, e.DmgMultiplier); if m <= 0, skip
    - dmg = baseDmg * m; type = e.DmgType
    - target.TakeDamage(dmg, type)
    - Logs OnHit damage outcome

- FallbackDamage
  - Applies baseDmg as Physical damage if no specific damage entries; logs result.

- DoOnHitImpactEffects
  - If OnHitImpactEffects present, applies each effect with (skill, source, target).

4) Constraints & Failure Modes
- Null handling
  - ExecuteSkill requires non-null inst and source; otherwise logs error and aborts.
  - SpawnProjectile requires a non-null sourceTr; logs warning and aborts if missing.
  - ApplyOnHit requires non-null skill, skill.Data, and target; otherwise logs and returns.
- Shielded/guard logic
  - ValidateFastReturnRules exists but does not return a boolean or block execution; effectively a no-op guard (potential logic gap).
- Damage flow
  - If no DamageTypes, falls back to BaseDamage with Physical type.
  - Negative multipliers are ignored (m <= 0 skip).
- Projectile lifecycle
  - Projectiles are created as separate GameObjects with kinematic rigidbodies; no OnHit effects are applied within creation (handled by ProjectileController on hit).
- Threading/async
  - All behavior is synchronous within Unity’s main thread (no explicit async handling).

5) Example
- Minimal usage (overload without transforms)
```csharp
// Example usage: simple skill execution from source to target
SkillInstance skill = /* obtain skill instance */;
EffectReceiver source = /* obtain source */;
EffectReceiver target = /* obtain target */;
SkillExecutor.ExecuteSkill(skill, source, target);
```
- With explicit transforms
```csharp
SkillInstance skill = /* obtain skill instance */;
EffectReceiver source = /* obtain source */;
Transform sourceTr = source?.transform;
EffectReceiver target = /* obtain target */;
Transform targetTr = target?.transform;

SkillExecutor.ExecuteSkill(skill, source, sourceTr, target, targetTr);
```

6) Unknowns
- Definitions and behavior of:
  - SkillInstance, SkillData, EffectReceiver
  - DebugManager, BalanceManager, DamageEntry, DamageType, SkillType
  - ProjectileController and its hit handling
- How OnCastImpactEffects and OnHitImpactEffects are structured (beyond their Apply method signatures)
- Any external animation/visual systems referenced (AnimationManager placeholder in comment)

```
