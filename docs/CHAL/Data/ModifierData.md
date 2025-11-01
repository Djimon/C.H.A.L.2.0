# CHAL.Data.ModifierData

_Automatically generated/updated from `Assets/src/Data/Defs/SkillModifierDef.cs`._

```text
1) Purpose
- Defines a ScriptableObject (ModifierDef) for editor-time definitions of skill modifiers (Unity asset).
- Defines a serializable runtime data class (ModifierData) used at runtime.
- Provides ToModifierData() to convert a ModifierDef asset into a runtime ModifierData instance (including a copy of AppliesTo).

```

```csharp
2) Public API
- Namespace/module
  - CHAL.Data

- Types
  - public class ModifierDef : ScriptableObject
    - public string modId
    - public ModifierTarget Target
    - public ModifierOperation Operation
    - public float Value = 1
      - defaulted to 1; intended to be set when applying the modifier
    - public List<SkillTag> AppliesTo
      - empty = global; otherwise acts as a tag filter
    - public ModifierHook Hook = ModifierHook.None
    - public ModifierData ToModifierData()
      - Returns a new ModifierData populated from this ModifierDef

  - public class ModifierData
    - public string Id
    - public ModifierTarget Target
    - public ModifierOperation Operation
    - public float Value
    - public List<SkillTag> AppliesTo
    - public ModifierHook Hook = ModifierHook.None

- Notes about asset creation
  - ModifierDef has [CreateAssetMenu(fileName = "SkillModifier", menuName = "Data/SkillModifier")]
```

```text
3) Key Behavior & Side Effects
- ToModifierData():
  - Creates and returns a new ModifierData with:
    - Id = modId
    - Target = Target
    - Operation = Operation
    - Value = Value
    - AppliesTo = new List<SkillTag>(AppliesTo)  // shallow copy
    - Hook = Hook
  - Side effects:
    - Produces a separate ModifierData instance; does not modify the source ModifierDef.
    - Requires AppliesTo to be non-null; otherwise new List<SkillTag>(AppliesTo) may throw.
- Asset behavior:
  - ModifierDef is a ScriptableObject; instances are created as Unity assets via the CreateAssetMenu.
  - Value defaults to 1 unless overridden in the asset.
  - AppliesTo empty means global behavior; non-empty applies tag-filtering.
```

```text
4) Constraints & Failure Modes
- AppliesTo null handling:
  - ToModifierData() assumes AppliesTo is non-null; null would cause an exception when constructing the list.
- No validation:
  - No runtime validation for Target, Operation, or Hook; all are exposed publicly and copied directly.
- Default values:
  - Value defaults to 1; the German-comment notes it should be set when applying the modifier.
- Threading/async:
  - No explicit threading or async behavior; all is synchronous.
```

```text
5) Example
- Minimal usage (assuming you have a ModifierDef asset instance):

```csharp
// Example usage
ModifierDef defAsset = /* obtain reference to a ModifierDef asset */;
ModifierData data = defAsset.ToModifierData();
// data now contains the runtime representation of the modifier
```

```

```text
6) Unknowns
- Definitions and exact semantics of:
  - ModifierTarget
  - ModifierOperation
  - SkillTag
  - ModifierHook
  (not defined in this file)
- Whether AppliesTo is ever null in practice; behavior if null is not defined here.
- How ModifierData is consumed at runtime beyond this conversion method.
```
