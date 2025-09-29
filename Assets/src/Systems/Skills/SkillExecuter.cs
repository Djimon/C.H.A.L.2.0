using CHAL.Core;
using CHAL.Data;
using CHAL.Systems.Enemy;
using CHAL.Systems.Hero;
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

            // 1. OnCast Effects
            if (inst.Data.OnCastEffects != null)
            {
                foreach (var effect in inst.Data.OnCastEffects)
                {
                    effect.Apply(inst, source, source); // self-target for buffs
                }
            }

            // 2. Cast time simulation
            float castTime = inst.CastTime;
            if (castTime > 0)
            {
                // später: AnimationManager.Play(inst.Data.AnimationType, castTime)
                DebugManager.Log($"[SkillExecutor] {source} casting for {castTime} seconds", DebugManager.EDebugLevel.Dev, "Skill");
            }

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

        public static void ExecuteSkill(SkillInstance inst, EffectReceiver source, EffectReceiver target)
        {
            ExecuteSkill(inst, source, null, target, null);
        }

        private static void ApplyMelee(SkillInstance inst, EffectReceiver source, EffectReceiver target)
        {
            if (target == null) return;
            if (ReferenceEquals(source, target)) return;
            if (!BalanceManager.Instance.Config.AllowFriendlyFire && source.Team == target.Team) return;

            DebugManager.Log($"[SkillExecutor] {source} hits {target} with {inst.Data.DisplayName}", DebugManager.EDebugLevel.Test, "Skill");
            ApplyOnHit(inst, source, target);
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

            Vector3 startPos = sourceTr.position;
            Vector3 dir;
            if (targetTr != null) dir = (targetTr.position - sourceTr.position);
            else dir = sourceTr.forward;
            if (dir.sqrMagnitude < 0.0001f) dir = sourceTr.forward;
            dir.Normalize();

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

        private static void ApplySpell(SkillInstance inst, EffectReceiver source, EffectReceiver target, Transform targetTr)
        {
            if (target == null) return;
            if (ReferenceEquals(source, target)) return;
            if (!BalanceManager.Instance.Config.AllowFriendlyFire && source.Team == target.Team) return;
            
            DebugManager.Log($"[SkillExecutor] {source} casts spell {inst.Data.DisplayName} on {target}", DebugManager.EDebugLevel.Dev, "Skill");
            ApplyOnHit(inst, source, target);
        }


        private static void ApplySummon(SkillInstance inst, EffectReceiver source)
        {
            DebugManager.Log($"[SkillExecutor] {source} summons unit via {inst.Data.DisplayName}", DebugManager.EDebugLevel.Test, "Skill");
            // später: Summon-Controller implementieren
        }

        internal static void ApplyOnHit(SkillInstance skill, EffectReceiver source, EffectReceiver enemyInstance)
        {
            var list = skill?.Data?.OnHitEffects;
            if (list == null || list.Count == 0) return;
            for (int i = 0; i < list.Count; i++)
            {
                list[i]?.Apply(skill, source, enemyInstance);
            }

            //Deal Damage
 
        }
    }
}
