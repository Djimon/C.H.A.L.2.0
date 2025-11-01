# CHAL.Systems.Skill.TriggerSkillImpact

_Automatically generated/updated from `Assets/src/Systems/Skills/Effekte/TriggeSkillImpact.cs`._

```text
1) Purpose
- Defines TriggerSkillImpact, a ScriptableObject-based SkillImpact that triggers another skill on hit.
- Stores a reference to the SkillData to trigger (SkillToTrigger).
- On Apply, if SkillToTrigger is set, logs a debug message and executes the triggered skill via the execution pipeline.

2) Public API
- Namespace/module: CHAL.Systems.Skill

- Types
  - public class TriggerSkillImpact : SkillImpactBase
    - Public fields
      - SkillData SkillToTrigger
        - Role: Skill to trigger on hit
    - Public methods
      - public override void Apply(SkillInstance skill, EffectReceiver source, EffectReceiver target)
        - Parameters:
          - SkillInstance skill
          - EffectReceiver source
          - EffectReceiver target
        - Returns: void
        - Behavior:
          - If SkillToTrigger == null, return
          - Debug log: "[Effect] {source} triggers skill {SkillToTrigger.DisplayName} on {target}" (DebugManager.Log, level Test, category "Skill")
          - var inst = new SkillInstance(SkillToTrigger, source)
          - SkillExecutor.ExecuteSkill(inst, source, target)

3) Key Behavior & Side Effects
- Validation: If SkillToTrigger is null, Apply exits early without side effects.
- Debug: Logs a message describing the trigger with source/target and the triggered skill name.
- Execution: Creates a new SkillInstance using SkillToTrigger and the provided source, then executes it via SkillExecutor.ExecuteSkill with the given source and target.
- Flow: Triggered skill runs through the same execution pipeline as a normal skill.

4) Constraints & Failure Modes
- Guard: Only SkillToTrigger is checked for null; no guards for source/target null.
- Unity specifics: This is a ScriptableObject (CreateAssetMenu attribute) and intended to be created as an asset.
- Side effects: May trigger another skill (recursive if the triggered skill also uses TriggerSkillImpact).

5) Example
- Not provided in file.

6) Unknowns
- Details of SkillData, SkillInstance, SkillExecutor, and DebugManager implementations.
- Whether triggering skills may themselves trigger TriggerSkillImpact or cause recursive loops.
- Threading/synchronization behavior of SkillExecutor when invoked from Apply.
```
