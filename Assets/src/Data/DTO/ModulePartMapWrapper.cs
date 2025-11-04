using System.Collections.Generic;
using UnityEngine;

namespace CHAL.Data
{
    [System.Serializable]
/// <summary>
/// Wraps a collection of module part mappings.
/// </summary>
    public class ModulePartMapWrapper
    {
        public List<ModulePartMapEntry> entries;
/// <summary>
/// Converts the entries to a dictionary mapping module IDs to their corresponding parts.
/// </summary>
/// <returns>A dictionary where each key is a module ID and the value is an array of parts.</returns>
        public Dictionary<string, string[]> ToDictionary()
        {
            var dict = new Dictionary<string, string[]>();
            foreach (var e in entries)
                dict[e.moduleId] = e.parts;
            return dict;
        }

    }

    [System.Serializable]
    public class ModulePartMapEntry
    {
        public string moduleId;
        public string[] parts;
    }
}
