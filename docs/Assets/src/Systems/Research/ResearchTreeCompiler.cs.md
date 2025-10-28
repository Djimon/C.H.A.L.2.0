# Assets/src/Systems/Research/ResearchTreeCompiler.cs

_Automatic generated/updated._

```markdown
## Purpose
- Defines the `ResearchTreeCompiled` class for holding compiled research tree data.
- Provides the `ResearchTreeCompiler` static class for compiling a `ResearchTreeDef` into a `ResearchTreeCompiled`.

## Public API
- Namespace: `CHAL.Systems.Research`
- Types
  - `sealed class ResearchTreeCompiled`
    - Public fields/properties:
      - `readonly Dictionary<string, ResearchNodeDef> nodesById` - Maps node IDs to their definitions.
      - `readonly Dictionary<string, (int lane, int stage)> posById` - Maps node IDs to their positions in the tree.
      - `readonly Dictionary<string, List<string>> parentsById` - Maps node IDs to their parent node IDs.
  - `static class ResearchTreeCompiler`
    - Public methods:
      - `static ResearchTreeCompiled Compile(ResearchTreeDef tree)` - Compiles a `ResearchTreeDef` into a `ResearchTreeCompiled`. Returns a compiled tree or an empty structure if the input is invalid.

## Key Behavior & Side Effects
- Logs errors if the input `tree` is null or if `researchTreeLanes` is empty.
- Validates the order of stages and checks for cycles in the tree structure.
- Logs errors for duplicate node IDs and invalid parent references.

## Constraints & Failure Modes
- Handles null and empty collections gracefully, returning empty dictionaries.
- Uses `StringComparer.Ordinal` for case-sensitive key comparisons in dictionaries.
- Cycle detection is performed using a depth-first search (DFS) approach.

## Example
```csharp
var compiledTree = ResearchTreeCompiler.Compile(researchTreeDef);
```

## Unknowns
- The structure and properties of `ResearchTreeDef` and `ResearchNodeDef` are not defined in this file.
```
