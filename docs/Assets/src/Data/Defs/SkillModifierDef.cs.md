# Assets/src/Data/Defs/SkillModifierDef.cs

_Automatic generated/updated._

```markdown
## Purpose
- Defines a `ModifierDef` class as a ScriptableObject for skill modifiers.
- Provides a method to convert `ModifierDef` to `ModifierData`.

## Public API
- Namespace: `CHAL.Data`
- Types
  - **[CreateAssetMenu] class** `ModifierDef` : `ScriptableObject`
    - Public fields/properties:
      - `string modId` - Identifier for the modifier.
      - `ModifierTarget Target` - Target of the modifier.
      - `ModifierOperation Operation` - Operation type of the modifier.
      - `float Value` - Value of the modifier (must be set when applying).
      - `List<SkillTag> AppliesTo` - Tags the modifier applies to; empty means global.
      - `ModifierHook Hook` - Hook type for the modifier.
    - Public methods:
      - `ModifierData ToModifierData()` - Converts `ModifierDef` to `ModifierData`.

  - **class** `ModifierData`
    - Public fields/properties:
      - `string Id` - Identifier for the modifier.
      - `ModifierTarget Target` - Target of the modifier.
      - `ModifierOperation Operation` - Operation type of the modifier.
      - `float Value` - Value of the modifier.
      - `List<SkillTag> AppliesTo` - Tags the modifier applies to.
      - `ModifierHook Hook` - Hook type for the modifier.

## Key Behavior & Side Effects
- `ToModifierData()` creates a new instance of `ModifierData` with the current values from `ModifierDef`.

## Constraints & Failure Modes
- `Value` must be set when applying the modifier.
- `AppliesTo` can be empty, indicating a global application.

## Example
```csharp
ModifierDef skillModifier = ScriptableObject.CreateInstance<ModifierDef>();
skillModifier.modId = "exampleModifier";
skillModifier.Target = ModifierTarget.SomeTarget;
skillModifier.Operation = ModifierOperation.SomeOperation;
skillModifier.Value = 1.5f;
ModifierData modifierData = skillModifier.ToModifierData();
```

## Unknowns
- Specific implementations or definitions of `ModifierTarget`, `ModifierOperation`, `SkillTag`, and `ModifierHook` cannot be determined from this file.
```
