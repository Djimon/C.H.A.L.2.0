# CHAL.Systems.Research.ResearchUnlockRegistry

_Automatically generated/updated from `Assets/src/Systems/Research/ResearchUnlockRegistry.cs`._

Purpose
- Defines a registry that tracks unlocked research targets across categories (world tiers, crafting features, recipes, skill branches, heroes) and a master catalog of all target IDs with an unlocked flag.
- Supports rebuilding state from a collection of research nodes and completed node IDs.
- Provides query surface for other systems to check unlocked status and obtain lists/progress.

Public API

Namespace/Module
- CHAL.Systems.Research

Types
- public sealed class ResearchUnlockRegistry
  - Public methods
    - public void Clear()
      - Resets all internal sets and the catalog.
    - public void RebuildFrom(IEnumerable<ResearchNodeDef> allNodes, IEnumerable<string> completedNodeIds)
      - Clears state, validates inputs, builds a by-id map from allNodes, applies unlocks for completedNodeIds, logs applied count.
    - public void InitializeCatalog(IEnumerable<ResearchNodeDef> allNodes, bool resetExisting = false)
      - Optionally clears catalog; iterates nodes to collect all unlock targetIds; adds each targetId to catalog as false (locked).
    - public void ApplyNodeUnlocks(string nodeId, IReadOnlyList<ResearchUnlock> unlocks, bool log = true)
      - Applies a list of unlocks to per-type sets and the catalog; validates targetIds; logs per-item issues and a final summary.
    - public void ApplyAlwaysUnlocked(IEnumerable<string> ids)
      - Forces given IDs to unlocked in catalog; does not alter per-type sets; logs changes when any ID transitions to unlocked.
    - public bool IsUnlockedWorldTier(string tierId)
      - Returns true if the world tier is unlocked (via catalog).
    - public bool IsUnlockedCraftingFeature(string feature)
      - Returns true if the crafting feature is unlocked (via catalog).
    - public bool IsUnlockedRecipe(string recipeId)
      - Returns true if the recipe is unlocked (via catalog).
    - public bool IsUnlockedSkillBranch(string branchId)
      - Returns true if the skill branch is unlocked (via catalog).
    - public bool IsUnlockedHero(string heroId)
      - Returns true if the hero is unlocked (via catalog).
    - public bool IsUnlocked(string targetId)
      - Generic check: true if the targetId is unlocked (via catalog).
    - public List<string> GetUnlockedIds()
      - Returns IDs currently marked unlocked in catalog.
    - public List<string> GetLockedIds()
      - Returns IDs currently marked locked in catalog.
    - public (int unlocked, int total) GetProgressSummary()
      - Returns counts of unlocked vs total catalog entries.
    - public IReadOnlyCollection<string> GetUnlockedWorldTiers()
      - Exposes currently unlocked world tiers.
    - public IReadOnlyCollection<string> GetUnlockedCraftingFeatures()
      - Exposes currently unlocked crafting features.
    - public IReadOnlyCollection<string> GetUnlockedRecipes()
      - Exposes currently unlocked recipes.
    - public IReadOnlyCollection<string> GetUnlockedSkillBranches()
      - Exposes currently unlocked skill branches.
    - public IReadOnlyCollection<string> GetUnlockedHeroes()
      - Exposes currently unlocked heroes.
    - public IReadOnlyDictionary<string, bool> GetAllFlags()
      - Exposes the full catalog of targetId -> unlocked flag.

Key Behavior & Side Effects

- Data structures
  - Internal per-category hash sets: _worldTiers, _craftingFeatures, _recipes, _skillBranches, _heroes
  - Master catalog: _catalog maps targetId to a bool (unlocked)
- RebuildFrom(allNodes, completedNodeIds)
  - Clears state; validates inputs; builds byId map from allNodes; for each non-empty completedId found in byId, applies unlocks with log disabled; logs number of applied nodes.
- InitializeCatalog(allNodes, resetExisting)
  - Optionally clears catalog if resetExisting is true; iterates allNodes and their unlocks; collects each non-empty targetId into catalog as false (locked); logs total added targetIds.
- ApplyNodeUnlocks(nodeId, unlocks, log)
  - For each unlock:
    - Validates targetId; logs a warning if empty (when log is true).
    - Updates per-type set corresponding to unlock.unlockType (WorldTier, CraftingFeature, Recipe, SkillBranch, Hero).
    - Ensures catalog entry exists for targetId; if currently false, sets to true and increments catalogChanged.
  - Logs a summary line with how many unlock effects were applied and how many catalog entries unlocked (if log).
- ApplyAlwaysUnlocked(ids)
  - For each id: trim, skip empty; add to catalog as true if missing; otherwise set true if previously false and note changes.
  - Logs number of IDs set to unlocked if any changes occurred.
- Query surface
  - IsUnlockedX methods read from catalog; early-exit on null/empty input.
  - IsUnlocked provides generic check against catalog.
  - GetUnlockedIds/GetLockedIds enumerate catalog by boolean flag.
  - GetProgressSummary computes unlocked/total from catalog.
  - GetAllFlags returns raw catalog; per-type sets are exposed separately via their GetUnlockedX methods.
- AlwaysUnlocked behavior note
  - When using ApplyAlwaysUnlocked, target IDs are marked unlocked in catalog without populating per-type sets (as noted in code comments).

Constraints & Failure Modes

- Null/invalid inputs
  - RebuildFrom: logs a warning and returns if allNodes or completedNodeIds are null.
  - InitializeCatalog: returns early if allNodes is null.
  - ApplyNodeUnlocks: if unlocks is null or empty, returns without changes; logs for empty targetIds if log is true.
  - IsUnlockedX: returns false for null/whitespace IDs.
  - GetProgressSummary: relies on _catalog count and values; no exceptions thrown.
- Threading/async
  - No explicit synchronization; not thread-safe by default.
- Performance
  - Uses HashSet and Dictionary with StringComparer.Ordinal for fast lookups; typical usage is O(n) for initial catalog build, O(1) per lookup thereafter.
- Side effects
  - DebugManager and UnityEngine.LogType are used for logs; side effects are IO/logging only.

Example

```csharp
// Example usage (pseudo-usage - depends on surrounding data types)
var registry = new CHAL.Systems.Research.ResearchUnlockRegistry();

// Assume allNodes: IEnumerable<ResearchNodeDef>, completedNodeIds: IEnumerable<string> are defined
registry.InitializeCatalog(allNodes);
registry.RebuildFrom(allNodes, completedNodeIds);

bool worldUnlocked = registry.IsUnlockedWorldTier("tier_01");
List<string> unlocked = registry.GetUnlockedIds();
```

Unknowns

- Definitions and structure of ResearchNodeDef, ResearchUnlock, ResearchUnlockTypes (external to this file).
- Exact behavior of DebugManager, CHAL.Core.DebugManager, and UnityEngine logging side effects.
- How this registry integrates with the broader game systems (e.g., when Unlocks are consumed by UI, save/load, or gameplay pacing).
- Any additional invariants or lifecycle guarantees beyond those explicit in this file.

Code example source (unchanged)
- See provided ResearchUnlockRegistry.cs for exact implementation details.
