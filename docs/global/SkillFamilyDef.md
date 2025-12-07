# Assets/src/Data/Defs/SkillFamilyDef.cs

_Automatically generated/updated from `Assets/src/Data/Defs/SkillFamilyDef.cs`._

# Purpose
- Defines a `SkillFamilyDef` class as a ScriptableObject for managing skill family data in Unity.

# Public API
- Namespace/module: `CHAL.Data`
- Types
  - `public class SkillFamilyDef : ScriptableObject`
    - Public fields/properties:
      - `string FamilyId` - Gets the family ID.
      - `SkillDeliveryTag[] Tags` - Gets the baseline tags for the skill family.
      - `float BaseDamage` - Gets the base damage value.
      - `float BaseRadius` - Gets the base radius value.
      - `float BaseDuration` - Gets the base duration value.

# Key Behavior & Side Effects
- The class is designed to be used as a data/config asset in Unity, allowing for the definition of skill family attributes.

# Constraints & Failure Modes
- No explicit guards or null handling are defined in the code.
- The class is intended for use within the Unity Editor as a ScriptableObject.

# Example
```csharp
// Example of creating a SkillFamilyDef asset in Unity
SkillFamilyDef skillFamily = ScriptableObject.CreateInstance<SkillFamilyDef>();
skillFamily.FamilyId = "FireSkills";
skillFamily.BaseDamage = 50f;
skillFamily.BaseRadius = 10f;
skillFamily.BaseDuration = 5f;
```

# Unknowns
- No information on how the `SkillDeliveryTag` type is defined or used.
