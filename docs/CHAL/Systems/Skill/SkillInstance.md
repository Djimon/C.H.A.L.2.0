# CHAL.Systems.Skill.SkillInstance

_Automatically generated/updated from `Assets/src/Systems/Skills/SkillInstance.cs`._

Purpose
- Defines a runtime representation of a skill instance, including computed values and cooldown state.
- Recalculates derived values from SkillData and the owning entity's modifiers.
- Holds a reference to the owning EffectReceiver.

Public API
- Namespace/module
  - CHAL.Systems.Skill
- Types
  - public class SkillInstance
    - Public properties
      - public SkillData Data { get; private set; } // underlying skill data
      - public float Damage { get; private set; } // calculated damage
      - public float CastTime { get; private set; } // calculated cast time
      - public float Cooldown { get; private set; } // calculated cooldown
      - public float Range { get; private set; } // calculated range
      - public float Duration { get; private set; } // calculated duration
      - public float ProjectileSpeed { get; private set; } // calculated projectile speed
      - public int ProjectileCount { get; private set; } // calculated projectile count
      - public float AoERadius { get; private set; } // calculated AoE radius
    - Public methods
      - public SkillInstance(SkillData data, EffectReceiver owner)
      - public void Recalculate()
      - public bool IsReady()
      - public void StartCooldown()
      - public void TickCooldown(float deltaTime)
      - public float GetCooldownRemaining()
      - public override string ToString()

Key surface notes
- Data property is read/write privately; all computed fields are exposed as read-only public properties.
- Constructor requires SkillData and an EffectReceiver owner; immediately calls Recalculate.

public surface (signatures exactly as in file)
- public SkillInstance(SkillData data, EffectReceiver owner)
- public void Recalculate()
- public bool IsReady()
- public void StartCooldown()
- public void TickCooldown(float deltaTime)
- public float GetCooldownRemaining()
- public override string ToString()
- public SkillData Data { get; private set; }
- public float Damage { get; private set; }
- public float CastTime { get; private set; }
- public float Cooldown { get; private set; }
- public float Range { get; private set; }
- public float Duration { get; private set; }
- public float ProjectileSpeed { get; private set; }
- public int ProjectileCount { get; private set; }
- public float AoERadius { get; private set; }

Key Behavior & Side Effects
- Construction
  - Stores data and owner; calls Recalculate() to compute initial values.
  - Uses Data.Tags (or empty list if null) and owner.ActiveModifiers.
  - Computes:
    - Damage, CastTime, Cooldown, Range (via BalanceManager.Instance.GetRangeValue(Data.Range)), Duration, ProjectileSpeed, ProjectileCount (cast from modifier result), AoERadius.
  - Logs initialization via DebugManager.Log.
- Recalculate
  - Refreshes all derived fields from current Data, tags, and active modifiers.
- Readiness and cooldown
  - IsReady: returns true if cooldownRemaining <= 0; clamps cooldownRemaining to 0 when ready.
  - StartCooldown: sets cooldownRemaining to Cooldown.
  - TickCooldown: decreases cooldownRemaining by deltaTime.
  - GetCooldownRemaining: returns non-negative remaining cooldown (Mathf.Max(0f, cooldownRemaining)).
- String representation
  - ToString returns a summary including DisplayName, Dmg, CD, Range, Dur, ProjSpeed, AoE.

Constraints & Failure Modes
- Null handling
  - Data.Tags is guarded: uses Data.Tags ?? new List<SkillTag>() to avoid null reference.
  - Constructor assumes non-null data and owner; null inputs could throw.
- External dependencies
  - Recalculate relies on BalanceManager.Instance.GetRangeValue and owner.ActiveModifiers; may fail if BalanceManager not initialized or owner/modifiers are unavailable.
  - Debug/logging invoked during Recalculate.
- Runtime state
  - cooldownRemaining is runtime; TickCooldown may drive it negative until GetCooldownRemaining clamps it to 0.
  - ProjectileCount is derived by casting to int after modifier application; potential narrowing if modifiers yield non-integral values.

Example
- Minimal usage (derivable from file)
```csharp
// Example usage (minimal)
SkillInstance skill = new SkillInstance(skillData, owner);
skill.Recalculate();
if (skill.IsReady())
{
    // perform skill usage
    skill.StartCooldown();
}

// In a game loop, advance cooldown
skill.TickCooldown(deltaTime);
float remaining = skill.GetCooldownRemaining();
```

Unknowns
- Definitions of SkillData, EffectReceiver, ModifierTarget, and Modifier logic (Apply) beyond usage here.
- Behavior of BalanceManager.GetRangeValue and how Range, etc., are balanced.
- Details of DebugManager.Log, and Tag types (SkillTag).
- Any higher-level integration (e.g., how SkillInstance is invoked by other systems) beyond this file.

Code references (relevant snippets)
- Recalculate uses: Data.Tags, ownedBy.ActiveModifiers, BalanceManager.Instance.GetRangeValue(Data.Range)
- Runtime fields: cooldownRemaining, Data, ownedBy
- Public interface: IsReady, StartCooldown, TickCooldown, GetCooldownRemaining, ToString

```csharp
// All code above is as shown in SkillInstance.cs
```
