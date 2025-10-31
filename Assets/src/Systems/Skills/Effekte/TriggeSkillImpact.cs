using CHAL.Data;
using CHAL.Systems.Unit;
using UnityEngine;

namespace CHAL.Systems.Skill
{
    [CreateAssetMenu(fileName = "TriggerSkill", menuName = "Skills/Impact/TriggerSkill")]
    public class TriggerSkillImpact : SkillImpactBase
    {
        [Tooltip("Skill that will be triggered on hit.")]
        public SkillData SkillToTrigger;

        public override void Apply(SkillInstance skill, EffectReceiver source, EffectReceiver target)
        {
            if (SkillToTrigger == null) return;

            DebugManager.Log($"[Effect] {source} triggers skill {SkillToTrigger.DisplayName} on {target}", DebugManager.EDebugLevel.Test, "Skill");
            // Basic execution pipeline:
            var inst = new SkillInstance(SkillToTrigger, source);
            SkillExecutor.ExecuteSkill(inst, source, target);
        }
    }
}
