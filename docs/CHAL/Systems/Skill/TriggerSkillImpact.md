# CHAL.Systems.Skill.TriggerSkillImpact

_Automatically generated/updated from `Assets/src/Systems/Skills/Effekte/TriggeSkillImpact.cs`._

1) Purpose
- Defines a skill impact that triggers another skill upon hitting a target.

2) Public API
- Namespace: `CHAL.Systems.Skill`
- Types
  - public class `TriggerSkillImpact` [extends `SkillImpactBase`]
    - Public fields/properties:
      - `SkillData SkillToTrigger`: The skill that will be triggered on hit.
    - Public methods:
      - `void Apply(SkillInstance skill, EffectReceiver source, EffectReceiver target)`: Applies the skill from the source to the target if the skill is available.

3) Key Behavior & Side Effects
- If `SkillToTrigger` is null, the method returns early without executing any skill.
- Logs a message indicating the skill being triggered on the target.

4) Constraints & Failure Modes
- Requires `SkillToTrigger` to be non-null to execute the skill.
- No threading or async behavior is present.

5) Example
```csharp
var triggerSkillImpact = new TriggerSkillImpact();
triggerSkillImpact.SkillToTrigger = someSkillData;
triggerSkillImpact.Apply(someSkillInstance, sourceReceiver, targetReceiver);
```

6) Unknowns
- None.
