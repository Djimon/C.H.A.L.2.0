# CHAL.Systems.Skill.SkillInstance

_Automatically generated/updated from `Assets/src/Systems/Skills/SkillInstance.cs`._

1) Purpose
- Defines the SkillInstance class which represents a runtime instance of a Skill for a specific owner.
- Computes and exposes derived stat values (Damage, CastTime, Cooldown, Range, Duration, ProjectileSpeed, ProjectileCount, AoERadius) from SkillData + owner modifiers.
- Manages cooldown state (IsReady, StartCooldown, TickCooldown, GetCooldownRemaining) and provides a debug-friendly string representation.

2) Public API
- Namespace/module
  - CHAL.Systems.Skill

- Types
  - public class SkillInstance

- Public properties (surface)
  - public SkillData Data { get; private set; } // underlying skill data
  - public float Damage { get; private set; }
  - public float CastTime { get; private set; }
  - public float Cooldown { get; private set; }
  - public float Range { get; private set; }
  - public float Duration { get; private set; }
  - public float ProjectileSpeed { get; private set; }
  - public int ProjectileCount { get; private set; }
  - public float AoERadius { get; private set; }

- Public methods
  - public SkillInstance(SkillData data, EffectReceiver owner)
  - public void Recalculate()
  - public bool IsReady()
  - public void StartCooldown()
  - public void TickCooldown(float deltaTime)
  - public float GetCooldownRemaining()
  - public override string ToString()

- Private/internal fields (not public API)
  - private EffectReceiver ownedBy
  - private float cooldownRemaining

3) Key Behavior & Side Effects
- Recalculate()
  - Builds tags from Data.Tags or defaults to an empty list.
  - Retrieves owner's active modifiers.
  - Computes derived stats via modifiers:
    - Damage, CastTime, Cooldown, Range (via BalanceManager.Instance.GetRangeValue(Data.Range)), Duration, ProjectileSpeed, ProjectileCount (cast from modifier), AoERadius.
  - Logs initialization summary via DebugManager.Log.

- IsReady()
  - Returns true if cooldownRemaining <= 0.
  - If <= 0, clamps cooldownRemaining to 0 and returns true; otherwise returns false.

- StartCooldown()
  - Sets cooldownRemaining to Cooldown.

- TickCooldown(float deltaTime)
  - Subtracts deltaTime from cooldownRemaining.

- GetCooldownRemaining()
  - Returns Mathf.Max(0f, cooldownRemaining).

- ToString()
  - Returns a concise summary: "<DisplayName>: Dmg={Damage}, CD={Cooldown}, Range={Range}, Dur={Duration}, ProjSpeed={ProjectileSpeed}, AoE={AoERadius}".

4) Constraints & Failure Modes
- Data.Tags null safety: uses Data.Tags ?? new List<SkillTag>() to avoid null reference.
- Potential null references not guarded in this file (Data or owner may be null if misused).
- Dependency on BalanceManager.Instance; assumes a valid instance.
- Dependency on owner’s ActiveModifiers and modifier system (Mods.Apply) for all derived stats.
- GetCooldownRemaining clamps negative values to zero; IsReady also enforces zero when ready.
- No explicit threading/async handling; cooldown updates are expected to be invoked from a controlled update loop.

5) Example
```csharp
// Assuming skillData and owner are available
var instance = new CHAL.Systems.Skill.SkillInstance(skillData, owner);
if (instance.IsReady())
{
    // Activate skill logic here
    instance.StartCooldown();
}
```

6) Unknowns
- Definitions and structure of SkillData, SkillTag, EffectReceiver, and the modifier system (ModifierTarget, Apply).
- Behavior and lifecycle of BalanceManager.GetRangeValue.
- Details of DebugManager and its logging configuration.
- Integration points with Unity's update loop or event system.

