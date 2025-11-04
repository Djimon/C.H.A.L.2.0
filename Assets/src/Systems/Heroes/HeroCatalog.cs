using System.Collections.Generic;
using UnityEngine;

namespace CHAL.Data
{
    [CreateAssetMenu(fileName = "HeroCatalog", menuName = "Data/Hero Catalog")]
/// <summary>
/// Manages a catalog of heroes for the game.
/// Provides functionality to store and retrieve hero definitions.
/// </summary>
    public class HeroCatalog : ScriptableObject
    {
        [Header("Alle Helden (Reihenfolge = UI-Reihenfolge)")]
        public List<HeroDef> allHeroes = new List<HeroDef>();

        // Lazy Index für Lookups
        private Dictionary<string, HeroDef> _byId;

        private void OnValidate()
        {
            // Duplikate warnen, Index bei Änderungen neu bauen
            var seen = new HashSet<string>();
            foreach (var h in allHeroes)
            {
                if (h == null || string.IsNullOrEmpty(h.HeroId)) continue; // HeroId ist in HeroDef vorhanden
                if (!seen.Add(h.HeroId))
                    DebugManager.Warning($"[HeroCatalog] Duplicate HeroId detected: {h.HeroId}", "Hero");
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

/// <summary>
/// Retrieves a read-only list of hero definitions for the UI.
/// </summary>
/// <returns>A list of hero definitions.</returns>
        public IReadOnlyList<HeroDef> GetAllForUI()
        {
            // Reihenfolge = wie in allHeroes angeordnet
            return allHeroes;
        }

/// <summary>
/// Retrieves a hero definition by its unique identifier.
/// </summary>
/// <param name="heroId">The unique identifier of the hero.</param>
/// <returns>The hero definition if found; otherwise, null.</returns>
        public HeroDef GetById(string heroId)
        {
            if (string.IsNullOrEmpty(heroId)) return null;
            EnsureIndex();
            return _byId.TryGetValue(heroId, out var def) ? def : null;
        }
    }
}
