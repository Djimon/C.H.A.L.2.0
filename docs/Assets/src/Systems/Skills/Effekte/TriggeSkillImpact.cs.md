# Assets/src/Systems/Skills/Effekte/TriggeSkillImpact.cs

_Automatic generated/updated._

```markdown
# Purpose
- Defines a `TriggerSkillImpact` class that triggers a skill upon an effect being applied.

# Public API
- Namespace: `CHAL.Systems.Skill`
- Types
  - **public class** `TriggerSkillImpact` [extends `SkillImpactBase`]
    - **public SkillData** `SkillToTrigger` - Skill that will be triggered on hit.
    - **public override void** `Apply(SkillInstance skill, EffectReceiver source, EffectReceiver target)` - Triggers the specified skill on the target if `SkillToTrigger` is not null.

# Key Behavior & Side Effects
- If `SkillToTrigger` is null, the method exits early without triggering any skill.
- Logs a message indicating the skill being triggered and the source and target of the effect.

# Constraints & Failure Modes
- No explicit guards against null or empty handling other than checking `SkillToTrigger`.
- Assumes `SkillExecutor.ExecuteSkill` handles any necessary error conditions related to skill execution.

# Example
```csharp
var triggerSkillImpact = new TriggerSkillImpact();
triggerSkillImpact.SkillToTrigger = someSkillData;
triggerSkillImpact.Apply(someSkillInstance, sourceReceiver, targetReceiver);
```

# Unknowns
- The behavior of `SkillExecutor.ExecuteSkill` and how it handles various states or errors is not defined in this file.
```
