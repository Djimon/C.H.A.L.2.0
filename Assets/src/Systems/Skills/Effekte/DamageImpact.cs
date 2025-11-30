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

        public override void Apply(SkillInstance skill, EffectReceiver source, EffectReceiver target)
        {
            if (skill == null || skill.skillData == null || target == null)
                return;

            // Phase 2: SkillInstance.Damage ist bereits FinalDMG_beforeDef (skalar)
            float baseDamage = skill.Damage;
            if (baseDamage <= 0f)
                return;

            // Damage-Quellen:
            //  - primär: lokale Damages-Liste auf diesem Impact
            //  - fallback: SkillData.DamageTypes
            List<DamageEntry> damageEntries = null;

            if (Damages != null && Damages.Count > 0)
            {
                damageEntries = Damages;
            }
            else if (skill.skillData.DamageTypes != null && skill.skillData.DamageTypes.Count > 0)
            {
                damageEntries = skill.skillData.DamageTypes;
            }

            var packet = new DamagePacket
            {
                IsHitBased = true,
                IsDot = false
            };

            if (damageEntries == null || damageEntries.Count == 0)
            {
                var fallbackType = DamageType.Physical; // TODO: globalen Default konfigurierbar machen

                packet.AddDamage(fallbackType, baseDamage);

                DebugManager.Log(
                    $"[DamageImpact] Fallback packet: {source} deals {baseDamage:F1} {fallbackType} to {target} (Total={packet.TotalDamageBeforeDef:F1})",
                    DebugManager.EDebugLevel.Test,
                    "Skill");

                target.TakeDamage(packet);
                return;
            }

            // Konfigurierte DamageEntries -> Packet befüllen
            for (int i = 0; i < damageEntries.Count; i++)
            {
                DamageEntry entry = damageEntries[i];
                var dmgType = entry.DmgType;
                var multiplier = entry.DmgMultiplier;

                // TODO: später Regeln für negative Multiplier (heal, recoup, leech) definieren.
                if (multiplier <= 0f)
                    continue;

                var finalDamage = baseDamage * multiplier;
                packet.AddDamage(dmgType, finalDamage);
            }

            if (packet.DamagePerType.Count == 0)
                return;

            // Debug-Ausgabe konsolidiert
            foreach (var kv in packet.DamagePerType)
            {
                DebugManager.Log(
                    $"[DamageImpact] {source} deals {kv.Value:F1} {kv.Key} to {target} (packet total={packet.TotalDamageBeforeDef:F1})",
                    DebugManager.EDebugLevel.Test,
                    "Skill");
            }

            // Phase 3: zentrale Defense-Pipeline
            target.TakeDamage(packet);
        }

    }
}
