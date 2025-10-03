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
        }

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

            DebugManager.Log($"Hero | Built skills: Rotation={socketedSkills.Count}, AutoAttack={(autoAttack != null ? autoAttack.Data.DisplayName : "none")}", DebugManager.EDebugLevel.Debug,"Hero");
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

        private void EnsureTarget()
        {
            // Target validieren oder neu suchen
            if (target == null || target.GetComponent<EnemyController>() == null)
            {
                target = FindNextEnemyTarget();
            }
        }

        private void Targeting()
        {
            if (!IsAlive) { _currentTarget = null; return; }

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

            // Wenn kein Target: neu wählen (Prio v0: Nearest; Alternative HighestHP)
            if (_currentTarget == null)
            {
                var team = heroInstance.Team;

                // Wähle deine gewünschte Prio:
                // Transform t = UnitLocator.Instance.GetHighestHPEnemy(myPos, team, sight);
                Transform t = UnitLocator.Instance != null
                    ? UnitLocator.Instance.GetNearestEnemy(myPos, team, sight)
                    : null;

                _currentTarget = t;
            }

        }

        private void DoMovement()
        {
            EnsureMoveAgentInitialized();

            if (_currentTarget == null || _move == null)
            {
                // Kein Target: optional zum „Spawn/Home“ laufen – v0: stehen.
                _move.StopOrHold();
                return;
            }

            // StoppingDistance = Reach aus nächster geplanten Aktion
            float reach = GetPlannedReachOrDefault();
            _move.StoppingDistance = reach;

            Vector3 targetPos = _currentTarget.position;

            // Ranged-Comfort: nur, wenn NICHT gerade caste
            bool isCasting = IsCasting(); 
            if (!isCasting && ShouldDoRangedBackstep(targetPos))
            {
                // v0: kleiner Rückschritt entlang -forward Richtung vom Target
                Vector3 dir = (transform.position - targetPos).normalized;
                float RangedComfortMin = 3.0f;
                Vector3 backTarget = transform.position + dir * (RangedComfortMin + 0.5f);
                _move.SetDestination(backTarget);
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
            autoAttack?.TickCooldown(dt);
        }


        private bool IsCasting()
        {
            return currentSkill != null;
        }

        private void Advance_CastTimerOrFinish(float dt)
        {
            // vormals Inline im Update
            castRemaining -= dt;

            DebugManager.Log(
                $"UI/HUD | Castbar {currentSkill.Data.DisplayName}: {Mathf.Max(0f, castRemaining):F2}s",
                DebugManager.EDebugLevel.Debug, "UI"
            );

            if (castRemaining > 0f) return;

            // --- Execute ---
            var enemyCtrl = target ? target.GetComponent<EnemyController>() : null;

            //TODO: Target in Sight? return 
            //TODO: Target in range? return

            if (enemyCtrl != null && enemyCtrl.EnemyInstance != null)
            {
                float dist = Vector3.Distance(transform.position, enemyCtrl.transform.position);
                if (dist <= currentSkill.Range)
                {
                    DebugManager.Log(
                        $"Combat/Hero | Execute {currentSkill.Data.DisplayName} → {enemyCtrl.EnemyData.EnemyId} (dist={dist:F1}m)",
                        DebugManager.EDebugLevel.Debug, "Fight"
                    );

                    SkillExecutor.ExecuteSkill(
                        currentSkill,
                        heroInstance,
                        transform,
                        enemyCtrl.EnemyInstance,
                        enemyCtrl.transform
                    );
                }
                // sonst: Out-of-Range 
            }
            // sonst: kein gültiges Ziel 

            currentSkill = null; // Cast abgeschlossen
        }

        private void Try_StartNextSkillByRotation()
        {
            var next = SelectNextReadySkill();
            if (next == null && autoAttack != null && autoAttack.IsReady())
                next = autoAttack;

            if (next == null || target == null) return;

            // --- CastStart ---
            currentSkill = next;
            castRemaining = Mathf.Max(0f, next.CastTime);

            DebugManager.Log(
                $"Anim | Play {next.Data.animationType} len={next.CastTime:F2}s",
                DebugManager.EDebugLevel.Debug, "Anim"
            );

            // Cooldown beim CastStart (wie bisher)
            next.StartCooldown();
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

            gameObject.SetActive(false);

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

            List<DamageEntry> entries = new List<DamageEntry> { new DamageEntry(DamageType.Physical, 1f) };
            sd.DamageTypes = entries;

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

            List<DamageEntry> entries = new List<DamageEntry> { new DamageEntry(DamageType.Physical, 1f) };
            sd.DamageTypes = entries;
                                                      
            return new SkillInstance(sd, owner);
        }

        // Liefert die Reach (StoppingDistance) für die NÄCHSTE geplante Aktion.
        // v0: Falls du noch keinen Zugriff auf den nächsten Skill-Range hast,
        // nimm Melee/Ranged-Heuristik oder den AutoAttack-Typ.
        private float GetPlannedReachOrDefault()
        {
            // TODO: Wenn dein Rotationssystem den nächsten Skill/AutoAttack liefert,
            //       gib dessen Range hier zurück (in Metern).
            //       Bis dahin: Heuristik – Hero/Enemy hat typ AutoAttackMelee?
            bool nextIsMelee = true; // <— ersetze später durch echte Abfrage
            float MeleeReachDefault = 1.5f;
            return nextIsMelee ? MeleeReachDefault : 0f;
        }

        // Optional: Ist das Ziel außerhalb Sicht?
        private static bool IsOutOfSight(Vector3 self, Vector3 target, float sightRange)
        {
            float sr2 = sightRange * sightRange;
            return (target - self).sqrMagnitude > sr2;
        }

        private void EnsureMoveAgentInitialized()
        {
            if (_move == null) _move = GetComponent<MoveAgent>();
            // Init nur einmal, danach kannst du Buffs/Debuffs per ApplyRuntimeSpeed() verändern
            if (_move != null && !_initedMove)
            {
                float baseSpeed = HeroDef.sightRange; // HeroDef.moveSpeed / EnemyDef.moveSpeed
                bool isHero = true;
                _move.Init(baseSpeed, isHero, radius: 0.35f, overridePriority: null);
                _initedMove = true;
            }
        }

        private bool ShouldDoRangedBackstep(Vector3 targetPos)
        {
            // Wenn du Nahkämpfer bist → nie backstep
            bool isRangedArchetype = true; // TODO: aus Def/Archetype ableiten
            if (!isRangedArchetype) return false;

            float RangedComfortMin = 3.0f; //TODO: global auslagern
            float min = RangedComfortMin;
            float sqr = (targetPos - transform.position).sqrMagnitude;
            return sqr < (min * min);
        }


        public EffectReceiver GetEffectReceiver()
        {
            return heroInstance;
        }
    }
}
