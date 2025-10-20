using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CHAL.Data
{
    [CreateAssetMenu(fileName = "ResearchNodeDef", menuName = "Research/Node")]
    public sealed class ResearchNodeDef : ScriptableObject
    {
        [Header("Identity")]
        public string id;
        public string title;

        [Header("Unlock Mapping")]
        public List<ResearchUnlock> unlocks = new List<ResearchUnlock>();

        [Header("Requirements (UND-Logik)")]
        public ResearchRequirement requirements = new ResearchRequirement();

        private void OnValidate()
        {
            if (string.IsNullOrWhiteSpace(title)) title = name; 

        }
    }

    [Serializable]
    public struct ResearchUnlock
    {
        public ResearchUnlockTypes unlockType;
        public string targetId;
    }
}