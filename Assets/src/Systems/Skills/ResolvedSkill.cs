using CHAL.Data;
using System.Collections.Generic;
using UnityEngine;

public sealed class ResolvedSkill
{
    // IDs / Metadaten
    public string SkillId { get; }
    public string FamilyId { get; }
    public string ModuleId { get; }
    public string CoreId { get; }
    public string ArchetypeId { get; }

    // Runtime-Werte (bereits mit Stats/Modifiers verrechnet)
    public float Damage { get; }
    public float Radius { get; }
    public float Duration { get; }
    public float Cooldown { get; }
    public float CastTime { get; }
    public float ProjectileSpeed { get; }
    public float Range { get; }
    public float AoERadius { get; private set; }
    public int ProjectileCount { get; private set; }

    public List<DamageEntry> DamageEntries { get; private set; }

    // Tags – finales Set nach Family + Module + ArchetypeOverride + Core
    public TagContext tagContext { get; }

    public SkillType? SkillType => tagContext.SkillType;
    public IReadOnlyList<SkillDeliveryTag> DeliveryTags => tagContext.DeliveryTags;
    public IReadOnlyList<SkillMechanicTag> MechanicTags => tagContext.MechanicTags;
    public DamageType? DamageType => tagContext.DamageType;

    // (Optional) weitere numerische Achsen kannst du hier später ergänzen,
    // wenn sie in deinem MD stehen.

    public ResolvedSkill(
        string skillId,
        string familyId,
        string moduleId,
        string coreId,
        string archetypeId,
        float damage,
        float radius,
        float duration,
        float cooldown,
        float castTime,
        float projectileSpeed,
        float range,
        float aoeRadius,
        int projectileCount,
        List<DamageEntry> damageEntries,
        TagContext tags)
    {
        SkillId = skillId;
        FamilyId = familyId;
        ModuleId = moduleId;
        CoreId = coreId;
        ArchetypeId = archetypeId;

        Damage = damage;
        Radius = radius;
        Duration = duration;
        Cooldown = cooldown;
        CastTime = castTime;
        ProjectileSpeed = projectileSpeed;
        Range = range;
        AoERadius = aoeRadius;
        ProjectileCount = projectileCount;

        DamageEntries = damageEntries;

        tagContext = tags;
        
    }

    public float TotalDamage
    {
        get
        {
            if (DamageEntries == null || DamageEntries.Count == 0)
                return Damage;

            float total = 0f;
            for (int i = 0; i < DamageEntries.Count; i++)
            {
                var entry = DamageEntries[i];
                if (entry.damageOutput > 0f)
                    total += entry.damageOutput;
            }
            return total;
        }
    }

    public void AddOrReplaceDamageEntries(List<DamageEntry> entries)
    {
        DamageEntries = new List<DamageEntry>(entries);
    }
}
