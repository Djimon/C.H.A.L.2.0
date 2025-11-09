# Assets/src/Systems/Research/ResearchTreeCompiler.cs

_Automatically generated/updated from `Assets/src/Systems/Research/ResearchTreeCompiler.cs`._

# Purpose
- Defines the `ResearchTreeCompiled` class and the `ResearchTreeCompiler` static class for compiling research tree definitions.

# Public API
- Namespace: `CHAL.Systems.Research`
- Types
  - `sealed class ResearchTreeCompiled`
    - `public readonly Dictionary<string, ResearchNodeDef> nodesById` - Maps node IDs to their definitions.
    - `public readonly Dictionary<string, (int lane, int stage)> posById` - Maps node IDs to their positions in the tree.
    - `public readonly Dictionary<string, List<string>> parentsById` - Maps node IDs to their parent node IDs.
    - `ResearchTreeCompiled(Dictionary<string, ResearchNodeDef> nodesById, Dictionary<string, (int lane, int stage)> posById, Dictionary<string, List<string>> parentsById)` - Constructor to initialize the compiled research tree.
  - `static class ResearchTreeCompiler`
    - `public static ResearchTreeCompiled Compile(ResearchTreeDef tree)` - Compiles a research tree definition into a `ResearchTreeCompiled` object.

# Key Behavior & Side Effects
- Logs errors if the input `tree` is null or if `researchTreeLanes` is empty.
- Logs errors for duplicate node IDs and missing parent nodes during compilation.
- Validates the stage order of nodes and logs errors if violated.
- Checks for cycles in the tree structure and logs if found.

# Constraints & Failure Modes
- Handles null or empty inputs for `tree` and `researchTreeLanes`.
- Uses `StringComparer.Ordinal` for dictionary key comparisons.
- The cycle detection uses a depth-first search (DFS) approach.

# Example
```csharp
var compiledTree = ResearchTreeCompiler.Compile(researchTreeDef);
```

# Unknowns
- The structure and properties of `ResearchNodeDef` and `ResearchTreeDef` are not defined in this file.

