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
        [Tooltip("Stabile, eindeutige ID (nie recyceln).")]
        public string id;

        [Tooltip("Kurzer Titel (eine Zeile).")]
        public string title;

        [Header("Placement")]
        [Range(0, 3), Tooltip("Lane 0..3 (z.B. Combat/World/Crafting/Roster).")]
        public int lane;

        [Tooltip("Stage im 10er-Raster (10,20,30,...) – höhere Zahl = weiter unten / später.")]
        public int stage = 10;

        [Tooltip("Parent-IDs, die vor diesem Knoten abgeschlossen sein müssen.")]
#if UNITY_EDITOR
        public List<ResearchNodeDef> parentRefs;
#endif
        public List<string> parents = new List<string>();


        [Header("Unlock Mapping")]
        public List<ResearchUnlock> unlocks = new List<ResearchUnlock>();

        [Header("Requirements (UND-Logik)")]
        public ResearchRequirement requirements = new ResearchRequirement();

        private void OnValidate()
        {
            // Soft checks (hartes Validieren über das Editor-Tool)
            if (stage < 0) stage = 0;
            if (string.IsNullOrWhiteSpace(title)) title = name;

#if UNITY_EDITOR
            // Entferne offensichtliche Duplikate in parents/gates (nur kosmetisch)
            if (parentRefs != null)
            {
                parents = parentRefs
                    .Where(r => r != null && !string.IsNullOrEmpty(r.id))
                    .Select(r => r.id)
                    .Distinct(StringComparer.Ordinal)
                    .ToList();
            }
#endif

            requirements?.ValidateSoft(
                msg => Debug.LogWarning($"[ResearchNodeDef:{name}] {msg}", this),
                $"Node '{id ?? name}'"
            );
        }
    }

    [Serializable]
    public struct ResearchUnlock
    {
        public ResearchUnlockTypes unlockType;
        public string targetId;
    }
}