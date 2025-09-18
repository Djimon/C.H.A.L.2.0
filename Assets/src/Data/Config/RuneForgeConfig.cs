using System;
using System.Collections.Generic;
using UnityEngine;

namespace CHAL.Data
{
    [Serializable]
    public class RuneForgeEntry
    {
        [Tooltip("Remain-Item, das als Input dient")]
        public ItemDef remain;

        [Tooltip("Mögliche Runen + Gewichtungen")]
        public List<RuneChance> runes;
    }

    [Serializable]
    public class RuneChance
    {
        public ItemDef rune;
        [Range(0f, 1f)] 
        public float weight;
    }

    [CreateAssetMenu(fileName = "RuneForgeConfig", menuName = "Config/RuneForgeConfig")]
    public class RuneForgeConfig : ScriptableObject
    {
        public List<RuneForgeEntry> entries;
    }
}