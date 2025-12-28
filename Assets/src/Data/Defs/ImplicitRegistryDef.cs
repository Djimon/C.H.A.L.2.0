// File: Assets/src/CHAL/Data/Implicits/ImplicitRegistryDef.cs
using System.Collections.Generic;
using UnityEngine;

namespace CHAL.Data
{
    [CreateAssetMenu(fileName = "ImplicitRegistry", menuName = "Data/Implicits/ImplicitRegistry")]
    public sealed class ImplicitRegistryDef : ScriptableObject
    {
        public List<ImplicitDef> Implicits = new List<ImplicitDef>();

        private void OnValidate()
        {
            if (Implicits == null) return;

            // Remove nulls, trim duplicates by Id (keep first)
            var seen = new HashSet<string>();
            for (int i = Implicits.Count - 1; i >= 0; i--)
            {
                var d = Implicits[i];
                if (d == null)
                {
                    Implicits.RemoveAt(i);
                    continue;
                }

                var id = (d.ImplicitId ?? string.Empty).Trim();
                if (string.IsNullOrEmpty(id))
                {
                    DebugManager.Warning($"[ImplicitRegistryDef] Implicit asset '{d.name}' has empty Id. Removed.", "System");
                    Implicits.RemoveAt(i);
                    continue;
                }

                if (!seen.Add(id))
                {
                    DebugManager.Warning($"[ImplicitRegistryDef] Duplicate Implicit Id '{id}' detected. Keeping first occurrence.", "System");
                    Implicits.RemoveAt(i);
                }
            }
        }
    }
}
