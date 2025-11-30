# Assets/src/Systems/Skills/DamageModifier.cs

_Automatically generated/updated from `Assets/src/Systems/Skills/DamageModifier.cs`._

# Purpose
- Defines the `DamageModifier` class for managing damage modifications in a game.
- Provides an enumeration `DamageModifierType` to categorize different types of damage modifications.

# Public API
- Namespace/module: None specified.

- Types
  - **public class DamageModifier**
    - Public fields/properties:
      - `string Id`: Identifier for the damage modifier.
      - `DamageModifierType Type`: Type of the damage modifier.
      - `DamageType TargetType`: Affected damage type (or "Any").
      - `DamageType SourceType`: Source damage type for conversion/gain.
      - `DamageType DestinationType`: Target damage type for conversion/gain.
      - `float Value`: Value representing the magnitude of the modifier.
      - `List<SkillTag> AppliesTo`: Tags to filter applicable skills.
      - `ModifierHook Hook`: Hook for additional effects (default is `None`).

  - **public enum DamageModifierType**
    - `Added`: +X flat damage of type T.
    - `Convert`: Convert % of A to B (no duplication).
    - `Gain`: Gain % of A as B (duplication).
    - `Increased`: +X% increased (additive).
    - `More`: X% more (multiplicative).

# Key Behavior & Side Effects
- The `DamageModifier` class allows for various types of damage modifications, including flat additions, percentage increases, and conversions between damage types.
- The `Hook` property enables additional effects when certain conditions are met (e.g., "OnHit more Damage").

# Constraints & Failure Modes
- No explicit guards or null handling are defined in the code.
- The behavior of the `Hook` property is dependent on its implementation, which is not detailed in this file.

# Example
```csharp
DamageModifier damageModifier = new DamageModifier
{
    Id = "modifier1",
    Type = DamageModifierType.Added,
    TargetType = DamageType.Fire,
    SourceType = DamageType.Physical,
    DestinationType = DamageType.Fire,
    Value = 15.0f,
    AppliesTo = new List<SkillTag> { SkillTag.Burn },
    Hook = ModifierHook.None
};
```

# Unknowns
- The definitions and behaviors of `DamageType`, `SkillTag`, and `ModifierHook` are not provided in this file.
