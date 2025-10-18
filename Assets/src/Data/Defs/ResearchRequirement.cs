using CHAL.Data;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ResearchRequirement
{
    [Min(0)] 
    public int waves;
    [Min(0)] 
    public int maps;

    public List<MapRequirement> mapRequirements = new List<MapRequirement>();

    [Min(0)]
    public int killsGeneral;

    public List<KillTagCount> killsByTag = new List<KillTagCount>();

    [Min(0)] public int eliteCount;
    [Min(0)] public int bossCount;

    public void ValidateSoft(Action<string> warn, string ctx)
    {
        if (waves < 0 || maps < 0 || killsGeneral < 0 || eliteCount < 0 || bossCount < 0)
            warn?.Invoke($"{ctx}: Negative Anforderungen sind nicht erlaubt.");

        if (killsByTag != null)
        {
            for (int i = 0; i < killsByTag.Count; i++)
            {
                var t = killsByTag[i];
                if (t == null) { warn?.Invoke($"{ctx}: killsByTag[{i}] ist null."); continue; }
                if (string.IsNullOrWhiteSpace(t.enemyTag))
                    warn?.Invoke($"{ctx}: killsByTag[{i}] hat leeren Tag.");
                if (t.count < 0)
                    warn?.Invoke($"{ctx}: killsByTag[{i}] hat negativen Count.");
            }
        }
    }

    public bool IsEmpty()
    {
        if (waves > 0 || maps > 0 || killsGeneral > 0 || eliteCount > 0 || bossCount > 0)
            return false;

        if (killsByTag != null)
            foreach (var t in killsByTag)
                if (t != null && t.count > 0) return false;

        return true;
    }
}

[Serializable]
public sealed class KillTagCount
{
    public string enemyTag;
    public int count;
}

[Serializable]
public struct MapRequirement
{
    public MapDifficulty difficulty;
    public int amount;
}
