# CHAL.Data.ModifierDef

_Automatically generated/updated from `Assets/src/Data/Defs/SkillModifierDef.cs`._

# Purpose
- Defines a modifier definition used in gameplay mechanics.
- Contains properties that define how the modifier behaves.

# Public API
- Namespace: CHAL.Data
- Types
  - public class ModifierDef : ScriptableObject
    - Public fields/properties:
      - string modId
      - ModifierTarget Target
      - ModifierOperation Operation
      - float Value (default: 1)
      - List<SkillTag> AppliesTo (empty = global, otherwise tag-filter)
      - ModifierHook Hook (default: ModifierHook.None)
    - Public methods:
      - ModifierData ToModifierData() : returns a new instance of ModifierData populated with the current object's data.

  - [Serializable]
    public class ModifierData
    - Public fields/properties:
      - string Id
      - ModifierTarget Target
      - ModifierOperation Operation
      - float Value
      - List<SkillTag> AppliesTo
      - ModifierHook Hook (default: ModifierHook.None)

# Key Behavior & Side Effects
- The `ToModifierData` method converts the current instance of `ModifierDef` to a `ModifierData` object.

# Constraints & Failure Modes
- The `Value` field must be set when applying the modifier.

# Example
- 
```csharp
ModifierDef modifier = ScriptableObject.CreateInstance<ModifierDef>();
modifier.modId = "exampleModifier";
modifier.Target = ModifierTarget.SomeTarget;
modifier.Operation = ModifierOperation.SomeOperation;
modifier.Value = 2.0f;
ModifierData data = modifier.ToModifierData();
```

# Unknowns
- None.

