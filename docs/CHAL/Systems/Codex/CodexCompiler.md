# Assets/src/Systems/Research/CodexCompiler.cs

_Automatically generated/updated from `Assets/src/Systems/Research/CodexCompiler.cs`._

# Purpose
- Defines the `CompiledCodex` class and the `CodexCompiler` static class for compiling a research tree definition into a compiled representation.

# Public API
- Namespace: `CHAL.Systems.Codex`
- Types
  - `sealed class CompiledCodex`
    - Public fields:
      - `Dictionary<string, CodexDeedDef> nodesById`: Maps node IDs to their definitions.
      - `Dictionary<string, (int lane, int stage)> posById`: Maps node IDs to their position in the tree.
      - `Dictionary<string, List<string>> parentsById`: Maps node IDs to their parent IDs.
    - Public methods:
      - `CompiledCodex(Dictionary<string, CodexDeedDef> nodesById, Dictionary<string, (int lane, int stage)> posById, Dictionary<string, List<string>> parentsById)`: Constructor to initialize the compiled codex.
  - `static class CodexCompiler`
    - Public methods:
      - `static CompiledCodex Compile(CodexDef tree)`: Compiles a research tree definition into a `CompiledCodex`. Returns a compiled representation of the research tree.

# Key Behavior & Side Effects
- Logs errors and warnings if the input tree is null or empty.
- Validates the stage order of nodes and logs errors for violations.
- Checks for cyclic dependencies in the tree and logs an error if found.

# Constraints & Failure Modes
- If `tree` is null, an empty `CompiledCodex` is returned.
- If `codexChapters` is null or empty, an empty `CompiledCodex` is returned.
- Duplicate node IDs are logged as errors but do not prevent compilation.
- The method `HasCycle` uses depth-first search (DFS) to detect cycles.

# Example
```csharp
CodexDef tree = new CodexDef(); // Assume this is properly initialized
CompiledCodex compiled = CodexCompiler.Compile(tree);
```

# Unknowns
- The structure and properties of `CodexDef` and `CodexDeedDef` are not defined in this file.

