using System;
using System.Collections.Generic;
using CHAL.Core;   // DebugManager
using CHAL.Data;   // ResearchNodeDef, ResearchUnlock, ResearchUnlockTypes

namespace CHAL.Systems.Research
{

    public sealed class UnlockRegistry
    {
        // Interne Sets für O(1) Lookups
        private readonly HashSet<string> _worldTiers = new HashSet<string>(StringComparer.Ordinal);
        private readonly HashSet<string> _craftingFeatures = new HashSet<string>(StringComparer.Ordinal);
        private readonly HashSet<string> _recipes = new HashSet<string>(StringComparer.Ordinal);
        private readonly HashSet<string> _skillBranches = new HashSet<string>(StringComparer.Ordinal);
        private readonly HashSet<string> _heroes = new HashSet<string>(StringComparer.Ordinal);

        // Public: Clear/Reset (z. B. beim Ladevorgang)
        public void Clear()
        {
            _worldTiers.Clear();
            _craftingFeatures.Clear();
            _recipes.Clear();
            _skillBranches.Clear();
            _heroes.Clear();
        }

        public void RebuildFrom(IEnumerable<ResearchNodeDef> allNodes, IEnumerable<string> completedNodeIds)
        {
            Clear();

            if (allNodes == null || completedNodeIds == null)
            {
                DebugManager.Log("UnlockRegistry.RebuildFrom: null-Argumente.", DebugManager.EDebugLevel.Dev, "Research", UnityEngine.LogType.Warning);
                return;
            }

            // Lookup id → def
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

        public void ApplyNodeUnlocks(string nodeId, IReadOnlyList<ResearchUnlock> unlocks, bool log = true)
        {
            if (unlocks == null || unlocks.Count == 0) return;

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
            }

            if (log) DebugManager.Log($"UnlockRegistry: Node '{nodeId}' Effekte angewandt ({unlocks.Count}).", DebugManager.EDebugLevel.Dev, "Research", UnityEngine.LogType.Log);
        }

        // -------------------- Query-API (andere Systeme lesen hier) --------------------

        public bool IsUnlockedWorldTier(string tierId) => !string.IsNullOrWhiteSpace(tierId) && _worldTiers.Contains(tierId);
        public bool IsUnlockedCraftingFeature(string feature) => !string.IsNullOrWhiteSpace(feature) && _craftingFeatures.Contains(feature);
        public bool IsUnlockedRecipe(string recipeId) => !string.IsNullOrWhiteSpace(recipeId) && _recipes.Contains(recipeId);
        public bool IsUnlockedSkillBranch(string branchId) => !string.IsNullOrWhiteSpace(branchId) && _skillBranches.Contains(branchId);
        public bool IsUnlockedHero(string heroId) => !string.IsNullOrWhiteSpace(heroId) && _heroes.Contains(heroId);

        // Optionale Helfer für UI/Debug:
        public IReadOnlyCollection<string> GetUnlockedWorldTiers() => _worldTiers;
        public IReadOnlyCollection<string> GetUnlockedCraftingFeatures() => _craftingFeatures;
        public IReadOnlyCollection<string> GetUnlockedRecipes() => _recipes;
        public IReadOnlyCollection<string> GetUnlockedSkillBranches() => _skillBranches;
        public IReadOnlyCollection<string> GetUnlockedHeroes() => _heroes;
    }
}
