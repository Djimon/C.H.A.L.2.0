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
        public static void ExecuteSkill(SkillInstance inst, EffectReceiver source, Transform sourceTr, EffectReceiver target, Transform targetTr)
        {
            if (inst == null || source == null)
            {
                DebugManager.Error("[SkillExecutor] Invalid skill or source");
                return;
            }

            DebugManager.Log($"[SkillExecutor] {source} starts casting {inst.Data.DisplayName}", DebugManager.EDebugLevel.Test, "Skill");

            Do_OnCastImpactEffects(inst, source);
            Handle_CastTimeHook(inst, source);
            HandleSkillByType(inst, source, sourceTr, target, targetTr);
        }

        private static void HandleSkillByType(SkillInstance inst, EffectReceiver source, Transform sourceTr, EffectReceiver target, Transform targetTr)
        {
            // 3. Apply main effect
            switch (inst.Data.SkillType)
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
                // später: AnimationManager.Play(inst.Data.AnimationType, castTime)
                DebugManager.Log($"[SkillExecutor] {source} casting for {castTime} seconds", DebugManager.EDebugLevel.Dev, "Skill");
            }
        }

        private static void Do_OnCastImpactEffects(SkillInstance inst, EffectReceiver source)
        {
            // 1. OnCast Effects
            if (inst.Data.OnCastImpactEffects != null)
            {
                foreach (var effect in inst.Data.OnCastImpactEffects)
                {
                    effect.Apply(inst, source, source); // self-target for buffs
                }
            }
        }

        public static void ExecuteSkill(SkillInstance inst, EffectReceiver source, EffectReceiver target)
        {
            ExecuteSkill(inst, source, null, target, null);
        }

        private static void ValidateFastReturnRules(EffectReceiver source, EffectReceiver target)
        {
            if (target == null) return;
            if (ReferenceEquals(source, target)) return;
            if (!BalanceManager.Instance.Config.AllowFriendlyFire && source.Team == target.Team) return;
        }

        private static void ApplyMelee(SkillInstance inst, EffectReceiver source, EffectReceiver target)
        {
            ValidateFastReturnRules(source, target);

            DebugManager.Log($"[SkillExecutor] {source} hits {target} with {inst.Data.DisplayName}", DebugManager.EDebugLevel.Test, "Skill");
            ApplyOnHit(inst, source, target);
        }

        private static void ApplySpell(SkillInstance inst, EffectReceiver source, EffectReceiver target, Transform targetTr)
        {
            ValidateFastReturnRules(source, target);

            DebugManager.Log($"[SkillExecutor] {source} casts spell {inst.Data.DisplayName} on {target}", DebugManager.EDebugLevel.Dev, "Skill");
            ApplyOnHit(inst, source, target);
        }

        private static void ApplySummon(SkillInstance inst, EffectReceiver source)
        {
            DebugManager.Log($"[SkillExecutor] {source} summons unit via {inst.Data.DisplayName}", DebugManager.EDebugLevel.Test, "Skill");
            // später: Summon-Controller implementieren
        }


        private static void SpawnProjectile(SkillInstance inst, EffectReceiver source, Transform sourceTr, EffectReceiver target, Transform targetTr)
        {
            DebugManager.Log($"[SkillExecutor] {source} launches projectile {inst.Data.DisplayName} at {target}", DebugManager.EDebugLevel.Test, "Skill");
            // Saubere Fallbacks: Wenn kein Transform mitgegeben wurde, kann man später Prefab-Owner o. ä. nutzen
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

            var go = new GameObject($"Projectile_{inst.Data.DisplayName}");
            var col = go.AddComponent<SphereCollider>(); col.isTrigger = true; col.radius = 0.1f;
            var rb = go.AddComponent<Rigidbody>(); rb.isKinematic = true;

            var pc = go.AddComponent<ProjectileController>();
            pc.transform.position = startPos;
            pc.Init(inst, source, target, dir, speed, life);

            DebugManager.Log($"[SkillExecutor] Spawned projectile {inst.Data.DisplayName} at {startPos} dir {dir} speed {speed} life {life}", DebugManager.EDebugLevel.Test, "Skill");
            // WICHTIG: KEINE OnHit-Effekte hier ausführen — das macht das Projektil bei Kollision
        }

        internal static void ApplyOnHit(SkillInstance skill, EffectReceiver source, EffectReceiver target)
        {
            if (skill == null || skill.Data == null || target == null)
            {
                DebugManager.Log($"Skill or target is null", DebugManager.EDebugLevel.Test, "Combat", LogType.Warning);
                return;
            }

            DoOnHitImpactEffects(skill, source, target);

            float baseDmg = Mathf.Max(0f, skill.Data.BaseDamage);
            var DmgEntries = skill.Data.DamageTypes;

            if (DmgEntries == null || DmgEntries.Count == 0)
                FallbackDamage(skill, target, baseDmg, DmgEntries);
            
            ApplyCompleteDamage(skill, target, baseDmg, DmgEntries);

        }

        private static void ApplyCompleteDamage(SkillInstance skill, EffectReceiver target, float baseDmg, System.Collections.Generic.List<DamageEntry> DmgEntries)
        {
            for (int i = 0; i < DmgEntries.Count; i++)
            {
                var e = DmgEntries[i];

                // negativ = ignorieren
                float m = Mathf.Max(0f, e.DmgMultiplier);
                if (m <= 0f) continue;

                float dmg = baseDmg * m;
                var type = e.DmgType;

                target.TakeDamage(dmg, type);
                DebugManager.Log($"OnHit | {skill.Data.DisplayName} → {target}: {dmg:F1} {type}", DebugManager.EDebugLevel.Test, "Combat");
            }
        }

        private static void FallbackDamage(SkillInstance skill, EffectReceiver target, float baseDmg, System.Collections.Generic.List<DamageEntry> DmgEntries)
        {
            // Fallback: voller BaseDamage als Physical
            target.TakeDamage(baseDmg, DamageType.Physical);
            DebugManager.Log(
                $"OnHit | {skill.Data.DisplayName} → {target} : {baseDmg:F1} Physical",
                DebugManager.EDebugLevel.Test, "Combat"
            );
            return;  
        }

        private static void DoOnHitImpactEffects(SkillInstance skill, EffectReceiver source, EffectReceiver target)
        {
            // 1) OnHit-Effekte (Buff/DoT u.ä.)
            var effects = skill.Data.OnHitImpactEffects;
            if (effects != null && effects.Count > 0)
            {
                for (int i = 0; i < effects.Count; i++)
                    effects[i]?.Apply(skill, source, target);
            }
        }
    }
}
