# CHAL.Data.SkillTag

_Automatically generated/updated from `Assets/src/Data/Enums/SkillEnums.cs`._

Purpose
- Defines data enums used for skills and modifiers in the CHAL.Data namespace.
- Groups related concepts: SkillType, SkillRange, SkillTag, ModifierTarget, ModifierOperation, ModifierHook.

Public API
- Namespace/module
  - CHAL.Data
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
    - //  beliebig erweiterbar
  - public enum ModifierOperation
    - Add        // +10
    - Mult       // 1.2
    - Replace    // fester Wert
  - public enum ModifierHook
    - None
    - OnCast
    - OnHit
    - OnCrit
    - OnKill

Key Behavior & Side Effects
- None defined in this file (only type definitions).

Constraints & Failure Modes
- None evident (no guards, threading, or runtime constraints expressed here).

Example
- Not derivable from this file (no usage examples provided).

Unknowns
- Semantics and usage of these enums within the broader codebase are not defined here.
- Any serialization, persistence, or Unity integration details are not specified.
- Exact numeric semantics beyond Self = 0 for SkillRange are not further constrained in this file.

