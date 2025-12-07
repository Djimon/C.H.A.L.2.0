namespace CHAL.Data
{
    public enum SkillType
    {
        Melee,      //
        Projectile, //fernkampf mit Porjektil speed, count, range
        Spell,      //casts mit effekete, AoE, Buff/Debuff, Aura
        Summon

    }

    public enum SkillRange
    {
        Self = 0,
        Melee,        // direkt angrenzend
        Reach,        // verlängerte Nahkampfreichweite (Speer, Hellebarde)
        MidDistance,  // mittlere Reichweite (typ. 5–10m)
        FarDistance   // Fernkampf / Magie (Bogen, Feuerball)
    }

    public enum SkillTag
    {
        //SkilLforms
        Melee,
        Projectile,
        Spell,
        AoE,
        Buff,
        Debuff,
        DoT,
        Aura,
        Summon,
        Trap,
        Orb,
        Mark,
        Movement,
        Nuke,
        Ground,
        Hazard,

        // Damgetypes
        Fire,
        Cold,
        Poison,
        Arcane,
        Holy,
        Physical
    }

    public enum ModifierTarget
    {
        Damage,
        CritChance,
        CritMultiplier,
        AttackSpeed,
        ProjectileCount,
        ProjectileSpeed,
        PierceChance,
        Range,
        AoERadius,
        Duration,
        //BuffDuration,
        //DebuffDuration,
        DoTMaxStacks,
        DoTDuration,
        TicksPerSecond,
        CastTime,
        Cooldown,
        SummonCount,
        SummonHP,
        SummonDamage,
        AuraRange,
        MovementSpeed,
        LeechFactor,
        HealAmount,
        StackLimit
        
        // … beliebig erweiterbar
    }

    public enum ModifierOperation
    {
        Add,        // +10
        Mult,       // ×1.2
        Replace     // fester Wert
    }

    public enum ModifierHook
    {
        None,
        OnCast,
        OnHit,
        OnCrit,
        OnKill,
        OnEnd
    }
}