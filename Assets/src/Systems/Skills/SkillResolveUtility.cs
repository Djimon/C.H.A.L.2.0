using CHAL.Core;
using CHAL.Data;
using System.Collections.Generic;
using Unity.VisualScripting;

namespace CHAL.Systems.Skill
{
    public static class SkillResolveUtility
    {
/// <summary>
/// Builds a TagContext based on the provided module, family, and override definitions.
/// </summary>
/// <param name="module">The skill module definition to base the context on.</param>
/// <param name="family">An optional skill family definition for additional tags.</param>
/// <param name="overrideDef">An optional override definition for the archetype module.</param>
/// <returns>The constructed TagContext.</returns>
        public static TagContext BuildTagContext(SkillModuleDef module, CoreType core)
        {
            if (module == null)
            {
                DebugManager.Error("[SkillResolveUtility] BuildTagContext: module is null.");
                return TagContext.From(null, new List<SkillDeliveryTag>(), new List<SkillMechanicTag>(), null);
            }

            // 1) Basis: SkillType & DamageType immer aus dem Modul
            var skillType = (SkillType?)module.SkillType;
            //TODO: Change to Core-Damagetype translation
            var damageType = (DamageType?)TranslateDamageType(module, core);

            // 2) Delivery-Tags aufsammeln: Module + Family + Override
            var delivery = new List<SkillDeliveryTag>();

            if (module.DeliveryTags != null)
                delivery.AddRange(module.DeliveryTags);


            // 3) Mechanic-Tags aktuell nur im Modul
            var mechanics = new List<SkillMechanicTag>();

            if (module.MechanicTags != null)
                mechanics.AddRange(module.MechanicTags);

            return TagContext.From(skillType, delivery, mechanics, damageType);
        }

/// <summary>
/// Resolves the base skill from the given module and overrides.
/// </summary>
/// <param name="module">The skill module definition.</param>
/// <param name="overrideDef">The archetype module override definition.</param>
/// <param name="archetypeId">The ID of the archetype.</param>
/// <returns>The resolved skill.</returns>
        public static ResolvedSkill ResolveBaseSkill(SkillModuleDef module, int skilltier, CoreType core)
        {
            if (module == null)
            {
                DebugManager.Error("[SkillResolveUtility] ResolveBaseSkill: module is null.");
                return null;
            }

            if (skilltier < module.minRequiredTier)
                DebugManager.Warning($"Inconsistant minRequiredTier {module.minRequiredTier} with skillTier {skilltier}","Skill");
                
            // TagContext aus Modul/Family/Override
            var tagContext = BuildTagContext(module, core);

            // 1) Basiswerte aus dem Modul
            // Namen ggf. an deine echten Felder anpassen.
            float damage = module.BaseDamage;
            float radius = module.Radius;
            float duration = module.Duration;
            float cooldown = module.Cooldown;
            float castTime = module.CastTime;
            float projSpd = module.ProjectileSpeed;
            int projCount = module.ProjectileCount;
            SkillRange skillrange = module.Range;

            //fill later via ResolvedSkill.AddOrReplaceDamageEntries(List<DamageEntry>)
            List<DamageEntry> dmgEntries = new List<DamageEntry>(); 

          
            // 3) IDs bestimmen
            var skillId = module.SkillId; // oder SkillId, falls du es so benannt hast
            var moduleId = module.SkillId; // Module = Skill-Def, daher gleiche ID erst mal okay

            return new ResolvedSkill(
                skillId: skillId,
                moduleId: moduleId,
                coretype: core,
                damage: damage,
                radius: radius,
                duration: duration,
                cooldown: cooldown,
                castTime: castTime,
                projectileSpeed: projSpd,
                projectileCount: projCount,
                range: skillrange,
                damageEntries: dmgEntries,
                tags: tagContext
            );
        }

        private static DamageType TranslateDamageType(SkillModuleDef module, CoreType core)
        {
            if (module == null)
                return DamageType.Physical;

            // Wenn Basic oder unbekannt: nimm das, was im SkillDef steht
            // (das passt zu deinem aktuellen Stand: SkillDef hat BaseDamageType)
            switch (core)
            {
 
                case CoreType.Basic:
                    return DamageType.Physical;
                case CoreType.Blazing:
                    return DamageType.Fire;
                case CoreType.Glacial:
                    return DamageType.Cold;
                case CoreType.Static:
                    return DamageType.Lightning;
                case CoreType.Venomous:
                    return DamageType.Poison;
                case CoreType.Infernal:
                    return DamageType.Daemonic;
                case CoreType.Radiant:
                    return DamageType.Holy;
                case CoreType.Seismic:
                    return DamageType.Earth;
                case CoreType.Aetheric:
                    return DamageType.Arcane;
                case CoreType.Nullified:
                    return DamageType.Void;
                case CoreType.Cthonic:
                    return DamageType.Abyssal;
                default:
                    return module.BaseDamageType;
            }
        }

        /// <summary>
        /// Resolves a SkillRange to its corresponding float value.
        /// </summary>
        /// <param name="range">The SkillRange to resolve.</param>
        /// <returns>The float value representing the range.</returns>
        public static float ResolveRangeAsFloat(SkillRange range)
        {

            switch (range)
            {
                case SkillRange.Self: return GameManager.Instance.BalanceConfig.skillRanges.selfRange;
                case SkillRange.MeleeRange: return GameManager.Instance.BalanceConfig.skillRanges.meleeRange;
                case SkillRange.Reach: return GameManager.Instance.BalanceConfig.skillRanges.reachRange;
                case SkillRange.MidDistance: return GameManager.Instance.BalanceConfig.skillRanges.midDistanceRange;
                case SkillRange.FarDistance: return GameManager.Instance.BalanceConfig.skillRanges.farDistanceRange;
                default: return 0f;
            }
        }
    }
}
