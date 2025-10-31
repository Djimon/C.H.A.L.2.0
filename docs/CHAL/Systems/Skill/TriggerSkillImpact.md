# CHAL.Systems.Skill.TriggerSkillImpact

_Automatically generated/updated from `Assets/src/Systems/Skills/Effekte/TriggeSkillImpact.cs`._

# Purpose
- Defines the `TriggerSkillImpact` class that triggers a skill upon an effect being applied.

# Public API
- Namespace: `CHAL.Systems.Skill`
- Types
  - public class `TriggerSkillImpact` [extends `SkillImpactBase`]
    - Public fields/properties:
      - `SkillData SkillToTrigger`: Skill to be triggered on hit.
    - Public methods:
      - `void Apply(SkillInstance skill, EffectReceiver source, EffectReceiver target)`: Triggers the specified skill on the target if `SkillToTrigger` is not null.

# Key Behavior & Side Effects
- If `SkillToTrigger` is null, the method exits early without triggering any skill.
- Logs the triggering of the skill using `DebugManager`.

# Constraints & Failure Modes
- No explicit guards against null or empty handling beyond checking `SkillToTrigger`.
- No threading or async notes present.

# Example
```csharp
var triggerSkillImpact = new TriggerSkillImpact();
triggerSkillImpact.SkillToTrigger = someSkillData; // Assign a SkillData instance
triggerSkillImpact.Apply(someSkillInstance, sourceReceiver, targetReceiver);
```

# Unknowns
- None.

