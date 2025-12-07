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
        private readonly List<ModifierData> _genericMods = new();

        private readonly List<DamageModifier> _damageMods = new();

        public IReadOnlyList<DamageModifier> DamageModifiers => _damageMods;

        /// <summary>
        /// Adds a modifier to the collection.
        /// </summary>
        /// <param name="mod">The modifier to add.</param>
        public void AddGenericModifier(ModifierData mod) => _genericMods.Add(mod);

/// <summary>
/// Adds a damage modifier to the collection.
/// </summary>
/// <param name="mod">The damage modifier to add.</param>
        public void AddDmgModifier(DamageModifier mod) => _damageMods.Add(mod);

/// <summary>
/// Removes a modifier from the collection.
/// </summary>
/// <param name="mod">The modifier to remove.</param>
        public void RemoveGenericModifier(ModifierData mod) => _genericMods.Remove(mod);

/// <summary>
/// Removes a damage modifier from the list of active modifiers.
/// </summary>
/// <param name="mod">The damage modifier to remove.</param>
        public void RemoveDmgModifier(DamageModifier mod) => _damageMods.Remove(mod);

/// <summary>
/// Applies modifiers to a base value based on the target and skill tags.
/// </summary>
/// <param name="target">The target to which the modifiers apply.</param>
/// <param name="baseValue">The initial value to modify.</param>
/// <param name="tags">A list of skill tags that may affect the modifiers.</param>
/// <returns>The modified value after applying the modifiers.</returns>
        public float Apply(ModifierTarget target, float baseValue, TagContext tagCTX)
        { 
            //List<SkillDeliveryTag>
            //TODO: umbaeu in TagContext mit ctx.GetmodifierTags()
            float add = 0f;
            float mult = 1f;
            float replace = -1f;

            var tags = tagCTX.GetModifierTags();

            foreach (var mod in _genericMods)
            {
                if (mod.Target != target) continue;
                if (mod.AppliesToTags != null 
                    && mod.AppliesToTags.Count > 0 
                    && !tags.Any(t => mod.AppliesToTags.Contains(t))) continue;

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
