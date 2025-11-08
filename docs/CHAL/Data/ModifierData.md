# CHAL.Data.ModifierData

_Automatically generated/updated from `Assets/src/Data/Defs/SkillModifierDef.cs`._

# Purpose
- Defines a modifier for gameplay mechanics, encapsulating properties that dictate its behavior.

# Public API
- Namespace: `CHAL.Data`
- Types
  - **public class ModifierDef : ScriptableObject**
    - Public fields/properties:
      - `string modId`: Identifier for the modifier.
      - `ModifierTarget Target`: The target of the modifier.
      - `ModifierOperation Operation`: The operation type of the modifier.
      - `float Value`: The value of the modifier (must be set when applying).
      - `List<SkillTag> AppliesTo`: Tags the modifier applies to; empty means global.
      - `ModifierHook Hook`: The hook type for the modifier.
    - Public methods:
      - `ModifierData ToModifierData()`: Converts the instance to a `ModifierData` object.

  - **[Serializable] public class ModifierData**
    - Public fields/properties:
      - `string Id`: Identifier for the modifier.
      - `ModifierTarget Target`: The target of the modifier.
      - `ModifierOperation Operation`: The operation type of the modifier.
      - `float Value`: The value of the modifier.
      - `List<SkillTag> AppliesTo`: Tags the modifier applies to.
      - `ModifierHook Hook`: The hook type for the modifier.

# Key Behavior & Side Effects
- `ToModifierData()` creates a new `ModifierData` instance populated with the current object's data.

# Constraints & Failure Modes
- The `Value` field must be set when applying the modifier.
- If `AppliesTo` is empty, the modifier is considered global.

# Example
```csharp
ModifierDef skillModifier = ScriptableObject.CreateInstance<ModifierDef>();
skillModifier.modId = "exampleModifier";
skillModifier.Target = ModifierTarget.SomeTarget;
skillModifier.Operation = ModifierOperation.SomeOperation;
skillModifier.Value = 2.0f;
skillModifier.AppliesTo = new List<SkillTag> { SkillTag.Tag1, SkillTag.Tag2 };
ModifierData modifierData = skillModifier.ToModifierData();
```

# Unknowns
- Specific details about `ModifierTarget`, `ModifierOperation`, `SkillTag`, and `ModifierHook` are not defined in this file.
