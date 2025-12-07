using CHAL.Core;
using CHAL.Data;
using System.Collections.Generic;
using Unity.VisualScripting;

namespace CHAL.Systems.Skill
{
    public static class SkillResolveUtility
    {
        public static TagContext BuildTagContext(
            SkillModuleDef module,
            SkillFamilyDef family = null,
            ArchetypeModuleOverrideDef overrideDef = null)
        {
            // 1) Basis: SkillType & DamageType immer aus dem Modul
            var skillType = (SkillType?)module.SkillType;
            var damageType = (DamageType?)module.BaseDamageType;

            // 2) Delivery-Tags aufsammeln: Module + Family + Override
            var delivery = new List<SkillDeliveryTag>();
            if (family != null && family.DeliveryTags != null)
                delivery.AddRange(family.DeliveryTags);

            if (module.DeliveryTags != null)
                delivery.AddRange(module.DeliveryTags);

            if (overrideDef != null && overrideDef.DeliveryTagsAdd != null)
                delivery.AddRange(overrideDef.DeliveryTagsAdd);


            // 3) Mechanic-Tags aktuell nur im Modul
            var mechanics = new List<SkillMechanicTag>();
            if (family != null && family.MechanicTags != null)
                mechanics.AddRange(family.MechanicTags);

            if (module.MechanicTags != null)
                mechanics.AddRange(module.MechanicTags);

            return TagContext.From(
                skillType,
                delivery,
                mechanics,
                damageType
            );
        }

        public static ResolvedSkill ResolveBaseSkill(
            SkillModuleDef module,
            ArchetypeModuleOverrideDef overrideDef,
            string archetypeId)
        {
            var family = module.skillFamily; // oder module.family, je nach deiner Property

            // TagContext aus Modul/Family/Override
            var tagContext = BuildTagContext(module, family, overrideDef);

            // 1) Basiswerte aus dem Modul
            // Namen ggf. an deine echten Felder anpassen.
            float damage = module.BaseDamage;
            float radius = module.AoERadius;
            float duration = module.Duration;
            float cooldown = module.Cooldown;
            float castTime = module.CastTime;
            float projSpd = module.ProjectileSpeed;
            int projCount = module.ProjectileCount;
            float aoeRad = module.AoERadius;
            float range = ResolveRangeAsFloat(module.Range);

            //fill later via ResolvedSkill.AddOrReplaceDamageEntries(List<DamageEntry>)
            List<DamageEntry> dmgEntries = new List<DamageEntry>(); 

            // 2) Archetype-Overrides anwenden (echte Overrides, keine Multipliers)
            if (overrideDef != null)
            {
                if (overrideDef.OverrideDamage)
                    damage = overrideDef.DamageOverride;

                if (overrideDef.OverrideRadius)
                    radius = overrideDef.RadiusOverride;

                if (overrideDef.OverrideDuration)
                    duration = overrideDef.DurationOverride;
            }

            // 3) IDs bestimmen
            var skillId = module.SkillId; // oder SkillId, falls du es so benannt hast
            var familyId = family != null ? family.FamilyId : string.Empty;
            var moduleId = module.SkillId; // Module = Skill-Def, daher gleiche ID erst mal okay
            var coreId = string.Empty; // Core kommt später

            return new ResolvedSkill(
                skillId: skillId,
                familyId: familyId,
                moduleId: moduleId,
                coreId: coreId,
                archetypeId: archetypeId,
                damage: damage,
                radius: radius,
                duration: duration,
                cooldown: cooldown,
                castTime: castTime,
                projectileSpeed: projSpd,
                projectileCount: projCount,
                range: range,
                aoeRadius: aoeRad,
                damageEntries: dmgEntries,
                tags: tagContext
            );
        }

        private static float ResolveRangeAsFloat(SkillRange range)
        {

            switch (range)
            {
                case SkillRange.Self: return GameManager.Instance.Config.skillRanges.selfRange;
                case SkillRange.MeleeRange: return GameManager.Instance.Config.skillRanges.meleeRange;
                case SkillRange.Reach: return GameManager.Instance.Config.skillRanges.reachRange;
                case SkillRange.MidDistance: return GameManager.Instance.Config.skillRanges.midDistanceRange;
                case SkillRange.FarDistance: return GameManager.Instance.Config.skillRanges.farDistanceRange;
                default: return 0f;
            }
        }
    }
}
