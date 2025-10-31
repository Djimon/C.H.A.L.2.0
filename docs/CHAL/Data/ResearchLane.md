# CHAL.Data.ResearchLane

_Automatically generated/updated from `Assets/src/Data/Defs/ResearchTreeDef.cs`._

# Purpose
- Defines a `ResearchTreeDef` ScriptableObject for managing research tree configurations in a game.

# Public API
- Namespace: `CHAL.Data`
- Types
  - **public sealed class ResearchTreeDef : ScriptableObject**
    - Public fields/properties:
      - `List<ResearchLane> researchLanes`: List of research lanes.
      - `int nodeWidth`: Width of nodes in the UI.
      - `int nodeHeight`: Height of nodes in the UI.
      - `int stageStepY`: Vertical step between stages in the UI.
      - `List<int> laneBaseX`: Base X positions for lanes.
      - `int topMarginY`: Top margin for the UI.
      - `Sprite defaultGateGlyph`: Default glyph for gates.
      - `List<ResearchTreeLane> researchTreeLanes`: List of research tree lanes.
    - Public methods:
      - `string GetLaneName(int lane)`: Returns the name of the lane at the specified index or "unknown lane" if out of bounds.
      - `Color GetLaneColor(int lane)`: Returns the color of the lane at the specified index or black if out of bounds.

  - **[Serializable] public struct ResearchLane**
    - Public fields/properties:
      - `string laneName`: Name of the research lane.
      - `Color laneColor`: Color of the research lane.

  - **[Serializable] public sealed class ResearchTreeLane**
    - Public fields/properties:
      - `string laneName`: Name of the research tree lane.
      - `Color laneColor`: Color of the research tree lane.
      - `List<ResearchTreeStage> stages`: List of stages in the research tree lane.

  - **[Serializable] public sealed class ResearchTreeStage**
    - Public fields/properties:
      - `List<ResearchTreeNodeRef> nodes`: List of nodes in this stage.

  - **[Serializable] public sealed class ResearchTreeNodeRef**
    - Public fields/properties:
      - `ResearchNodeDef node`: Reference to the research node.
      - `List<ResearchNodeDef> parentRefs`: List of parent node references.

# Key Behavior & Side Effects
- `GetLaneName` and `GetLaneColor` methods handle out-of-bounds access by returning default values.

# Constraints & Failure Modes
- `nodeWidth`, `nodeHeight`, and `stageStepY` must be greater than or equal to 1 due to the `[Min(1)]` attribute.
- Accessing lanes in `GetLaneName` and `GetLaneColor` methods requires valid indices; otherwise, default values are returned.

# Example
```csharp
ResearchTreeDef researchTree = ScriptableObject.CreateInstance<ResearchTreeDef>();
string laneName = researchTree.GetLaneName(0);
Color laneColor = researchTree.GetLaneColor(0);
```

# Unknowns
- No information on the `ResearchNodeDef` type or its properties.

