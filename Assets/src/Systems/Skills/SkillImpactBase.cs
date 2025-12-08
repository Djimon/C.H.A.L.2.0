using CHAL.Systems.Unit;
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
        //public abstract void Apply(SkillInstance skill, EffectReceiver source, EffectReceiver target);

        /// <summary>
        /// Applies the specified skill from the source to the target, considering the hit result.
        /// </summary>
        /// <param name="skill">The skill to apply.</param>
        /// <param name="source">The effect receiver initiating the skill.</param>
        /// <param name="target">The effect receiver receiving the skill.</param>
        /// <param name="hit">The result of the hit.</param>
        public abstract void Apply(SkillInstance skill, EffectReceiver source, EffectReceiver target, HitResult hit);
        //{
        //    Apply(skill, source, target);
        //}
    }
}
