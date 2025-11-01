# CHAL.Systems.Skill.DebuffSettings

_Automatically generated/updated from `Assets/src/Systems/Skills/DebuffStatusEffect.cs`._

Purpose
- Defines a runtime Debuff on a unit (negative modifier).
- Debuff lifecycle (add/remove) is managed centrally by EffectReceiver on apply/expire.
- Configured via DebuffSettings (duration, stacking behavior, and modifier).

Public API
- Namespace: CHAL.Systems.Skill

- Types
  - public class DebuffStatusEffect : ActiveStatusEffect
    - public DebuffSettings Settings
      - Debuff configuration used by the effect
    - public int CurrentStacks
      - Current number of active stacks (starts at 1)
    - public bool modifierApplied
      - Flag indicating whether the modifier was applied (usage not shown here)
    - public DebuffStatusEffect(DebuffSettings settings)
      - Constructor; computes identifiers, duration, stacking, and kind
    - public void TryAddStack(EffectReceiver source)
      - Reapply handling: increases stacks or refreshes duration based on stacking mode

  - [System.Serializable]
    public class DebuffSettings
    - public string EffectId
      - Identifier for the debuff effect (default "Debuff_Default")
    - public ModifierData Modifier
      - Optional modifier data to apply
    - public float BaseDuration
      - Base duration in seconds (non-negative)
    - public int BaseMaxStacks
      - Maximum allowed stacks (minimum 1)
    - public StackingMode Stacking
      - Stacking mode (e.g., RefreshDuration, AddStacks)

Key Behavior & Side Effects
- DebuffStatusEffect constructor behavior
  - Determines EffectId:
    - If Settings?.EffectId is non-empty, use it
    - Else if Settings?.Modifier != null, use Settings.Modifier.Id
    - Else use "Debuff"
  - Modifier = Settings?.Modifier
  - BaseDuration = max(0, Settings?.BaseDuration ?? 0)
  - RemainingTime = BaseDuration
  - _currentMaxStacks = max(1, Settings?.BaseMaxStacks ?? 1)
  - CurrentStacks = 1
  - Stacking = Settings?.Stacking ?? StackingMode.RefreshDuration
  - Kind = StatusType.Debuff (same modifier lifecycle as buffs)

- TryAddStack(source) behavior
  - If Stacking == StackingMode.AddStacks
    - If CurrentStacks < _currentMaxStacks, CurrentStacks++
    - RemainingTime = BaseDuration (refresh on reapply)
  - Else if Stacking == StackingMode.RefreshDuration
    - RemainingTime = BaseDuration
  - Other stacking modes: ignored here (decision to replace/ignore handled centrally by EffectReceiver)
  - Note: Optional dynamic max-stacks extension is shown in comments but not active

- DebuffSettings semantics
  - Debuff uses the same stacking semantics as DoT (per comment)
  - Stacking defaults to RefreshDuration if not provided

Constraints & Failure Modes
- Null safety and defaults
  - Settings can be null; constructor handles null safely by applying defaults
  - BaseDuration is clamped to non-negative
  - _currentMaxStacks defaults to at least 1
  - Stacking defaults to RefreshDuration when not specified
- No explicit threading/async handling; no explicit error paths
- TryAddStack currently uses Only the Stacking value and duration; source parameter is currently unused
- Unity serialization: DebuffSettings is [System.Serializable], suitable for Unity inspector

Example
```csharp
// Example usage
DebuffSettings s = new DebuffSettings
{
    BaseDuration = 6f,
    BaseMaxStacks = 2,
    Stacking = StackingMode.AddStacks
};

DebuffStatusEffect debuff = new DebuffStatusEffect(s);
debuff.TryAddStack(null);
```

Unknowns
- Details of ActiveStatusEffect base class behavior and properties (beyond what's used here)
- Exact meanings and values of StackingMode enum beyond used cases
- How EffectReceiver applies/removes effects in practice (centralized management)
- How modifierApplied is used elsewhere in the codebase
- Full semantics of ModifierData and its Id field (beyond how it’s referenced here)
- Any interaction with other systems not visible in this file (e.g., DoT-like interactions, UI updates)

Code-derived surface summary focuses strictly on types, members, and explicit behavior visible in this file.
