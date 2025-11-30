using CHAL.Core;
using CHAL.Data;
using CHAL.Systems.Enemy;
using CHAL.Systems.Hero;
using CHAL.Systems.Unit;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace CHAL.Systems.Skill
{
    public static class SkillExecutor
    {
/// <summary>
/// Executes a skill from a source to a target, applying effects based on the skill instance.
/// </summary>
/// <param name="inst">The skill instance to execute.</param>
/// <param name="source">The effect receiver initiating the skill.</param>
/// <param name="sourceTr">The transform of the source.</param>
/// <param name="target">The effect receiver that is the target of the skill.</param>
/// <param name="targetTr">The transform of the target.</param>
        public static void ExecuteSkill(SkillInstance inst, EffectReceiver source, Transform sourceTr, EffectReceiver target, Transform targetTr)
        {
            if (inst == null || source == null)
            {
                DebugManager.Error("[SkillExecutor] Invalid skill or source");
                return;
            }

            DebugManager.Log($"[SkillExecutor] {source} starts casting {inst.skillData.DisplayName}", DebugManager.EDebugLevel.Test, "Skill");

            Do_OnCastImpactEffects(inst, source);
            Handle_CastTimeHook(inst, source);
            HandleSkillByType(inst, source, sourceTr, target, targetTr);
        }

        private static void HandleSkillByType(SkillInstance inst, EffectReceiver source, Transform sourceTr, EffectReceiver target, Transform targetTr)
        {
            // 3. Apply main effect
            switch (inst.skillData.SkillType)
            {
                case SkillType.Melee:
                    ApplyMelee(inst, source, target);
                    break;

                case SkillType.Projectile:
                    SpawnProjectile(inst, source, sourceTr, target, targetTr);
                    break;

                case SkillType.Spell:
                    ApplySpell(inst, source, target, targetTr);
                    break;

                case SkillType.Summon:
                    ApplySummon(inst, source);
                    break;
            }
        }

        private static void Handle_CastTimeHook(SkillInstance inst, EffectReceiver source)
        {
            // 2. Cast time simulation
            float castTime = inst.CastTime;
            if (castTime > 0)
            {
                // spÃ¤ter: AnimationManager.Play(inst.Data.AnimationType, castTime)
                DebugManager.Log($"[SkillExecutor] {source} casting for {castTime} seconds", DebugManager.EDebugLevel.Dev, "Skill");
            }
        }

        private static void Do_OnCastImpactEffects(SkillInstance inst, EffectReceiver source)
        {
            // 1. OnCast Effects
            if (inst.skillData.OnCastImpactEffects != null)
            {
                foreach (var effect in inst.skillData.OnCastImpactEffects)
                {
                    effect.Apply(inst, source, source); // self-target for buffs
                }
            }
        }

/// <summary>
/// Executes a skill on a target from a specified source.
/// </summary>
/// <param name="inst">The skill instance to execute.</param>
/// <param name="source">The effect receiver initiating the skill.</param>
/// <param name="target">The effect receiver that is the target of the skill.</param>
        public static void ExecuteSkill(SkillInstance inst, EffectReceiver source, EffectReceiver target)
        {
            ExecuteSkill(inst, source, null, target, null);
        }

        private static bool ValidateFastReturnRules(EffectReceiver source, EffectReceiver target)
        {
            if (target == null)
            {
                DebugManager.Log("[SkillExecutor] FastReturn: target is null", DebugManager.EDebugLevel.Test, "Combat", LogType.Warning);
                return false;
            }
            // Source trifft sich selbst -> in der Regel nicht gewünscht
            if (ReferenceEquals(source, target))
            {
                DebugManager.Log("[SkillExecutor] FastReturn: source == target (self-hit blocked)", DebugManager.EDebugLevel.Test, "Combat");
                return false;
            }

            // Friendly Fire deaktiviert und gleicher Team-Tag -> blocken
            if (!BalanceManager.Instance.Config.AllowFriendlyFire && source.Team == target.Team)
            {
                DebugManager.Log("[SkillExecutor] FastReturn: friendly fire blocked", DebugManager.EDebugLevel.Test, "Combat");
                return false;
            }

            // Alles okay, Skill darf weiterlaufen
            return true;
        }

        private static void ApplyMelee(SkillInstance inst, EffectReceiver source, EffectReceiver target)
        {
            if (!ValidateFastReturnRules(source, target))
                return;

            var hit = HitResolver.Resolve(source, target, inst);

            DebugManager.Log(
                $"[SkillExecutor] {source} attempts melee hit on {target} with {inst.skillData.DisplayName} (IsHit={hit.IsHit}, IsCrit={hit.IsCrit})",
                DebugManager.EDebugLevel.Test,
                "Skill");

            ApplyOnHit(inst, source, target, hit);
        }

        private static void ApplySpell(SkillInstance inst, EffectReceiver source, EffectReceiver target, Transform targetTr)
        {
            if (!ValidateFastReturnRules(source, target))
                return;

            var hit = HitResolver.Resolve(source, target, inst);

            DebugManager.Log(
                $"[SkillExecutor] {source} casts spell {inst.skillData.DisplayName} on {target} (IsHit={hit.IsHit}, IsCrit={hit.IsCrit})",
                DebugManager.EDebugLevel.Dev,
                "Skill");

            ApplyOnHit(inst, source, target, hit); ;
        }

        private static void ApplySummon(SkillInstance inst, EffectReceiver source)
        {
            DebugManager.Log($"[SkillExecutor] {source} summons unit via {inst.skillData.DisplayName}", DebugManager.EDebugLevel.Test, "Skill");
            //TODO:: Summon-mechanik implementieren evlt über SuumonController?
        }


        private static void SpawnProjectile(SkillInstance inst, EffectReceiver source, Transform sourceTr, EffectReceiver target, Transform targetTr)
        {
            DebugManager.Log($"[SkillExecutor] {source} launches projectile {inst.skillData.DisplayName} at {target}", DebugManager.EDebugLevel.Test, "Skill");
            // Saubere Fallbacks: Wenn kein Transform mitgegeben wurde, kann man spÃ¤ter Prefab-Owner o. Ã¤. nutzen
            if (sourceTr == null)
            {
                DebugManager.Warning("[SkillExecutor] SpawnProjectile: source Transform not provided", "Skill");
                return;
            }

            Vector3 startPos, dir;
            ComputeSpawnAndDirection(sourceTr, targetTr, out startPos, out dir);
            CreateProjectile(inst, source, target, startPos, dir);
        }

        
        private static void ComputeSpawnAndDirection(Transform sourceTr, Transform targetTr, out Vector3 startPos, out Vector3 dir)
        {
            startPos = sourceTr.position;
            if (targetTr != null) dir = (targetTr.position - sourceTr.position);
            else dir = sourceTr.forward;
            if (dir.sqrMagnitude < 0.0001f) dir = sourceTr.forward;
            dir.Normalize();
        }

        private static void CreateProjectile(SkillInstance inst, EffectReceiver source, EffectReceiver target, Vector3 startPos, Vector3 dir)
        {
            float speed = Mathf.Max(0.01f, inst.ProjectileSpeed);
            float life = Mathf.Max(0.1f, inst.Range / speed);

            var go = new GameObject($"Projectile_{inst.skillData.DisplayName}");
            var col = go.AddComponent<SphereCollider>(); col.isTrigger = true; col.radius = 0.1f;
            var rb = go.AddComponent<Rigidbody>(); rb.isKinematic = true;

            var pc = go.AddComponent<ProjectileController>();
            pc.transform.position = startPos;
            pc.Init(inst, source, target, dir, speed, life);

            DebugManager.Log($"[SkillExecutor] Spawned projectile {inst.skillData.DisplayName} at {startPos} dir {dir} speed {speed} life {life}", DebugManager.EDebugLevel.Test, "Skill");
            // WICHTIG: KEINE OnHit-Effekte hier ausfÃ¼hren â€” das macht das Projektil bei Kollision
        }

        internal static void ApplyOnHit(SkillInstance skill, EffectReceiver source, EffectReceiver target)
        {
            if (skill == null || skill.skillData == null || target == null)
            {
                DebugManager.Log($"[SkillExecutor] ApplyOnHit aborted: skill or target is null", DebugManager.EDebugLevel.Test, "Combat", LogType.Warning);
                return;
            }

            // WICHTIG:
            // - Kein direkter Schaden mehr in ApplyOnHit.
            // - Schaden wird ausschließlich über OnHitImpactEffects ausgeführt,
            //   typischerweise über einen DamageImpact.
            //
            // Diese Methode übernimmt nur noch Routing/Triggering.
            var hit = HitResolver.Resolve(source, target, skill);

            DoOnHitImpactEffects(skill, source, target, hit);
        }

        internal static void ApplyOnHit(SkillInstance skill, EffectReceiver source, EffectReceiver target, HitResult hit)
        {
            if (skill == null || skill.skillData == null || target == null)
            {
                DebugManager.Log("[SkillExecutor] ApplyOnHit aborted: skill/target null",
                    DebugManager.EDebugLevel.Test, "Combat", LogType.Warning);
                return;
            }

            if (!hit.IsHit)
            {
                DebugManager.Log(
                    $"[SkillExecutor] Hit missed: {source} -> {target} with {skill.skillData.DisplayName}",
                    DebugManager.EDebugLevel.Test,
                    "Combat");
                // TODO: OnMiss/OnDodge-Effekte hier triggern, falls gewünscht.
                return;
            }

            // Kein direkter Schaden mehr hier – nur Routing!
            DoOnHitImpactEffects(skill, source, target, hit);
        }


        private static void DoOnHitImpactEffects(SkillInstance skill, EffectReceiver source, EffectReceiver target, HitResult hit)
        {
            // 1) OnHit-Effekte (Buff/DoT/Damage etc.)
            var effects = skill.skillData.OnHitImpactEffects;
            if (effects != null && effects.Count > 0)
            {
                for (int i = 0; i < effects.Count; i++)
                {
                    var effect = effects[i];
                    if (effect == null)
                        continue;

                    effect.Apply(skill, source, target, hit);
                }
            }
        }

        private static void DoOnHitImpactEffects(SkillInstance skill, EffectReceiver source, EffectReceiver target)
        {
            var defaultHit = HitResult.CreateDefault(skill, source, target);
            DoOnHitImpactEffects(skill, source, target, defaultHit);
        }
    }
}
