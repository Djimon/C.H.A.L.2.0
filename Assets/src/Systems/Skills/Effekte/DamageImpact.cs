using CHAL.Data;
using CHAL.Systems.Unit;
using System.Collections.Generic;
using UnityEngine;

namespace CHAL.Systems.Skill
{
    [CreateAssetMenu(fileName = "DamageImpact", menuName = "Skills/Impact/Damage")]
/// <summary>
/// Represents the damage impact of a skill, including various damage entries.
/// </summary>
    public class DamageImpact : SkillImpactBase
    {
        [Tooltip("Damage entries applied by this effect (elemental/physical).")]
        public List<DamageEntry> Damages;

/// <summary>
/// Applies the skill effect to the target, dealing damage based on the skill and damage multipliers.
/// </summary>
/// <param name="skill">The skill instance being applied.</param>
/// <param name="source">The effect receiver that applies the skill.</param>
/// <param name="target">The effect receiver that receives the damage.</param>
        public override void Apply(SkillInstance skill, EffectReceiver source, EffectReceiver target)
        {
           
            foreach (var damage in Damages)
            {
                var dmgType = damage.DmgType ;
                var finalDamage = skill.Damage * damage.DmgMultiplier;
                DebugManager.Log($"[Effect] {source} deals {finalDamage} {dmgType} on {target}", DebugManager.EDebugLevel.Test, "Skill");
                target.TakeDamage(finalDamage, dmgType);
            }
   
        }
    }
}
