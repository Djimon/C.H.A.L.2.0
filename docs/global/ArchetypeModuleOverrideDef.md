# Assets/src/Data/Defs/ArchetypeModuleOverrideDef.cs

_Automatically generated/updated from `Assets/src/Data/Defs/ArchetypeModuleOverrideDef.cs`._

# Purpose
- Defines a ScriptableObject for overriding module settings associated with archetypes in a skill system.

# Public API
- Namespace/module: CHAL.Data
- Types
  - public class ArchetypeModuleOverrideDef : ScriptableObject
    - Public fields/properties:
      - ModuleId: string - Gets the module ID.
      - ArchetypeId: string - Gets the archetype ID.
      - OverrideDamage: bool - Indicates if damage override is used.
      - DamageOverride: float - Gets the damage override value.
      - OverrideRadius: bool - Indicates if radius override is used.
      - RadiusOverride: float - Gets the radius override value.
      - OverrideDuration: bool - Indicates if duration override is used.
      - DurationOverride: float - Gets the duration override value.
      - DeliveryTagsAdd: List<SkillDeliveryTag> - Gets the tags to add.
      - EffectsAdd: string[] - Gets the effects to add.
      - EffectsRemove: string[] - Gets the effects to remove.

# Key Behavior & Side Effects
- None specified.

# Constraints & Failure Modes
- None specified.

# Example
```csharp
var archetypeOverride = ScriptableObject.CreateInstance<ArchetypeModuleOverrideDef>();
archetypeOverride.ModuleId; // Access the module ID
```

# Unknowns
- None.
