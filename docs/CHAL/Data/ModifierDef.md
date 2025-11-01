# CHAL.Data.ModifierDef

_Automatically generated/updated from `Assets/src/Data/Defs/SkillModifierDef.cs`._

```text
1) Purpose
- Defines editor and runtime data structures for a skill modifier.
- ModifierDef (ScriptableObject) enables creation of modifier assets via Unity editor.
- ModifierData (Serializable) represents the runtime form of a modifier; ToModifierData() converts ModifierDef to ModifierData.
```

```text
2) Public API
- Namespace/module
  - CHAL.Data

- Types
  - Public class ModifierDef : ScriptableObject
    - Public string modId
    - Public ModifierTarget Target
    - Public ModifierOperation Operation
    - Public float Value = 1
    - Public List<SkillTag> AppliesTo
    - Public ModifierHook Hook = ModifierHook.None
    - Public ModifierData ToModifierData()
      - Returns a ModifierData instance with fields copied from this modifier definition
      - AppliesTo is copied via new List<SkillTag>(this.AppliesTo)

  - Public class ModifierData
    - [Serializable]
    - Public string Id
    - Public ModifierTarget Target
    - Public ModifierOperation Operation
    - Public float Value
    - Public List<SkillTag> AppliesTo
    - Public ModifierHook Hook = ModifierHook.None
```

```text
3) Key Behavior & Side Effects
- ModifierDef.ToModifierData()
  - Creates a new ModifierData with fields mapped as:
    - Id <- modId
    - Target <- Target
    - Operation <- Operation
    - Value <- Value
    - AppliesTo <- a new List<SkillTag>(AppliesTo)
    - Hook <- Hook
  - Includes no explicit null checks; relies on AppliesTo being non-null for the List copy.
- Editor-facing: ModifierDef is a ScriptableObject with CreateAssetMenu attribute, enabling asset creation with:
  - fileName = "SkillModifier"
  - menuName = "Data/SkillModifier"
```

```text
4) Constraints & Failure Modes
- AppliesTo copy behavior
  - Uses new List<SkillTag>(this.AppliesTo); if AppliesTo is null, ToModifierData() will throw ArgumentNullException.
- Nullability
  - No guards; null-handling not implemented in ToModifierData().
- Runtime surface
  - ModifierData is Serializable for runtime use; ModifierDef remains an editor asset type.
```

```text
5) Example
- Minimal usage (conversion from editor asset to runtime data)
```csharp
// Assuming you have a ModifierDef asset reference named editorDef
ModifierData data = editorDef.ToModifierData();
// data now contains Id, Target, Operation, Value, AppliesTo copy, and Hook
```
```

```text
6) Unknowns
- Definitions and allowed values for:
  - ModifierTarget
  - ModifierOperation
  - SkillTag
  - ModifierHook
- Exact semantics of empty AppliesTo (global) vs non-empty (tag-filter) beyond the inline comment
- Whether Unity initializes AppliesTo to an empty list by default or leaves it null
```
