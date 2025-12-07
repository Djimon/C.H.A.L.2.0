# Assets/src/Systems/Skills/Effekte/TriggeSkillImpact.cs

_Automatically generated/updated from `Assets/src/Systems/Skills/Effekte/TriggeSkillImpact.cs`._

1) Purpose
- Defines a skill impact that triggers another skill upon hitting a target.

2) Public API
- Namespace: CHAL.Systems.Skill
- Types
  - public class TriggerSkillImpact : SkillImpactBase
    - Public fields/properties:
      - SkillData SkillToTrigger: The skill that will be triggered on hit.
    - Public methods:
      - public override void Apply(SkillInstance skill, EffectReceiver source, EffectReceiver target): Applies the skill from the source to the target if the skill is available.

3) Key Behavior & Side Effects
- If `SkillToTrigger` is null, the method returns early without executing any skill.
- Logs the triggering of the skill using `DebugManager`.
- Creates a new `SkillInstance` for the `SkillToTrigger` and executes it using `SkillExecutor`.

4) Constraints & Failure Modes
- Requires `SkillToTrigger` to be non-null to execute the skill.
- No threading or async handling is evident.

5) Example
```csharp
// Example of creating and applying a TriggerSkillImpact
TriggerSkillImpact triggerSkillImpact = ScriptableObject.CreateInstance<TriggerSkillImpact>();
triggerSkillImpact.SkillToTrigger = someSkillData; // Assign a SkillData instance
triggerSkillImpact.Apply(someSkillInstance, sourceReceiver, targetReceiver);
```

6) Unknowns
- None.
