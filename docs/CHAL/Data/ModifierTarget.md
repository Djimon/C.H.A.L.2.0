# CHAL.Data.ModifierTarget

_Automatically generated/updated from `Assets/src/Data/Enums/SkillEnums.cs`._

```text
1) Purpose
- Define game-related enums under CHAL.Data for skills, ranges, tags, and modifiers.
- Provide public surface: SkillType, SkillRange, SkillTag, ModifierTarget, ModifierOperation, ModifierHook.
- Serve as type-safe categorizations for skills and their modifiers without behavior logic.

2) Public API
- Namespace/module: CHAL.Data

- Types
  - public enum SkillType
    - Melee
    - Projectile
    - Spell
    - Summon

  - public enum SkillRange
    - Self = 0
    - Melee
    - Reach
    - MidDistance
    - FarDistance

  - public enum SkillTag
    - Melee
    - Projectile
    - Spell
    - AoE
    - Buff
    - Debuff
    - DoT
    - Aura
    - Summon
    - Fire
    - Cold
    - Poison
    - Arcane
    - Holy
    - Physical

  - public enum ModifierTarget
    - Damage
    - CritChance
    - CritMultiplier
    - AttackSpeed
    - ProjectileCount
    - ProjectileSpeed
    - PierceChance
    - Range
    - AoERadius
    - Duration
    - BuffDuration
    - DebuffDuration
    - DoTMaxStacks
    - DotDuration
    - CastTime
    - Cooldown
    - SummonCount
    - SummonHP
    - SummonDamage
    - AuraRange
    - MovementSpeed
    - Resist
    - Armor
    - MaxHP
    - LeechFactor

  - public enum ModifierOperation
    - Add
    - Mult
    - Replace

  - public enum ModifierHook
    - None
    - OnCast
    - OnHit
    - OnCrit
    - OnKill

3) Key Behavior & Side Effects
- No runtime behavior, methods, or state changes present.
- Pure type definitions; no side effects.

4) Constraints & Failure Modes
- No explicit constraints beyond standard C# enum semantics (underlying type default int).
- Self in SkillRange is explicitly 0; others follow default incremental values.

5) Example
```csharp
using CHAL.Data;

public class ExampleUsage
{
    public void Demo()
    {
        SkillType t = SkillType.Projectile;
        SkillRange r = SkillRange.Melee;
        ModifierTarget m = ModifierTarget.Damage;
        ModifierOperation op = ModifierOperation.Add;
        ModifierHook h = ModifierHook.OnHit;
    }
}
```

6) Unknowns
- No serialization attributes or mappings specified.
- How these enums map to runtime data (skills) or UI is not defined here.
- No default instances or helper methods provided.
