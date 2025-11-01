# global.ResearchNodeEditorWindow

_Automatically generated/updated from `Assets/src/Editor/ResearchNodeEditorWindow.cs`._

1) Purpose
- Defines a Unity EditorWindow (ResearchNodeEditorWindow) to inspect and edit a ResearchNodeDef.
- Provides a header with the node name and actions (Ping, Select) and a body that renders the node with a cached inspector or a minimal fallback.
- Fully editor-only (wrapped in UNITY_EDITOR) to offer in-Editor tooling for ResearchNodeDef assets.

2) Public API
- public sealed class ResearchNodeEditorWindow : EditorWindow
  - Public methods
    - public static void ShowFor(ResearchNodeDef node)
      - Opens the window for the given node; returns early if node is null.
      - Sets window title to the node's name/title, initializes size, focuses, repaints, and logs the open action.

Note: No public fields or properties are exposed. Lifecycle methods OnEnable, OnDisable, and OnGUI are private.

3) Key Behavior & Side Effects
- ShowFor(ResearchNodeDef node)
  - If node is null: returns without action.
  - Creates window instance via CreateWindow<ResearchNodeEditorWindow>("Research Node").
  - Assigns win._node = node.
  - Sets win.titleContent to include node.title or node.name.
  - Sets minSize to (380, 300).
  - Focuses and repaints the window.
  - Logs the opening via DebugManager.Log with category "Research" at Dev level.
- OnEnable()
  - If _node != null, creates a cached Editor for _node via Editor.CreateEditor(_node).
- OnDisable()
  - If _cachedInspector exists, destroys it with DestroyImmediate and clears the reference.
- OnGUI()
  - If _node is null:
    - Displays info help box: "No ResearchNode selected."
    - Presents a Close button; if pressed, closes the window.
  - Else:
    - Header: horizontal toolbar with node.name, a Ping button (EditorGUIUtility.PingObject(_node)), and a Select button (Selection.activeObject = _node).
    - Body: scrollable area displaying either the cached inspector or a minimal fallback:
      - If _cachedInspector is null or its target != _node, recreate via Editor.CreateEditor(_node).
      - If _cachedInspector exists:
        - BeginChangeCheck, render _cachedInspector.OnInspectorGUI(), and if changed, mark _node dirty via EditorUtility.SetDirty(_node).
      - Else (fallback):
        - Create a SerializedObject(_node) and render properties: "id", "title", "unlocks", "requirements" (the latter two with showChildren enabled), then ApplyModifiedProperties().

4) Constraints & Failure Modes
- UNITY_EDITOR only: Code compiled and available only in the Unity Editor.
- ShowFor with null node: no window opened.
- _node and _cachedInspector synchronization: if _node changes externally, the code rebuilds the cached inspector in OnGUI when needed.
- Fallback path assumes fields "id", "title", "unlocks", "requirements" exist on ResearchNodeDef; if absent, property lookups may fail in that path.
- On GUI changes, the node is marked dirty to prompt saving in the editor.

5) Example
- Open the editor for a ResearchNodeDef instance:
```csharp
// Assuming 'myNode' is a ResearchNodeDef instance
ResearchNodeEditorWindow.ShowFor(myNode);
```

6) Unknowns
- Location and structure of CHAL.Data.ResearchNodeDef beyond the using; no other API surface shown here.
- Details of DebugManager.Log implementation and log routing.
- Behavior when the ResearchNodeDef asset is modified outside of this window (beyond the explicit SetDirty call).
