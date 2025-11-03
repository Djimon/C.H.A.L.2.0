# CHAL.Data.ModifierHook

_Automatically generated/updated from `Assets/src/Data/Enums/SkillEnums.cs`._

1) Purpose
- Defines public enums under the CHAL.Data namespace for skills and modifiers.
- Provides: SkillType, SkillRange, SkillTag, ModifierTarget, ModifierOperation, ModifierHook.
- Enumerators are taken directly from code; comments describe intended usage where present.

2) Public API
- Namespace: CHAL.Data
- Types
  - public enum SkillType
    - Melee
    - Projectile // fernkampf mit Porjektil speed, count, range
    - Spell      // casts mit effekete, AoE, Buff/Debuff, Aura
    - Summon
  - public enum SkillRange
    - Self = 0
    - Melee        // direkt angrenzend
    - Reach        // verlngerte Nahkampfreichweite (Speer, Hellebarde)
    - MidDistance  // mittlere Reichweite (typ. 510m)
    - FarDistance  // Fernkampf / Magie (Bogen, Feuerball)
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

3) Key Behavior & Side Effects
- No runtime behavior defined in this file; only type definitions.
- No methods, properties, or side effects present.

4) Constraints & Failure Modes
- SkillRange defines Self = 0 explicitly; other values are auto-incremented.
- No threading, async, or performance-guard guarantees specified.
- No null handling or input validation is defined here (surface is purely enum declarations).

5) Example
- Not derivable from this file (no usage examples or methods).

6) Unknowns
- How these enums are consumed elsewhere (parsing, serialization, UI mapping) is not defined here.
- Any intended flag semantics or bitwise combinations are not specified (these are plain enums, not [Flags]).
- Exact runtime validation, localization, or persistence behavior is not shown.
- Any additional modifier targets, operations, or hooks not listed here are unknown.
