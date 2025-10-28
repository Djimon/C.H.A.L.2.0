# Assets/src/Data/Defs/ResearchTreeDef.cs

_Automatic generated/updated._

```markdown
## Purpose
- Defines a `ResearchTreeDef` ScriptableObject for managing research tree configurations in a game.

## Public API
- Namespace: `CHAL.Data`
- Types
  - **public sealed class ResearchTreeDef** [extends ScriptableObject]
    - Public fields/properties:
      - `List<ResearchLane> researchLanes`: List of research lanes.
      - `int nodeWidth`: Width of nodes in the UI.
      - `int nodeHeight`: Height of nodes in the UI.
      - `int stageStepY`: Vertical step between stages in the UI.
      - `List<int> laneBaseX`: Base X positions for lanes.
      - `int topMarginY`: Top margin for the UI layout.
      - `Sprite defaultGateGlyph`: Default glyph for gates in the UI.
      - `List<ResearchTreeLane> researchTreeLanes`: List of research tree lanes.
    - Public methods:
      - `string GetLaneName(int lane)`: Returns the name of the lane at the specified index or "unknown lane" if out of bounds.
      - `Color GetLaneColor(int lane)`: Returns the color of the lane at the specified index or black if out of bounds.

  - **[Serializable] public struct ResearchLane**
    - Public fields/properties:
      - `string laneName`: Name of the research lane.
      - `Color laneColor`: Color associated with the research lane.

  - **[Serializable] public sealed class ResearchTreeLane**
    - Public fields/properties:
      - `string laneName`: Name of the research tree lane.
      - `Color laneColor`: Color associated with the research tree lane.
      - `List<ResearchTreeStage> stages`: List of stages in the research tree lane.

  - **[Serializable] public sealed class ResearchTreeStage**
    - Public fields/properties:
      - `List<ResearchTreeNodeRef> nodes`: List of node references in this stage.

  - **[Serializable] public sealed class ResearchTreeNodeRef**
    - Public fields/properties:
      - `ResearchNodeDef node`: Reference to a research node.
      - `List<ResearchNodeDef> parentRefs`: List of parent node references.

## Key Behavior & Side Effects
- `GetLaneName` and `GetLaneColor` methods handle out-of-bounds access by returning default values.

## Constraints & Failure Modes
- `GetLaneName` and `GetLaneColor` methods check for valid lane indices (0 to count-1) to prevent exceptions.

## Example
```csharp
ResearchTreeDef researchTree = ScriptableObject.CreateInstance<ResearchTreeDef>();
string laneName = researchTree.GetLaneName(0);
Color laneColor = researchTree.GetLaneColor(0);
```

## Unknowns
- No information on the usage or integration of `ResearchNodeDef` and its properties.
```
