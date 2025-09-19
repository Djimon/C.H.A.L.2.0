using System.Collections.Generic;
using UnityEngine;

namespace CHAL.Systems.Localization
{
    [System.Serializable]
    public class LocalizationEntry
    {
        public string key;
        public string value;
    }

    [System.Serializable]
    public class LocalizationDict
    {
        public List<LocalizationEntry> entries = new();

        public Dictionary<string, string> ToDictionary()
        {
            var dict = new Dictionary<string, string>();
            foreach (var e in entries)
            {
                if (!string.IsNullOrEmpty(e.key))
                    dict[e.key] = e.value;
            }
            return dict;
        }
    }
}
