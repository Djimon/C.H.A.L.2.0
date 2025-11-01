# CHAL.Systems.Skill.TriggerSkillImpact

_Automatically generated/updated from `Assets/src/Systems/Skills/Effekte/TriggeSkillImpact.cs`._

1) Purpose
- Defines TriggerSkillImpact, a ScriptableObject-based SkillImpact that can trigger another skill on hit.
- Exposes a public SkillData field SkillToTrigger to configure which skill to trigger.
- On Apply, if SkillToTrigger is set, logs a debug message and executes the configured skill against the given source/target.

2) Public API
- Namespace/module: CHAL.Systems.Skill
- Types
  - public class TriggerSkillImpact : SkillImpactBase
    - Public fields/properties
      - public SkillData SkillToTrigger
        - Description: Skill to be triggered on hit.
    - Public methods
      - public override void Apply(SkillInstance skill, EffectReceiver source, EffectReceiver target)
        - Parameters: SkillInstance skill, EffectReceiver source, EffectReceiver target
        - Returns: void
        - Behavior:
          - If SkillToTrigger is null, returns immediately.
          - Logs: "[Effect] {source} triggers skill {SkillToTrigger.DisplayName} on {target}"
          - Creates a new SkillInstance(SkillToTrigger, source)
          - Calls SkillExecutor.ExecuteSkill(inst, source, target)

- Attributes
  - [CreateAssetMenu(fileName = "TriggerSkill", menuName = "Skills/Impact/TriggerSkill")]
  - [Tooltip("Skill that will be triggered on hit.")] on SkillToTrigger

3) Key Behavior & Side Effects
- When Apply is invoked with a non-null SkillToTrigger:
  - Logs debug information about the trigger event.
  - Instantiates a SkillInstance using SkillToTrigger and the provided source.
  - Delegates execution to SkillExecutor.ExecuteSkill with the new instance and the given source/target.
- Side effects:
  - A new skill is executed against the target, as defined by SkillToTrigger.
  - No modification to source/target performed within this method beyond the skill execution.

4) Constraints & Failure Modes
- Guard: If SkillToTrigger is null, Apply returns without side effects.
- Null handling: Source and target are passed to SkillExecutor; no null guards are shown for these parameters in this file.
- Serialized config: Requires SkillToTrigger to be assigned in the Unity inspector to trigger a skill.
- Dependencies implied: SkillInstance, SkillExecutor, SkillData, DebugManager, and EffectReceiver types are used but defined elsewhere.

6) Unknowns
- Details of SkillImpactBase behavior and interactions with other skill systems are not defined in this file.
- Exact behavior of SkillExecutor.ExecuteSkill (synchronous vs. asynchronous, error handling) is not defined here.
- How SkillToTrigger.DisplayName resolves at runtime beyond usage in the log message.
- Any side effects on the source/target outside of the executed skill’s own effects are not specified here.
