# global.ResearchTreeeEditor

_Automatically generated/updated from `Assets/src/Editor/ResearchTreeeEditor.cs`._

```csharp
- Purpose
  - Unity Editor extension for CHAL.Data.ResearchTreeDef (CustomEditor).
  - Render and manage visual tree structure: lanes, stages, and nodes using ReorderableLists.
  - Support syncing Tree Lanes from Visual Lanes, creating new node assets, validating IDs, and compiling the tree.

- Public API
  - Namespace/module
    - Unity Editor extension (uses UnityEditor, UnityEditorInternal)
  - Types
    - public sealed class ResearchTreeDefEditor : Editor
      - Public methods
        - public override void OnInspectorGUI()
          - Main inspector UI entry point (renders and handles interactions)
      - Inherited/implicit surface (not redefined here)
        - Unity's Editor lifecycle methods (OnEnable, etc. are private in this file)
      - Notes
        - This class is bound to CHAL.Data.ResearchTreeDef via [CustomEditor(typeof(CHAL.Data.ResearchTreeDef))]

- Key Behavior & Side Effects
  - Editor initialization
    - OnEnable: casts target to ResearchTreeDef and builds all stage/node lists.
  - UI & layout
    - Renders and manages:
      - Visual lane configuration (researchLanes)
      - Layout constants (nodeWidth, nodeHeight, stageStepY, topMarginY, laneBaseX, defaultGateGlyph)
      - AlwaysUnlocked IDs (with a Validate button)
      - Real-time "Actual Research Tree" tabs for tree lanes
      - Active lane editor: shows stages and per-stage node sublists
  - ReorderableLists
    - Stages per lane: one ReorderableList per lane (stagesProp)
      - Header: "Stages in Tree Lane {laneIdx}"
      - Height driven by CalcStageHeight
      - Elements render stage header and a nested Nodes sublist
      - Add: creates a new stage with 0 nodes
      - Change: applies and rebuilds all lists
    - Nodes per stage: per-stage ReorderableList (nodesProp)
      - Header: "Nodes" with a Create Node button
      - Elements render:
        - Node reference (node field)
        - Parents management UI (Add/Remove/Inline list)
      - Add: creates a new Node element; initializes node+parents
      - Change: applies and updates serialized state
  - Node creation
    - Create Node: opens a Save File panel, creates a ResearchNodeDef asset with a heuristic ID (laneName + node file name, sanitized), pings and opens ResearchNodeEditorWindow
    - If user cancels, logs a debug message and aborts
  - Parents management
    - ShowParentPickerMenu: builds a list of candidate parent nodes from earlier stages in the same lane
    - Adds selected parent with Undo support; applies/updates serialized state
    - Clear removes all parents in that node entry
  - Syncing lanes
    - SyncTreeLanesFromVisual: mirrors visual lanes into tree lanes
      - Shortens tree lanes if visual has fewer lanes
      - Extends tree lanes with empty stages if visual has more lanes
      - Copies laneName and laneColor from visual to tree lanes (stages left intact)
      - Rebuilds internal lists after syncing
  - Validation & compile
    - ValidateAlwaysUnlockedIds: collects non-empty IDs, deduplicates, checks for overlaps with node unlock targets, shows dialog, logs warnings if overlaps exist
    - CollectNodeTargetIds: utility to gather all node unlock target IDs from the tree
    - RunCompile: calls ResearchTreeCompiler.Compile(_tree), logs counts (lanes, stages, nodes, parent links), shows a diagnostic dialog, and warns if no nodes found
  - Node/ID utilities
    - CreateNewNodeAsset uses SanitizeIdPart to produce stable IDs
    - CollectExistingNodeIds prevents ID collisions when creating new nodes
  - Editor state & refresh
    - After Sync or structural changes, rebuilds lists and clamps _activeLane
    - Uses serializedObject.ApplyModifiedProperties/Update to synchronize Unity's serialized state
  - Debug/logging
    - Uses CHAL.Core.DebugManager for internal logs
    - Logs include creation, syncing, compilation results, and errors

- Constraints & Failure Modes
  - Guarding and null checks
    - Many early returns when tree or lanes are null or indices are out of range
    - Uses Mathf.Clamp to guard lane/stage indices
  - Editor-only behavior
    - Entire file is wrapped in UNITY_EDITOR; editor tooling may not run in builds
  - Asset creation flow
    - CreateNewNodeAsset relies on a valid baseDir and a user-specified path; cancellation aborts asset creation
  - Synchronization caveats
    - Sync does not rewrite stage content; only lane-level metadata (name/color) and lane list size
    - After syncing, internal lists are rebuilt; active tab is re-clamped
  - External dependencies
    - ResearchTreeDef, ResearchLane, ResearchTreeLane, ResearchNodeDef, ResearchTreeCompiler, DebugManager, and ResearchNodeEditorWindow are external types not defined in this file
  - Performance considerations
    - Dynamic rebuild of nested ReorderableLists and height calculations per frame while UI is open

- Example
  - Not applicable / not clearly derivable from this file

- Unknowns
  - Exact behavior and structure of:
    - CHAL.Data.ResearchTreeDef, ResearchLane, ResearchTreeLane
    - ResearchNodeDef, ResearchTreeCompiler
    - ResearchNodeEditorWindow
    - CHAL.Core.DebugManager
  - Details of ResearchTreeDef’s data contracts beyond what this editor relies on
  - Runtime impact of syncing vs. manual edits on validation/compile outcomes
```
