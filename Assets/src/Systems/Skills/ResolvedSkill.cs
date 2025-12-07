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

        tagContext = tags;
    }
}
