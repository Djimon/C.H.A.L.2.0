# CHAL.Data.ResearchLane

_Automatically generated/updated from `Assets/src/Data/Defs/ResearchTreeDef.cs`._

# Purpose
- Defines the `ResearchTreeDef` ScriptableObject for managing research trees in the game.

# Public API
- Namespace: `CHAL.Data`
- Types
  - public sealed class `ResearchTreeDef` : ScriptableObject
    - Public fields/properties:
      - `List<ResearchLane> researchLanes`: List of research lanes.
      - `int nodeWidth`: Width of the nodes in the UI.
      - `int nodeHeight`: Height of the nodes in the UI.
      - `int stageStepY`: Vertical step between stages in the UI.
      - `List<int> laneBaseX`: Base X positions for lanes.
      - `int topMarginY`: Top margin for the UI.
      - `Sprite defaultGateGlyph`: Default glyph for gates in the UI.
      - `List<string> alwaysUnlockedIds`: List of IDs that are always unlocked.
      - `List<ResearchTreeLane> researchTreeLanes`: List of research tree lanes.
    - Public methods:
      - `string GetLaneName(int lane)`: Returns the name of the specified lane or "unknown lane" if out of range.
      - `Color GetLaneColor(int lane)`: Returns the color of the specified lane or black if out of range.
  - [Serializable] public struct `ResearchLane`
    - Public fields/properties:
      - `string laneName`: Name of the lane.
      - `Color laneColor`: Color of the lane.
  - [Serializable] public sealed class `ResearchTreeLane`
    - Public fields/properties:
      - `string laneName`: Name of the lane.
      - `Color laneColor`: Color of the lane.
      - `List<ResearchTreeStage> stages`: List of stages in the lane.
  - [Serializable] public sealed class `ResearchTreeStage`
    - Public fields/properties:
      - `List<ResearchTreeNodeRef> nodes`: List of nodes in this stage.
  - [Serializable] public sealed class `ResearchTreeNodeRef`
    - Public fields/properties:
      - `ResearchNodeDef node`: Reference to the research node.
      - `List<ResearchNodeDef> parentRefs`: List of parent node references.

# Key Behavior & Side Effects
- `GetLaneName(int lane)`: Validates the lane index and returns the corresponding lane name or a default message.
- `GetLaneColor(int lane)`: Validates the lane index and returns the corresponding lane color or a default color.

# Constraints & Failure Modes
- Methods `GetLaneName` and `GetLaneColor` handle out-of-range indices by returning default values.
- No threading or async behavior is evident in this file.

# Example
```csharp
ResearchTreeDef researchTree = ScriptableObject.CreateInstance<ResearchTreeDef>();
string laneName = researchTree.GetLaneName(0);
Color laneColor = researchTree.GetLaneColor(1);
```

# Unknowns
- No information on how `ResearchNodeDef` is defined or used.

