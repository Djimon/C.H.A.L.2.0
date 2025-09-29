using CHAL.Data;
using CHAL.Systems.Enemy;
using CHAL.Systems.Skill;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CHAL.Systems.Hero
{
    public class HeroController : MonoBehaviour, IUnitController
    {
        [SerializeField]
        public HeroDef HeroDef; //{ get; private set; }
        
        private SkillInstance autoAttack;

        [SerializeField] 
        private List<SkillInstance> socketedSkills = new();

        public List<SkillData> debugSocketSkills = new();


        public Transform target; // aktuelles Target (EnemyController o.ä.)

        private HeroInstance heroInstance;
        private SkillInstance currentSkill;
        private float castRemaining = 0f;

        public bool IsAlive => heroInstance != null && heroInstance.CurrentHP > 0;

        // Events
        public event Action<HeroController> OnHeroDied;

        public void Start()
        {
            if (HeroDef != null && heroInstance == null)
                Init(HeroDef);

            if (socketedSkills == null) socketedSkills = new List<SkillInstance>();

            BuildSkillInstances();

            if (autoAttack == null)
                DebugManager.Log("[HeroController] Warnung: AutoAttack SkillInstance ist nicht gesetzt.", DebugManager.EDebugLevel.Dev, "Hero", LogType.Warning);

        }

        private void BuildSkillInstances()
        {
            socketedSkills.Clear();

            autoAttack = (HeroDef?.Archetype?.primAttackType == PrimaryAttackArchetype.Ranged)
                ? BuildBaseAttackRanged(heroInstance)
                : BuildBaseAttackMelee(heroInstance);

            if (debugSocketSkills != null)
            {
                foreach (var sd in debugSocketSkills)
                    if (sd != null)
                        socketedSkills.Add(new SkillInstance(sd, heroInstance)); // :contentReference[oaicite:3]{index=3}
            }

            DebugManager.Log($"Hero | Built skills: Rotation={socketedSkills.Count}, AutoAttack={(autoAttack != null ? autoAttack.Data.DisplayName : "none")}");
        }

        // Initialisierung
        public void Init(HeroDef def)
        {
            if (def == null)
            {
                DebugManager.Log("[HeroController] Init ohne HeroDef aufgerufen.", DebugManager.EDebugLevel.Dev, "Hero");
                return;
            }

            HeroDef = def;
            heroInstance = new HeroInstance(def);
            heroInstance.Team = UnitTeam.Player;

            DebugManager.Log($"[HeroController] Spawned Hero {def.HeroId} ({def.DisplayName})",
                DebugManager.EDebugLevel.Test, "Hero");
        }

        private void Update()
        {
            if (heroInstance == null || !IsAlive)
                return;

            float dt = Time.deltaTime;

            // Effekte ticken lassen
            heroInstance.UpdateEffects(dt);

            // Target validieren oder neu suchen
            if (target == null || target.GetComponent<EnemyController>() == null)
            {
                target = FindNextEnemyTarget();
            }

            //Cooldowns aller Skill ticken lassen
            foreach (var s in socketedSkills)
                s?.TickCooldown(dt);
            autoAttack?.TickCooldown(dt); 

            if (currentSkill != null)
            {
                castRemaining -= dt;

                // (Optional: HUD Castbar via Gizmos später)
                DebugManager.Log($"UI/HUD | Castbar {currentSkill.Data.DisplayName}: {Mathf.Max(0f, castRemaining):F2}s");

                if (castRemaining <= 0f)
                {
                    // --- Execute ---
                    var enemyCtrl = target ? target.GetComponent<EnemyController>() : null;
                    if (enemyCtrl != null && enemyCtrl.EnemyInstance != null)
                    {
                        // Range-Check unmittelbar vor Execute
                        float dist = Vector3.Distance(transform.position, enemyCtrl.transform.position);
                        if (dist <= currentSkill.Range)
                        {
                            DebugManager.Log($"Combat/Hero | Execute {currentSkill.Data.DisplayName} → {enemyCtrl.EnemyData.EnemyId} (dist={dist:F1}m)");
                            SkillExecutor.ExecuteSkill(
                                currentSkill,
                                heroInstance,
                                transform,
                                enemyCtrl.EnemyInstance,
                                enemyCtrl.transform
                            );
                            // OnHit-Logs erfolgen im Executor/Projectile.
                        }
                        else
                        {
                            DebugManager.Log($"Targeting | Out of Range: {currentSkill.Data.DisplayName} dist={dist:F1}m > {currentSkill.Range:F1}m");
                        }
                    }
                    else
                    {
                        DebugManager.Log($"Targeting | Kein gültiges Ziel für {currentSkill.Data.DisplayName}.");
                    }

                    currentSkill = null; // Cast abgeschlossen
                }

                return; // solange Casting läuft, keine neue Skillwahl
            }

            var next = SelectNextReadySkill();
            if (next == null)
            {
                // Fallback: AutoAttack nur wenn ALLE onCooldown
                if (autoAttack != null && autoAttack.IsReady())
                    next = autoAttack;
            }

            if (next != null && target != null)
            {
                // --- CastStart ---
                DebugManager.Log($"Combat/Hero | CastStart {next.Data.DisplayName} (castTime={next.CastTime:F2}s)");
                currentSkill = next;
                castRemaining = Mathf.Max(0f, next.CastTime);

                // (Phase 4 „Anim“-Hook später; jetzt Log)
                DebugManager.Log($"Anim | Play {next.Data.animationType} len={next.CastTime:F2}s");

                // Cooldown startet beim CastStart (so ist GCD-ähnliches Verhalten möglich)
                next.StartCooldown();
            }

            // Debug: Per Taste Schaden an sich selbst
            if (Input.GetKeyDown(KeyCode.H))
            {
                TakeDamage(5, DamageType.Physical);
            }
        }

        private SkillInstance SelectNextReadySkill()
        {
            // Regel: Nimm den ersten „ready“ in Prioritätsreihenfolge.
            // (Spätere Erweiterung: GCD, „short vs long cast“, Resource-Checks, Flags)
            foreach (var s in socketedSkills)
            {
                if (s == null) continue;
                if (s.IsReady()) return s;
            }
            return null;
        }

        private Transform FindNextEnemyTarget()
        {
            var enemies = FindObjectsByType<EnemyController>(FindObjectsSortMode.None);
            if (enemies == null || enemies.Length == 0) return null;
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

        private void Die()
        {
            DebugManager.Log($"[HeroController] {HeroDef.DisplayName} died.",
                DebugManager.EDebugLevel.Test, "Hero");

            OnHeroDied?.Invoke(this);

            // ToDO: Animation, Despawn, Cleanup
        }

        private SkillInstance BuildBaseAttackMelee(HeroInstance owner)
        {
            var sd = ScriptableObject.CreateInstance<SkillData>();
            sd.SkillId = "base_attack_melee";
            sd.DisplayName = "Base Melee";
            sd.BaseDamage = 5f;
            sd.CastTime = 0.30f;
            sd.Cooldown = 1.20f;
            sd.Range = SkillRange.Melee;
            sd.animationType = AnimationType.MeleeSwing;   // deinen Enum verwenden
                                                      // sd.OnCastEffects = new List<SkillImpactBase>(); // optional
                                                      // sd.OnHitEffects  = new List<SkillImpactBase>(); // optional

            return new SkillInstance(sd, owner);     // nutzt eure Recalculate-Logik
        }

        private SkillInstance BuildBaseAttackRanged(HeroInstance owner)
        {
            var sd = ScriptableObject.CreateInstance<SkillData>();
            sd.SkillId = "base_attack_ranged";
            sd.DisplayName = "Base Ranged";
            sd.BaseDamage = 4f;
            sd.CastTime = 0.20f;
            sd.Cooldown = 1.00f;
            sd.Range = SkillRange.FarDistance;
            sd.animationType = AnimationType.Shoot;  // oder Projectile – passend zu deinem Enum
                                                      // sd.ProjectileSpeed = 18f;              // falls Feld vorhanden
            return new SkillInstance(sd, owner);
        }

        public EffectReceiver GetEffectReceiver()
        {
            return heroInstance;
        }
    }
}
