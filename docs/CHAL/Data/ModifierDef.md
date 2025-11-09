# Assets/src/Data/Defs/SkillModifierDef.cs

_Automatically generated/updated from `Assets/src/Data/Defs/SkillModifierDef.cs`._

# Purpose
- Defines a modifier definition used in gameplay mechanics with properties that dictate its behavior.

# Public API
- Namespace: CHAL.Data
- Types
  - public class ModifierDef : ScriptableObject
    - Public fields/properties:
      - string modId
      - ModifierTarget Target
      - ModifierOperation Operation
      - float Value (default is 1)
      - List<SkillTag> AppliesTo (empty list means global)
      - ModifierHook Hook (default is ModifierHook.None)
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
      - ModifierHook Hook (default is ModifierHook.None)

# Key Behavior & Side Effects
- The `ToModifierData` method converts the `ModifierDef` instance into a `ModifierData` object, copying its properties.

# Constraints & Failure Modes
- The `Value` field must be set when applying the modifier.
- An empty `AppliesTo` list indicates that the modifier is global.

# Example
```csharp
ModifierDef skillModifier = ScriptableObject.CreateInstance<ModifierDef>();
skillModifier.modId = "exampleModifier";
skillModifier.Target = ModifierTarget.SomeTarget;
skillModifier.Operation = ModifierOperation.SomeOperation;
skillModifier.Value = 2.0f;
```

# Unknowns
- None.

