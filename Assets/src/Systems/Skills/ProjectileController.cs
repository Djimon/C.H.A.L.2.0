using CHAL.Core;
using CHAL.Systems.Unit;
using UnityEngine;

namespace CHAL.Systems.Skill
{
/// <summary>
/// Manages the behavior and movement of projectiles in the game.
/// </summary>
    public class ProjectileController : MonoBehaviour
    {
        private Vector3 direction;
        private float speed;
        private float lifespan;

        private SkillInstance skill;
        private EffectReceiver source;
        private EffectReceiver target;

/// <summary>
/// Initializes the projectile with the specified parameters.
/// </summary>
/// <param name="inst">The skill instance associated with the projectile.</param>
/// <param name="src">The effect receiver that is the source of the projectile.</param>
/// <param name="tgt">The effect receiver that is the target of the projectile.</param>
/// <param name="dir">The direction in which the projectile will move.</param>
/// <param name="projSpeed">The speed of the projectile.</param>
/// <param name="life">The lifespan of the projectile.</param>
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
            MoveForward();

            Check_LifetimeExpiration();
        }

        private void MoveForward()
        {
            float delta = Time.deltaTime;
            transform.position += direction * speed * delta;
            lifespan -= delta;
        }

        private void Check_LifetimeExpiration()
        {
            if (lifespan <= 0f)
            {
                DebugManager.Log($"[Projectile] {skill.Data.DisplayName} expired before hitting", DebugManager.EDebugLevel.Dev, "Projectile");
                Destroy(gameObject);
            }
        }


        private void OnTriggerEnter(Collider other)
        {
            EffectReceiver targetReceiver;
            ValidateFastReturns(other,out targetReceiver);

            //TODO: nur markierte targets treffen?
            // vergleich target mit targetReceiver

            SkillExecutor.ApplyOnHit(skill, source, targetReceiver);
            DebugManager.Log($"[Projectile] {skill.Data.DisplayName} hit {targetReceiver}", DebugManager.EDebugLevel.Test, "Projectile");

            Destroy(gameObject);
        }

        private void ValidateFastReturns(Collider other, out EffectReceiver targRE)
        {
            targRE = null;
            if (!other.CompareTag("Unit"))
                return;
            if (other.gameObject.layer == LayerMask.NameToLayer("Projectile"))
                return;

            // Alle Units haben einen Controller mit EffectReceiver
            var unitCtrl = other.GetComponent<IUnitController>() ?? other.GetComponentInParent<IUnitController>();
            if (unitCtrl == null)
                return;

            targRE = unitCtrl.GetEffectReceiver();
            if (targRE == null)
                return;

            // Self-hit niemals erlaubt
            if (ReferenceEquals(source, targRE))
                return;

            // Friendly-Fire global
            if (!BalanceManager.Instance.Config.AllowFriendlyFire && source.Team == targRE.Team)
                return;
        }
    }
}
