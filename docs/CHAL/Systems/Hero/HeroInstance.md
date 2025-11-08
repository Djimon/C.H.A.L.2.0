# CHAL.Systems.Hero.HeroInstance

_Automatically generated/updated from `Assets/src/Systems/Heroes/HeroInstance.cs`._

# Purpose
- Defines a `HeroInstance` class representing an instance of a hero with attributes and skills.

# Public API
- Namespace: `CHAL.Systems.Hero`
- Types
  - public class `HeroInstance` : `EffectReceiver`
    - Public fields/properties:
      - `HeroDef heroDef`: Definition of the hero.
      - `ArchetypeDef Archetype`: Gets the archetype of the hero.
      - `int Level`: Current level of the hero.
      - `Dictionary<HeroAttribs, int> attributes`: Hero's attributes.
      - `List<SkillInstance> Skills`: List of skills the hero possesses.
      - `GameObject currentTarget`: The current target of the hero.
      - `event Action<HeroInstance> Died`: Event triggered when the hero dies.
    - Public methods:
      - `void TakeDamage(float amount, DamageType type)`: Applies damage to the hero and handles death if health falls below zero.
      - `float GetEffectiveMovementSpeed()`: Calculates the effective movement speed of the hero.
      - `float GetEffectiveBaseDamage()`: Calculates the effective base damage of the hero.
      - `void LevelUp()`: Increases the hero's level by one, up to a maximum of 100.

# Key Behavior & Side Effects
- The `TakeDamage` method reduces the hero's health and triggers death if health falls below zero.
- The `LevelUp` method increases the hero's level and allocates attribute points based on the growth pattern.

# Constraints & Failure Modes
- The `TakeDamage` method is idempotent; calling it when the hero is dead has no effect.
- The `LevelUp` method does not allow leveling beyond 100.
- Attribute points are allocated based on the total growth and current level.

# Example
```csharp
HeroDef heroDef = new HeroDef(); // Assume this is initialized properly
HeroInstance hero = new HeroInstance(heroDef);
hero.TakeDamage(10f, DamageType.Physical);
hero.LevelUp();
```

# Unknowns
- The implementation details of `EffectReceiver`, `HeroDef`, `ArchetypeDef`, `SkillInstance`, and `DamageType` cannot be determined from this file.
- The behavior of `ActiveModifiers` and how modifiers are applied is not detailed in this file.

