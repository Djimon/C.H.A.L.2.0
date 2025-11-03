# CHAL.Systems.Skill.DebuffSettings

_Automatically generated/updated from `Assets/src/Systems/Skills/DebuffStatusEffect.cs`._

1) Purpose
- Runtime DEBUFF on a unit (negative modifier). Modifier add/remove is handled centrally by EffectReceiver (on apply/expire).
- DebuffStatusEffect implements stacking/refresh behavior and lifecycle integration for debuffs.
- DebuffSettings defines configuration for a DebuffStatusEffect (target modifier, duration, stacking, etc.).

2) Public API
- Namespace/module
  - CHAL.Systems.Skill

- Types

  - public class DebuffStatusEffect : ActiveStatusEffect
    - Public fields
      - public DebuffSettings Settings — configuration for this debuff
      - public int CurrentStacks — number of active stacks (initially 1)
      - private int _currentMaxStacks — maximum number of stacks (initially 1)
      - public StackingMode Stacking — stacking behavior (AddStacks / RefreshDuration / etc.)
      - public bool modifierApplied — flag (usage not shown in this file)
    - Public methods
      - public DebuffStatusEffect(DebuffSettings settings)
        - Constructor that initializes: Settings, EffectId, Modifier, BaseDuration, RemainingTime, _currentMaxStacks, CurrentStacks, Stacking, Kind
        - Sets Kind = StatusType.Debuff
      - public void TryAddStack(EffectReceiver source)
        - Reapply handling based on Stacking
        - If StackingMode.AddStacks:
          - If CurrentStacks < _currentMaxStacks, then CurrentStacks++
          - RemainingTime = BaseDuration (refresh on reapply)
        - Else if StackingMode.RefreshDuration:
          - RemainingTime = BaseDuration
        - Other modes: ignored here (central handling in EffectReceiver.ApplyEffect)
    - Inheritance
      - Extends ActiveStatusEffect (base class is assumed to provide core status effect behavior)

  - [System.Serializable] public class DebuffSettings
    - Public fields
      - public string EffectId = "Debuff_Default"
      - public ModifierData Modifier
      - public float BaseDuration = 5f
      - public int BaseMaxStacks = 1
      - public StackingMode Stacking = StackingMode.RefreshDuration
        - Semantics align with DoT-style stacking

3) Key Behavior & Side Effects
- DebuffStatusEffect constructor
  - EffectId resolution:
    - If Settings?.EffectId is non-empty, use Settings.EffectId
    - Else if Settings?.Modifier is non-null, use Settings.Modifier.Id
    - Else use "Debuff"
  - Modifier = Settings?.Modifier
  - BaseDuration = max(0f, Settings?.BaseDuration ?? 0f)
  - RemainingTime = BaseDuration
  - _currentMaxStacks = max(1, Settings?.BaseMaxStacks ?? 1)
  - CurrentStacks = 1
  - Stacking = Settings?.Stacking ?? StackingMode.RefreshDuration
  - Kind = StatusType.Debuff

- TryAddStack(source)
  - If Stacking == StackingMode.AddStacks:
    - If CurrentStacks < _currentMaxStacks, increment CurrentStacks
    - RemainingTime = BaseDuration (refresh on reapply)
  - Else if Stacking == StackingMode.RefreshDuration:
    - RemainingTime = BaseDuration
  - Else
    - No action here (decision left to central EffectReceiver.ApplyEffect)
  - Note: There is a commented hint about dynamically deriving max stacks from modifiers:
    - Optional: int bonus = (int)source.ActiveModifiers.Apply(ModifierTarget.DebuffMaxStacks, 0f, null);
    - _currentMaxStacks = Mathf.Max(1, Settings.BaseMaxStacks + bonus);

4) Constraints & Failure Modes
- Null/empty handling
  - Settings may be null; defaults used:
    - BaseDuration clamp to >= 0
    - _currentMaxStacks defaults to 1
    - Stacking defaults to RefreshDuration
  - EffectId resolves safely using null-conditional checks
- Threading/async
  - Not explicit in this file
- Performance/allocation hints
  - No explicit hints beyond straightforward field initialization and simple checks

5) Example
- Not derivable from this file; no explicit usage example provided.

6) Unknowns
- Details of ActiveStatusEffect base class (exact fields like EffectId, RemainingTime, BaseDuration, etc.)
- Implementation/details of EffectReceiver and how it handles Add/Replace semantics
- Full definitions of StackingMode, StatusType, ModifierData, and Modifier semantics
- How modifierApplied is used elsewhere (not shown in this file)
- Any external interactions beyond what is declared here (e.g., DoT/Debuff targets)

