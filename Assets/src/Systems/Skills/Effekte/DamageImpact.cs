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

            if (skill == null || skill.Data == null || target == null)
                return;

            // 1) Basis-Schaden: bereits berechneter SkillInstance-Damage
            // (inkl. StatScaling + Modifiers aus SkillInstance.Recalculate()).
            float baseDamage = skill.Damage;
            if (baseDamage <= 0f)
                return;

            // 2) Damage-Quellen bestimmen:
            //    - Primär: lokal konfigurierte Damages-Liste auf diesem Impact.
            //    - Falls leer/null: fallback auf SkillData.DamageTypes.
            List<DamageEntry> damageEntries = null;

            if (Damages != null && Damages.Count > 0)
            {
                damageEntries = Damages;
            }
            else if (skill.Data.DamageTypes != null && skill.Data.DamageTypes.Count > 0)
            {
                damageEntries = skill.Data.DamageTypes;
            }

            // 3) Wenn immer noch nichts da ist → Fallback: voller Damage als Physical.
            if (damageEntries == null || damageEntries.Count == 0)
            {
                var fallbackType = DamageType.Physical; // TODO: ggf. Default-Typ konfigurieren
                DebugManager.Log(
                    $"[DamageImpact] Fallback damage: {source} deals {baseDamage:F1} {fallbackType} to {target} (no DamageEntries configured).",
                    DebugManager.EDebugLevel.Test,
                    "Skill");

                target.TakeDamage(baseDamage, fallbackType);
                return;
            }

            // 4) Konfigurierte DamageEntries anwenden
            for (int i = 0; i < damageEntries.Count; i++)
            {
                DamageEntry entry = damageEntries[i];

                var dmgType = entry.DmgType;

                var multiplier = entry.DmgMultiplier;

                //TODO: define negative multilpier: inverted Dmg, recoup? heal?
                //Negative Multiplier ersmtal ignorieren
                if (multiplier <= 0f)
                    continue;

                var finalDamage = baseDamage * multiplier;

                DebugManager.Log(
                    $"[DamageImpact] {source} deals {finalDamage:F1} {dmgType} to {target}",
                    DebugManager.EDebugLevel.Test,
                    "Skill");

                target.TakeDamage(finalDamage, dmgType);
            }

        }
    }
}
