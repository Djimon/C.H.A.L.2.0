
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
        public EffectKind Kind;

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

    public enum EffectKind 
    { 
        DoT,
        Buff,
        Debuff,
        Aura
    }

}
