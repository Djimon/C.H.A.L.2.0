# CHAL.Systems.Skill.SkillInstance

_Automatically generated/updated from `Assets/src/Systems/Skills/SkillInstance.cs`._

# Purpose
- Defines the `SkillInstance` class, representing an instance of a skill with various attributes and effects.

# Public API
- Namespace: `CHAL.Systems.Skill`
- Types
  - public class `SkillInstance`
    - Public fields/properties:
      - `SkillData Data`: The skill data associated with this instance.
      - `float Damage`: Calculated damage of the skill.
      - `float CastTime`: Time taken to cast the skill.
      - `float Cooldown`: Cooldown duration of the skill.
      - `float Range`: Effective range of the skill.
      - `float Duration`: Duration of the skill's effects.
      - `float ProjectileSpeed`: Speed of projectiles fired by the skill.
      - `int ProjectileCount`: Number of projectiles fired by the skill.
      - `float AoERadius`: Area of effect radius of the skill.
    - Public methods:
      - `SkillInstance(SkillData data, EffectReceiver owner)`: Constructor that initializes the skill instance with data and owner.
      - `void Recalculate()`: Recalculates the skill's attributes based on current modifiers and data.
      - `bool IsReady()`: Checks if the cooldown period has ended; returns true if ready.
      - `void StartCooldown()`: Starts the cooldown by setting the remaining time to the full cooldown duration.
      - `void TickCooldown(float deltaTime)`: Reduces the remaining cooldown time by the specified delta time.
      - `float GetCooldownRemaining()`: Gets the remaining cooldown time, ensuring it is not negative.
      - `override string ToString()`: Returns a string representation of the object, including its properties.

# Key Behavior & Side Effects
- The `Recalculate` method updates the skill's attributes based on active modifiers and logs the initialized skill details.
- The `IsReady` method resets `cooldownRemaining` to zero if it is less than or equal to zero and returns true.
- The `StartCooldown` method sets `cooldownRemaining` to the skill's cooldown duration.
- The `TickCooldown` method decreases `cooldownRemaining` by the specified `deltaTime`.

# Constraints & Failure Modes
- The `Recalculate` method assumes that `Data.Tags` can be null and handles it by initializing to an empty list.
- The `GetCooldownRemaining` method ensures that the returned value is not negative using `Mathf.Max`.

# Example
```csharp
SkillData skillData = new SkillData(); // Assume this is initialized properly
EffectReceiver effectReceiver = new EffectReceiver(); // Assume this is initialized properly
SkillInstance skillInstance = new SkillInstance(skillData, effectReceiver);
skillInstance.StartCooldown();
if (skillInstance.IsReady())
{
    // Skill can be used
}
```

# Unknowns
- The implementation details of `SkillData`, `EffectReceiver`, and `ModifierTarget` are not provided in this file.
- The behavior of `BalanceManager.Instance.GetRangeValue` is not defined in this file.

