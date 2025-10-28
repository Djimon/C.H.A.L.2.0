# CHAL.Systems.Hero.HeroInstance

_Automatically generated/updated from `Assets/src/Systems/Heroes/HeroInstance.cs`._

# Purpose
- Defines the `HeroInstance` class representing a hero in the game with attributes, skills, and health management.

# Public API
- Namespace: `CHAL.Systems.Hero`
- Types
  - `public class HeroInstance : EffectReceiver`
    - Public fields/properties:
      - `HeroDef heroDef`: Definition of the hero.
      - `ArchetypeDef Archetype`: Gets the archetype of the hero.
      - `int Level`: Current level of the hero.
      - `Dictionary<HeroAttribs, int> attributes`: Hero's attributes.
      - `List<SkillInstance> Skills`: List of skills the hero possesses.
      - `GameObject currentTarget`: The current target of the hero.
      - `event Action<HeroInstance> Died`: Event triggered when the hero dies.
    - Public methods:
      - `public HeroInstance(HeroDef def)`: Constructor initializing the hero with a definition.
      - `public override void TakeDamage(float amount, DamageType type)`: Reduces health by `amount`, triggers death if health falls below zero.
      - `public float GetEffectiveMovementSpeed()`: Returns the hero's base movement speed.
      - `protected override void OnDeath()`: Handles the death of the hero.
      - `public float GetEffectiveBaseDamage()`: Returns the hero's base damage.
      - `public void LevelUp()`: Increases the hero's level and updates attributes.

# Key Behavior & Side Effects
- The constructor initializes the hero's attributes and health based on the provided `HeroDef`.
- `TakeDamage` method reduces health and triggers death if health is zero or below.
- `OnDeath` method sets the hero as dead and invokes the `Died` event.
- `LevelUp` method increases the hero's level, allocates attribute points based on growth patterns, and logs the new attributes.

# Constraints & Failure Modes
- The `TakeDamage` method is idempotent; calling it when the hero is dead has no effect.
- The `LevelUp` method prevents leveling beyond 100.
- Attribute allocation in `LevelUp` is based on the total growth and may lead to uneven distribution if not managed.

# Example
```csharp
HeroDef heroDef = new HeroDef(); // Assume this is initialized properly
HeroInstance hero = new HeroInstance(heroDef);
hero.TakeDamage(10);
hero.LevelUp();
```

# Unknowns
- The implementation details of `HeroDef`, `ArchetypeDef`, `SkillInstance`, and `EffectReceiver` are not provided.
- The behavior of `ActiveModifiers` and how they affect attributes is not defined in this file.

