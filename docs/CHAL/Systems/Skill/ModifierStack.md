# CHAL.Systems.Skill.ModifierStack

_Automatically generated/updated from `Assets/src/Systems/Skills/SkillModifierStack.cs`._

```text
1) Purpose
- Defines a ModifierStack that stores ModifierData entries and can apply them to a base value for a specific ModifierTarget.
- Supports additive, multiplicative, and replacement modifier operations.
- Applies optional AppliesTo filtering using a provided list of SkillTag values.

2) Public API

- Namespace/module
  - CHAL.Systems.Skill

- Types
  - public class ModifierStack
    - Public methods
      - public void AddModifier(ModifierData mod)
        - Adds a modifier to the internal collection.
      - public void RemoveModifier(ModifierData mod)
        - Removes a modifier from the internal collection.
      - public float Apply(ModifierTarget target, float baseValue, List<SkillTag> tags)
        - Computes the final value for the given target and base value, constrained by AppliesTo and provided tags.
        - Returns:
          - replacement value if any Replace modifier is applied, otherwise
          - (baseValue + additive) multiplied by the multiplicative factor.

3) Key Behavior & Side Effects
- AddModifier/RemoveModifier mutate the internal _mods List<ModifierData>.
- Apply behavior:
  - Iterates all modifiers; considers only those with mod.Target == target.
  - If mod.AppliesTo is non-null and non-empty, includes the modifier only if any tag in tags is contained in mod.AppliesTo.
  - For applicable modifiers, accumulates:
    - Add: add += mod.Value
    - Mult: mult *= (1 + mod.Value)
    - Replace: replace = mod.Value
  - After iteration:
    - If replace >= 0, returns replace.
    - Otherwise, returns (baseValue + add) * mult.
- No explicit thread-safety; relies on single-threaded usage unless external synchronization is used.
- No null checks; passing null to AddModifier can lead to runtime errors during Apply.

4) Constraints & Failure Modes
- Not thread-safe.
- Null ModifierData entries are not guarded; adding null will cause a NullReferenceException during Apply.
- Multiple Replace modifiers: the last encountered value determines the final replacement result.
- Complexity: O(n) per Apply, where n is the number of stored modifiers.

5) Example
- Minimal usage (illustrative; assumes ModifierData and related types are constructible):
  - var stack = new CHAL.Systems.Skill.ModifierStack();
  - var m = new ModifierData
  - {
  -     Target = someTarget,
  -     Value = 5f,
  -     Operation = ModifierOperation.Add,
  -     AppliesTo = new List<SkillTag> { SkillTag.Fire }
  - };
  - stack.AddModifier(m);
  - float result = stack.Apply(someTarget, 10f, new List<SkillTag> { SkillTag.Fire });

6) Unknowns
- Definitions and members of ModifierData, ModifierTarget, ModifierOperation, and SkillTag (from CHAL.Data) are not provided here.
- Exact behavior when AppliesTo is null vs. empty (inferred from code: null/empty bypasses the AppliesTo filter).
- Any additional overloads or extensions related to ModifierStack are not present in this file.
```
