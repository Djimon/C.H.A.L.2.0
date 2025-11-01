# CHAL.Systems.Research.ResearchUnlockRegistry

_Automatically generated/updated from `Assets/src/Systems/Research/ResearchUnlockRegistry.cs`._

Purpose
- Defines a registry that tracks unlocked research targets across world tiers, crafting features, recipes, skill branches, and heroes.
- Maintains a catalog mapping targetId -> unlocked (bool) for quick lookups.
- Provides APIs to rebuild from node definitions, initialize the catalog, apply node unlocks, and query unlock state.

Public API
- Namespace/module
  - CHAL.Systems.Research

- Types
  - public sealed class ResearchUnlockRegistry
    - Public fields/properties: none
    - Public methods:
      - public void Clear()
      - public void RebuildFrom(IEnumerable<ResearchNodeDef> allNodes, IEnumerable<string> completedNodeIds)
      - public void InitializeCatalog(IEnumerable<ResearchNodeDef> allNodes, bool resetExisting = false)
      - public void ApplyNodeUnlocks(string nodeId, IReadOnlyList<ResearchUnlock> unlocks, bool log = true)
    - Query API:
      - public bool IsUnlockedWorldTier(string tierId)
      - public bool IsUnlockedCraftingFeature(string feature)
      - public bool IsUnlockedRecipe(string recipeId)
      - public bool IsUnlockedSkillBranch(string branchId)
      - public bool IsUnlockedHero(string heroId)
      - public bool IsUnlocked(string targetId)
      - public List<string> GetUnlockedIds()
      - public List<string> GetLockedIds()
      - public (int unlocked, int total) GetProgressSummary()
      - public IReadOnlyCollection<string> GetUnlockedWorldTiers()
      - public IReadOnlyCollection<string> GetUnlockedCraftingFeatures()
      - public IReadOnlyCollection<string> GetUnlockedRecipes()
      - public IReadOnlyCollection<string> GetUnlockedSkillBranches()
      - public IReadOnlyCollection<string> GetUnlockedHeroes()
      - public IReadOnlyDictionary<string, bool> GetAllFlags()

Key Behavior & Side Effects
- Clear
  - Empties internal sets (_worldTiers, _craftingFeatures, _recipes, _skillBranches, _heroes) and the _catalog dictionary.
- RebuildFrom(allNodes, completedNodeIds)
  - Clears internal state, validates arguments; logs a warning and returns if critical args are null.
  - Calls InitializeCatalog(allNodes, resetExisting: false).
  - Builds a map byId: id -> ResearchNodeDef for non-null, non-empty ids.
  - Iterates completedNodeIds; for each non-empty id found in byId, calls ApplyNodeUnlocks(id, def.unlocks, log: false).
  - Logs how many completed nodes were applied.
- InitializeCatalog(allNodes, resetExisting)
  - If resetExisting is true, clears _catalog.
  - If allNodes is null, returns.
  - Iterates all node definitions; for each with non-null unlocks, iterates unlocks and adds each non-empty targetId to _catalog with false (locked) if not already present.
  - Logs the number of targetIds added to the catalog.
- ApplyNodeUnlocks(nodeId, unlocks, log)
  - If unlocks is null or empty, returns.
  - For each unlock entry:
    - If targetId is null/whitespace, optionally logs a warning (depending on log) and continues.
    - Switch on unlockType to add targetId to the corresponding internal set (WorldTier, CraftingFeature, Recipe, SkillBranch, Hero).
    - Ensures a catalog entry exists for targetId; if currently false, sets to true and increments catalogChanged.
  - Logs a summary of applied effects and catalog changes when log is true.
- Query methods
  - Simple membership checks against the internal collections or catalog.
- GetUnlockedIds/GetLockedIds/GetProgressSummary
  - Build and return data from _catalog based on boolean flags.
- Helper accessors
  - GetUnlockedWorldTiers/GetUnlockedCraftingFeatures/GetUnlockedRecipes/GetUnlockedSkillBranches/GetUnlockedHeroes
    - Return the internal sets as IReadOnlyCollection.
  - GetAllFlags
    - Return the internal _catalog as IReadOnlyDictionary<string, bool>.

Constraints & Failure Modes
- Null/empty handling
  - RebuildFrom returns early with a warning if allNodes or completedNodeIds are null.
  - ApplyNodeUnlocks logs warnings if unlock entries have empty targetId.
  - IsUnlocked* helpers return false for empty/whitespace IDs.
- Threading
  - No explicit synchronization; not thread-safe.
- Logging
  - Uses DebugManager with Dev-level logs for internal state changes; can be disabled via log=false in internal calls.
- Data exposure
  - GetAllFlags exposes the catalog as a read-only dictionary; internal state still governs updates via public API methods.
- Performance
  - Catalog lookups are O(1) via HashSet/Dictionary; catalog size scales with number of distinct targetIds discovered in nodes.

Example
- Minimal usage (derivable from file)
```csharp
// Example usage (assumes you have allNodes and completedNodeIds available)
var registry = new CHAL.Systems.Research.ResearchUnlockRegistry();
registry.InitializeCatalog(allNodes, resetExisting: true);
registry.RebuildFrom(allNodes, completedNodeIds);
```

Unknowns
- Details of ResearchNodeDef, ResearchUnlock, and ResearchUnlockTypes beyond their usage here (structure, additional fields, and semantics).
- Exact behavior of DebugManager and UnityEngine.LogType integration beyond what is shown.
- Any external systems consuming these IsUnlocked/GetUnlocked* results beyond this file.
- Thread-safety guarantees or required synchronization for multi-threaded contexts.
