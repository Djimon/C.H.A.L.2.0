# CHAL.Data.ModifierDef

_Automatically generated/updated from `Assets/src/Data/Defs/SkillModifierDef.cs`._

# Purpose
- Defines a ScriptableObject for skill modifiers in the game.
- Provides a method to convert the modifier definition into a runtime data structure.

# Public API
- Namespace: CHAL.Data
- Types
  - **public class ModifierDef : ScriptableObject**
    - Public fields/properties:
      - `string modId`: Identifier for the modifier.
      - `ModifierTarget Target`: Target of the modifier.
      - `ModifierOperation Operation`: Operation type of the modifier.
      - `float Value`: Value of the modifier (must be set when applying).
      - `List<SkillTag> AppliesTo`: Tags the modifier applies to; empty means global.
      - `ModifierHook Hook`: Hook type for the modifier.
    - Public methods:
      - `ModifierData ToModifierData()`: Converts the modifier definition to `ModifierData`.

  - **[Serializable] public class ModifierData**
    - Public fields/properties:
      - `string Id`: Identifier for the modifier.
      - `ModifierTarget Target`: Target of the modifier.
      - `ModifierOperation Operation`: Operation type of the modifier.
      - `float Value`: Value of the modifier.
      - `List<SkillTag> AppliesTo`: Tags the modifier applies to.
      - `ModifierHook Hook`: Hook type for the modifier.

# Key Behavior & Side Effects
- `ToModifierData()` creates a new instance of `ModifierData` populated with the fields from `ModifierDef`.

# Constraints & Failure Modes
- The `Value` field must be set when applying the modifier.
- The `AppliesTo` list can be empty, indicating a global application.

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
- Specific implementations or definitions of `ModifierTarget`, `ModifierOperation`, `SkillTag`, and `ModifierHook` cannot be determined from this file.

