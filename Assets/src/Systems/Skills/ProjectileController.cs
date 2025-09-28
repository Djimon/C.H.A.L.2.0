using CHAL.Core;
using CHAL.Data;
using CHAL.Systems;
using CHAL.Systems.Enemy;
using CHAL.Systems.Hero;
using CHAL.Systems.Skill;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace CHAL.Systems.Skill
{
    public class ProjectileController : MonoBehaviour
    {
        private Vector3 direction;
        private float speed;
        private float lifespan;

        private SkillInstance skill;
        private EffectReceiver source;
        private EffectReceiver target;

        public void Init(SkillInstance inst, EffectReceiver src, EffectReceiver tgt, Vector3 dir, float projSpeed, float life)
        {
            skill = inst;
            source = src;
            target = tgt;
            direction = dir.normalized;
            speed = projSpeed;
            lifespan = life;

            DebugManager.Log($"[Projectile] Spawned {inst.Data.DisplayName} from {src} towards {tgt}", DebugManager.EDebugLevel.Test, "Projectile");
        }

        private void Update()
        {
            float delta = Time.deltaTime;
            transform.position += direction * speed * delta;
            lifespan -= delta;

            if (lifespan <= 0f)
            {
                DebugManager.Log($"[Projectile] {skill.Data.DisplayName} expired before hitting", DebugManager.EDebugLevel.Dev, "Projectile");
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Unit")) return;
            if (other.gameObject.layer == LayerMask.NameToLayer("Projectile")) return;

            // Alle Units haben einen Controller mit EffectReceiver
            var unitCtrl = other.GetComponent<IUnitController>() ?? other.GetComponentInParent<IUnitController>(); 
            if (unitCtrl == null) return;

            var targetReceiver = unitCtrl.GetEffectReceiver();
            if (targetReceiver == null) return;

            // Self-hit niemals erlaubt
            if (ReferenceEquals(source, targetReceiver)) return;

            // Friendly-Fire global
            if (!BalanceManager.Instance.Config.AllowFriendlyFire && source.Team == targetReceiver.Team) return;

            DebugManager.Log($"[Projectile] {skill.Data.DisplayName} hit {targetReceiver}", DebugManager.EDebugLevel.Test, "Projectile");
            SkillExecutor.ApplyOnHit(skill, source, targetReceiver);

            Destroy(gameObject);
        }
    }
}
