using CHAL.Data;
using CHAL.Systems.Hero;
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
            float finalDamage = skill.Damage; // schon durch Mods (Str, Buffs, Passives) berechnet
            DamageType dmgType = skill.Data.DamageTypes.Count > 0
                ? skill.Data.DamageTypes[0].type
                : DamageType.Physical;

            DebugManager.Log($"[Effect] {source} deals {finalDamage} {dmgType} on {target}", DebugManager.EDebugLevel.Test, "Skill");
            target.TakeDamage(finalDamage, dmgType);
        }
    }
}