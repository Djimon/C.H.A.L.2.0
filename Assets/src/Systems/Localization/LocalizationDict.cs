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
/// <summary>
/// Represents a collection of localization entries.
/// </summary>
    public class LocalizationDict
    {
        public List<LocalizationEntry> entries = new();

/// <summary>
/// Converts the entries to a dictionary with string keys and values.
/// </summary>
/// <returns>A dictionary containing the entries as key-value pairs.</returns>
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
