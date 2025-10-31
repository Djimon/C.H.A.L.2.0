# CHAL.Systems.Skill.SkillInstance

_Automatically generated/updated from `Assets/src/Systems/Skills/SkillInstance.cs`._

# Purpose
- Defines the `SkillInstance` class representing a skill with various properties and cooldown management.

# Public API
- Namespace: `CHAL.Systems.Skill`
- Types
  - `public class SkillInstance`
    - Public fields/properties:
      - `SkillData Data`: Skill data associated with the instance.
      - `float Damage`: Calculated damage of the skill.
      - `float CastTime`: Time taken to cast the skill.
      - `float Cooldown`: Cooldown duration of the skill.
      - `float Range`: Effective range of the skill.
      - `float Duration`: Duration the skill effect lasts.
      - `float ProjectileSpeed`: Speed of projectiles launched by the skill.
      - `int ProjectileCount`: Number of projectiles launched.
      - `float AoERadius`: Area of effect radius of the skill.
    - Public methods:
      - `SkillInstance(SkillData data, EffectReceiver owner)`: Constructor initializing the skill instance.
      - `void Recalculate()`: Recalculates skill properties based on modifiers.
      - `bool IsReady()`: Checks if the skill is ready to be used (cooldown is complete).
      - `void StartCooldown()`: Starts the cooldown for the skill.
      - `void TickCooldown(float deltaTime)`: Reduces the remaining cooldown by `deltaTime`.
      - `float GetCooldownRemaining()`: Returns the remaining cooldown time.
      - `override string ToString()`: Returns a string representation of the skill instance.

# Key Behavior & Side Effects
- `Recalculate()`: Updates skill properties based on the current modifiers and logs the initialization.
- `IsReady()`: Resets `cooldownRemaining` to 0 if it is less than or equal to 0 and returns true.
- `StartCooldown()`: Sets `cooldownRemaining` to the skill's cooldown duration.
- `TickCooldown(float deltaTime)`: Decreases `cooldownRemaining` by the specified `deltaTime`.

# Constraints & Failure Modes
- `Recalculate()`: Assumes `Data.Tags` can be null and handles it by initializing to an empty list.
- `IsReady()`: Returns true only if `cooldownRemaining` is less than or equal to 0.
- `GetCooldownRemaining()`: Ensures the returned value is not negative using `Mathf.Max`.

# Example
```csharp
SkillData skillData = new SkillData(); // Assume this is initialized properly
EffectReceiver effectReceiver = new EffectReceiver(); // Assume this is initialized properly
SkillInstance skillInstance = new SkillInstance(skillData, effectReceiver);
skillInstance.StartCooldown();
```

# Unknowns
- The structure and properties of `SkillData` and `EffectReceiver` are not defined in this file.
- The implementation details of `ActiveModifiers` and how they apply modifiers are not provided.

