# CHAL.Data.SkillRange

_Automatically generated/updated from `Assets/src/Data/Enums/SkillEnums.cs`._

1) Purpose
- Defines skill-related enums under the CHAL.Data namespace.
- Encapsulates categorization for skills (type, range, tags) and modifier configuration (target, operation, hook).
- Contains only public enum types; no runtime logic.

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
- None. No runtime behavior or state changes are defined in this file; it only declares enum types.

4) Constraints & Failure Modes
- None evident. No guards, threading, async, or performance considerations are encoded.

5) Example
- (Not applicable; no usage/example is derivable from this file alone.)

6) Unknowns
- None explicitly unknown; the file provides only enum declarations and inline comments, with no further behavioral contracts.
