# Assets/src/Systems/Research/CodexUnlockRegistry.cs

_Automatically generated/updated from `Assets/src/Systems/Research/CodexUnlockRegistry.cs`._

# Purpose
- Defines the `CodexUnlockRegistry` class for managing unlockable items in a research system.

# Public API
- Namespace: `CHAL.Systems.Codex`
- Types
  - **public sealed class** `CodexUnlockRegistry`
    - **Public methods:**
      - `void Clear()`
      - `void RebuildFrom(IEnumerable<CodexDeedDef> allNodes, IEnumerable<string> completedNodeIds)`
      - `void InitializeCatalog(IEnumerable<CodexDeedDef> allNodes, bool resetExisting = false)`
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
- `RebuildFrom(...)`: Clears existing data and rebuilds the registry from provided nodes and completed IDs; logs warnings for null arguments.
- `InitializeCatalog(...)`: Initializes or resets the catalog with research nodes; logs the number of added target IDs.
- `ApplyNodeUnlocks(...)`: Applies unlocks to a node, updating the catalog and logging warnings for invalid unlocks.
- `ApplyAlwaysUnlocked(...)`: Marks specified IDs as always unlocked, updating the catalog.

# Constraints & Failure Modes
- Methods handle null or empty collections gracefully, returning early without changes.
- Logging occurs for invalid inputs or states, aiding in debugging.

# Example
```csharp
var registry = new CodexUnlockRegistry();
registry.RebuildFrom(allNodes, completedNodeIds);
bool isUnlocked = registry.IsUnlockedWorldTier("tier1");
```

# Unknowns
- The definitions and structure of `CodexDeedDef` and `ResearchUnlock` types are not provided in this file.

