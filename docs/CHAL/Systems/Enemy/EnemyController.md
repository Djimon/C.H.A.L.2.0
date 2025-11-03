# CHAL.Systems.Enemy.EnemyController

_Automatically generated/updated from `Assets/src/Systems/Enemy/EnemyController.cs`._

1) Purpose
- Defines EnemyController (MonoBehaviour) implementing IUnitController for enemy units.
- Manages EnemyDef, EnemyData, EnemyInstance and a list of base SkillInstance attacks; coordinates targeting, movement, cooldowns, and casting.
- Emits OnEnemyKilled when an enemy dies and handles basic damage intake.

2) Public API
- Namespace / Module: CHAL.Systems.Enemy

- Public types
  - public class EnemyController : MonoBehaviour, IUnitController

- Public fields / properties
  - public EnemyDef EnemyDef;
  - public EnemyStruct EnemyData { get; private set; }
  - public EnemyInstance EnemyInstance { get; private set; }
  - public Transform target;
  - public bool IsAlive => EnemyInstance != null && EnemyInstance.CurrentHP > 0;
  - public static event Action<EnemyController, EnemyDef, EnemyStruct, Vector3> OnEnemyKilled;

- Public methods
  - public void Init(EnemyStruct enemstruct)
  - public void TakeDamage(float amount, DamageType type)
  - public EffectReceiver GetEffectReceiver()

3) Key Behavior & Side Effects
- Lifecycle / initialization
  - OnEnable: UnitLocator.Instance.Register(this)
  - OnDisable: UnitLocator.Instance.Unregister(this)
  - Start: if (EnemyDef != null && EnemyInstance == null) Init(EnemyData)
  - Init(EnemyStruct): loads EnemyDef from UnitRegistry using enemstruct.EnemyId; errors if not found; sets EnemyDef, EnemyData; creates EnemyInstance(def, enemstruct); assigns team to AI; subscribes to EnemyInstance.OnDied; calls BuildAttacksFromDef
  - BuildAttacksFromDef: clears _attacks; validates EnemyDef/baseAttacks; creates SkillInstance per baseAttack tied to EnemyInstance; logs count

- Per-frame behavior
  - Update: exits if not alive; ticks status effects; runs Targeting, DoMovement; ticks skill cooldowns; selects next hero target; handles casting progress or starts next skill by rotation
  - Tick_ReceiverStatusEffects: EnemyInstance.UpdateEffects(dt)
  - Targeting: maintains _currentTarget; validates current target; if none, finds nearest enemy via UnitLocator; updates public target
  - DoMovement: ensures MoveAgent initialized; if no target or movement component, clears path; sets stopping distance from planned reach; applies ranged backstep if applicable; otherwise moves toward target or stops
  - Tick_SkillCooldowns: ticks cooldowns for all _attacks
  - GetNextHeroTarget: caches next HeroController target from target Transform
  - Try_StartNextSkillByRotation: selects first ready attack; if available and hero target alive, starts cast by storing _currentSkill and _castRemaining; logs animation start; starts cooldown on the chosen skill
  - Advance_CastTimeOrFinish: decreases _castRemaining; on finish, if target alive and in range, executes Skill via SkillExecutor; logs targets/range checks; clears _currentSkill

- Casting / execution details
  - IsCasting: returns true if _currentSkill != null
  - SelectNextReadyAttack: returns first ready attack from _attacks (order reflects Def-based priority)
  - Execute window: uses SkillExecutor.ExecuteSkill with EnemyInstance as source, transform as origin, target’s EffectReceiver and Transform

- Targeting / range logic
  - GetPlannedReachOrDefault: returns a default melee range (1.5f) for planned action
  - ShouldDoRangedBackstep: stubbed as always true for ranged archetype; computes backstep target when close to target

- Damage / death
  - TakeDamage(amount, type): exits if not alive; applies damage to EnemyInstance; logs HP changes
  - HandleEnemyDied(inst): logs death; invokes OnEnemyKilled with context; despawns: deactivates and destroys game object
  - OnDestroy: detaches OnDied handler from EnemyInstance if present

- Targeting helpers
  - FindNextHeroTarget: collects all HeroController instances; returns closest alive hero

- Mouse & effect interface
  - GetEffectReceiver: returns EnemyInstance

- Misc
  - GetEffectReceiver is used as an EffectReceiver for skills
  - OnEnemyKilled event provides (this, EnemyDef, EnemyData, transform.position)

4) Constraints & Failure Modes
- Null checks / guards
  - Init logs error and returns if EnemyId lookup fails
  - BuildAttacksFromDef logs warning if baseAttacks is empty
  - Update and targeting handle null targets/missing components gracefully
- Dependency expectations
  - UnitLocator, UnitRegistry, MoveAgent, SkillInstance, SkillExecutor, HeroController, EnemyInstance, and related systems are consumed but not defined here
  - UnitLocator.Instance may be null in targeting; code guards against this
- Casting assumptions
  - Cast starts only if a ready attack exists and a valid hero target is alive
  - Casting completes only if target is alive and within range; otherwise logs out-of-range
- Death handling
  - OnEnemyKilled invoked even if no external listeners are attached (null-safe)
  - Death leads to deactivation and destruction of the GameObject
- State consistency
  - OnDied handler unsubscribes on destroy
  - MoveAgent initialized once; baseSpeed derived from EnemyDef.sightRange (odd coupling noted)
- Performance / allocation
  - FindNextHeroTarget scans all HeroController instances; uses FindObjectsByType, which may be costly if called frequently

5) Example
- Not derivable from this file alone; no minimal external example provided.

6) Unknowns
- Exact implementations and contracts for:
  - EnemyDef, EnemyStruct, EnemyInstance
  - SkillInstance, MoveAgent, SkillExecutor
  - UnitRegistry, UnitLocator, HeroController, DebugManager
  - FindObjectsByType / FindObjectsSortMode behavior
- How EnemyDef.baseAttacks is structured and how priority is defined beyond source order
- Any side effects of EnemyInstance.OnDied beyond local handling
- Details of how animations are wired to Skills (animationType is logged, but wiring is not shown)

