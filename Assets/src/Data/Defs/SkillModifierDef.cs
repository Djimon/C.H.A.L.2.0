using CHAL.Data;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CHAL.Data
{
    // ========= Editor-Klasse =========
    [CreateAssetMenu(fileName = "SkillModifier", menuName = "Data/SkillModifier")]
/// <summary>
/// Represents a modifier definition used in gameplay mechanics.
/// Contains properties that define how the modifier behaves.
/// </summary>
    public class ModifierDef : ScriptableObject
    {
        public string modId;
        public ModifierTarget Target;
        public ModifierOperation Operation;
        public float Value = 1; //muss bein anwenden der Midifier imemr gesetzt werden
        public List<SkillDeliveryTag> AppliesTo;   // leer = global, sonst tag-Filter
        public ModifierHook Hook = ModifierHook.None;

/// <summary>
/// Converts the current instance to a ModifierData object.
/// </summary>
/// <returns>A new instance of ModifierData populated with the current object's data.</returns>
        public ModifierData ToModifierData()
        {
            return new ModifierData
            {
                Id = modId,
                Target = this.Target,
                Operation = this.Operation,
                Value = this.Value,
                AppliesTo = new List<SkillDeliveryTag>(this.AppliesTo),
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
        public List<SkillDeliveryTag> AppliesTo;
        public ModifierHook Hook = ModifierHook.None;
    }
}
