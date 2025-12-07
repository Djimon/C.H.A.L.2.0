# Assets/src/Systems/Skills/SkillModifierStack.cs

_Automatically generated/updated from `Assets/src/Systems/Skills/SkillModifierStack.cs`._

# Purpose
- Defines a `ModifierStack` class that manages a collection of modifiers for skill-related operations, including generic and damage modifiers.

# Public API
- Namespace: `CHAL.Systems.Skill`
- Types
  - public class `ModifierStack`
    - Public properties:
      - `IReadOnlyList<DamageModifier> DamageModifiers` - Provides access to the list of damage modifiers.
    - Public methods:
      - `void AddGenericModifier(ModifierData mod)` - Adds a generic modifier to the collection.
      - `void AddDmgModifier(DamageModifier mod)` - Adds a damage modifier to the collection.
      - `void RemoveGenericModifier(ModifierData mod)` - Removes a generic modifier from the collection.
      - `void RemoveDmgModifier(DamageModifier mod)` - Removes a damage modifier from the collection.
      - `float Apply(ModifierTarget target, float baseValue, List<SkillTag> tags)` - Applies modifiers to a base value based on the target and skill tags; returns the modified value.

# Key Behavior & Side Effects
- The `Apply` method modifies a base value based on the active modifiers, considering the target and applicable skill tags.
- Modifiers can perform addition, multiplication, or replacement of the base value.
- The method processes both generic and damage modifiers.

# Constraints & Failure Modes
- The `Apply` method ignores modifiers that do not match the target or applicable skill tags.
- If a modifier's operation is `Replace`, it will return the replacement value instead of calculating the modified value.

# Example
```csharp
ModifierStack stack = new ModifierStack();
stack.AddGenericModifier(new ModifierData { /* initialize generic modifier */ });
stack.AddDmgModifier(new DamageModifier { /* initialize damage modifier */ });
float modifiedValue = stack.Apply(target, baseValue, tags);
```

# Unknowns
- The structure and properties of `ModifierData`, `DamageModifier`, `ModifierTarget`, and `SkillTag` are not defined in this file.
