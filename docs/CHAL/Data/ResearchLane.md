# CHAL.Data.ResearchLane

_Automatically generated/updated from `Assets/src/Data/Defs/ResearchTreeDef.cs`._

Purpose
- Defines a Unity ScriptableObject (ResearchTreeDef) that configures a research tree UI, lanes, and structure.
- Encapsulates UI layout constants, lane definitions, initial unlocks, and the hierarchical research tree (lanes, stages, node refs).
- Provides helper accessors (GetLaneName, GetLaneColor) to look up lane metadata by index.

Public API
- Namespace/Module
  - CHAL.Data

- Types

  - public sealed class ResearchTreeDef : ScriptableObject
    - Public fields
      - List<ResearchLane> researchLanes
      - int nodeWidth
      - int nodeHeight
      - int stageStepY
      - List<int> laneBaseX
      - int topMarginY
      - Sprite defaultGateGlyph
      - List<string> alwaysUnlockedIds
      - List<ResearchTreeLane> researchTreeLanes
    - Public methods
      - string GetLaneName(int lane)
      - Color GetLaneColor(int lane)

  - public struct ResearchLane
    - Public fields
      - string laneName
      - Color laneColor

  - public sealed class ResearchTreeLane
    - Public fields
      - string laneName
      - Color laneColor
      - List<ResearchTreeStage> stages

  - public sealed class ResearchTreeStage
    - Public fields
      - List<ResearchTreeNodeRef> nodes

  - public sealed class ResearchTreeNodeRef
    - Public fields
      - ResearchNodeDef node
      - List<ResearchNodeDef> parentRefs

Key Behavior & Side Effects
- ResearchTreeDef.GetLaneName(int lane)
  - Returns researchLanes[lane].laneName when 0 <= lane < researchLanes.Count
  - Otherwise returns "unknown lane"
- ResearchTreeDef.GetLaneColor(int lane)
  - Returns researchLanes[lane].laneColor when 0 <= lane < researchLanes.Count
  - Otherwise returns Color.black
- Both helpers are read-only accessors; no state mutations occur.

Constraints & Failure Modes
- UI layout constants
  - nodeWidth, nodeHeight, stageStepY have [Min(1)] constraints (must be >= 1) in the editor.
- Serialization
  - All public fields are Unity-serialized; nested types are marked Serializable.
- Nullability
  - Lists are initialized by default in the field initializers; runtime null checks are not present beyond the index guards in GetLaneName/GetLaneColor.
- Cross-references
  - ResearchTreeNodeRef references ResearchNodeDef, which is defined elsewhere (not in this file).

Example
- Not derivable from this file alone; no runnable example provided.

Unknowns
- The definition and usage of ResearchNodeDef (external to this file).
- How alwaysUnlockedIds are interpreted at runtime.
- How researchTreeLanes is consumed by the UI or game logic beyond this file.
- Any runtime validation for consistency between lanes, stages, and node refs.
