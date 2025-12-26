# Assets/src/Data/Defs/SkillModifierDef.cs

_Automatically generated/updated from `Assets/src/Data/Defs/SkillModifierDef.cs`._

1) Purpose
- Defines a `SkillModifierDef` class for gameplay modifier definitions.
- Provides a method to convert `SkillModifierDef` instances to `ModifierData` objects.

2) Public API
- Namespace: `CHAL.Data`
- Types
  - public class `SkillModifierDef` [extends `ScriptableObject`]
    - Public fields/properties:
      - `string modId`: Identifier for the modifier.
      - `ModifierTarget Target`: Target of the modifier.
      - `ModifierOperation Operation`: Operation type of the modifier.
      - `float Value`: Value of the modifier (must be set when applying).
      - `List<string> AppliesToTags`: Tags to filter application (empty = global).
      - `ModifierHook Hook`: Hook type for the modifier.
    - Public methods:
      - `ModifierData ToModifierData()`: Converts to `ModifierData` object.

3) Key Behavior & Side Effects
- `ToModifierData()` creates a new `ModifierData` instance populated with the current object's data.

4) Constraints & Failure Modes
- `Value` must be set when applying the modifier.
- `AppliesToTags` can be empty, indicating a global application.

5) Example
```csharp
SkillModifierDef skillModifier = ScriptableObject.CreateInstance<SkillModifierDef>();
skillModifier.modId = "exampleModifier";
skillModifier.Target = ModifierTarget.SomeTarget;
skillModifier.Operation = ModifierOperation.SomeOperation;
skillModifier.Value = 2.0f;
skillModifier.AppliesToTags = new List<string> { "tag1", "tag2" };
ModifierData modifierData = skillModifier.ToModifierData();
```

6) Unknowns
- None.
