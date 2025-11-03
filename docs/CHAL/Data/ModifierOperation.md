# CHAL.Data.ModifierOperation

_Automatically generated/updated from `Assets/src/Data/Enums/SkillEnums.cs`._

1) Purpose
- Defines public enums used by the skill system in namespace CHAL.Data: SkillType, SkillRange, SkillTag, ModifierTarget, ModifierOperation, ModifierHook.
- Specifies discrete categories for skills, their ranges, tags, and modifier specifications (targets, operations, hooks).
- Contains no executable logic or methods; only type definitions.

2) Public API
- Namespace: CHAL.Data
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
- No runtime behavior; all members are simple enum values.
- SkillRange.Self is explicitly defined as 0; other values auto-increment from there.
- Underlying type is int by default (not explicitly declared).

4) Constraints & Failure Modes
- No guards, threading, or async behavior defined here.
- No validation or parsing logic; relies on usage context to enforce correctness.

5) Example
```csharp
// Example usage (minimal)
SkillType t = SkillType.Projectile;
SkillRange r = SkillRange.Melee;
```

6) Unknowns
- Semantics of each enum value (how they affect gameplay) are not defined in this file.
- Any broader validation, serialization format, or usage patterns are outside this file.
- German inline comments present in the code are not defined as part of the API semantics.

