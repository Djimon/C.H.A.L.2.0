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

        public List<DamageEntry> Damages;

/// <summary>
/// Applies the skill effect from the source to the target based on the hit result.
/// </summary>
/// <param name="skill">The skill instance to apply.</param>
/// <param name="source">The effect receiver that initiates the skill.</param>
/// <param name="target">The effect receiver that receives the skill.</param>
/// <param name="hit">The result of the hit.</param>
        public override void Apply(SkillInstance skill, EffectReceiver source, EffectReceiver target, HitResult hit)
        {
            if (skill == null || skill.skillData == null || target == null)
                return;

            // SkillInstance.Damage enthält bereits FinalDMG_beforeDef pro DamageType.
            var damageEntries = skill.Damage;
            if (damageEntries == null || damageEntries.Count == 0)
                return;

            var packet = CombatCalculator.BuildDamagePacket(skill, source, target, hit);


            if (packet.DamagePerType.Count == 0)
                return;

            // Debug-Ausgabe konsolidiert
            foreach (var kv in packet.DamagePerType)
            {
                DebugManager.Log(
                    $"[DamageImpact] {source} deals {kv.Value:F1} {kv.Key} to {target} (packet total={packet.TotalDamageBeforeDef:F1})",
                    DebugManager.EDebugLevel.Test,
                    "Skill"
                );
            }

            // Phase 3: zentrale Defense-Pipeline
            target.TakeDamage(packet);
        }

/// <summary>
/// Applies the skill effect from the source to the target.
/// </summary>
/// <param name="skill">The skill instance to apply.</param>
/// <param name="source">The effect receiver that initiates the skill.</param>
/// <param name="target">The effect receiver that receives the skill effect.</param>
        public override void Apply(SkillInstance skill, EffectReceiver source, EffectReceiver target)
        {
            var hit = CombatCalculator.Resolve(source, target,skill);
            Apply(skill, source, target, hit);    
        }
    }
}
