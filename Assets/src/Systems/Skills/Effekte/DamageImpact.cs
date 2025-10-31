using CHAL.Data;
using CHAL.Systems.Unit;
using System.Collections.Generic;
using UnityEngine;

namespace CHAL.Systems.Skill
{
    [CreateAssetMenu(fileName = "DamageImpact", menuName = "Skills/Impact/Damage")]
    public class DamageImpact : SkillImpactBase
    {
        [Tooltip("Damage entries applied by this effect (elemental/physical).")]
        public List<DamageEntry> Damages;

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