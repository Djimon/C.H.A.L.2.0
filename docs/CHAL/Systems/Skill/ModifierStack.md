# CHAL.Systems.Skill.ModifierStack

_Automatically generated/updated from `Assets/src/Systems/Skills/SkillModifierStack.cs`._

# Purpose
- Defines a `ModifierStack` class that manages a collection of modifiers for modifying values based on specific conditions.

# Public API
- Namespace: `CHAL.Systems.Skill`
- Types
  - public class `ModifierStack`
    - Public methods:
      - `void AddModifier(ModifierData mod)` - Adds a modifier to the collection.
      - `void RemoveModifier(ModifierData mod)` - Removes a modifier from the collection.
      - `float Apply(ModifierTarget target, float baseValue, List<SkillTag> tags)` - Applies modifiers to a base value and returns the modified value.

# Key Behavior & Side Effects
- The `Apply` method modifies a base value based on the target and skill tags, considering different operations (Add, Mult, Replace) defined in the modifiers.

# Constraints & Failure Modes
- The `Apply` method skips modifiers that do not match the target or applicable tags.
- If a modifier's operation is Replace, it returns the modifier's value directly, ignoring other modifiers.

# Example
```csharp
ModifierStack stack = new ModifierStack();
stack.AddModifier(new ModifierData { /* initialization */ });
float modifiedValue = stack.Apply(target, baseValue, tags);
```

# Unknowns
- The structure and properties of `ModifierData`, `ModifierTarget`, and `SkillTag` are not defined in this file.

