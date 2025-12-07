# Assets/src/Data/Defs/SkillModuleDef.cs

_Automatically generated/updated from `Assets/src/Data/Defs/SkillModuleDef.cs`._

# Purpose
- Defines a ScriptableObject for skill module definitions in the game.

# Public API
- Namespace/module: None specified
- Types
  - public class SkillModuleDef : ScriptableObject
    - Public fields/properties:
      - string Id: Unique identifier for the skill module.
      - SkillFamilyDef Family: The family to which the skill module belongs.
      - SkillDeliveryTag[] TagsAdd: Additional tags for the skill module.
      - bool OverrideDamage: Indicates if damage should be overridden.
      - float DamageOverride: The value to override the base damage.
      - bool OverrideRadius: Indicates if radius should be overridden.
      - float RadiusOverride: The value to override the base radius.
      - bool OverrideDuration: Indicates if duration should be overridden.
      - float DurationOverride: The value to override the base duration.
      - string[] OnCastEffects: Effects triggered on casting the skill.
      - string[] OnHitEffects: Effects triggered on hitting a target.
      - string[] OnEndEffects: Effects triggered when the skill ends.

# Key Behavior & Side Effects
- This class serves as a data container for skill module configurations, allowing for customization of various skill parameters.

# Constraints & Failure Modes
- No explicit guards or null/empty handling noted.
- Threading/async notes not applicable.
- Performance/allocation hints not evident.

# Example
```csharp
SkillModuleDef skillModule = ScriptableObject.CreateInstance<SkillModuleDef>();
skillModule.Id = "Fireball";
skillModule.OverrideDamage = true;
skillModule.DamageOverride = 50f;
```

# Unknowns
- No information on how this class interacts with other components or systems in the game.
