using CHAL.Data;
using UnityEngine;

namespace CHAL.Systems.Skill
{
    public class BuffEffect : ActiveEffect
    {
        public BuffSettings Settings;
        public int CurrentStacks;
        private int CurrentMaxStacks = 1;

        public StackingMode Stacking = StackingMode.RefreshDuration;
        public bool modifierApplied = false;

        public BuffEffect(BuffSettings settings)
        {
            Settings = settings;
            EffectId = settings.EffectId;
            BaseDuration = settings.BaseDuration;
            RemainingTime = settings.BaseDuration;
            Modifier = settings.Modifier;
            CurrentMaxStacks = Mathf.Max(1, settings.BaseMaxStacks);
            Stacking = settings.Stacking;
            Kind = EffectKind.Buff;
        }

        /// <summary>Versucht, einen weiteren Stack hinzuzufügen (oder Dauer zu refreshen).</summary>
        public void TryAddStack(EffectReceiver source)
        {
            // Optional: MaxStacks dynamisch aus Mods ableiten (parallele Mechanik zu DoT)
            // int bonusStacks = (int)source.ActiveModifiers.Apply(ModifierTarget.BuffMaxStacks, 0, new List<SkillTag>{ SkillTag.Buff });
            // CurrentMaxStacks = Settings.BaseMaxStacks + bonusStacks;

            CurrentStacks = Mathf.Min(CurrentStacks, CurrentMaxStacks);

            if (Stacking == StackingMode.AddStacks)
            {
                if (CurrentStacks < CurrentMaxStacks)
                {
                    CurrentStacks++;
                }
                // In beiden Fällen (neuer Stack ODER Cap) die Dauer auffrischen:
                RemainingTime = BaseDuration;
            }
            else if (Stacking == StackingMode.RefreshDuration)
            {
                RemainingTime = BaseDuration;
            }

            // IgnoreIfActive / Replace → zentral in EffectReceiver.ApplyEffect behandeln
        }

    }

    [System.Serializable]
    public class BuffSettings
    {
        public string EffectId = "DefaultBuff";
        public ModifierData Modifier;          // Stat-Änderung während der Laufzeit
        public float BaseDuration = 6f;
        public int BaseMaxStacks = 1;
        public StackingMode Stacking = StackingMode.RefreshDuration;
    }
}
