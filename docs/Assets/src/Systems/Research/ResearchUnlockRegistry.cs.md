# Assets/src/Systems/Research/ResearchUnlockRegistry.cs

_Automatic generated/updated._

```markdown
## Purpose
- Defines the `ResearchUnlockRegistry` class for managing research unlocks in a game system.
- Provides methods to clear, rebuild, and apply unlocks based on research nodes.

## Public API
- Namespace: `CHAL.Systems.Research`
- Types:
  - `public sealed class ResearchUnlockRegistry`
    - Public methods:
      - `void Clear()`
      - `void RebuildFrom(IEnumerable<ResearchNodeDef> allNodes, IEnumerable<string> completedNodeIds)`
      - `void InitializeCatalog(IEnumerable<ResearchNodeDef> allNodes, bool resetExisting = false)`
      - `void ApplyNodeUnlocks(string nodeId, IReadOnlyList<ResearchUnlock> unlocks, bool log = true)`
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

## Key Behavior & Side Effects
- `Clear()`: Resets all internal collections.
- `RebuildFrom()`: Clears existing data, initializes catalog, and applies unlocks based on completed nodes. Logs warnings for null arguments and issues during processing.
- `InitializeCatalog()`: Populates the catalog with target IDs from provided nodes, logging the number of added IDs.
- `ApplyNodeUnlocks()`: Applies unlocks from a node, updating internal collections and the catalog, with logging for issues and changes.

## Constraints & Failure Modes
- Methods handle null or empty inputs gracefully, logging warnings as necessary.
- Uses `StringComparer.Ordinal` for case-sensitive string comparisons in collections.
- Performance considerations are not explicitly mentioned.

## Example
```csharp
var registry = new ResearchUnlockRegistry();
registry.RebuildFrom(allNodes, completedNodeIds);
var unlockedRecipes = registry.GetUnlockedRecipes();
```

## Unknowns
- The definitions and structures of `ResearchNodeDef` and `ResearchUnlock` are not provided in this file.
- The behavior of `DebugManager` is not detailed in this file.
```
