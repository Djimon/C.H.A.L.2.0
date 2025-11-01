# CHAL.Systems.Skill.StatusType

_Automatically generated/updated from `Assets/src/Systems/Skills/ActiveStatusEffect.cs`._

```text
1) Purpose
- Define data structures for active status effects within the skill system.
- Provide stacking strategy and status type enums used by skills.
- Expose a serializable data container (ActiveStatusEffect) with effect metadata, timing, and modifier references.

2) Public API
- Namespace/module
  - CHAL.Systems.Skill

- Types
  - public class ActiveStatusEffect [extends ...]
    - Public fields
      - string EffectId: identifier for the effect
      - StatusType Kind: type of status (DoT, Buff, Debuff, Aura)
      - EffectReceiver source: origin of the effect
      - EffectReceiver target: recipient of the effect
      - float BaseDuration: base duration of the effect
      - float RemainingTime: time remaining for the effect
      - ModifierData Modifier: modifier data applied by this effect

  - public enum StackingMode
    - RefreshDuration
      - Refresh duration; do not increase stacks
    - AddStacks
      - Increment stacks up to a maximum; refresh duration
    - IgnoreIfActive
      - If already active, ignore new instance
    - Replace
      - Replace the existing effect with the new one

  - public enum StatusType
    - DoT
    - Buff
    - Debuff
    - Aura

3) Key Behavior & Side Effects
- No methods or behavior are defined in this file; it is a data container.
- ActiveStatusEffect is marked [Serializable], enabling serialization of its fields.
- Semantics of stacking modes are documented via enum names and comments, but actual handling logic is not included here.

4) Constraints & Failure Modes
- No constructors or validation logic; all fields are public.
- Dependency on EffectReceiver and ModifierData types (defined elsewhere) for field types.
- No threading, async, or lifetime management specified in this file.

5) Example
```csharp
// Minimal usage example (placeholders for source/target/modifier)
var effect = new ActiveStatusEffect
{
  EffectId = "poison_tick",
  Kind = StatusType.DoT,
  source = someSourceReceiver,     // EffectReceiver
  target = someTargetReceiver,     // EffectReceiver
  BaseDuration = 5.0f,
  RemainingTime = 5.0f,
  Modifier = someModifierData          // ModifierData
};
```

6) Unknowns
- How ActiveStatusEffect instances are created/triggered in practice (caller logic).
- The definitions and behavior of EffectReceiver and ModifierData.
- Any defaults for stacking behavior (e.g., MaxStacks) not present in this file.
- Serialization details beyond the [Serializable] attribute (Unity vs. .NET serialization specifics).
- Interactions with other systems (e.g., lifecycle, removal, updates) not defined here.
```
