using CHAL.Data;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CHAL.Data
{
    // ========= Editor-Klasse =========
    [CreateAssetMenu(fileName = "SkillModifier", menuName = "Data/SkillModifier")]
    public class ModifierDef : ScriptableObject
    {
        public string modId;
        public ModifierTarget Target;
        public ModifierOperation Operation;
        public float Value = 1; //muss bein anwenden der Midifier imemr gesetzt werden
        public List<SkillTag> AppliesTo;   // leer = global, sonst tag-Filter
        public ModifierHook Hook = ModifierHook.None;

        public ModifierData ToModifierData()
        {
            return new ModifierData
            {
                Id = modId,
                Target = this.Target,
                Operation = this.Operation,
                Value = this.Value,
                AppliesTo = new List<SkillTag>(this.AppliesTo),
                Hook = this.Hook
            };
        }
    }

    // ========= Runtime-Klasse =========
    [Serializable]
    public class ModifierData
    {
        public string Id;
        public ModifierTarget Target;
        public ModifierOperation Operation;
        public float Value;
        public List<SkillTag> AppliesTo;
        public ModifierHook Hook = ModifierHook.None;
    }
}