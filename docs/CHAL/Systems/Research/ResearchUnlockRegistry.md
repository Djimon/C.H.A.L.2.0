# CHAL.Systems.Research.ResearchUnlockRegistry

_Automatically generated/updated from `Assets/src/Systems/Research/ResearchUnlockRegistry.cs`._

# Purpose
- Defines the `ResearchUnlockRegistry` class for managing research unlocks in a game.

# Public API
- Namespace: `CHAL.Systems.Research`
- Types
  - `public sealed class ResearchUnlockRegistry`
    - Public fields/properties: None
    - Public methods:
      - `public void Clear()`
      - `public void RebuildFrom(IEnumerable<ResearchNodeDef> allNodes, IEnumerable<string> completedNodeIds)`
      - `public void InitializeCatalog(IEnumerable<ResearchNodeDef> allNodes, bool resetExisting = false)`
      - `public void ApplyNodeUnlocks(string nodeId, IReadOnlyList<ResearchUnlock> unlocks, bool log = true)`
      - `public void ApplyAlwaysUnlocked(IEnumerable<string> ids)`
      - `public bool IsUnlockedWorldTier(string tierId)`
      - `public bool IsUnlockedCraftingFeature(string feature)`
      - `public bool IsUnlockedRecipe(string recipeId)`
      - `public bool IsUnlockedSkillBranch(string branchId)`
      - `public bool IsUnlockedHero(string heroId)`
      - `public bool IsUnlocked(string targetId)`
      - `public List<string> GetUnlockedIds()`
      - `public List<string> GetLockedIds()`
      - `public (int unlocked, int total) GetProgressSummary()`
      - `public IReadOnlyCollection<string> GetUnlockedWorldTiers()`
      - `public IReadOnlyCollection<string> GetUnlockedCraftingFeatures()`
      - `public IReadOnlyCollection<string> GetUnlockedRecipes()`
      - `public IReadOnlyCollection<string> GetUnlockedSkillBranches()`
      - `public IReadOnlyCollection<string> GetUnlockedHeroes()`
      - `public IReadOnlyDictionary<string, bool> GetAllFlags()`

# Key Behavior & Side Effects
- `Clear()`: Resets all internal collections.
- `RebuildFrom()`: Rebuilds the registry from provided nodes and completed IDs; logs warnings for null arguments.
- `InitializeCatalog()`: Initializes or resets the catalog with research nodes; logs the number of added target IDs.
- `ApplyNodeUnlocks()`: Applies unlocks to a node; logs warnings for invalid unlocks and updates the catalog.
- `ApplyAlwaysUnlocked()`: Marks specified IDs as always unlocked; logs the number of changes.

# Constraints & Failure Modes
- Methods handle null or empty collections gracefully.
- Logging occurs for invalid inputs or states when specified.
- Uses `StringComparer.Ordinal` for case-sensitive string comparisons in collections.

# Example
```csharp
var registry = new ResearchUnlockRegistry();
registry.RebuildFrom(allNodes, completedNodeIds);
bool isUnlocked = registry.IsUnlockedWorldTier("tier1");
```

# Unknowns
- None.

