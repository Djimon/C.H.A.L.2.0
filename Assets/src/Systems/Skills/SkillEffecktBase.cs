using CHAL.Systems.Hero;
using System;
using UnityEngine;

namespace CHAL.Systems.Skill
{
    [Serializable]
    public abstract class SkillImpactBase : ScriptableObject
    {
        [Tooltip("Optional: unique identifier for debugging or balancing.")]
        public string EffectId;

        /// <summary>
        /// Executes the effect from source to target.
        /// </summary>
        public abstract void Apply(SkillInstance skill, EffectReceiver source, EffectReceiver target);
    }
}
