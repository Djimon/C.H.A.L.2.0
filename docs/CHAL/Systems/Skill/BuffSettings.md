# CHAL.Systems.Skill.BuffSettings

_Automatically generated/updated from `Assets/src/Systems/Skills/BuffStatusEffect.cs`._

1) Purpose
- Defines BuffStatusEffect class representing a buff-type, stackable status effect with duration handling, derived from ActiveStatusEffect.
- Provides TryAddStack(source) to apply/reapply stacks according to stacking mode, refreshing duration as needed.
- Defines BuffSettings (serializable) to configure a BuffStatusEffect (ID, duration, max stacks, stacking behavior, and modifier).

2) Public API
- Namespace: CHAL.Systems.Skill

- Types
  - public class BuffStatusEffect : ActiveStatusEffect
    - public BuffSettings Settings
      - configuration for this buff (EffectId, Modifier, duration, max stacks, stacking mode)
    - public int CurrentStacks
      - current number of active stacks
    - private int CurrentMaxStacks
      - maximum number of stacks allowed
    - public StackingMode Stacking
      - stacking behavior mode (e.g., RefreshDuration, AddStacks)
    - public bool modifierApplied
      - flag (purpose not fully defined in this file)
    - public BuffStatusEffect(BuffSettings settings)
      - constructor; initializes from settings
        - sets EffectId, BaseDuration, RemainingTime, Modifier
        - calculates CurrentMaxStacks = Max(1, settings.BaseMaxStacks)
        - sets Stacking and Kind = StatusType.Buff
    - public void TryAddStack(EffectReceiver source)
      - reapply logic for stacking
        - clamps CurrentStacks to CurrentMaxStacks
        - if Stacking == StackingMode.AddStacks
          - if CurrentStacks < CurrentMaxStacks -> CurrentStacks++
          - RemainingTime = BaseDuration
        - else if Stacking == StackingMode.RefreshDuration
          - RemainingTime = BaseDuration
        - note: dynamic max-stacks via mods is commented out
        - note: IgnoreIfActive/Replace handling is centralized in EffectReceiver.ApplyEffect (not in this file)

  - [System.Serializable]
    public class BuffSettings
      - public string EffectId = "DefaultBuff"
      - public ModifierData Modifier
        - stat changes during uptime
      - public float BaseDuration = 5f
      - public int BaseMaxStacks = 1
      - public StackingMode Stacking = StackingMode.RefreshDuration

3) Key Behavior & Side Effects
- Construction
  - BuffStatusEffect(settings) wires properties from settings:
    - EffectId, BaseDuration, RemainingTime, Modifier
    - CurrentMaxStacks = max(1, settings.BaseMaxStacks)
    - Stacking = settings.Stacking
    - Kind = StatusType.Buff
- Stacking behavior (TryAddStack)
  - Ensures CurrentStacks does not exceed CurrentMaxStacks
  - If StackingMode.AddStacks:
    - increment CurrentStacks if not at max
    - refresh RemainingTime to BaseDuration
  - If StackingMode.RefreshDuration:
    - refresh RemainingTime to BaseDuration
  - Note: other stacking modes have no effect in this method
  - Comment indicates dynamic max-stacks via mods is possible but disabled; central handling for IgnoreIfActive/Replace is done elsewhere

4) Constraints & Failure Modes
- CurrentMaxStacks is clamped to at least 1 during construction
- CurrentStacks is clamped to CurrentMaxStacks at the start of TryAddStack
- No null checks for Settings/Modifier in this file; relies on external initialization
- Only two stacking modes are explicitly handled in TryAddStack (AddStacks and RefreshDuration)
- BuffSettings is serializable, enabling Unity inspector usage

5) Example
```csharp
// Minimal usage example
var settings = new BuffSettings
{
    EffectId = "PowerUp",
    BaseDuration = 10f,
    BaseMaxStacks = 3,
    Stacking = StackingMode.AddStacks,
    Modifier = null // provide a valid ModifierData if needed
};

var buff = new BuffStatusEffect(settings);
```

6) Unknowns
- Details of ActiveStatusEffect base class (fields like EffectId, BaseDuration, RemainingTime, Modifier, Kind) are not defined in this file.
- Definition and values of StackingMode enum beyond those referenced (e.g., other modes).
- Definition of EffectReceiver and how ApplyEffect interacts with IgnoreIfActive/Replace (comment references).
- Definition and usage of ModifierData and how modifierApplied is intended to be used.
- Behaviour of BuffStatusEffect outside TryAddStack (e.g., how and when it is updated elsewhere).
