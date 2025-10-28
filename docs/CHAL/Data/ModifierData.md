# CHAL.Data.ModifierData

_Automatically generated/updated from `Assets/src/Data/Defs/SkillModifierDef.cs`._

# Purpose
- Defines a `ModifierDef` class for creating skill modifiers as ScriptableObjects.
- Provides a method to convert `ModifierDef` to `ModifierData`.

# Public API
- Namespace: `CHAL.Data`
- Types
  - **public class ModifierDef : ScriptableObject**
    - Public fields/properties:
      - `public string modId;`
      - `public ModifierTarget Target;`
      - `public ModifierOperation Operation;`
      - `public float Value;` // Must be set when applying the modifier.
      - `public List<SkillTag> AppliesTo;` // Empty list means global, otherwise tag-filtered.
      - `public ModifierHook Hook;`
    - Public methods:
      - `public ModifierData ToModifierData();` // Converts `ModifierDef` to `ModifierData`.

  - **[Serializable] public class ModifierData**
    - Public fields/properties:
      - `public string Id;`
      - `public ModifierTarget Target;`
      - `public ModifierOperation Operation;`
      - `public float Value;`
      - `public List<SkillTag> AppliesTo;`
      - `public ModifierHook Hook;`

# Key Behavior & Side Effects
- `ToModifierData` creates a new instance of `ModifierData` populated with the fields from `ModifierDef`.

# Constraints & Failure Modes
- `Value` must be set before applying the modifier.
- `AppliesTo` can be empty, indicating a global application.

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
- Specific details about `ModifierTarget`, `ModifierOperation`, `SkillTag`, and `ModifierHook` types cannot be determined from this file.

