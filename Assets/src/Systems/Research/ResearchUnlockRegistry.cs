using System;
using System.Collections.Generic;
using CHAL.Core;   // DebugManager
using CHAL.Data;   // ResearchNodeDef, ResearchUnlock, ResearchUnlockTypes

namespace CHAL.Systems.Research
{

    public sealed class ResearchUnlockRegistry
    {
        // Interne Sets fÃ¼r O(1) Lookups
        private readonly HashSet<string> _worldTiers = new HashSet<string>(StringComparer.Ordinal);
        private readonly HashSet<string> _craftingFeatures = new HashSet<string>(StringComparer.Ordinal);
        private readonly HashSet<string> _recipes = new HashSet<string>(StringComparer.Ordinal);
        private readonly HashSet<string> _skillBranches = new HashSet<string>(StringComparer.Ordinal);
        private readonly HashSet<string> _heroes = new HashSet<string>(StringComparer.Ordinal);

        // VollstÃ¤ndiger Katalog aller bekannten targetIds mit Bool-Flag
        private readonly Dictionary<string, bool> _catalog = new Dictionary<string, bool>(StringComparer.Ordinal);


        // Public: Clear/Reset (z. B. beim Ladevorgang)
        public void Clear()
        {
            _worldTiers.Clear();
            _craftingFeatures.Clear();
            _recipes.Clear();
            _skillBranches.Clear();
            _heroes.Clear();
            _catalog.Clear();
        }

        public void RebuildFrom(IEnumerable<ResearchNodeDef> allNodes, IEnumerable<string> completedNodeIds)
        {
            Clear();

            if (allNodes == null || completedNodeIds == null)
            {
                DebugManager.Log("UnlockRegistry.RebuildFrom: null-Argumente.", DebugManager.EDebugLevel.Dev, "Research", UnityEngine.LogType.Warning);
                return;
            }

            InitializeCatalog(allNodes, resetExisting: false);

            // Lookup id â†’ def
            var byId = new Dictionary<string, ResearchNodeDef>(StringComparer.Ordinal);
            foreach (var n in allNodes)
            {
                if (n != null && !string.IsNullOrWhiteSpace(n.id))
                    byId[n.id] = n;
            }

            int applied = 0;
            foreach (var id in completedNodeIds)
            {
                if (string.IsNullOrWhiteSpace(id)) continue;
                if (!byId.TryGetValue(id, out var def) || def == null) continue;

                ApplyNodeUnlocks(id, def.unlocks, /*log=*/false);
                applied++;
            }

            DebugManager.Log($"UnlockRegistry.RebuildFrom: applied={applied}", DebugManager.EDebugLevel.Dev, "Research", UnityEngine.LogType.Log);
        }

        public void InitializeCatalog(IEnumerable<ResearchNodeDef> allNodes, bool resetExisting = false)
        {
            if (resetExisting) _catalog.Clear();
            if (allNodes == null) return;

            int added = 0;
            foreach (var def in allNodes)
            {
                if (def == null || def.unlocks == null) continue;
                foreach (var u in def.unlocks)
                {
                    if (string.IsNullOrWhiteSpace(u.targetId)) continue;
                    if (!_catalog.ContainsKey(u.targetId))
                    {
                        _catalog.Add(u.targetId, false); // zunÃ¤chst locked
                        added++;
                    }
                }
            }

            DebugManager.Log($"UnlockRegistry.InitializeCatalog: +{added} targetIds in catalog.",
                DebugManager.EDebugLevel.Dev, "Research", UnityEngine.LogType.Log);
        }

        public void ApplyNodeUnlocks(string nodeId, IReadOnlyList<ResearchUnlock> unlocks, bool log = true)
        {
            if (unlocks == null || unlocks.Count == 0) return;

            int catalogChanged = 0;

            for (int i = 0; i < unlocks.Count; i++)
            {
                var eff = unlocks[i];
                if (string.IsNullOrWhiteSpace(eff.targetId))
                {
                    if (log) DebugManager.Log($"UnlockRegistry: Node '{nodeId}' Effekt {i} hat leeres targetId.", DebugManager.EDebugLevel.Dev, "Research", UnityEngine.LogType.Warning);
                    continue;
                }

                switch (eff.unlockType)
                {
                    case ResearchUnlockTypes.WorldTier:
                        _worldTiers.Add(eff.targetId);
                        break;

                    case ResearchUnlockTypes.CraftingFeature:
                        _craftingFeatures.Add(eff.targetId);
                        break;

                    case ResearchUnlockTypes.Recipe:
                        _recipes.Add(eff.targetId);
                        break;

                    case ResearchUnlockTypes.SkillBranch:
                        _skillBranches.Add(eff.targetId);
                        break;

                    case ResearchUnlockTypes.Hero:
                        _heroes.Add(eff.targetId);
                        break;

                    default:
                        if (log) DebugManager.Log($"UnlockRegistry: Node '{nodeId}' Effekt {i} unbekannter Typ '{eff.unlockType}'.", DebugManager.EDebugLevel.Dev, "Research", UnityEngine.LogType.Warning);
                        break;
                }

                // Katalog-Flag setzen (neu)
                if (!_catalog.ContainsKey(eff.targetId))
                    _catalog.Add(eff.targetId, false);
                if (_catalog[eff.targetId] == false)
                {
                    _catalog[eff.targetId] = true;
                    catalogChanged++;
                }
            }

            if (log) DebugManager.Log($"UnlockRegistry: Node '{nodeId}' Effekte angewandt ({unlocks.Count}),  catalog +{catalogChanged} freigeschaltet.", DebugManager.EDebugLevel.Dev, "Research", UnityEngine.LogType.Log);
        }

