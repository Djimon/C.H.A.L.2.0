# CHAL.Systems.Hero.HeroInstance

_Automatically generated/updated from `Assets/src/Systems/Heroes/HeroInstance.cs`._

1) Purpose
- Defines the HeroInstance class that represents a single hero with stats, growth, HP, and basic combat interactions.
- Encapsulates growth logic (attribute progression) and death handling, including a Died event for external listeners.
- Renders an API surface for interaction (damage, movement speed, damage output, level-up, and stat initialization).

2) Public API
- Namespace/module: CHAL.Systems.Hero

- Type: public class HeroInstance : EffectReceiver
  - Fields
    - public HeroDef heroDef; // hero definition backing this instance
    - public int Level = 1; // current hero level
    - public Dictionary<HeroAttribs, int> attributes = new(); // current attribute values
    - public List<SkillInstance> Skills; // skills associated with the hero
    - public GameObject currentTarget; // current target (UI/game logic reference)
  - Properties
    - public ArchetypeDef Archetype => heroDef.Archetype; // archetype of the hero
  - Events
    - public event Action<HeroInstance> Died; // invoked when hero dies
  - Constructors
    - public HeroInstance(HeroDef def)
  - Methods
    - public override void TakeDamage(float amount, DamageType type)
      - Applies damage; if not already dead, reduces CurrentHP and triggers OnDeath when HP < 0
    - public float GetEffectiveMovementSpeed()
      - Returns base movement speed (no modifiers applied in this file)
    - public float GetEffectiveBaseDamage()
      - Returns base damage (no active modifiers applied in this file)
    - [ContextMenu("Debug/LevelUP")] public void LevelUp()
      - Increases Level (up to 100), distributes attribute points based on growth targets and accumulators, and logs new attribute values
  - Protected/private
    - protected override void OnDeath()
      - Idempotent death handling: marks as dead, sets HP to 0, logs death, raises Died event
  - Notes
    - private fields exist for internal state (e.g., _accumulator, _goals, _totalGrowth) but are not part of the public API surface

3) Key Behavior & Side Effects
- Construction
  - Stores heroDef; logs error if Archetype is null
  - If Archetype.SignaturePassive exists, adds its modifier to ActiveModifiers
  - Calls InitStats()
  - Sets MaxHP to heroDef.BaseHealth and CurrentHP to MaxHP
- Damage handling
  - If already dead (_isDead), ignores damage
  - Reduces CurrentHP by amount; if CurrentHP < 0, calls OnDeath()
- Death handling
  - OnDeath(); ensures idempotent behavior
  - Sets _isDead = true and CurrentHP = 0
  - Logs death and notifies subscribers via Died
- Leveling up
  - Increments Level up to a max of 100
  - Determines ptsThisLevel = 5 every 5 levels, otherwise 4
  - Distributes ptsThisLevel across attributes proportionally to _goals using _accumulator
  - While any accumulator >= 1.0, selects the highest accumulator (randomized tie-break) to increment the corresponding attribute by 1, then reduces that accumulator by 1.0
  - Logs current attribute values after level-up
- Stats initialization
  - Builds startMap and targetMap from fixed starting values and Archetype.GrowthConfig targets
  - Uses GrowthPattern.growthPriority to map Core/Secondary/Tertiary/Edge to actual HeroAttribs in a defined order
  - Initializes _accumulator for all HeroAttribs
  - Sets attributes and _goals for each stat in the order defined by Archetype
  - Computes _totalGrowth as the sum of all _goals
- Movement and damage output
  - GetEffectiveMovementSpeed returns heroDef.BaseMovementSpeed
  - GetEffectiveBaseDamage returns heroDef.BaseDamage

4) Constraints & Failure Modes
- Archetype null handling
  - If Archetype is null, an error is logged but constructor continues; later access may cause null references
- Growth division edge case
  - LevelUp distributes points using share = kv.Value / _totalGrowth; if _totalGrowth == 0, division by zero occurs (not guarded)
- Level cap
  - LevelUp stops increasing Level at 100
- State assumptions
  - Skills is not initialized in the constructor (may be null unless set elsewhere)
  - CurrentHP/MaxHP rely on heroDef having BaseHealth; methods assume heroDef is populated
- Threading/async
  - No explicit threading or async handling; use is single-threaded within Unity game loop
- Side effects
  - LevelUp produces log output and may trigger random tie-breaks during attribute distribution

5) Example
- Not derivable from this file due to unknown HeroDef structure and surrounding types; omitted.

6) Unknowns
- Definitions/structures of:
  - HeroDef, ArchetypeDef, LevelGrowthRole, GrowthConfig, GrowthPattern, HeroAttribs, DamageType
  - Archetype.SignaturePassive.ToModifierData() return type and ActiveModifiers
  - DebugManager, its Log/Error usage and log levels
  - SkillInstance, EffectReceiver, and how Skills are initialized/used
  - GameObject integration and CurrentHP/MaxHP fields (likely in a base class)
- Exact contents of Archetype.GrowthConfig and GrowthPattern.growthPriority
- Any external lifecycle methods or Unity-specific behavior beyond what's shown

