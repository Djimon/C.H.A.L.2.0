
using CHAL.Data;
using CHAL.Systems.Hero;
using System;
using System.Collections.Generic;

namespace CHAL.Systems.Skill
{
    [Serializable]
    public class ActiveEffect
    {
        public string EffectId;
        public ModifierData Modifier;
        public float BaseDuration;
        public float RemainingTime;
        public HeroInstance Source; //wer hat den effek verursacht
    }

    public class DoTEffect : ActiveEffect
    {
        public DoTSettings DoTsettings;
        public int CurrentStacks;
        private int CurrentMaxStacks = 1;
        public float internalTickTimer;

        // Regeln für Stacking
        public StackBehavior Stacking = StackBehavior.AddStacks;

        public DoTEffect(DoTSettings settings)
        {
            DoTsettings = settings;
            EffectId = settings.EffectId;
            RemainingTime = settings.BaseDuration;
            CurrentMaxStacks = settings.BaseMaxStacks;
            internalTickTimer = settings.TickInterval;
        }

        /// <summary>
        /// Versucht, einen weiteren Stack hinzuzufügen.
        /// </summary>
        public void TryAddStack(HeroInstance source)
        {
            //Recalculate Max Stacks
            int bonusStacks = (int)source.ActiveModifiers.Apply(
                ModifierTarget.DoTMaxStacks,
                0,
                new List<SkillTag> { SkillTag.DoT }
            );

            CurrentMaxStacks = DoTsettings.BaseMaxStacks + bonusStacks;
            CurrentStacks = Math.Min(CurrentStacks, CurrentMaxStacks);

            //Apply Stacks, if possible
            if (CurrentStacks < CurrentMaxStacks)
            {
                CurrentStacks++;
            }
            else
            {
                RemainingTime = BaseDuration;
            }
        }
    }

    [System.Serializable]
    public class DoTSettings
    {
        public string EffectId = "DefaultDoT";
        public DamageType DamageType = DamageType.Poison;
        public float DamagePerTick = 1f;
        public float TickInterval = 1f;
        public float BaseDuration = 5f;
        public int BaseMaxStacks = 1;
        public StackBehavior Stacking = StackBehavior.AddStacks;
    }

    public enum StackBehavior 
    {
        RefreshDuration, 
        AddStacks,
        Replace 
    }

}
