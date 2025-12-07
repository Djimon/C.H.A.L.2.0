# Assets/src/Systems/Heroes/HeroInstance.cs

_Automatically generated/updated from `Assets/src/Systems/Heroes/HeroInstance.cs`._

# Purpose
- Defines a `HeroInstance` class representing an instance of a hero with attributes and skills.

# Public API
- Namespace: `CHAL.Systems.Hero`
- Types
  - public class `HeroInstance` : `EffectReceiver`, `IAttributeHolder`
    - Public fields/properties:
      - `HeroDef heroDef`: Definition of the hero.
      - `ArchetypeDef Archetype`: Gets the archetype of the hero.
      - `int Level`: Current level of the hero.
      - `Dictionary<HeroAttribs, int> attributes`: Hero's attributes.
      - `List<SkillInstance> Skills`: List of skills the hero possesses.
      - `GameObject currentTarget`: The current target of the hero.
      - `event Action<HeroInstance> Died`: Event triggered when the hero dies.
      - `int CurrentXP`: Current experience points of the hero.
      - `int TotalXP`: Total experience points earned by the hero.
      - `int TotalOrbitPointsEarned`: Total orbit points earned by the hero.
      - `int UnspentOrbitPoints`: Orbit points that have not been spent.
      - `int UnlockedSockets`: Number of sockets unlocked for the hero.
    - Public methods:
      - `void TakeDamage(float amount, DamageType type)`: Applies damage to the hero and handles death if health falls below zero.
      - `float GetEffectiveMovementSpeed()`: Calculates the effective movement speed of the hero.
      - `float GetEffectiveBaseDamage()`: Calculates the effective base damage of the hero.
      - `void LevelUp()`: Increases the hero's level by one, up to a maximum of 100.
      - `void AddXP(int amount)`: Adds experience points to the hero if the amount is positive and the hero has not reached the level cap.
      - `void Debug_ForceLevelUp()`: Forces the hero to level up immediately.
      - `void ApplyProgressData(HeroProgressData progress)`: Applies the progress data to the hero, initializing values if the data is null.
      - `void FillProgressData(HeroProgressData target)`: Fills the progress data for the specified hero.

# Key Behavior & Side Effects
- The `TakeDamage` method reduces the hero's health and triggers death if health falls below zero.
- The `LevelUp` method increases the hero's level and allocates attribute points based on growth patterns.
- The `AddXP` method adds experience points and attempts to apply level-ups based on the current experience.
- The `ApplyProgressData` method initializes the hero's attributes and levels based on the provided progress data.

# Constraints & Failure Modes
- The `TakeDamage` method does not apply damage if the hero is already dead.
- The `LevelUp` method does not increase the level if it is already at the maximum of 100.
- The `AddXP` method does not add experience points if the amount is zero or negative, or if the hero has reached the level cap.
- The `ApplyProgressData` method resets the hero's attributes if the provided progress data is null.

# Example
```csharp
HeroDef heroDef = new HeroDef(); // Assume this is initialized properly
HeroInstance hero = new HeroInstance(heroDef);
hero.TakeDamage(10f, DamageType.Physical);
hero.AddXP(50);
hero.Debug_ForceLevelUp();
```

# Unknowns
- The implementation details of `EffectReceiver`, `HeroDef`, `ArchetypeDef`, `SkillInstance`, and `DamageType` cannot be determined from this file.
- The behavior of `ActiveModifiers` and how it interacts with the hero's attributes is not defined in this file.
