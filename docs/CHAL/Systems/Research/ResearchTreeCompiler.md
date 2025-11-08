# CHAL.Systems.Research.ResearchTreeCompiler

_Automatically generated/updated from `Assets/src/Systems/Research/ResearchTreeCompiler.cs`._

# Purpose
- Defines the `ResearchTreeCompiled` class and the `ResearchTreeCompiler` static class for compiling research tree definitions.

# Public API
- Namespace: `CHAL.Systems.Research`
- Types
  - `sealed class ResearchTreeCompiled`
    - `public readonly Dictionary<string, ResearchNodeDef> nodesById` - Maps node IDs to their definitions.
    - `public readonly Dictionary<string, (int lane, int stage)> posById` - Maps node IDs to their position in the tree (lane and stage).
    - `public readonly Dictionary<string, List<string>> parentsById` - Maps node IDs to their parent node IDs.
    - `ResearchTreeCompiled(Dictionary<string, ResearchNodeDef> nodesById, Dictionary<string, (int lane, int stage)> posById, Dictionary<string, List<string>> parentsById)` - Constructor to initialize the compiled research tree.
  - `static class ResearchTreeCompiler`
    - `public static ResearchTreeCompiled Compile(ResearchTreeDef tree)` - Compiles a research tree definition into a `ResearchTreeCompiled` object.

# Key Behavior & Side Effects
- Logs errors and warnings if the input tree is null or empty.
- Validates the stage order of nodes and logs errors for violations.
- Checks for cyclic dependencies in the tree and logs an error if found.

# Constraints & Failure Modes
- If the input `tree` is null, an empty `ResearchTreeCompiled` is returned.
- If `researchTreeLanes` is null or empty, an empty `ResearchTreeCompiled` is returned.
- Duplicate node IDs are logged as errors but do not prevent compilation.
- Parent nodes must exist and be in a valid stage order; violations are logged.

# Example
```csharp
var compiledTree = ResearchTreeCompiler.Compile(researchTreeDef);
```

# Unknowns
- The structure and properties of `ResearchTreeDef` and `ResearchNodeDef` are not defined in this file.

