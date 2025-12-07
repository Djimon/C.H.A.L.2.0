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
      - DamageMultiplier: float - Gets the damage multiplier.
      - RadiusMultiplier: float - Gets the radius multiplier.
      - DurationMultiplier: float - Gets the duration multiplier.
      - TagsAdd: SkillDeliveryTag[] - Gets the tags to add.
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
