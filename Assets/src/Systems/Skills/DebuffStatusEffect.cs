using CHAL.Data;
using CHAL.Systems.Unit;
using UnityEngine;
using static UnityEngine.InputManagerEntry;

namespace CHAL.Systems.Skill
{
    /// <summary>
    /// Runtime DEBUFF on a unit (negative modifier).  
    /// Modifier add/remove is handled centrally by EffectReceiver (on apply/expire).
    /// </summary>
    public class DebuffStatusEffect : ActiveStatusEffect
    {
        public DebuffSettings Settings;
        public int CurrentStacks = 1;
        private int _currentMaxStacks = 1;

        public StackingMode Stacking = StackingMode.RefreshDuration;
        public bool modifierApplied;

        public DebuffStatusEffect(DebuffSettings settings)
        {
            Settings = settings;
            EffectId = string.IsNullOrEmpty(settings?.EffectId)
                ? (settings?.Modifier != null ? settings.Modifier.Id : "Debuff")
                : settings.EffectId;

            Modifier = settings?.Modifier;
            BaseDuration = Mathf.Max(0f, settings != null ? settings.BaseDuration : 0f);
            RemainingTime = BaseDuration;

            _currentMaxStacks = Mathf.Max(1, settings != null ? settings.BaseMaxStacks : 1);
            CurrentStacks = 1;
            Stacking = settings != null ? settings.Stacking : StackingMode.RefreshDuration;

            Kind = StatusType.Debuff; // nutzt denselben Modifier-Lifecycle wie Buffs (Add/Remove im Receiver)
        }

        /// <summary>
        /// Reapply-Handling: increase stack till macxStacks or refresh duration.
        /// </summary>
        public void TryAddStack(EffectReceiver source)
        {
            // Optional: max stacks dynamisch aus Modifiers ableiten (analog zu DoT), wenn ihr das nutzt:
            // int bonus = (int)source.ActiveModifiers.Apply(ModifierTarget.DebuffMaxStacks, 0f, null);
            // _currentMaxStacks = Mathf.Max(1, Settings.BaseMaxStacks + bonus);

            if (Stacking == StackingMode.AddStacks)
            {
                if (CurrentStacks < _currentMaxStacks) CurrentStacks++;
                RemainingTime = BaseDuration; // refresh on reapply
            }
            else if (Stacking == StackingMode.RefreshDuration)
            {
                RemainingTime = BaseDuration;
            }
            // IgnoreIfActive / Replace → zentral in EffectReceiver.ApplyEffect(...) entscheiden
        }
    }

    [System.Serializable]
    public class DebuffSettings
    {
        public string EffectId = "Debuff_Default";
        public ModifierData Modifier;
        public float BaseDuration = 5f;
        public int BaseMaxStacks = 1;
        public StackingMode Stacking = StackingMode.RefreshDuration; // same semantics as DoT
    }
}
