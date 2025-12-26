using CHAL.Data;
using System.Collections.Generic;
using UnityEngine;

public sealed class ResolvedSkill
{
    // IDs / Metadaten
    public string SkillId { get; }
    public string ModuleId { get; }
    public CoreType CoreType { get; }

    // Runtime-Werte (bereits mit Stats/Modifiers verrechnet)
    public float Damage { get; private set; }
    public float Radius { get; private set; }
    public float Duration { get; private set; }
    public float Cooldown { get; private set; }
    public float CastTime { get; private set; }
    public float ProjectileSpeed { get; private set; }
    public int ProjectileCount { get; private set; }

    public SkillRange Range = SkillRange.MeleeRange;
    

    public List<DamageEntry> DamageEntries { get; private set; }

    // Tags
    public TagContext tagContext { get; }

    public SkillType? SkillType => tagContext.SkillType;
    public IReadOnlyList<SkillDeliveryTag> DeliveryTags => tagContext.DeliveryTags;
    public IReadOnlyList<SkillMechanicTag> MechanicTags => tagContext.MechanicTags;
    public DamageType? DamageType => tagContext.DamageType;

    // (Optional) weitere numerische Achsen kannst du hier später ergänzen,
    // wenn sie in deinem MD stehen.

    public ResolvedSkill(
        string skillId,
        string moduleId,
        CoreType coretype,
        float damage,
        float radius,
        float duration,
        float cooldown,
        float castTime,
        float projectileSpeed,
        SkillRange range,
        int projectileCount,
        List<DamageEntry> damageEntries,
        TagContext tags)
    {
        SkillId = skillId;
        ModuleId = moduleId;
        CoreType = coretype;

        Damage = damage;
        Radius = radius;
        Duration = duration;
        Cooldown = cooldown;
        CastTime = castTime;
        ProjectileSpeed = projectileSpeed;
        Range = range;

        ProjectileCount = projectileCount;

        DamageEntries = damageEntries;

        tagContext = tags;
        
    }

/// <summary>
/// Updates the runtime values for the skill parameters.
/// </summary>
/// <param name="damage">The damage dealt by the skill.</param>
/// <param name="radius">The radius of the skill's effect.</param>
/// <param name="duration">The duration of the skill's effect.</param>
/// <param name="cooldown">The cooldown time before the skill can be used again.</param>
/// <param name="castTime">The time taken to cast the skill.</param>
/// <param name="projectileSpeed">The speed of the projectile.</param>
/// <param name="range">The range of the skill.</param>
/// <param name="aoeRadius">The area of effect radius.</param>
/// <param name="projectileCount">The number of projectiles to be fired.</param>
    public void UpdateRuntimeValues(
        float damage,
        float radius,
        float duration,
        float cooldown,
        float castTime,
        float projectileSpeed,
        SkillRange range,
        int projectileCount)
    {
        Damage = damage;
        Radius = radius;
        Duration = duration;
        Cooldown = cooldown;
        CastTime = castTime;
        ProjectileSpeed = projectileSpeed;
        Range = range;
        ProjectileCount = projectileCount;
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

/// <summary>
/// Adds or replaces the damage entries with the provided list.
/// </summary>
/// <param name="entries">The list of damage entries to add or replace.</param>
    public void AddOrReplaceDamageEntries(List<DamageEntry> entries)
    {
        DamageEntries = new List<DamageEntry>(entries);
    }
}
