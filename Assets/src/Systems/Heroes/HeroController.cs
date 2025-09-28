using CHAL.Data;
using CHAL.Systems.Enemy;
using System;
using UnityEngine;

namespace CHAL.Systems.Hero
{
    public class HeroController : MonoBehaviour, IUnitController
    {
        private HeroInstance heroInstance;

        [SerializeField]
        public HeroDef HeroDef; //{ get; private set; }
        public bool IsAlive => heroInstance != null && heroInstance.CurrentHP > 0;

        // Events
        public event Action<HeroController> OnHeroDied;

        // Platzhalter: spätere Skill-/AI-Komponenten
        public Transform target; // aktuelles Target (EnemyController o.ä.)
        public float attackInterval = 2f;
        private float attackTimer = 0f;

        // Initialisierung
        public void Init(HeroDef def)
        {
            HeroDef = def;
            heroInstance = new HeroInstance(def);

            heroInstance.Team = UnitTeam.Player;

            DebugManager.Log($"[HeroController] Spawned Hero {def.HeroId} ({def.DisplayName})",
                DebugManager.EDebugLevel.Test, "Hero");
        }

        public void Start()
        {
            Init(HeroDef);
        }

        private void Update()
        {
            if (heroInstance == null || !IsAlive)
                return;

            // Effekte ticken lassen
            heroInstance.UpdateEffects(Time.deltaTime);

            // Target validieren oder neu suchen
            if (target == null || target.GetComponent<EnemyController>() == null)
            {
                target = FindNextEnemyTarget();
            }

            // Platzhalter: Autoattack-Loop
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0f && target != null)
            {
                PerformBasicAttack();
                attackTimer = attackInterval;
            }

            // Debug: Per Taste Schaden an sich selbst
            if (Input.GetKeyDown(KeyCode.H))
            {
                TakeDamage(5, DamageType.Physical);
            }
        }

        private Transform FindNextEnemyTarget()
        {
            DebugManager.Log("Test");
            var enemies = FindObjectsByType<EnemyController>(FindObjectsSortMode.None);
            if (enemies.Length == 0) return null;

            if (enemies.Length == 1) return enemies[0].transform;

            Transform best = null;
            float minDist = float.MaxValue;

            foreach (var e in enemies)
            {
                if (e == null || e.EnemyInstance == null) continue;

                float dist = Vector3.Distance(transform.position, e.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    best = e.transform;
                }
            }

            return best;
        }

        public void TakeDamage(float amount, DamageType type)
        {
            if (!IsAlive) return;

            heroInstance.TakeDamage(amount, type);
            DebugManager.Log($"[HeroController] {HeroDef.DisplayName} took {amount} {type} damage (HP={heroInstance.CurrentHP}/{heroInstance.MaxHP})",
                DebugManager.EDebugLevel.Dev, "Hero");

            if (heroInstance.CurrentHP <= 0)
            {
                Die();
            }
        }

        private void PerformBasicAttack()
        {
            if (target == null) return;

            // Platzhalter: Einfach Damage direkt auf Target
            var enemyCtrl = target.GetComponent<EnemyController>();
            if (enemyCtrl != null)
            {
                enemyCtrl.EnemyInstance.TakeDamage(10, DamageType.Physical); // fixer Wert als Test
                DebugManager.Log($"[HeroController] {HeroDef.DisplayName} attacked {enemyCtrl.EnemyData.EnemyId}",
                    DebugManager.EDebugLevel.Test, "Hero");
            }
        }

        private void Die()
        {
            DebugManager.Log($"[HeroController] {HeroDef.DisplayName} died.",
                DebugManager.EDebugLevel.Test, "Hero");

            OnHeroDied?.Invoke(this);

            // später: Animation, Despawn, Cleanup
        }

        public EffectReceiver GetEffectReceiver()
        {
            return heroInstance;
        }
    }
}
