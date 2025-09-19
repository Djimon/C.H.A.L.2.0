using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ModulePartMapWrapper
{
    public List<ModulePartMapEntry> entries;
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
