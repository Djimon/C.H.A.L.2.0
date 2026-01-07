using System;
using System.Collections.Generic;
using CHAL.Core;   // DebugManager
using CHAL.Data;   // ResearchNodeDef, ResearchUnlock, ResearchUnlockTypes

namespace CHAL.Systems.Research
{

    public sealed class CodexUnlockRegistry
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
/// <summary>
/// Clears all data from the internal collections.
/// </summary>
        public void Clear()
        {
            _worldTiers.Clear();
            _craftingFeatures.Clear();
            _recipes.Clear();
            _skillBranches.Clear();
            _heroes.Clear();
            _catalog.Clear();
        }

/// <summary>
/// Rebuilds the research data from the provided nodes and completed node IDs.
/// </summary>
/// <param name="allNodes">The collection of all research nodes.</param>
/// <param name="completedNodeIds">The collection of completed node IDs.</param>
        public void RebuildFrom(IEnumerable<CodexNodeDef> allNodes, IEnumerable<string> completedNodeIds)
        {
            Clear();

            if (allNodes == null || completedNodeIds == null)
            {
                DebugManager.Log("UnlockRegistry.RebuildFrom: null-Argumente.", DebugManager.EDebugLevel.Dev, "Research", UnityEngine.LogType.Warning);
                return;
            }

            InitializeCatalog(allNodes, resetExisting: false);

            // Lookup id â†’ def
            var byId = new Dictionary<string, CodexNodeDef>(StringComparer.Ordinal);
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

/// <summary>
/// Initializes the research catalog with the provided nodes.
/// Optionally resets existing entries if specified.
/// </summary>
/// <param name="allNodes">The collection of research nodes to add.</param>
/// <param name="resetExisting">Indicates whether to clear existing entries.</param>
        public void InitializeCatalog(IEnumerable<CodexNodeDef> allNodes, bool resetExisting = false)
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

/// <summary>
/// Applies the specified unlocks to a node identified by its ID.
/// Optionally logs warnings for any invalid unlocks.
/// </summary>
/// <param name="nodeId">The ID of the node to apply unlocks to.</param>
/// <param name="unlocks">A list of research unlocks to apply.</param>
/// <param name="log">Indicates whether to log warnings (default is true).</param>
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

/// <summary>
/// Applies the "always unlocked" status to a collection of IDs.
/// </summary>
/// <param name="ids">The collection of IDs to apply the status to.</param>
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

/// <summary>
/// Checks if the specified world tier is unlocked.
/// </summary>
/// <param name="tierId">The ID of the world tier to check.</param>
/// <returns>True if the world tier is unlocked; otherwise, false.</returns>
        public bool IsUnlockedWorldTier(string tierId)
        {
            if (string.IsNullOrWhiteSpace(tierId)) return false;
            return _catalog.TryGetValue(tierId, out var flag) && flag;
        }

/// <summary>
/// Checks if the specified crafting feature is unlocked.
/// </summary>
/// <param name="feature">The name of the crafting feature to check.</param>
/// <returns>True if the crafting feature is unlocked; otherwise, false.</returns>
        public bool IsUnlockedCraftingFeature(string feature)
        {
            if (string.IsNullOrWhiteSpace(feature)) return false;
            return _catalog.TryGetValue(feature, out var flag) && flag;
        }

/// <summary>
/// Checks if the specified recipe is unlocked.
/// </summary>
/// <param name="recipeId">The ID of the recipe to check.</param>
/// <returns>True if the recipe is unlocked; otherwise, false.</returns>
        public bool IsUnlockedRecipe(string recipeId)
        {
            if (string.IsNullOrWhiteSpace(recipeId)) return false;
            return _catalog.TryGetValue(recipeId, out var flag) && flag;
        }

/// <summary>
/// Checks if the specified skill branch is unlocked.
/// </summary>
/// <param name="branchId">The ID of the skill branch to check.</param>
/// <returns>True if the skill branch is unlocked; otherwise, false.</returns>
        public bool IsUnlockedSkillBranch(string branchId)
        {
            if (string.IsNullOrWhiteSpace(branchId)) return false;
            return _catalog.TryGetValue(branchId, out var flag) && flag;
        }

/// <summary>
/// Determines if the specified hero is unlocked.
/// </summary>
/// <param name="heroId">The ID of the hero to check.</param>
/// <returns>True if the hero is unlocked; otherwise, false.</returns>
        public bool IsUnlockedHero(string heroId)
        {
            if (string.IsNullOrWhiteSpace(heroId)) return false;
            return _catalog.TryGetValue(heroId, out var flag) && flag;
        }

        // Generisch bleibt unverÃ¤ndert â€“ ist jetzt identisch zur obigen Logik
/// <summary>
/// Checks if the specified target ID is unlocked.
/// </summary>
/// <param name="targetId">The ID to check for unlock status.</param>
/// <returns>True if the target ID is unlocked; otherwise, false.</returns>
        public bool IsUnlocked(string targetId)
        {
            if (string.IsNullOrWhiteSpace(targetId)) return false;
            return _catalog.TryGetValue(targetId, out var flag) && flag;
        }


/// <summary>
/// Retrieves a list of IDs that are currently unlocked.
/// </summary>
/// <returns>A list of unlocked IDs as strings.</returns>
        public List<string> GetUnlockedIds()
        {
            var list = new List<string>(_catalog.Count);
            foreach (var kv in _catalog) if (kv.Value) list.Add(kv.Key);
            return list;
        }

/// <summary>
/// Retrieves a list of IDs that are currently locked.
/// </summary>
/// <returns>A list of locked IDs as strings.</returns>
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
/// <summary>
/// Gets a collection of world tiers that are unlocked.
/// </summary>
/// <returns>A read-only collection of unlocked world tier names.</returns>
        public IReadOnlyCollection<string> GetUnlockedWorldTiers() => _worldTiers;
/// <summary>
/// Gets a collection of crafting features that are unlocked.
/// </summary>
/// <returns>A read-only collection of unlocked crafting feature names.</returns>
        public IReadOnlyCollection<string> GetUnlockedCraftingFeatures() => _craftingFeatures;
/// <summary>
/// Gets a collection of unlocked recipes.
/// </summary>
/// <returns>A read-only collection of unlocked recipe names.</returns>
        public IReadOnlyCollection<string> GetUnlockedRecipes() => _recipes;
/// <summary>
/// Gets a collection of skill branches that are unlocked.
/// </summary>
/// <returns>A read-only collection of unlocked skill branch names.</returns>
        public IReadOnlyCollection<string> GetUnlockedSkillBranches() => _skillBranches;
/// <summary>
/// Gets a collection of heroes that are unlocked.
/// </summary>
/// <returns>A read-only collection of unlocked hero names.</returns>
        public IReadOnlyCollection<string> GetUnlockedHeroes() => _heroes;
/// <summary>
/// Retrieves all flags from the catalog.
/// </summary>
/// <returns>A read-only dictionary of flag names and their values.</returns>
        public IReadOnlyDictionary<string, bool> GetAllFlags() => _catalog;
    }
}
