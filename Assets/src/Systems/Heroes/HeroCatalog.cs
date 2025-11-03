using System.Collections.Generic;
using UnityEngine;

namespace CHAL.Data
{
    [CreateAssetMenu(fileName = "HeroCatalog", menuName = "Data/Hero Catalog")]
    public class HeroCatalog : ScriptableObject
    {
        [Header("Alle Helden (Reihenfolge = UI-Reihenfolge)")]
        public List<HeroDef> allHeroes = new List<HeroDef>();

        // Lazy Index fr Lookups
        private Dictionary<string, HeroDef> _byId;

        private void OnValidate()
        {
            // Duplikate warnen, Index bei nderungen neu bauen
            var seen = new HashSet<string>();
            foreach (var h in allHeroes)
            {
                if (h == null || string.IsNullOrEmpty(h.HeroId)) continue; // HeroId ist in HeroDef vorhanden
                if (!seen.Add(h.HeroId))
                    DebugManager.Warning($"[HeroCatalog] Duplicate HeroId detected: {h.HeroId}", this);
            }
            _byId = null;
        }

        private void EnsureIndex()
        {
            if (_byId != null) return;
            _byId = new Dictionary<string, HeroDef>();
            foreach (var h in allHeroes)
            {
                if (h == null || string.IsNullOrEmpty(h.HeroId)) continue;
                _byId[h.HeroId] = h; // last wins
            }
        }

        // --- API ---

        public IReadOnlyList<HeroDef> GetAllForUI()
        {
            // Reihenfolge = wie in allHeroes angeordnet
            return allHeroes;
        }

        public HeroDef GetById(string heroId)
        {
            if (string.IsNullOrEmpty(heroId)) return null;
            EnsureIndex();
            return _byId.TryGetValue(heroId, out var def) ? def : null;
        }
    }
}
