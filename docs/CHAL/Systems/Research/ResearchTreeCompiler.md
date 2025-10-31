# CHAL.Systems.Research.ResearchTreeCompiler

_Automatically generated/updated from `Assets/src/Systems/Research/ResearchTreeCompiler.cs`._

# Purpose
- Defines the `ResearchTreeCompiled` class and the `ResearchTreeCompiler` static class for compiling research trees.

# Public API
- Namespace: `CHAL.Systems.Research`
- Types
  - **sealed class** `ResearchTreeCompiled`
    - Public fields/properties:
      - `readonly Dictionary<string, ResearchNodeDef> nodesById` - Maps node IDs to their definitions.
      - `readonly Dictionary<string, (int lane, int stage)> posById` - Maps node IDs to their position in the tree.
      - `readonly Dictionary<string, List<string>> parentsById` - Maps node IDs to their parent node IDs.
    - Public methods:
      - **constructor** `ResearchTreeCompiled(Dictionary<string, ResearchNodeDef> nodesById, Dictionary<string, (int lane, int stage)> posById, Dictionary<string, List<string>> parentsById)`
  - **static class** `ResearchTreeCompiler`
    - Public methods:
      - `static ResearchTreeCompiled Compile(ResearchTreeDef tree)` - Compiles a research tree definition into a `ResearchTreeCompiled` object.

# Key Behavior & Side Effects
- Handles null checks for the input `ResearchTreeDef` and its lanes.
- Logs errors for duplicate node IDs, missing parent nodes, and stage order violations.
- Validates acyclic dependencies in the research tree using depth-first search.

# Constraints & Failure Modes
- Returns an empty `ResearchTreeCompiled` if the input tree is null or has no lanes.
- Ignores nodes with null IDs or empty IDs.
- Logs errors for invalid parent references and cyclic dependencies.

# Example
```csharp
var compiledTree = ResearchTreeCompiler.Compile(researchTreeDef);
```

# Unknowns
- The structure and properties of `ResearchNodeDef` and `ResearchTreeDef` are not defined in this file.

