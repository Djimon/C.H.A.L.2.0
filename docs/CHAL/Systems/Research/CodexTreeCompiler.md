# Assets/src/Systems/Research/CodexTreeCompiler.cs

_Automatically generated/updated from `Assets/src/Systems/Research/CodexTreeCompiler.cs`._

# Purpose
- Defines the `CodexTreeCompiler` class for compiling research tree definitions into a structured format.
- Provides the `ResearchTreeCompiled` class to hold the compiled research tree data.

# Public API
- Namespace: `CHAL.Systems.Research`
- Types
  - `sealed class ResearchTreeCompiled`
    - `public readonly Dictionary<string, CodexNodeDef> nodesById`: Maps node IDs to their definitions.
    - `public readonly Dictionary<string, (int lane, int stage)> posById`: Maps node IDs to their position in the tree.
    - `public readonly Dictionary<string, List<string>> parentsById`: Maps node IDs to their parent node IDs.
    - `public ResearchTreeCompiled(Dictionary<string, CodexNodeDef> nodesById, Dictionary<string, (int lane, int stage)> posById, Dictionary<string, List<string>> parentsById)`: Constructor to initialize the compiled tree.
  - `static class CodexTreeCompiler`
    - `public static ResearchTreeCompiled Compile(CodexTreeDef tree)`: Compiles a research tree definition into a `ResearchTreeCompiled` object.

# Key Behavior & Side Effects
- Logs errors and warnings if the input tree is null or empty.
- Validates the stage order of nodes and logs errors for violations.
- Checks for cyclic dependencies in the tree structure and logs if found.

# Constraints & Failure Modes
- If `tree` is null, returns an empty `ResearchTreeCompiled` object.
- If `researchTreeLanes` is null or empty, returns an empty `ResearchTreeCompiled` object.
- Duplicate node IDs are logged as errors but do not stop compilation.
- Parent nodes must exist and must precede child nodes in stage order; violations are logged.
- Cyclic dependencies are detected and logged.

# Example
```csharp
var compiledTree = CodexTreeCompiler.Compile(researchTreeDef);
```

# Unknowns
- The structure and properties of `CodexTreeDef` and `CodexNodeDef` are not defined in this file.

