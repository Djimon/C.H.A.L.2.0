using CHAL.Core;
using CHAL.Data;
using CHAL.Systems.AI;
using CHAL.Systems.Enemy;
using CHAL.Systems.Skill;
using CHAL.Systems.Unit;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CHAL.Systems.Hero
{
/// <summary>
/// Manages the hero's actions and interactions in the game.
/// Implements the IUnitController interface for unit control functionality.
/// </summary>
    public class HeroController : MonoBehaviour, IUnitController
    {
        [SerializeField]
        public HeroDef HeroDef; //{ get; private set; }
        
        //private SkillInstance autoAttack;

        [SerializeField] 
        private List<SkillInstance> socketedSkills = new();

        public List<SkillModuleDef> debugSocketSkills = new();

        private HeroInstance heroInstance;
        public HeroInstance RuntimeHeroInstance => heroInstance;
        private SkillInstance currentSkill;
        private float castRemaining = 0f;

        public bool IsAlive => heroInstance != null && heroInstance.CurrentHP > 0;

        //AI
        private MoveAgent _move;
        private bool _initedMove = false;
        private Transform _currentTarget;

        // Events
        public event Action<HeroController> OnHeroDied;

        private void OnEnable()
        {
            UnitLocator.Instance.Register(this);
        }

        private void OnDisable()
        {
            UnitLocator.Instance.Unregister(this);

            if (heroInstance != null)                  // <â€” NEU: Abo lÃ¶sen
                heroInstance.Died -= OnHeroInstanceDied;
        }

        private void OnDestroy()
        {
            UnitLocator.Instance.Unregister(this);
        }

        public void Start()
        {
            if (HeroDef != null && heroInstance == null)
                Init(HeroDef);

            if (socketedSkills == null) socketedSkills = new List<SkillInstance>();

            //fallback AutoAttack
            socketedSkills.Add(new SkillInstance(HeroDef.fallBackAttack, heroInstance));
            //BuildSkillInstances(); //eher im init(HeroDef)

            //if (autoAttack == null)
            //    DebugManager.Log("[HeroController] Warning: AutoAttack SkillInstance is not set.", DebugManager.EDebugLevel.Dev, "Hero", LogType.Warning);

        }

        //private void BuildSkillInstances()
        //{
        //    //socketedSkills.Clear();

        //    autoAttack = (HeroDef?.Archetype?.primAttackType == PrimaryAttackArchetype.Ranged)
        //        ? BuildBaseAttackRanged(heroInstance)
        //        : BuildBaseAttackMelee(heroInstance);

        //    if (debugSocketSkills != null)
        //    {
        //        foreach (var sd in debugSocketSkills)
        //            if (sd != null)
        //                socketedSkills.Add(new SkillInstance(sd, heroInstance)); // :contentReference[oaicite:3]{index=3}
        //    }

        //    DebugManager.Log($"Hero | Built skills: Rotation={socketedSkills.Count}, AutoAttack={(autoAttack != null ? autoAttack.skillModule.DisplayName : "none")}", DebugManager.EDebugLevel.Debug,"Hero");
        //}

        // Initialisierung
        /// <summary>
        /// Initializes the hero with the specified definition.
        /// </summary>
        /// <param name="def">The hero definition to initialize the hero with.</param>
        public void Init(HeroDef def, HeroProgressData progressData = null)
        {
            if (def == null)
            {
                DebugManager.Log("[HeroController] Init ohne HeroDef aufgerufen.", DebugManager.EDebugLevel.Dev, "Hero");
                return;
            }

            HeroDef = def;

            //TODO: build SkillInstances based on SocketedModules + def.Archetype
            heroInstance = new HeroInstance(def, progressData);
            heroInstance.Team = UnitTeam.Player;

            heroInstance.Died += OnHeroInstanceDied;

            DebugManager.Log(
                $"[HeroController] Spawned Hero {def.HeroId} ({def.DisplayName}) at L{heroInstance.Level} XP={heroInstance.CurrentXP}",
                DebugManager.EDebugLevel.Test,
                "Hero"
            );
        }

        private void Update()
        {
            if (heroInstance == null || !IsAlive)
                return;

            float dt = Time.deltaTime;


            Tick_ReceiverStatusEffects(dt);

            //EnsureTarget(); //Brutforce only for debug

            Targeting();

            DoMovement();

            Tick_SkillCooldown(dt);

            if (IsCasting())
                Advance_CastTimerOrFinish(dt);
            else
                Try_StartNextSkillByRotation();


            Handle_DebugShortcuts();
        }


        private void Tick_ReceiverStatusEffects(float dt)
        {
            // Effekte ticken lassen
            heroInstance.UpdateEffects(dt);
        }


        private void Targeting()
        {
            if (!IsAlive)
            {
                _currentTarget = null;
                return;
            }

            float sight = HeroDef.sightRange; // Def.sightRange oder BalanceConfig.SightRangeDefault
            var myPos = transform.position;

            // Falls wir ein Ziel hatten: ist es noch valide (lebt & in Sicht)?
            if (_currentTarget != null)
            {
                // Lebt es noch?
                var er = _currentTarget.GetComponent<EnemyController>();
                if (er == null || er.GetEffectReceiver().CurrentHP <= 0f || IsOutOfSight(myPos, _currentTarget.position, sight))
                {
                    _currentTarget = null; // Lock lösen
                }
            }

            // Wenn kein Target: neu wÃ¤hlen (Prio v0: Nearest; Alternative HighestHP)
            if (_currentTarget == null)
            {
                var team = heroInstance.Team;

                // WÃ¤hle deine gewÃ¼nschte Prio:
                // Transform t = UnitLocator.Instance.GetHighestHPEnemy(myPos, team, sight);
                Transform t = UnitLocator.Instance != null
                    ? UnitLocator.Instance.GetNearestEnemy(myPos, team, sight)
                    : null;

                if (t != null)
                {
                    _currentTarget = t;
                    DebugManager.DebugLog($"[Hero]{gameObject.name}: has new Target: {_currentTarget.name}", "Combat");
                }
                else
                {
                    _currentTarget = null;
                }
            }
        }

        private void DoMovement()
        {
            EnsureMoveAgentInitialized();

            if (_currentTarget == null || _move == null)
            {
                // Kein Target: optional zum Spawn/Home laufen v0: stehen.
                _move.ClearPathHard();
                _move.StopOrHold();
                return;
            }

            // StoppingDistance = Reach aus nÃ¤chster geplanten Aktion
            float reach = GetPlannedReachOrDefault();
            _move.StoppingDistance = reach;

            Vector3 targetPos = _currentTarget.position;

            // Ranged-Comfort: nur, wenn NICHT gerade caste
            bool isCasting = IsCasting(); 
            if (!isCasting && ShouldDoRangedBackstep(targetPos))
            {
                // v0: kleiner RÃ¼ckschritt entlang -forward Richtung vom Target
                Vector3 dir = (transform.position - targetPos).normalized;
                float RangedComfortMin = 3.0f;
                Vector3 backTarget = transform.position + dir * (RangedComfortMin + 0.5f);

                //TODO: Only for ranged attackers!!
                //_move.SetDestination(backTarget);

                return;
            }

            // Normaler Move bis in StoppingRange
            if (!_move.IsInStoppingRange(targetPos))
                _move.SetDestination(targetPos);
            else
                _move.StopOrHold();
        }

        private void Tick_SkillCooldown(float dt)
        {
            //Cooldowns aller Skill ticken lassen
            foreach (var s in socketedSkills)
                s?.TickCooldown(dt);
        }


        private bool IsCasting()
        {
            return currentSkill != null;
        }

        private void Advance_CastTimerOrFinish(float dt)
        {
            // vormals Inline im Update
            castRemaining -= dt;

            DebugManager.DebugLog($"Advance casttime {castRemaining} for '{currentSkill}' on {_currentTarget.name} ", "Skill");

            if (castRemaining > 0f) return;

            DebugManager.DebugLog($"execute Start {castRemaining} for '{currentSkill}' on {_currentTarget.name} ", "Skill");

            // --- Execute ---
            var enemyCtrl = _currentTarget ? _currentTarget.GetComponent<EnemyController>() : null;

            //TODO: Target in Sight? return 
            //TODO: Target in range? return

            if (enemyCtrl != null && enemyCtrl.EnemyInstance != null)
            {
                float dist = Vector3.Distance(transform.position, enemyCtrl.transform.position);
                float range = GameManager.Instance.Config.GetRangeValue(currentSkill.Range);
                DebugManager.DebugLog($"Range:{currentSkill.Range.ToString()} = {range}");

                if (dist <= range)
                {
                    DebugManager.Log(
                        $"Combat/Hero | Execute {currentSkill.skillModule.DisplayName} on {enemyCtrl.EnemyData.EnemyId} (dist={dist:F1}m)",
                        DebugManager.EDebugLevel.Debug, "Combat"
                    );

                    SkillExecutor.ExecuteSkill(
                        currentSkill,
                        heroInstance,
                        transform,
                        enemyCtrl.EnemyInstance,
                        enemyCtrl.transform
                    );
                }
                else
                {
                    DebugManager.DebugLog($"[HERO] Enemy {_currentTarget.name}: out of Range ({dist}>{range})!", "Combat");
                }
            }
            else
            {
                DebugManager.DebugLog($"[HERO] No valid target", "Combat");
                _currentTarget = null;
            }
                // sonst: kein gültiges Ziel 

            currentSkill = null; // Cast abgeschlossen
        }

        private void Try_StartNextSkillByRotation()
        {
            var next = SelectNextReadySkill();
            ////if (next == null && autoAttack != null && autoAttack.IsReady())
            ////    next = autoAttack;

            if (next == null || _currentTarget == null) return;

            // --- CastStart ---
            currentSkill = next;
            castRemaining = Mathf.Max(0f, next.CastTime);

            DebugManager.Log(
                $"Hero {next.skillModule.SkillId} len={next.CastTime:F2}s", DebugManager.EDebugLevel.Debug, "Skill"
            );

            // Cooldown beim CastStart (wie bisher)
            currentSkill.StartCooldown();
        }

        private void Handle_DebugShortcuts()
        {
            // Debug: Per Taste Schaden an sich selbst
            if (Input.GetKeyDown(KeyCode.H))
            {
                TakeDamage(5, DamageType.Physical);
            }
        }


        private SkillInstance SelectNextReadySkill()
        {
            // Regel: Nimm den ersten â€žreadyâ€œ in PrioritÃ¤tsreihenfolge.
            // (SpÃ¤tere Erweiterung: GCD, â€žshort vs long castâ€œ, Resource-Checks, Flags)
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

/// <summary>
/// Applies damage to the hero based on the specified amount and damage type.
/// </summary>
/// <param name="amount">The amount of damage to apply.</param>
/// <param name="type">The type of damage being inflicted.</param>
        public void TakeDamage(float amount, DamageType type)
        {
            if (!IsAlive) return;

            heroInstance.TakeDamage(amount, type);
            DebugManager.Log($"[HeroController] {HeroDef.DisplayName} took {amount} {type} damage (HP={heroInstance.CurrentHP}/{heroInstance.MaxHP})",
                DebugManager.EDebugLevel.Dev, "Hero");

        }

        private void OnHeroInstanceDied(HeroInstance inst)   // <â€” NEU
        {
            if (inst != heroInstance) return;
            Die();
        }

        private void Die()
        {
            DebugManager.Log($"[HeroController] {HeroDef.DisplayName} died.",
                DebugManager.EDebugLevel.Test, "Hero");

            OnHeroDied?.Invoke(this);

            currentSkill = null;
            _move?.StopOrHold();

            gameObject.SetActive(false);
            Destroy(gameObject);

            // ToDO: Animation, Despawn, Cleanup
        }

        private SkillInstance BuildBaseAttackMelee(HeroInstance owner)
        {
            var sd = ScriptableObject.CreateInstance<SkillModuleDef>();
            sd.SkillId = "base_attack_melee";
            sd.DisplayName = "Base Melee";
            sd.BaseDamage = owner.GetEffectiveBaseDamage();
            sd.CastTime = 0.30f;
            sd.Cooldown = 0.50f;
            sd.Range = SkillRange.Reach;
            sd.animationType = AnimationType.MeleeSwing;   // deinen Enum verwenden

            sd.BaseDamageType = DamageType.Physical;

            return new SkillInstance(sd, owner);     // nutzt eure Recalculate-Logik
        }

        private SkillInstance BuildBaseAttackRanged(HeroInstance owner)
        {
            var sd = ScriptableObject.CreateInstance<SkillModuleDef>();
            sd.SkillId = "base_attack_ranged";
            sd.DisplayName = "Base Ranged";
            sd.BaseDamage = owner.GetEffectiveBaseDamage();
            sd.CastTime = 0.20f;
            sd.Cooldown = 1.00f;
            sd.Range = SkillRange.FarDistance;
            sd.animationType = AnimationType.Shoot;  // oder Projectile â€“ passend zu deinem Enum
            sd.BaseDamageType = DamageType.Physical;

            return new SkillInstance(sd, owner);
        }

        // Liefert die Reach (StoppingDistance) fÃ¼r die NÃ„CHSTE geplante Aktion.
        // v0: Falls du noch keinen Zugriff auf den nÃ¤chsten Skill-Range hast,
        // nimm Melee/Ranged-Heuristik oder den AutoAttack-Typ.
        private float GetPlannedReachOrDefault()
        {
            // TODO: Wenn dein Rotationssystem den nÃ¤chsten Skill/AutoAttack liefert,
            //       gib dessen Range hier zurÃ¼ck (in Metern).
            //       Bis dahin: Heuristik â€“ Hero/Enemy hat typ AutoAttackMelee?
            bool nextIsMelee = true; // <â€” ersetze spÃ¤ter durch echte Abfrage
            float MeleeReachDefault = 1.5f;
            return nextIsMelee ? MeleeReachDefault : 0f;
        }

        // Optional: Ist das Ziel auÃŸerhalb Sicht?
        private static bool IsOutOfSight(Vector3 self, Vector3 target, float sightRange)
        {
            float sr2 = sightRange * sightRange;
            return (target - self).sqrMagnitude > sr2;
        }

        private void EnsureMoveAgentInitialized()
        {
            if (_move == null) _move = GetComponent<MoveAgent>();
            // Init nur einmal, danach kannst du Buffs/Debuffs per ApplyRuntimeSpeed() verÃ¤ndern
            if (_move != null && !_initedMove)
            {
                float baseSpeed = HeroDef.BaseMovementSpeed; // HeroDef.moveSpeed / EnemyDef.moveSpeed
                bool isHero = true;
                _move.Init(baseSpeed, isHero, radius: 0.35f, overridePriority: null);
                _initedMove = true;
            }
        }

        private bool ShouldDoRangedBackstep(Vector3 targetPos)
        {
            // Wenn du NahkÃ¤mpfer bist â†’ nie backstep
            bool isRangedArchetype = true; // TODO: aus Def/Archetype ableiten
            if (!isRangedArchetype) return false;

            float RangedComfortMin = 3.0f; //TODO: global auslagern
            float min = RangedComfortMin;
            float sqr = (targetPos - transform.position).sqrMagnitude;
            return sqr < (min * min);
        }


/// <summary>
/// Retrieves the EffectReceiver associated with the hero instance.
/// </summary>
/// <returns>The EffectReceiver of the hero.</returns>
        public EffectReceiver GetEffectReceiver()
        {
            return heroInstance;
        }
    }
}
