using System;
using System.Collections.Generic;
using CHAL.Data;
using CHAL.Systems.Unit;

namespace CHAL.Systems.Skill
{
    [Serializable]
    public readonly struct HitContext
    {
        public readonly SkillInstance Skill;
        public readonly EffectReceiver Attacker;
        public readonly EffectReceiver Defender;
        public readonly IReadOnlyList<SkillTag> Tags;

        public readonly bool IsAttack;
        public readonly bool IsSpell;
        public readonly bool IsProjectile;
        public readonly bool IsAoE;

        public HitContext(SkillInstance skill, EffectReceiver attacker, EffectReceiver defender)
        {
            Skill = skill;
            Attacker = attacker;
            Defender = defender;

            Tags = skill.skillData.Tags;
            if (Tags == null)
                Tags = Array.Empty<SkillTag>();

            var type = skill?.skillData?.SkillType ?? SkillType.Melee;
            IsAttack = (type == SkillType.Melee || type == SkillType.Projectile);
            IsSpell = (type == SkillType.Spell);
            IsProjectile = (type == SkillType.Projectile);
            IsAoE = skill?.skillData?.isAoE ?? false;
        }
    }


    [Serializable]
    public readonly struct HitResult
    {
        public readonly HitContext Context;

        public readonly bool IsHit;
        public readonly bool IsCrit;

        public readonly float HitChance;

        public readonly float CritChance;

        public readonly float CritMultiplier;

        public HitResult(HitContext context, bool isHit, bool isCrit, float hitChance, float critChance, float critMultiplier)
        {
            Context = context;
            IsHit = isHit;
            IsCrit = isCrit;
            HitChance = hitChance;
            CritChance = critChance;
            CritMultiplier = critMultiplier;
        }

        public static HitResult CreateDefault(SkillInstance skill, EffectReceiver attacker, EffectReceiver defender)
        {
            var ctx = new HitContext(skill, attacker, defender);
            return new HitResult(ctx, true, false, 1f, 0f, 1f);
        }
    }
}
