using System;
using System.Collections.Generic;

namespace CHAL.Data
{
    [Serializable]
    public enum SkillType
    {
        Melee,      //Nahkmampf
        Ranged,     //fernkampf mit Porjektil speed, count, range
        Spell,      //casts mit effekete, AoE, Buff/Debuff, Aura
        Summon

    }

    [Serializable]
    public enum SkillRange
    {
        Self = 0,
        MeleeRange,        // direkt angrenzend
        Reach,        // verlängerte Nahkampfreichweite (Speer, Hellebarde)
        MidDistance,  // mittlere Reichweite (typ. 5–10m)
        FarDistance   // Fernkampf / Magie (Bogen, Feuerball)
    }

    [Serializable]
    public enum SkillDeliveryTag
    {
        //Melee, //-> already SkillType
        Projectile,
        AoE,  //->Area = dmg + Auras 
        Orb,
        Beam,
        Nova,
        Ground, //= Area on Ground  != AoE (Area on Target)    
        Spin,
        Chain,
        Cone,

        //Nur hier damit alter code nicht kaputt geht
        //TODO: DoTStatusEffect ändern und dann hier rauslöschen
        DoT, //verschoben in SkillMechanicTag

    }

    [Serializable]
    public enum SkillMechanicTag
    {
        //Mechanik/Rolle
        Buff,
        Debuff,
        DoT,
        Curse, //=debuff?
        Mark,
        Summon, // -> SkillType
        Knockback,
        Aura,
        Movement,
        Hazard,
        Trigger

        // Damgetypes - use normal DamageType enum
    }

    [Serializable]
    public enum ModifierTarget
    {
        SkillDamage,
        CritChance,
        CritMultiplier,
        AttackSpeed,
        ProjectileCount,
        ProjectileSpeed,
        PierceChance,
        Range,
        Radius,
        Duration,
        //BuffDuration,
        //DebuffDuration,
        DoTMaxStacks,
        DoTDuration,
        DotDamage,
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

    [Serializable]
    public enum ModifierOperation
    {
        Add,        // +10
        Mult,       // ×1.2
        Replace     // fester Wert
    }

    [Serializable]
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