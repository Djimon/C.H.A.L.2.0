# CHAL.Systems.Hero.HeroInstance

_Automatically generated/updated from `Assets/src/Systems/Heroes/HeroInstance.cs`._

```text
1) Purpose
- Defines public class HeroInstance within CHAL.Systems.Hero that derives from EffectReceiver.
- Represents a runtime hero instance linked to a HeroDef; manages level, attributes, growth accumulators, skills, and life state.
- Provides basic damage handling, movement speed/damage base accessors, stat initialization, and level-up growth logic.

```

2) Public API
- Namespace/module
  - CHAL.Systems.Hero

- Types
  - public class HeroInstance : EffectReceiver
    - Public fields/properties
      - HeroDef heroDef
        - The hero definition data backing this instance.
      - ArchetypeDef Archetype => heroDef.Archetype
        - Convenience access to the archetype from the hero definition.
      - int Level
        - Current hero level (initially 1).
      - Dictionary<HeroAttribs, int> attributes
        - Current attribute points per attribute (e.g., STR, DEX, CON, INT, WIL).
      - List<SkillInstance> Skills
        - Skill instances available to this hero.
      - GameObject currentTarget
        - Currently targeted object (if any).
    - Public methods
      - HeroInstance(HeroDef def)
        - Constructor. Initializes heroDef, applies signature passive if present, initializes stats, and sets HP from heroDef.BaseHealth.
      - void TakeDamage(float amount, DamageType type)
        - Applies damage; ignores if already dead; reduces CurrentHP; triggers OnDeath when HP falls below zero.
      - float GetEffectiveMovementSpeed()
        - Returns base movement speed from heroDef (no active modifiers applied here).
      - void LevelUp()
        - Increments Level (up to 100), allocates attribute points proportionally to growth shares, resolves point distribution when accumulators cross integer thresholds, and logs new attribute state.
      - float GetEffectiveBaseDamage()
        - Returns BaseDamage from heroDef (no modifiers applied here).
      - void OnDeath()  // overrides EffectReceiver
        - Death handler (idempotent): marks as dead, zeros CurrentHP, logs death, and fires Died event.
    - Public events
      - event Action<HeroInstance> Died
        - Invoked when the hero dies; subscribers are notified with this instance.

- Note on lifecycle/side effects
  - Constructor applies Archetype.SignaturePassive (if present) via ActiveModifiers.AddModifier.
  - LevelUp distributes points across attributes using a proportional accumulator model driven by GrowthPattern and GrowthConfig in Archetype.

```

3) Key Behavior & Side Effects
- Initialization flow (constructor)
  - Stores heroDef.
  - Logs error if Archetype (heroDef.Archetype) is null.
  - If Archetype.SignaturePassive exists, adds its modifier data to ActiveModifiers.
  - Calls InitStats to prepare growth-based attributes.
  - Sets MaxHP = heroDef.BaseHealth and CurrentHP = MaxHP.
- Damage handling (TakeDamage)
  - If _isDead, ignores damage.
  - Subtracts amount from CurrentHP.
  - If CurrentHP < 0, calls OnDeath().
- Death handling (OnDeath)
  - Guards against multiple invocations with _isDead.
  - Sets _isDead = true and CurrentHP = 0.
  - Logs death and raises Died event.
- Stats initialization (InitStats)
  - Builds startMap (initial values per LevelGrowthRole).
  - Builds targetMap from Archetype.GrowthConfig.*Target values.
  - Defines the stat order via slots (Core, Secondary1, Secondary2, Tertiary, Edge).
  - Initializes _accumulator for all HeroAttribs to 0.
  - For each growth priority slot, assigns initial attributes and computes _goals as target - start.
  - Computes _totalGrowth as the sum of all goal values.
- Base damage and movement speed accessors
  - GetEffectiveBaseDamage returns heroDef.BaseDamage.
  - GetEffectiveMovementSpeed returns heroDef.BaseMovementSpeed (no modifiers applied here).
- Level up (LevelUp)
  - If Level >= 100, exit.
  - Level++ and log level up.
  - Determine ptsThisLevel (5 points on multiples of 5, otherwise 4).
  - For each stat, compute share = kv.Value / _totalGrowth and increase _accumulator by share * ptsThisLevel.
  - While any accumulator >= 1.0, pick the next stat by highest accumulator (random tiebreak), increment that attribute by 1, and reduce its accumulator by 1.0.
  - Logs the new attribute distribution after leveling.

```

4) Constraints & Failure Modes
- Archetype null handling
  - If Archetype is null, constructor logs an error but subsequent code may dereference Archetype, potentially leading to exceptions (e.g., InitStats, GrowthConfig access).
- Death/state guards
  - Damage is ignored after _isDead becomes true.
  - OnDeath is idempotent via _isDead guard.
- Level cap
  - LevelUp is gated at Level >= 100.
- Growth logic assumptions
  - InitStats relies on Archetype.GrowthConfig.GrowthPattern and GrowthConfig.Target values; null references are not guarded, so missing config could raise exceptions.
- Random tie-break in LevelUp
  - Uses Guid.NewGuid() for randomization when accumulators tie.

```

5) Example
- Minimal usage (assuming valid HeroDef and supporting systems exist)

```csharp
// Assuming heroDef is a valid HeroDef instance in scope
var hero = new CHAL.Systems.Hero.HeroInstance(heroDef);
hero.LevelUp();
```

```

6) Unknowns
- Details of EffectReceiver base class (HP management, modifiers system) beyond usage in this file.
- Implementations and semantics of:
  - HeroDef, ArchetypeDef, LevelGrowthRole, GrowthConfig, LevelGrowthConfig, HeroAttribs.
  - DebugManager, ActiveModifiers, and ToModifierData behavior.
  - SkillInstance type and how Skills interact with this class.
- Multithreading/async behavior and Unity execution order beyond [ContextMenu] usage.
- How currentTarget is used elsewhere (targeting/AI integration) beyond its declaration.
