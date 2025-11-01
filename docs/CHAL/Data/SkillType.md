# CHAL.Data.SkillType

_Automatically generated/updated from `Assets/src/Data/Enums/SkillEnums.cs`._

1) Purpose
- Defines core skill-related enums used in the data layer.
- Groups SkillType, SkillRange, SkillTag, ModifierTarget, ModifierOperation, and ModifierHook under CHAL.Data.

2) Public API
- Namespace/module
  - CHAL.Data
- Types
  - public enum SkillType
    - Melee
    - Projectile
    - Spell
    - Summon
  - public enum SkillRange
    - Self
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
- No runtime flows, state changes, or error handling defined in this file.
- Enums serve as data definitions only; no methods or logic provided.

4) Constraints & Failure Modes
- No guards, null checks, or threading considerations present.
- ModifierTarget includes a note: "beliebig erweiterbar" (extensions are possible), indicating future extensibility.

5) Example
- (Not derivable from this file; no example provided.)

6) Unknowns
- How these enums are consumed elsewhere (data models, serialization, or runtime logic) is not shown.
- Serialization behavior (e.g., integer vs. string) and Unity-specific serialization implications are not specified.
- Exact runtime constraints, defaults, or mappings beyond this file are not available.
