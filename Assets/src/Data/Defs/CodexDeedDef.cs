using CHAL.Systems.Codex;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CHAL.Data
{
    [CreateAssetMenu(fileName = "ResearchNodeDef", menuName = "Research/Node")]
    public sealed class CodexDeedDef : ScriptableObject
    {
        [Header("Identity")]
        public string id;
        public string title;

        [Header("Unlock Mapping")]
        public List<CodexUnlock> unlocks = new List<CodexUnlock>();

        [Header("Requirements (UND-Logik)")]
        public DeedRequirement requirements = new DeedRequirement();
        internal string desc;

        private void OnValidate()
        {
            if (string.IsNullOrWhiteSpace(title)) title = name; 

        }
    }

    [Serializable]
    public struct CodexUnlock
    {
        public CodexUnlockTypes unlockType;
        public string targetId;
    }
}