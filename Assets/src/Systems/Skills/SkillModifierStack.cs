using CHAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CHAL.Systems.Skill
{
/// <summary>
/// Represents a collection of modifiers that can be added or removed.
/// </summary>
    public class ModifierStack
    {
        private readonly List<ModifierData> _mods = new();

/// <summary>
/// Adds a modifier to the collection.
/// </summary>
/// <param name="mod">The modifier to add.</param>
        public void AddModifier(ModifierData mod) => _mods.Add(mod);

/// <summary>
/// Removes a modifier from the collection.
/// </summary>
/// <param name="mod">The modifier to remove.</param>
        public void RemoveModifier(ModifierData mod) => _mods.Remove(mod);

/// <summary>
/// Applies modifiers to a base value based on the target and skill tags.
/// </summary>
/// <param name="target">The target to which the modifiers apply.</param>
/// <param name="baseValue">The initial value to modify.</param>
/// <param name="tags">A list of skill tags that may affect the modifiers.</param>
/// <returns>The modified value after applying the modifiers.</returns>
        public float Apply(ModifierTarget target, float baseValue, List<SkillTag> tags)
        {
            float add = 0f;
            float mult = 1f;
            float replace = -1f;

            foreach (var mod in _mods)
            {
                if (mod.Target != target) continue;
                if (mod.AppliesTo != null && mod.AppliesTo.Count > 0 &&
                    !tags.Any(t => mod.AppliesTo.Contains(t))) continue;

                switch (mod.Operation)
                {
                    case ModifierOperation.Add: add += mod.Value; break;
                    case ModifierOperation.Mult: mult *= (1 + mod.Value); break;
                    case ModifierOperation.Replace: replace = mod.Value; break;
                }
            }

            if (replace >= 0) return replace;
            return (baseValue + add) * mult;
        }
    }


}
