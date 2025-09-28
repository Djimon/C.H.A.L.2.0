using CHAL.Systems.Hero;
using UnityEngine;

namespace CHAL.Systems.Skill
{
    [CreateAssetMenu(fileName = "TriggerSkillEffect", menuName = "Skills/Effects/TriggerSkill")]
    public class TriggerSkillEffect : SkillEffectBase
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
