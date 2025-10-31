using CHAL.Data;
using CHAL.Systems.Unit;
using System;
using System.Collections.Generic;

namespace CHAL.Systems.Skill
{
    public class DoTStatusEffect : ActiveStatusEffect
    {
        public DoTSettings DoTsettings;
        public int CurrentStacks;
        private int CurrentMaxStacks = 1;
        public float internalTickTimer;

        // Regeln für Stacking
        public StackingMode Stacking = StackingMode.AddStacks;

        public DoTStatusEffect(DoTSettings settings)
        {
            DoTsettings = settings;
            EffectId = settings.EffectId;
            RemainingTime = settings.BaseDuration;
            CurrentMaxStacks = settings.BaseMaxStacks;
            internalTickTimer = settings.TickInterval;
        }

        /// <summary>
        /// Reapply-Handling: increase stack till macxStacks or refresh duration.
        /// </summary>
        public void TryAddStack(EffectReceiver source)
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
        public StackingMode Stacking = StackingMode.AddStacks;
    }
}