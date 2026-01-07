using CHAL.Data;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CHAL.Systems.Research
{

    [Serializable]
/// <summary>
/// Represents the requirements for a research task, including waves, maps, and kill counts.
/// </summary>
    public class DeedRequirement
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
        [Min(0)] public int championCount;

/// <summary>
/// Validates the soft requirements and triggers warnings for any invalid conditions.
/// </summary>
/// <param name="warn">The action to invoke for warnings.</param>
/// <param name="ctx">The context string for the warning messages.</param>
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

/// <summary>
/// Checks if the current instance is empty, meaning it has no active waves, maps, or kills.
/// </summary>
/// <returns>True if empty; otherwise, false.</returns>
        public bool IsEmpty()
        {
            if (waves > 0 || maps > 0 || killsGeneral > 0 || eliteCount > 0 || bossCount > 0 || championCount > 0)
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
}
