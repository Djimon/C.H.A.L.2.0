using CHAL.Core;
using CHAL.Data;
using CHAL.Systems.AI;
using CHAL.Systems.Hero;
using CHAL.Systems.Loot;
using CHAL.Systems.Skill;
using CHAL.Systems.Unit;
using CHAL.Systems.Wave;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CHAL.Systems.Enemy
{
/// <summary>
/// Manages the behavior and state of enemy units in the game.
/// Implements the IUnitController interface for unit control functionality.
/// </summary>
    public class EnemyController : MonoBehaviour, IUnitController
    {
        public EnemyDef EnemyDef;
        public EnemyStruct EnemyData { get; private set; }
        public EnemyInstance EnemyInstance { get; private set; }

        private readonly List<SkillInstance> _attacks = new();

        public Transform target;

        // Casting-State
        private SkillInstance _currentSkill;
        private float _castRemaining;

        public bool IsAlive => EnemyInstance != null && EnemyInstance.CurrentHP > 0;

        //AI
        private MoveAgent _move;
        private bool _initedMove = false;
        private Transform _currentTarget;

        // Static Event fÃ¼r alle EnemyController
        public static event Action<EnemyController, EnemyDef, EnemyStruct, Vector3> OnEnemyKilled;

        private void OnEnable()
        {
            UnitLocator.Instance.Register(this);
        }

        private void OnDisable()
        {
            UnitLocator.Instance.Unregister(this);
        }


        private void Start()
        {
            // Falls via Inspector schon gesetzt: sofort initialisieren
            if (EnemyDef != null && EnemyInstance == null)
                Init(EnemyData);
        }

/// <summary>
/// Initializes the enemy instance with the provided enemy structure.
/// </summary>
/// <param name="enemstruct">The structure containing enemy data.</param>
        public void Init(EnemyStruct enemstruct)
        {
            var def = UnitRegistry.Instance.GetEnemyByID(enemstruct.EnemyId);
            if (def == null)
            {
                DebugManager.Error($"EnemyDef '{enemstruct.EnemyId}' not found!");
                return;
            }

            EnemyDef = def;
            EnemyData = enemstruct;

            EnemyInstance = new EnemyInstance(def, enemstruct);
            EnemyInstance.Team = UnitTeam.AI;

            DebugManager.Log($"Enemy | Spawned {def.enemyId} (HP={EnemyInstance.MaxHP})",DebugManager.EDebugLevel.Dev,"Enemy");

            EnemyInstance.OnDied += HandleEnemyDied;

            BuildAttacksFromDef();
        }

        private void BuildAttacksFromDef()
        {
            _attacks.Clear();

            if (EnemyDef == null || EnemyDef.baseAttacks == null || EnemyDef.baseAttacks.Count == 0)
            {
                DebugManager.Log("Enemy | Warnung: baseAttacks ist leer.", DebugManager.EDebugLevel.Dev, "Combat");
                return;
            }

            // Reihenfolge in baseAttacks = Rotations-Prio
            foreach (var sd in EnemyDef.baseAttacks)
            {
                if (sd == null) continue;

                // Annahme: SkillInstance besitzt einen Ctor(SkillData)
                var inst = new SkillInstance(sd, EnemyInstance);
                _attacks.Add(inst);
            }

            DebugManager.Log($"Enemy | Loaded {_attacks.Count} attacks from Def.", DebugManager.EDebugLevel.Dev, "Combat");
        }

        private void Update()
        {
            if (!IsAlive || EnemyInstance == null) 
                return;

            float dt = Time.deltaTime;

            Tick_ReceiverStatusEffects(dt);

            Targeting();

            DoMovement();

            Tick_SkillCooldowns(dt);
            HeroController heroCtrl = GetNextHeroTarget();

            // 4) Laufenden Cast abwickeln
            if (IsCasting())
                Advance_CastTimeOrFinish(dt, heroCtrl);
            else
                Try_StartNextSkillByRotation(heroCtrl);

        }

        private void Tick_ReceiverStatusEffects(float dt)
        {
            // 1) Effekte ticken
            EnemyInstance.UpdateEffects(dt);
        }

        private void Targeting()
        {
            if (!IsAlive) { _currentTarget = null; return; }

            float sight = EnemyDef.sightRange; // Def.sightRange oder BalanceConfig.SightRangeDefault
            var myPos = transform.position;

            // Falls wir ein Ziel hatten: ist es noch valide (lebt & in Sicht)?
            if (_currentTarget != null)
            {
                // Lebt es noch?
                var er = _currentTarget.GetComponent<HeroController>();
                if (er == null || er.GetEffectReceiver().CurrentHP <= 0f || IsOutOfSight(myPos, _currentTarget.position, sight))
                {
                    _currentTarget = null; // Lock lÃ¶sen
                }
            }

            // Wenn kein Target: neu wÃ¤hlen (Prio v0: Nearest; Alternative HighestHP)
            if (_currentTarget == null)
            {
                var team = EnemyInstance.Team;

                // TODO: AbhÃ¤nging von Prio aus EnemyDef
                // Transform t = UnitLocator.Instance.GetHighestHPEnemy(myPos, team, sight);
                Transform t = UnitLocator.Instance != null
                    ? UnitLocator.Instance.GetNearestEnemy(myPos, team, sight)
                    : null;

                if (t != null)
                {
                    _currentTarget = t;
                }
                else
                {
                    _currentTarget = null;
                }
            }
            target = _currentTarget;
        }

        private void DoMovement()
        {
            EnsureMoveAgentInitialized();

            if (_currentTarget == null || _move == null)
            {
                // Kein Target: optional zum â€žSpawn/Homeâ€œ laufen â€“ v0: stehen.
                _move.ClearPathHard();
                return;
            }

            // StoppingDistance = Reach aus nÃ¤chster geplanten Aktion
            float reach = GetPlannedReachOrDefault();
            _move.StoppingDistance = reach;

            Vector3 targetPos = _currentTarget.position;

            // Ranged-Comfort: nur, wenn NICHT gerade caste
            bool isCasting = IsCasting(); // du hast die Logik bereits im Controller
            if (!isCasting && ShouldDoRangedBackstep(targetPos))
            {
                // v0: kleiner RÃ¼ckschritt entlang -forward Richtung vom Target
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

        private void Tick_SkillCooldowns(float dt)
        {
            // 2) Cooldowns ticken
            foreach (var s in _attacks)
                s?.TickCooldown(dt);
        }

        private HeroController GetNextHeroTarget()
        {
            // 3) Ziel prÃ¼fen/suchen
            if (target == null || target.GetComponent<HeroController>() == null)
                target = FindNextHeroTarget();
            var heroCtrl = target ? target.GetComponent<HeroController>() : null;
            return heroCtrl;
        }

        private void Try_StartNextSkillByRotation(HeroController heroCtrl)
        {
            // 5) Neuen Skill wÃ¤hlen (nur aus den Def-Autoattacks)
            var next = SelectNextReadyAttack();
            if (next != null && heroCtrl != null && heroCtrl.IsAlive)
            {
                // --- CastStart ---
                //DebugManager.Log($"Combat/Enemy | CastStart {next.Data.DisplayName} (castTime={next.CastTime:F2}s)");
                _currentSkill = next;
                _castRemaining = Mathf.Max(0f, next.CastTime);

                // (Phase 4: Animations-Hook als Stub)
                DebugManager.Log($"Anim | Enemy Play {next.skillData.animationType} len={next.CastTime:F2}s", DebugManager.EDebugLevel.Debug, "Anim");

                // Cooldown startet bei CastStart (analog Hero)
                next.StartCooldown();
            }
        }

        private bool IsCasting()
        {
            return _currentSkill != null;
        }

        private void Advance_CastTimeOrFinish(float dt, HeroController heroCtrl)
        {
            _castRemaining -= dt;

            // (Phase 6 HUD spÃ¤ter) â€” hier nur Nachweis
            //DebugManager.Log($"UI/HUD | Enemy Castbar {_currentSkill.Data.DisplayName}: {Mathf.Max(0, _castRemaining):F2}s");

            if (_castRemaining <= 0f)
            {
                // --- Execute ---
                if (heroCtrl != null && heroCtrl.IsAlive)
                {
                    float dist = Vector3.Distance(transform.position, heroCtrl.transform.position);
                    if (dist <= _currentSkill.Range)
                    {
                        DebugManager.Log($"Execute {_currentSkill.skillData.DisplayName} â†’ {heroCtrl.name} (dist={dist:F1}m)",DebugManager.EDebugLevel.Dev,"Combat");

                        SkillExecutor.ExecuteSkill(
                            _currentSkill,
                            EnemyInstance,                // Quelle: EffectReceiver
                            transform,                    // Quelle-Transform (VFX/Projectile)
                            heroCtrl.GetEffectReceiver(), // Ziel: EffectReceiver
                            heroCtrl.transform            // Ziel-Transform
                        );
                        // OnHit-Logs kommen aus dem Skill/Projectile.
                    }
                    else
                    {
                        DebugManager.Log($"Targeting | Out of Range (Enemy): {_currentSkill.skillData.DisplayName} dist={dist:F1}m > {_currentSkill.Range:F1}m",DebugManager.EDebugLevel.Dev, "Combat");
                    }
                }
                else
                {
                    DebugManager.Log($"Targeting | Enemy has no valid target for {_currentSkill.skillData.DisplayName}.", DebugManager.EDebugLevel.Dev, "Combat");
                }

                _currentSkill = null; // Cast abgeschlossen
            }

            return; // solange gecastet wird, keine neue Auswahl
        }


        private SkillInstance SelectNextReadyAttack()
        {
            // Nimm den ersten â€žreadyâ€œ gemÃ¤ÃŸ PrioritÃ¤t (Reihenfolge in baseAttacks)
            foreach (var s in _attacks)
            {
                if (s == null) continue;
                if (s.IsReady()) return s;
            }
            return null;
        }

        private Transform FindNextHeroTarget()
        {
            var heroes = FindObjectsByType<HeroController>(FindObjectsSortMode.None);
            if (heroes == null || heroes.Length == 0) return null;

            Transform best = null;
            float minDist = float.MaxValue;

            foreach (var h in heroes)
            {
                if (h == null || !h.IsAlive) continue;

                float d = Vector3.Distance(transform.position, h.transform.position);
                if (d < minDist)
                {
                    minDist = d;
                    best = h.transform;
                }
            }
            return best;
        }

/// <summary>
/// Applies damage to the enemy based on the specified amount and damage type.
/// </summary>
/// <param name="amount">The amount of damage to apply.</param>
/// <param name="type">The type of damage being dealt.</param>
        public void TakeDamage(float amount, DamageType type)
        {
            if (!IsAlive) return;

            EnemyInstance.TakeDamage(amount, type);
            DebugManager.Log($"Enemy | {EnemyDef.displayNameKey} took {amount} {type} (HP={EnemyInstance.CurrentHP}/{EnemyInstance.MaxHP})");

        }

        private void OnDestroy()
        {
            if (EnemyInstance != null)
                EnemyInstance.OnDied -= HandleEnemyDied;
        }

        private void HandleEnemyDied(EnemyInstance inst)
        {
            GameManager.Instance.Stats.OnEnemyKilled(EnemyDef.enemyId, EnemyData.Rank, EnemyDef.baseTags, EnemyData.bonusTags);

            DebugManager.Log($"Enemy {EnemyData.EnemyId} ({EnemyData.Rank}) killed!", DebugManager.EDebugLevel.Dev, "Combat");
            // Event feuern: sagt nur â€žich bin totâ€œ, inkl. Position
            OnEnemyKilled?.Invoke(this, EnemyDef, EnemyData, transform.position);

            // Hier: Animator/VFX/Despawn, Collider off, Loot-Trigger etc.
            //TODO: make pooling
            gameObject.SetActive(false);
            Destroy(gameObject);
            
        }

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
                float baseSpeed = EnemyDef.sightRange; // HeroDef.moveSpeed / EnemyDef.moveSpeed
                bool isHero = false;
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

        //private void OnMouseDown()
        //{
        //    EnemyInstance.TakeDamage(999, DamageType.Physical);
        //}

/// <summary>
/// Retrieves the EffectReceiver associated with the enemy instance.
/// </summary>
/// <returns>The EffectReceiver of the enemy.</returns>
        public EffectReceiver GetEffectReceiver()
        {
            return EnemyInstance;
        }

    }
}

