// File: Assets/src/CHAL/Data/Affixes/AffixRegistryDef.cs
using System.Collections.Generic;
using UnityEngine;

namespace CHAL.Data
{
    [CreateAssetMenu(fileName = "AffixRegistry", menuName = "Data/Affixes/AffixRegistry")]
    public sealed class AffixRegistryDef : ScriptableObject
    {
        public List<AffixDef> Affixes = new List<AffixDef>();


        private void OnValidate()
        {
            if (Affixes == null) return;

            // Remove nulls, trim duplicates by Id (keep first)
            var seen = new HashSet<string>();
            for (int i = Affixes.Count - 1; i >= 0; i--)
            {
                var d = Affixes[i];
                if (d == null)
                {
                    Affixes.RemoveAt(i);
                    continue;
                }

                var id = (d.AffixId ?? string.Empty).Trim();
                if (string.IsNullOrEmpty(id))
                {
                    DebugManager.Warning($"[AffixRegistry] Affixes asset '{d.name}' has empty Id. Removed.", "System");
                    Affixes.RemoveAt(i);
                    continue;
                }

                if (!seen.Add(id))
                {
                    DebugManager.Warning($"[AffixRegistry] Duplicate Affix Id '{id}' detected. Keeping first occurrence.", "System");
                    Affixes.RemoveAt(i);
                }
            }
        }
    }
}
