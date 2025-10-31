# CHAL.Systems.Skill.ModifierStack

_Automatically generated/updated from `Assets/src/Systems/Skills/SkillModifierStack.cs`._

# Purpose
- Defines a `ModifierStack` class for managing a stack of modifiers that can be applied to a base value.

# Public API
- Namespace: `CHAL.Systems.Skill`
- Types
  - `public class ModifierStack`
    - Public methods:
      - `void AddModifier(ModifierData mod)` - Adds a modifier to the stack.
      - `void RemoveModifier(ModifierData mod)` - Removes a modifier from the stack.
      - `float Apply(ModifierTarget target, float baseValue, List<SkillTag> tags)` - Applies modifiers to a base value based on the target and tags.

# Key Behavior & Side Effects
- `AddModifier` and `RemoveModifier` modify the internal list of modifiers.
- `Apply` calculates a new value based on the modifiers that match the target and applicable tags, supporting addition, multiplication, and replacement operations.

# Constraints & Failure Modes
- Modifiers are only applied if they match the specified `ModifierTarget` and any applicable `SkillTag`.
- If a modifier's operation is `Replace`, it overrides the final value.

# Example
```csharp
ModifierStack stack = new ModifierStack();
stack.AddModifier(new ModifierData { /* initialization */ });
float modifiedValue = stack.Apply(target, baseValue, tags);
```

# Unknowns
- The structure and properties of `ModifierData`, `ModifierTarget`, and `SkillTag` are not defined in this file.
- The behavior of `ModifierOperation` is not detailed in this file.

