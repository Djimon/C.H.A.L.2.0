# CHAL.Systems.Skill.BuffSettings

_Automatically generated/updated from `Assets/src/Systems/Skills/BuffStatusEffect.cs`._

```csharp
- Purpose
  - Defines BuffStatusEffect: a buff-type status effect with stacking and duration behavior.
  - Defines BuffSettings: a serializable configuration container for BuffStatusEffect.
  - Integrates with existing status-system types (ActiveStatusEffect, StatusType, StackingMode, ModifierData).

- Public API
  - Namespace/module: CHAL.Systems.Skill
  - Types
    - public class BuffStatusEffect : ActiveStatusEffect
      - Public fields
        - public BuffSettings Settings; // configuration for this buff
        - public int CurrentStacks; // current number of active stacks
        - private int CurrentMaxStacks = 1; // maximum allowed stacks (clamped at 1)
        - public StackingMode Stacking = StackingMode.RefreshDuration; // stacking behavior
        - public bool modifierApplied = false; // (flag, usage not shown in this file)
      - Public constructor
        - BuffStatusEffect(BuffSettings settings)
          - Sets Settings = settings
          - EffectId = settings.EffectId
          - BaseDuration = settings.BaseDuration
          - RemainingTime = settings.BaseDuration
          - Modifier = settings.Modifier
          - CurrentMaxStacks = Mathf.Max(1, settings.BaseMaxStacks)
          - Stacking = settings.Stacking
          - Kind = StatusType.Buff
      - Public methods
        - public void TryAddStack(EffectReceiver source)
          - Reapplies/extends stacks according to StackingMode
          - Clamps CurrentStacks to CurrentMaxStacks: CurrentStacks = Mathf.Min(CurrentStacks, CurrentMaxStacks)
          - If Stacking == StackingMode.AddStacks
            - If CurrentStacks < CurrentMaxStacks, increment CurrentStacks
            - Refresh remaining duration: RemainingTime = BaseDuration
          - Else if Stacking == StackingMode.RefreshDuration
            - Refresh remaining duration: RemainingTime = BaseDuration
          - Note: IgnoreIfActive / Replace is handled centrally in EffectReceiver.ApplyEffect
    - public class BuffSettings
      - [System.Serializable]
      - public string EffectId = "DefaultBuff"; // identifier for the buff effect
      - public ModifierData Modifier; // stat changes during runtime
      - public float BaseDuration = 5f; // base duration in seconds
      - public int BaseMaxStacks = 1; // base maximum stacks
      - public StackingMode Stacking = StackingMode.RefreshDuration; // stacking behavior

- Key Behavior & Side Effects
  - Initialization (BuffStatusEffect constructor)
    - Applies settings to internal fields
    - Ensures CurrentMaxStacks is at least 1
    - Sets Buff Kind
  - TryAddStack flow
    - Optional: dynamic max stacks logic is present as commented code (not active)
    - Clamps CurrentStacks to CurrentMaxStacks
    - If StackingMode.AddStacks and not at cap, increments CurrentStacks
    - In both AddStacks and RefreshDuration, updates RemainingTime to BaseDuration
    - If StackingMode.RefreshDuration, only duration is refreshed
  - External note
    - IgnoreIfActive / Replace behavior is managed in EffectReceiver.ApplyEffect, not here

- Constraints & Failure Modes
  - Guard: CurrentMaxStacks is initialized with Mathf.Max(1, BaseMaxStacks) to avoid <1
  - Clamp: CurrentStacks is clamped via Mathf.Min(CurrentStacks, CurrentMaxStacks)
  - Null handling: BuffSettings and related fields are public; no null checks shown in this file
  - Threading/async: None explicit; Unity-related data used on main thread

- Example
  ```csharp
  // Example: create a buff with stacking and apply a stack
  var settings = new BuffSettings
  {
      EffectId = "PowerBoost",
      BaseDuration = 8f,
      BaseMaxStacks = 3,
      Stacking = StackingMode.AddStacks
      // Modifier can be provided if available
  };

  var buff = new BuffStatusEffect(settings);
  buff.TryAddStack(null); // stack or refresh depending on stacking mode
  ```

- Unknowns
  - Definitions and members of: ActiveStatusEffect, EffectReceiver, StackingMode, StatusType, ModifierData
  - How Modifier interacts with runtime (other than being assigned)
  - Details of how buffs are applied/removed elsewhere in the system
  - Any additional behavior of CurrentStacks initialization (default is 0 unless set elsewhere)

```
