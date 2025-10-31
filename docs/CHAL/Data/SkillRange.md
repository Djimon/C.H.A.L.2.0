# CHAL.Data.SkillRange

_Automatically generated/updated from `Assets/src/Data/Enums/SkillEnums.cs`._

# Purpose
- Defines various enums related to skills in the game.

# Public API
- Namespace: `CHAL.Data`
- Types:
  - `public enum SkillType`
    - Values: `Melee`, `Projectile`, `Spell`, `Summon`
  - `public enum SkillRange`
    - Values: `Self`, `Melee`, `Reach`, `MidDistance`, `FarDistance`
  - `public enum SkillTag`
    - Values: `Melee`, `Projectile`, `Spell`, `AoE`, `Buff`, `Debuff`, `DoT`, `Aura`, `Summon`, `Fire`, `Cold`, `Poison`, `Arcane`, `Holy`, `Physical`
  - `public enum ModifierTarget`
    - Values: `Damage`, `CritChance`, `CritMultiplier`, `AttackSpeed`, `ProjectileCount`, `ProjectileSpeed`, `PierceChance`, `Range`, `AoERadius`, `Duration`, `BuffDuration`, `DebuffDuration`, `DoTMaxStacks`, `DotDuration`, `CastTime`, `Cooldown`, `SummonCount`, `SummonHP`, `SummonDamage`, `AuraRange`, `MovementSpeed`, `Resist`, `Armor`, `MaxHP`, `LeechFactor`
  - `public enum ModifierOperation`
    - Values: `Add`, `Mult`, `Replace`
  - `public enum ModifierHook`
    - Values: `None`, `OnCast`, `OnHit`, `OnCrit`, `OnKill`

# Key Behavior & Side Effects
- Enums define types, ranges, tags, targets, operations, and hooks for skills, which can be used throughout the game logic.

# Constraints & Failure Modes
- No explicit guards or null/empty handling noted.
- Enums are extensible as indicated by comments.

# Example
```csharp
SkillType mySkill = SkillType.Spell;
SkillRange myRange = SkillRange.MidDistance;
```

# Unknowns
- No information on how these enums are utilized in the broader context of the application.

