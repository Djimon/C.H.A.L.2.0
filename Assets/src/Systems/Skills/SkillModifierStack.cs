using CHAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CHAL.Systems.Skill
{
    public class ModifierStack
    {
        private readonly List<ModifierData> _mods = new();

        public void AddModifier(ModifierData mod) => _mods.Add(mod);

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