#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using CHAL.Data;   // ResearchNodeDef

public sealed class ResearchNodeEditorWindow : EditorWindow
{
    private ResearchNodeDef _node;
    private Editor _cachedInspector;
    private Vector2 _scroll;

    public static void ShowFor(ResearchNodeDef node)
    {
        if (node == null) return;
        var win = CreateWindow<ResearchNodeEditorWindow>("Research Node");
        win._node = node;
        win.titleContent = new GUIContent($"Node: {node.title ?? node.name}");
        win.minSize = new Vector2(380, 300);
        win.Focus();
        win.Repaint();
        DebugManager.Log($"ResearchNodeEditorWindow opened for '{node.name}'",
            DebugManager.EDebugLevel.Dev, "Research", LogType.Log);
    }

    private void OnEnable()
    {
        if (_node != null)
            _cachedInspector = Editor.CreateEditor(_node);
    }

    private void OnDisable()
    {
        if (_cachedInspector != null)
        {
            DestroyImmediate(_cachedInspector);
            _cachedInspector = null;
        }
    }

    private void OnGUI()
    {
        if (_node == null)
        {
            EditorGUILayout.HelpBox("No ResearchNode selected.", MessageType.Info);
            if (GUILayout.Button("Close")) Close();
            return;
        }

        // Header
        using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
        {
            GUILayout.Label(_node.name, EditorStyles.miniLabel);
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Ping", EditorStyles.toolbarButton, GUILayout.Width(60)))
                EditorGUIUtility.PingObject(_node);
            if (GUILayout.Button("Select", EditorStyles.toolbarButton, GUILayout.Width(60)))
                Selection.activeObject = _node;
        }

        // Body (Default-Inspector des ScriptableObjects)
        using (var scroll = new EditorGUILayout.ScrollViewScope(_scroll))
        {
            _scroll = scroll.scrollPosition;

            if (_cachedInspector == null || _cachedInspector.target != _node)
                _cachedInspector = Editor.CreateEditor(_node);

            if (_cachedInspector != null)
            {
                EditorGUI.BeginChangeCheck();
                _cachedInspector.OnInspectorGUI();
                if (EditorGUI.EndChangeCheck())
                {
                    EditorUtility.SetDirty(_node);
                }
            }
            else
            {
                // Fallback: minimal
                var so = new SerializedObject(_node);
                so.Update();
                EditorGUILayout.PropertyField(so.FindProperty("id"));
                EditorGUILayout.PropertyField(so.FindProperty("title"));
                EditorGUILayout.PropertyField(so.FindProperty("unlocks"), true);
                EditorGUILayout.PropertyField(so.FindProperty("requirements"), true);
                so.ApplyModifiedProperties();
            }
        }
    }
}
#endif
