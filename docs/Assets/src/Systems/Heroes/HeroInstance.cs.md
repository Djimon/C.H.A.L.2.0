# Assets/src/Systems/Heroes/HeroInstance.cs

_Automatic generated/updated._

```markdown
# Purpose
- Defines the `HeroInstance` class, representing a hero character in the game.
- Manages hero attributes, skills, and state (e.g., health, level).

# Public API
- Namespace: `CHAL.Systems.Hero`
- Types
  - `public class HeroInstance : EffectReceiver`
    - Public fields/properties:
      - `HeroDef heroDef`: Definition of the hero.
      - `ArchetypeDef Archetype`: Gets the archetype of the hero.
      - `int Level`: Current level of the hero.
      - `Dictionary<HeroAttribs, int> attributes`: Current attributes of the hero.
      - `List<SkillInstance> Skills`: List of skills the hero possesses.
      - `GameObject currentTarget`: The current target of the hero.
      - `event Action<HeroInstance> Died`: Event triggered when the hero dies.
    - Public methods:
      - `public HeroInstance(HeroDef def)`: Constructor that initializes the hero with a definition.
      - `public override void TakeDamage(float amount, DamageType type)`: Applies damage to the hero; triggers death if health drops below zero.
      - `public float GetEffectiveMovementSpeed()`: Returns the effective movement speed of the hero.
      - `protected override void OnDeath()`: Handles the death of the hero; triggers the `Died` event.
      - `public float GetEffectiveBaseDamage()`: Returns the base damage of the hero.
      - `[ContextMenu("Debug/LevelUP")] public void LevelUp()`: Levels up the hero and distributes attribute points.

# Key Behavior & Side Effects
- The constructor initializes the hero's attributes and health based on the provided `HeroDef`.
- `TakeDamage` reduces the hero's health and triggers death if health falls below zero.
- `OnDeath` marks the hero as dead and invokes the `Died` event.
- `LevelUp` increases the hero's level and redistributes attribute points based on growth patterns.

# Constraints & Failure Modes
- The `TakeDamage` method is idempotent; calling it after the hero is dead has no effect.
- The `LevelUp` method prevents leveling beyond level 100.
- Attribute distribution during leveling is based on a total growth calculation; if `_totalGrowth` is zero, it may lead to division by zero.

# Example
```csharp
HeroDef heroDef = new HeroDef(); // Assume this is properly initialized
HeroInstance hero = new HeroInstance(heroDef);
hero.TakeDamage(10);
hero.LevelUp();
```

# Unknowns
- The implementation details of `EffectReceiver`, `HeroDef`, `ArchetypeDef`, `SkillInstance`, and `HeroAttribs` cannot be determined from this file.
- The behavior of `DebugManager` methods is not defined in this file.
```
