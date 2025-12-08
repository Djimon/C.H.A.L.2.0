using CHAL.Data;
using CHAL.Systems.Unit;
using UnityEngine;

namespace CHAL.Systems.Skill
{
    [CreateAssetMenu(fileName = "TriggerSkill", menuName = "Skills/Impact/TriggerSkill")]
/// <summary>
/// Represents a skill impact that triggers another skill on hit.
/// </summary>
    public class TriggerSkillImpact : SkillImpactBase
    {
        [Tooltip("Skill that will be triggered on hit.")]
        public SkillModuleDef SkillToTrigger;

        /// <summary>
        /// Applies the skill from the source to the target if the skill is available.
        /// </summary>
        /// <param name="skill">The skill instance to apply.</param>
        /// <param name="source">The effect receiver that triggers the skill.</param>
        /// <param name="target">The effect receiver that receives the skill.</param>
        public override void Apply(SkillInstance skill, EffectReceiver source, EffectReceiver target, HitResult hit)
        {
            if (SkillToTrigger == null) return;

            DebugManager.Log($"[Effect] {source} triggers skill {SkillToTrigger.DisplayName} on {target}", DebugManager.EDebugLevel.Test, "Skill");
            // Basic execution pipeline:
            var inst = new SkillInstance(SkillToTrigger, source);
            SkillExecutor.ExecuteSkill(inst, source, target);
        }
    }
}
