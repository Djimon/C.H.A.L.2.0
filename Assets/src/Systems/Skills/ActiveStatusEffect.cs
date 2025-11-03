using CHAL.Data;
using CHAL.Systems.Unit;
using System;

namespace CHAL.Systems.Skill
{
    [Serializable]
    public class ActiveStatusEffect
    {
        public string EffectId;
        public StatusType Kind;

        public EffectReceiver source;
        public EffectReceiver target;

        public float BaseDuration;
        public float RemainingTime;

        public ModifierData Modifier;
    }

    public enum StackingMode
    {
        RefreshDuration,     // Dauer erneuern, keine Stacks erhöhen
        AddStacks,     // Stack++ bis MaxStacks, Dauer erneuern
        IgnoreIfActive,      // wenn vorhanden -> ignorieren
        Replace              // vorhandenen Effekt ersetzen
    }

    public enum StatusType 
    { 
        DoT,
        Buff,
        Debuff,
        Aura
    }

}
