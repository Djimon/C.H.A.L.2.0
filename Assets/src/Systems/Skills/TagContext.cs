using CHAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;

public sealed class TagContext
{
    public SkillType? SkillType { get; }
    public IReadOnlyList<SkillDeliveryTag> DeliveryTags { get; }
    public IReadOnlyList<SkillMechanicTag> MechanicTags { get; }
    public DamageType? DamageType { get; }

    // Zentral generierte flache Sicht:
    public IReadOnlyCollection<string> _modifierTags;
    public IReadOnlyCollection<string> _uiTags;

    public TagContext(
        SkillType? skillType,
        IReadOnlyList<SkillDeliveryTag> deliveryTags,
        IReadOnlyList<SkillMechanicTag> mechanicTags,
        DamageType? damageType)
    {
        SkillType = skillType;
        DeliveryTags = deliveryTags ?? Array.Empty<SkillDeliveryTag>();
        MechanicTags = mechanicTags ?? Array.Empty<SkillMechanicTag>();
        DamageType = damageType;
    }

/// <summary>
/// Retrieves a collection of modifier tags as read-only strings.
/// </summary>
/// <returns>A read-only collection of modifier tags.</returns>
    public IReadOnlyCollection<string> GetModifierTags()
        => _modifierTags = BuildModifierTags();

/// <summary>
/// Retrieves a collection of UI tags as read-only strings.
/// </summary>
/// <returns>A read-only collection of UI tags.</returns>
    public IReadOnlyCollection<string> GetUiTags()
        => _uiTags = BuildUiTags();

    private IReadOnlyCollection<string> BuildModifierTags()
    {
        var set = new HashSet<string>();

        if (SkillType.HasValue)
            set.Add($"type:{SkillType.Value}");

        if (DamageType.HasValue)
            set.Add($"element:{DamageType.Value}");

        foreach (var d in DeliveryTags)
            set.Add($"delivery:{d}");

        foreach (var m in MechanicTags)
            set.Add($"mechanic:{m}");

        return set;
    }

    private IReadOnlyCollection<string> BuildUiTags()
    {
        var set = new HashSet<string>();

        if (SkillType.HasValue)
            set.Add(SkillType.Value.ToString());

        if (DamageType.HasValue)
            set.Add(DamageType.Value.ToString());

        foreach (var d in DeliveryTags)
            set.Add(d.ToString());

        foreach (var m in MechanicTags)
            set.Add(m.ToString());

        return set;
    }

    // optional Convenience-Factory für Skills:
/// <summary>
/// Creates a new instance of TagContext from the specified parameters.
/// </summary>
/// <param name="type">The type of skill.</param>
/// <param name="delivery">A collection of skill delivery tags.</param>
/// <param name="mechanics">A collection of skill mechanic tags.</param>
/// <param name="damageType">The type of damage, if applicable.</param>
/// <returns>A new TagContext instance.</returns>
    public static TagContext From(
        SkillType? type,
        IEnumerable<SkillDeliveryTag> delivery,
        IEnumerable<SkillMechanicTag> mechanics,
        DamageType? damageType)
    {
        return new TagContext(
            type,
            delivery?.ToArray() ?? Array.Empty<SkillDeliveryTag>(),
            mechanics?.ToArray() ?? Array.Empty<SkillMechanicTag>(),
            damageType);
    }
}