        public void ApplyAlwaysUnlocked(IEnumerable<string> ids)
        {
            if (ids == null) return;

            int changed = 0;
            foreach (var raw in ids)
            {
                var id = string.IsNullOrWhiteSpace(raw) ? null : raw.Trim();
                if (string.IsNullOrEmpty(id)) continue;

                if (!_catalog.ContainsKey(id))
                    _catalog.Add(id, true);
                else if (_catalog[id] == false)
                {
                    _catalog[id] = true;
                    changed++;
                }

                // Wichtig: Gates laufen Ã¼ber _catalog â†’ typ-spezifische Sets mÃ¼ssen wir hier NICHT fÃ¼llen.
                // (IsUnlocked* liest bereits aus _catalog; GetUnlocked*()-AufzÃ¤hlungen bleiben optional separat.)
            }

            if (changed > 0)
                DebugManager.Log($"UnlockRegistry: AlwaysUnlocked applied, +{changed} IDs gesetzt.", DebugManager.EDebugLevel.Dev, "Research", UnityEngine.LogType.Log);
        }

        // -------------------- Query-API (andere Systeme lesen hier) --------------------

        public bool IsUnlockedWorldTier(string tierId)
        {
            if (string.IsNullOrWhiteSpace(tierId)) return false;
            return _catalog.TryGetValue(tierId, out var flag) && flag;
        }

        public bool IsUnlockedCraftingFeature(string feature)
        {
            if (string.IsNullOrWhiteSpace(feature)) return false;
            return _catalog.TryGetValue(feature, out var flag) && flag;
        }

        public bool IsUnlockedRecipe(string recipeId)
        {
            if (string.IsNullOrWhiteSpace(recipeId)) return false;
            return _catalog.TryGetValue(recipeId, out var flag) && flag;
        }

        public bool IsUnlockedSkillBranch(string branchId)
        {
            if (string.IsNullOrWhiteSpace(branchId)) return false;
            return _catalog.TryGetValue(branchId, out var flag) && flag;
        }

        public bool IsUnlockedHero(string heroId)
        {
            if (string.IsNullOrWhiteSpace(heroId)) return false;
            return _catalog.TryGetValue(heroId, out var flag) && flag;
        }

        // Generisch bleibt unverÃ¤ndert â€“ ist jetzt identisch zur obigen Logik
        public bool IsUnlocked(string targetId)
        {
            if (string.IsNullOrWhiteSpace(targetId)) return false;
            return _catalog.TryGetValue(targetId, out var flag) && flag;
        }


        public List<string> GetUnlockedIds()
        {
            var list = new List<string>(_catalog.Count);
            foreach (var kv in _catalog) if (kv.Value) list.Add(kv.Key);
            return list;
        }

        public List<string> GetLockedIds()
        {
            var list = new List<string>(_catalog.Count);
            foreach (var kv in _catalog) if (!kv.Value) list.Add(kv.Key);
            return list;
        }

        public (int unlocked, int total) GetProgressSummary()
        {
            int total = _catalog.Count;
            int unlocked = 0;
            foreach (var v in _catalog.Values) if (v) unlocked++;
            return (unlocked, total);
        }

        // Optionale Helfer fÃ¼r UI/Debug:
        public IReadOnlyCollection<string> GetUnlockedWorldTiers() => _worldTiers;
        public IReadOnlyCollection<string> GetUnlockedCraftingFeatures() => _craftingFeatures;
        public IReadOnlyCollection<string> GetUnlockedRecipes() => _recipes;
        public IReadOnlyCollection<string> GetUnlockedSkillBranches() => _skillBranches;
        public IReadOnlyCollection<string> GetUnlockedHeroes() => _heroes;
        public IReadOnlyDictionary<string, bool> GetAllFlags() => _catalog;
    }
}
