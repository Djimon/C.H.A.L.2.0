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
    private IReadOnlyCollection<string> _modifierTags;
    private IReadOnlyCollection<string> _uiTags;

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

    public IReadOnlyCollection<string> GetModifierTags()
        => _modifierTags ??= BuildModifierTags();

    public IReadOnlyCollection<string> GetUiTags()
        => _uiTags ??= BuildUiTags();

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
