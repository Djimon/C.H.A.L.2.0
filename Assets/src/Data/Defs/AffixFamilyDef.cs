using System;
using System.Collections.Generic;
using UnityEngine;

namespace CHAL.Data
{
    [CreateAssetMenu(fileName = "new AffixFamily", menuName = "Data/Affix Family")]
    public sealed class AffixFamilyDef : ScriptableObject
    {
        [Header("Family Meta")]
        public string FamilyName;

        [Header("Weighted Affix Entries")]
        public List<AffixEntry> Entries = new List<AffixEntry>();

        [Header("Tag Limits (optional)")]
        public List<TagLimitEntry> TagLimits = new List<TagLimitEntry>();

        // ---------- Public API ----------
        //TODO

        // ---------- Helpers ----------

        private static string NormalizeTag(string t) => string.IsNullOrWhiteSpace(t) ? "" : t.Trim().ToLowerInvariant();

        private void OnValidate()
        {
            // Entries: negative Gewichte & whitespace-IDs korrigieren
            if (Entries != null)
            {
                for (int i = 0; i < Entries.Count; i++)
                {
                    var e = Entries[i];
                    if (e.Weight < 0) e.Weight = 0;
                    if (!string.IsNullOrEmpty(e.AffixId)) e.AffixId = e.AffixId.Trim();
                    Entries[i] = e;
                }
            }

            // TagLimits: negative Limits und Tags normalisieren
            if (TagLimits != null)
            {
                for (int i = 0; i < TagLimits.Count; i++)
                {
                    var t = TagLimits[i];
                    t.Tag = NormalizeTag(t.Tag);
                    // -1 = kein Limit; >=0 = gültig
                    if (t.Limit < -1) t.Limit = -1;
                    TagLimits[i] = t;
                }
            }
        }
    }

    [Serializable]
    public struct AffixEntry
    {
        public string AffixId;
        public int Weight;
    }

    [Serializable]
    public struct TagLimitEntry
    {
        public string Tag;
        public int Limit;
    }
}
