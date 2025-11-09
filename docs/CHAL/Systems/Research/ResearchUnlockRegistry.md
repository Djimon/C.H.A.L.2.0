# Assets/src/Systems/Research/ResearchUnlockRegistry.cs

_Automatically generated/updated from `Assets/src/Systems/Research/ResearchUnlockRegistry.cs`._

# Purpose
- Defines the `ResearchUnlockRegistry` class for managing research unlocks in the system.

# Public API
- Namespace: `CHAL.Systems.Research`
- Types
  - `public sealed class ResearchUnlockRegistry`
    - Public methods:
      - `void Clear()`
      - `void RebuildFrom(IEnumerable<ResearchNodeDef> allNodes, IEnumerable<string> completedNodeIds)`
      - `void InitializeCatalog(IEnumerable<ResearchNodeDef> allNodes, bool resetExisting = false)`
      - `void ApplyNodeUnlocks(string nodeId, IReadOnlyList<ResearchUnlock> unlocks, bool log = true)`
      - `void ApplyAlwaysUnlocked(IEnumerable<string> ids)`
      - `bool IsUnlockedWorldTier(string tierId)`
      - `bool IsUnlockedCraftingFeature(string feature)`
      - `bool IsUnlockedRecipe(string recipeId)`
      - `bool IsUnlockedSkillBranch(string branchId)`
      - `bool IsUnlockedHero(string heroId)`
      - `bool IsUnlocked(string targetId)`
      - `List<string> GetUnlockedIds()`
      - `List<string> GetLockedIds()`
      - `(int unlocked, int total) GetProgressSummary()`
      - `IReadOnlyCollection<string> GetUnlockedWorldTiers()`
      - `IReadOnlyCollection<string> GetUnlockedCraftingFeatures()`
      - `IReadOnlyCollection<string> GetUnlockedRecipes()`
      - `IReadOnlyCollection<string> GetUnlockedSkillBranches()`
      - `IReadOnlyCollection<string> GetUnlockedHeroes()`
      - `IReadOnlyDictionary<string, bool> GetAllFlags()`

# Key Behavior & Side Effects
- `Clear()`: Resets all internal collections.
- `RebuildFrom()`: Rebuilds the registry from provided nodes and completed node IDs; logs warnings for null arguments.
- `InitializeCatalog()`: Initializes or resets the catalog with research nodes.
- `ApplyNodeUnlocks()`: Applies unlocks to a node and updates the catalog; logs warnings for invalid unlocks.
- `ApplyAlwaysUnlocked()`: Marks specified IDs as always unlocked; does not modify specific sets.

# Constraints & Failure Modes
- Methods handle null or empty collections gracefully.
- Uses `StringComparer.Ordinal` for case-sensitive string comparisons.
- Logging occurs for invalid inputs or operations when specified.

# Example
```csharp
var registry = new ResearchUnlockRegistry();
registry.RebuildFrom(allNodes, completedNodeIds);
bool isUnlocked = registry.IsUnlockedWorldTier("tier1");
```

# Unknowns
- None.

