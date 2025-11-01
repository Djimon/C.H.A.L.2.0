# CHAL.Systems.Hero.HeroController

_Automatically generated/updated from `Assets/src/Systems/Heroes/HeroController.cs`._

1) Purpose
- Defines HeroController as a MonoBehaviour implementing IUnitController to manage hero behavior.
- Coordinates hero definition, skill wiring, targeting, movement, casting, and death events.
- Integrates with UnitLocator and MoveAgent for AI/navigation.

2) Public API
- Namespace/module
  - CHAL.Systems.Hero

- Types
  - public class HeroController : MonoBehaviour, IUnitController
    - Public fields/properties
      - public HeroDef HeroDef
        - Hero configuration/definition for this controller
      - public List<SkillData> debugSocketSkills
        - Optional debug-only skill data to socket into rotation
      - public Transform target
        - Current target transform (enemy or similar)
      - public bool IsAlive => heroInstance != null && heroInstance.CurrentHP > 0
        - Read-only status of the hero
    - Public events
      - public event Action<HeroController> OnHeroDied
        - Notifies listeners when this hero dies
    - Public methods
      - public void Start()
        - Initialization hook; may call Init(HeroDef) if needed and BuildSkillInstances
      - public void Init(HeroDef def)
        - Initialize hero with given definition; creates HeroInstance and subscribes to Died
      - public void TakeDamage(float amount, DamageType type)
        - Apply damage to the heroInstance and log
      - public EffectReceiver GetEffectReceiver()
        - Returns the associated EffectReceiver (heroInstance)

3) Key Behavior & Side Effects
- Lifecycle hooks
  - OnEnable(): registers this unit with UnitLocator
  - OnDisable(): unregisters from UnitLocator; if tied, unsubscribes hero Died handler
  - OnDestroy(): unregisters from UnitLocator
- Initialization and skill setup
  - Start(): if HeroDef is set and no heroInstance, calls Init(HeroDef); ensures socketedSkills list exists; builds SkillInstances; logs if autoAttack is missing
  - Init(HeroDef def): creates heroInstance from def, assigns Team = Player, subscribes to Died
  - BuildSkillInstances(): clears socketedSkills; chooses base autoAttack based on Archetype.primAttackType (Ranged vs Melee); appends any debugSocketSkills; logs summary
- Runtime loop (per Update)
  - Tick_ReceiverStatusEffects(dt): advances heroInstance effects
  - Targeting(): selects/validates _currentTarget; updates public target
  - DoMovement(): initializes MoveAgent if needed; moves toward currentTarget with stopping distance; includes ranged backstep heuristic
  - Tick_SkillCooldown(dt): ticks cooldowns of all socketedSkills and autoAttack
  - If casting: Advance_CastTimerOrFinish(dt); else Try_StartNextSkillByRotation()
  - Handle_DebugShortcuts(): responds to debug inputs (e.g., H to take self-damage)
- Casting and execution
  - IsCasting(): whether a currentSkill is active
  - Advance_CastTimerOrFinish(dt): decrements castRemaining; on finish, if target is valid and in range, executes currentSkill via SkillExecutor; clears currentSkill
  - Try_StartNextSkillByRotation(): selects next ready skill; if none, uses autoAttack if ready; starts cast, initializes cast timer, logs, and starts cooldown
- Targeting and movement
  - Targeting(): keeps _currentTarget alive if valid; otherwise finds nearest enemy within sight via UnitLocator
  - DoMovement(): sets destination to target; respects stopping distance; handles non-target idle case
- Combat helpers
  - BuildBaseAttackMelee/Hero and BuildBaseAttackRanged/Hero: construct SkillData ScriptableObjects and wrap in SkillInstance
  - GetPlannedReachOrDefault(): heuristic for next action reach (default melee 1.5m)
  - IsOutOfSight(...): static helper to determine visibility based on distance
  - EnsureMoveAgentInitialized(): lazy initialization of MoveAgent with hero speed and settings
  - ShouldDoRangedBackstep(...): heuristic to backstep when ranged archetype is too close
- Death and cleanup
  - OnHeroInstanceDied(HeroInstance): triggers Die() if the dying instance matches
  - Die(): logs death, invokes OnHeroDied, stops actions, deactivates/destroys the GameObject
- Effect reception
  - GetEffectReceiver(): returns the hero’s EffectReceiver (heroInstance)

4) Constraints & Failure Modes
- Defensive null checks:
  - Many methods guard against null heroInstance, null UnitLocator, and null target
- Ownership and unsubscription:
  - OnDisable ensures hero Died handler is unsubscribed to avoid callbacks after destruction
- Movement integration:
  - MoveAgent is initialized once; if missing, movement logic gracefully skips
- Casting safety:
  - Advance_CastTimerOrFinish checks for valid enemy target and range before executing
- Debug/logging:
  - Logs at various levels; can aid tracing but relies on DebugManager
- External dependencies:
  - Uses UnitLocator, MoveAgent, SkillExecutor, DebugManager; behavior depends on those systems existing in the project

5) Example
- Not applicable / not clearly derivable from this file alone

6) Unknowns
- Details of HeroDef, HeroInstance, SkillInstance, SkillData, and related data structures beyond their usage here
- Exact behavior of UnitLocator.GetNearestEnemy and other locator methods
- Full semantics of DebugManager logging levels and how SkillExecutor.ExecuteSkill behaves in detail
- Any additional constraints imposed by external systems (e.g., animation synchronization, event timing) beyond what is shown
