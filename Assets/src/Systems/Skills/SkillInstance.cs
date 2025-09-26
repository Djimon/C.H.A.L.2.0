using CHAL.Data;
using CHAL.Systems.Hero;
using System.Collections.Generic;
using static UnityEngine.UI.GridLayoutGroup;

namespace CHAL.Systems.Skill
{
    public class SkillInstance
    {
        public SkillData Data { get; private set; }

        private HeroInstance ownedByHero;

        // berechnete Werte
        public float Damage { get; private set; }
        public float Cooldown { get; private set; }
        public float Range { get; private set; }
        public float Duration { get; private set; }
        public float ProjectileSpeed { get; private set; }
        public int ProjectileCount { get; private set; }
        public float AoERadius { get; private set; }

        public SkillInstance(SkillData data, HeroInstance owner)
        {
            Data = data;
            ownedByHero = owner;
            Recalculate();

        }

        public void Recalculate()
        {
            var tags = Data.Tags ?? new List<SkillTag>();
            var mods = ownedByHero.ActiveModifiers;

            Damage = mods.Apply(ModifierTarget.Damage, Data.BaseDamage, tags);
            Cooldown = mods.Apply(ModifierTarget.Cooldown, Data.Cooldown, tags);
            Range = mods.Apply(ModifierTarget.Range, Data.Range, tags);
            Duration = mods.Apply(ModifierTarget.Duration, Data.Duration, tags);
            ProjectileSpeed = mods.Apply(ModifierTarget.ProjectileSpeed, Data.ProjectileSpeed, tags);
            ProjectileCount = (int)mods.Apply(ModifierTarget.ProjectileCount, Data.ProjectileCount, tags);
            AoERadius = mods.Apply(ModifierTarget.AoERadius, Data.AoERadius, tags);
        }

        public override string ToString()
        {
            return $"{Data.DisplayName}: Dmg={Damage}, CD={Cooldown}, Range={Range}, " +
                   $"Dur={Duration}, ProjSpeed={ProjectileSpeed}, AoE={AoERadius}";
        }
    }
}
