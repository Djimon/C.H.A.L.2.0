# Assets/src/Systems/Skills/SkillModifierStack.cs

_Automatically generated/updated from `Assets/src/Systems/Skills/SkillModifierStack.cs`._

# Purpose
- Defines a `ModifierStack` class that manages a collection of modifiers for skill-related operations.

# Public API
- Namespace: `CHAL.Systems.Skill`
- Types
  - public class `ModifierStack`
    - Public methods:
      - `void AddModifier(ModifierData mod)` - Adds a modifier to the collection.
      - `void RemoveModifier(ModifierData mod)` - Removes a modifier from the collection.
      - `float Apply(ModifierTarget target, float baseValue, List<SkillTag> tags)` - Applies modifiers to a base value based on the target and skill tags; returns the modified value.

# Key Behavior & Side Effects
- The `Apply` method modifies a base value based on the active modifiers, considering the target and applicable skill tags.
- Modifiers can perform addition, multiplication, or replacement of the base value.

# Constraints & Failure Modes
- The `Apply` method ignores modifiers that do not match the target or applicable skill tags.
- If a modifier's operation is `Replace`, it will return the replacement value instead of calculating the modified value.

# Example
```csharp
ModifierStack stack = new ModifierStack();
stack.AddModifier(new ModifierData { /* initialize modifier */ });
float modifiedValue = stack.Apply(target, baseValue, tags);
```

# Unknowns
- The structure and properties of `ModifierData`, `ModifierTarget`, and `SkillTag` are not defined in this file.

