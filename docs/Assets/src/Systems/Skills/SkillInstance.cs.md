# Assets/src/Systems/Skills/SkillInstance.cs

_Automatic generated/updated._

```markdown
## Purpose
- Defines the `SkillInstance` class for managing skill properties and cooldowns in a game system.

## Public API
- Namespace: `CHAL.Systems.Skill`
- Types
  - `public class SkillInstance`
    - Public fields/properties:
      - `SkillData Data`: Skill data associated with the instance.
      - `float Damage`: Calculated damage value.
      - `float CastTime`: Calculated cast time.
      - `float Cooldown`: Calculated cooldown duration.
      - `float Range`: Calculated range of the skill.
      - `float Duration`: Calculated duration of the skill effect.
      - `float ProjectileSpeed`: Calculated speed of projectiles.
      - `int ProjectileCount`: Calculated number of projectiles.
      - `float AoERadius`: Calculated area of effect radius.
    - Public methods:
      - `SkillInstance(SkillData data, EffectReceiver owner)`: Constructor initializing the skill instance.
      - `void Recalculate()`: Recalculates skill properties based on modifiers.
      - `bool IsReady()`: Checks if the skill is ready to be used (cooldown is complete).
      - `void StartCooldown()`: Starts the cooldown timer for the skill.
      - `void TickCooldown(float deltaTime)`: Reduces the remaining cooldown time by `deltaTime`.
      - `float GetCooldownRemaining()`: Returns the remaining cooldown time.
      - `override string ToString()`: Returns a string representation of the skill instance.

## Key Behavior & Side Effects
- `Recalculate()`: Updates skill properties based on active modifiers and logs the initialization.
- `IsReady()`: Resets `cooldownRemaining` to 0 if it is less than or equal to 0.
- `StartCooldown()`: Sets `cooldownRemaining` to the value of `Cooldown`.
- `TickCooldown(float deltaTime)`: Decreases `cooldownRemaining` by the specified `deltaTime`.

## Constraints & Failure Modes
- `Recalculate()`: Handles null `Data.Tags` by initializing to an empty list.
- `IsReady()`: Ensures `cooldownRemaining` does not go below 0.
- No threading or async handling is evident in the file.

## Example
```csharp
SkillData skillData = new SkillData(); // Assume this is initialized properly
EffectReceiver effectReceiver = new EffectReceiver(); // Assume this is initialized properly
SkillInstance skillInstance = new SkillInstance(skillData, effectReceiver);
skillInstance.StartCooldown();
```

## Unknowns
- The structure and properties of `SkillData` and `EffectReceiver` are not defined in this file.
- The implementation details of `ActiveModifiers` and `Apply` method are not provided.
```
