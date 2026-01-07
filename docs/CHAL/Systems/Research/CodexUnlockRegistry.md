# Assets/src/Systems/Research/CodexUnlockRegistry.cs

_Automatically generated/updated from `Assets/src/Systems/Research/CodexUnlockRegistry.cs`._

# Purpose
- Defines the `CodexUnlockRegistry` class for managing unlockable research elements in the game.

# Public API
- Namespace: `CHAL.Systems.Research`
- Types
  - **public sealed class** `CodexUnlockRegistry`
    - **public void** `Clear()`
      - Clears all data from the internal collections.
    - **public void** `RebuildFrom(IEnumerable<CodexNodeDef> allNodes, IEnumerable<string> completedNodeIds)`
      - Rebuilds the research data from the provided nodes and completed node IDs.
    - **public void** `InitializeCatalog(IEnumerable<CodexNodeDef> allNodes, bool resetExisting = false)`
      - Initializes the research catalog with the provided nodes.
    - **public void** `ApplyNodeUnlocks(string nodeId, IReadOnlyList<ResearchUnlock> unlocks, bool log = true)`
      - Applies the specified unlocks to a node identified by its ID.
    - **public void** `ApplyAlwaysUnlocked(IEnumerable<string> ids)`
      - Applies the "always unlocked" status to a collection of IDs.
    - **public bool** `IsUnlockedWorldTier(string tierId)`
      - Checks if the specified world tier is unlocked.
    - **public bool** `IsUnlockedCraftingFeature(string feature)`
      - Checks if the specified crafting feature is unlocked.
    - **public bool** `IsUnlockedRecipe(string recipeId)`
      - Checks if the specified recipe is unlocked.
    - **public bool** `IsUnlockedSkillBranch(string branchId)`
      - Checks if the specified skill branch is unlocked.
    - **public bool** `IsUnlockedHero(string heroId)`
      - Determines if the specified hero is unlocked.
    - **public bool** `IsUnlocked(string targetId)`
      - Checks if the specified target ID is unlocked.
    - **public List<string>** `GetUnlockedIds()`
      - Retrieves a list of IDs that are currently unlocked.
    - **public List<string>** `GetLockedIds()`
      - Retrieves a list of IDs that are currently locked.
    - **public (int unlocked, int total)** `GetProgressSummary()`
      - Provides a summary of unlocked and total items.
    - **public IReadOnlyCollection<string>** `GetUnlockedWorldTiers()`
      - Gets a collection of unlocked world tiers.
    - **public IReadOnlyCollection<string>** `GetUnlockedCraftingFeatures()`
      - Gets a collection of unlocked crafting features.
    - **public IReadOnlyCollection<string>** `GetUnlockedRecipes()`
      - Gets a collection of unlocked recipes.
    - **public IReadOnlyCollection<string>** `GetUnlockedSkillBranches()`
      - Gets a collection of unlocked skill branches.
    - **public IReadOnlyCollection<string>** `GetUnlockedHeroes()`
      - Gets a collection of unlocked heroes.
    - **public IReadOnlyDictionary<string, bool>** `GetAllFlags()`
      - Retrieves all flags from the catalog.

# Key Behavior & Side Effects
- `Clear()` resets all internal collections.
- `RebuildFrom()` initializes the registry and logs warnings for null arguments.
- `ApplyNodeUnlocks()` modifies internal collections based on provided unlocks and logs warnings for invalid entries.
- `ApplyAlwaysUnlocked()` updates the catalog to mark specified IDs as always unlocked.

# Constraints & Failure Modes
- Methods handle null or empty collections gracefully.
- Logging occurs for invalid inputs in `RebuildFrom()` and `ApplyNodeUnlocks()`.
- No threading or async behavior is evident in the code.

# Example
```csharp
var registry = new CodexUnlockRegistry();
registry.RebuildFrom(allNodes, completedNodeIds);
bool isUnlocked = registry.IsUnlockedWorldTier("tier1");
```

# Unknowns
- The definitions and structures of `CodexNodeDef` and `ResearchUnlock` are not provided in this file.

