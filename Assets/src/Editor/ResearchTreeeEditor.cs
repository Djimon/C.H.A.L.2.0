#if UNITY_EDITOR
using CHAL.Core;                    // DebugManager
using CHAL.Data;
using CHAL.Systems.Research;        // ResearchTreeCompiler
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

// Erwartete Struktur in ResearchTreeDef:
// - Visual: public List<ResearchLane> researchLanes
// - Tree:   public List<ResearchTreeLane> researchTreeLanes

[CustomEditor(typeof(CHAL.Data.ResearchTreeDef))]
public sealed class ResearchTreeDefEditor : Editor
{
    private ResearchTreeDef _tree;
    private int _activeLane;
    private Vector2 _scroll;

    // ReorderableLists
    private readonly Dictionary<int, ReorderableList> _stagesLists = new();
    private readonly Dictionary<(int lane, int stage), ReorderableList> _nodesLists = new();

    // Styles
    private GUIStyle _tabStyleOn;
    private GUIStyle _tabStyleOff;
    private GUIStyle _headerStyle;

    private void OnEnable()
    {
        _tree = (CHAL.Data.ResearchTreeDef)target;
        BuildAllStageAndNodeLists();
        //BuildStyles();
    }

    private void EnsureStyles()
    {
        // Fallback auf Built-in Skin, falls EditorStyles (früh) null liefert
        var toolbar = EditorStyles.toolbarButton ?? EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector).button;
        if (_tabStyleOn == null) _tabStyleOn = new GUIStyle(toolbar) { fontStyle = FontStyle.Bold };
        if (_tabStyleOff == null) _tabStyleOff = new GUIStyle(toolbar);
        if (_headerStyle == null) _headerStyle = new GUIStyle(EditorStyles.boldLabel ?? GUI.skin.label) { fontSize = 12 };
    }

    private void BuildAllStageAndNodeLists()
    {
        _stagesLists.Clear();
        _nodesLists.Clear();

        if (_tree.researchTreeLanes == null) return;

        for (int li = 0; li < _tree.researchTreeLanes.Count; li++)
        {
            int laneIdx = li;
            var lanesProp = serializedObject.FindProperty("researchTreeLanes");
            var laneProp = lanesProp.GetArrayElementAtIndex(li);
            var stagesProp = laneProp.FindPropertyRelative("stages");

            // Stages pro Lane
            var stagesList = new ReorderableList(serializedObject, stagesProp, true, true, true, true);
            stagesList.drawHeaderCallback = r => EditorGUI.LabelField(r, $"Stages in Tree Lane {laneIdx}");

            stagesList.elementHeightCallback = idx =>
            {
                if (idx < 0 || idx >= stagesProp.arraySize) return EditorGUIUtility.singleLineHeight + 20f;
                // Sicherstellen, dass es eine Nodes-ReorderableList für diese Stage gibt
                var key = (lane: laneIdx, stage: idx);
                if (!_nodesLists.TryGetValue(key, out var nlist))
                {
                    var nodesPropX = stagesProp.GetArrayElementAtIndex(idx).FindPropertyRelative("nodes");
                    nlist = BuildNodesList(nodesPropX, laneIdx, idx);
                    _nodesLists[key] = nlist;
                }
                return CalcStageHeight(laneIdx, idx, nlist, stagesProp);
            };

            stagesList.drawElementCallback = (rect, si, active, focused) =>
            {
                if (si < 0 || si >= stagesProp.arraySize) return;

                var stageProp = stagesProp.GetArrayElementAtIndex(si);
                var nodesProp = stageProp.FindPropertyRelative("nodes");

                // Stage-Header
                var rHeader = new Rect(rect.x, rect.y + 2, rect.width, EditorGUIUtility.singleLineHeight);
                EditorGUI.LabelField(rHeader, $"Stage {si}", _headerStyle);

                // Nodes-Sublist vorbereiten
                var key = (lane: laneIdx, stage: si);
                if (!_nodesLists.TryGetValue(key, out var nodeList))
                {
                    nodeList = BuildNodesList(nodesProp, laneIdx, si);
                    _nodesLists[key] = nodeList;
                }

                // Höhe der Nodes-Liste sauber berechnen, damit Buttons sicher innerhalb des Elements liegen
                float nodesH = CalcNodesListHeight(nodeList, nodesProp);

                var rNodes = new Rect(rect.x, rHeader.yMax + 4f, rect.width, nodesH);
                nodeList.DoList(rNodes); 
            };

            stagesList.onAddCallback = list =>
            {
                list.serializedProperty.arraySize++;
                var newStage = list.serializedProperty.GetArrayElementAtIndex(list.serializedProperty.arraySize - 1);
                newStage.FindPropertyRelative("nodes").arraySize = 0;
                serializedObject.ApplyModifiedProperties();
            };
            stagesList.onChangedCallback = _ =>
            {
                serializedObject.ApplyModifiedProperties();
                BuildAllStageAndNodeLists();
            };

            _stagesLists[li] = stagesList;

            // Nodes-Listen vorbereiten
            for (int si = 0; si < stagesProp.arraySize; si++)
            {
                var nodesProp = stagesProp.GetArrayElementAtIndex(si).FindPropertyRelative("nodes");
                var k = (lane: laneIdx, stage: si);
                _nodesLists[k] = BuildNodesList(nodesProp, laneIdx, si);
            }
        }
    }

    private ReorderableList BuildNodesList(SerializedProperty nodesProp, int laneIndex, int stageIndex)
    {
        var list = new ReorderableList(serializedObject, nodesProp, true, true, true, true);

        list.drawHeaderCallback = r =>
        {
            var labelRect = new Rect(r.x, r.y, r.width - 120, r.height);
            EditorGUI.LabelField(labelRect, "Nodes");

            var btnRect = new Rect(r.x + r.width - 110, r.y + 1, 105, r.height - 2);
            if (GUI.Button(btnRect, "Create Node"))
            {
                // Lane-Name sicher bestimmen (SerializedProperty + Clamp)
                string laneName = "none";
                var lanesProp = serializedObject.FindProperty("researchTreeLanes");
                if (lanesProp != null && lanesProp.arraySize > 0)
                {
                    int safeLane = Mathf.Clamp(laneIndex, 0, lanesProp.arraySize - 1);
                    var laneProp = lanesProp.GetArrayElementAtIndex(safeLane);
                    var nameProp = laneProp?.FindPropertyRelative("laneName");
                    laneName = nameProp != null ? (nameProp.stringValue ?? "") : "";
                }
                else
                {
                    // Fallback: _tree lesen (nur read, falls SerializedProperty fehlt)
                    if (_tree != null && _tree.researchTreeLanes != null && _tree.researchTreeLanes.Count > 0)
                    {
                        int safeLane = Mathf.Clamp(laneIndex, 0, _tree.researchTreeLanes.Count - 1);
                        laneName = _tree.researchTreeLanes[safeLane]?.laneName ?? "";
                    }
                }

                DebugManager.Log($"Create Node in laneIndex={laneIndex} (resolved laneName='{laneName}')",
                    DebugManager.EDebugLevel.Dev, "Research", LogType.Log);

                string treeDir = System.IO.Path.GetDirectoryName(AssetDatabase.GetAssetPath(target));
                CreateNewNodeAsset(treeDir, laneName);  // nur erstellen, kein Referenzieren
                GUIUtility.ExitGUI();
            }
        };

        list.elementHeight = EditorGUIUtility.singleLineHeight * 3 + 12;

        list.drawElementCallback = (rect, idx, active, focused) =>
        {
            var elem = nodesProp.GetArrayElementAtIndex(idx);
            var nodeProp = elem.FindPropertyRelative("node");
            var parentsProp = elem.FindPropertyRelative("parentRefs");

            float y = rect.y + 2;
            var rNode = new Rect(rect.x, y, rect.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.PropertyField(rNode, nodeProp, new GUIContent($"Node #{idx}"));

            y += EditorGUIUtility.singleLineHeight + 2;

            // Parents-Zeile
            var parentLabel = new Rect(rect.x, y, 70, EditorGUIUtility.singleLineHeight);
            EditorGUI.LabelField(parentLabel, "Parents:");

            var addBtn = new Rect(parentLabel.xMax + 4, y, 60, EditorGUIUtility.singleLineHeight);
            if (GUI.Button(addBtn, "+ Add"))
            {
                //ShowParentPickerMenu(parentsProp, laneIndex, stageIndex);
                Undo.RecordObject(target, "Add Parent");
                ShowParentPickerMenu(parentsProp, laneIndex, stageIndex);
                serializedObject.Update();
            }

            var clearBtn = new Rect(addBtn.xMax + 4, y, 70, EditorGUIUtility.singleLineHeight);
            if (GUI.Button(clearBtn, "Clear"))
            {
                Undo.RecordObject(target, "Clear Parents");
                parentsProp.arraySize = 0;
                serializedObject.ApplyModifiedProperties();
                serializedObject.Update();
            }

            y += EditorGUIUtility.singleLineHeight + 2;

            // Parents-Liste inline
            var box = new Rect(rect.x, y, rect.width, EditorGUIUtility.singleLineHeight * Mathf.Max(1, parentsProp.arraySize) + 8);
            GUI.Box(box, GUIContent.none);
            y += 4;

            for (int pi = 0; pi < parentsProp.arraySize; pi++)
            {
                var pProp = parentsProp.GetArrayElementAtIndex(pi);
                var r = new Rect(rect.x + 4, y, rect.width - 72, EditorGUIUtility.singleLineHeight);
                EditorGUI.ObjectField(r, pProp, GUIContent.none);
                var rem = new Rect(r.xMax + 4, y, 60, EditorGUIUtility.singleLineHeight);
                if (GUI.Button(rem, "Remove"))
                {
                    parentsProp.DeleteArrayElementAtIndex(pi);
                    serializedObject.ApplyModifiedProperties();
                    break;
                }
                y += EditorGUIUtility.singleLineHeight + 2;
            }
        };

        list.onAddCallback = l =>
        {
            l.serializedProperty.arraySize++;
            var e = l.serializedProperty.GetArrayElementAtIndex(l.serializedProperty.arraySize - 1);
            e.FindPropertyRelative("node").objectReferenceValue = null;
            e.FindPropertyRelative("parentRefs").arraySize = 0;
            serializedObject.ApplyModifiedProperties();
        };

        return list;
    }

    private void ShowParentPickerMenu(SerializedProperty parentsProp, int laneIndex, int stageIndex)
    {
        var menu = new GenericMenu();

        // Aktuellen Zustand holen & prüfen
        var lanesProp = serializedObject.FindProperty("researchTreeLanes");
        if (lanesProp == null || lanesProp.arraySize == 0)
        {
            menu.AddDisabledItem(new GUIContent("No lanes defined"));
            menu.ShowAsContext();
            return;
        }

        // laneIndex/ stageIndex gegen aktuellen Zustand clampen
        int safeLane = Mathf.Clamp(laneIndex, 0, lanesProp.arraySize - 1);
        var laneProp = lanesProp.GetArrayElementAtIndex(safeLane);
        if (laneProp == null)
        {
            menu.AddDisabledItem(new GUIContent("Invalid lane reference"));
            menu.ShowAsContext();
            return;
        }

        var stagesProp = laneProp.FindPropertyRelative("stages");
        int stageCount = stagesProp != null ? stagesProp.arraySize : 0;
        if (stageCount == 0)
        {
            menu.AddDisabledItem(new GUIContent("No stages in this lane"));
            menu.ShowAsContext();
            return;
        }

        int safeStage = Mathf.Clamp(stageIndex, 0, stageCount - 1);

        // Kandidaten aus früheren Stages derselben Lane einsammeln
        var candidates = new List<UnityEngine.Object>();
        for (int s = 0; s < safeStage; s++)
        {
            var nodesProp = stagesProp.GetArrayElementAtIndex(s).FindPropertyRelative("nodes");
            for (int i = 0; i < nodesProp.arraySize; i++)
            {
                var nProp = nodesProp.GetArrayElementAtIndex(i).FindPropertyRelative("node");
                var obj = nProp.objectReferenceValue;
                if (obj != null) candidates.Add(obj);
            }
        }

        if (candidates.Count == 0)
        {
            menu.AddDisabledItem(new GUIContent("No earlier nodes in this lane"));
        }
        else
        {
            foreach (var c in candidates.Distinct())
            {
                var label = c.name;
                menu.AddItem(new GUIContent(label), false, () =>
                {
                    Undo.RecordObject(target, "Add Parent");
                    int idx = parentsProp.arraySize;
                    parentsProp.InsertArrayElementAtIndex(idx);
                    parentsProp.GetArrayElementAtIndex(idx).objectReferenceValue = c;
                    serializedObject.ApplyModifiedProperties();
                    serializedObject.Update();
                });
            }
        }

        menu.ShowAsContext();
    }

    public override void OnInspectorGUI()
    {
        EnsureStyles();
        serializedObject.Update();

        // --- VISUELLE KONFIG (unverändert) ---
        EditorGUILayout.LabelField("Lane Labels & Colors (Visual)", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("researchLanes"), includeChildren: true);

        EditorGUILayout.Space(8);
        EditorGUILayout.LabelField("Layout-Constants (UI)", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("nodeWidth"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("nodeHeight"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("stageStepY"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("topMarginY"));
        var laneBaseXProp = serializedObject.FindProperty("laneBaseX");
        if (laneBaseXProp != null)
            EditorGUILayout.PropertyField(laneBaseXProp, new GUIContent("laneBaseX"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("defaultGateGlyph"));

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Actual Research Tree", EditorStyles.boldLabel);

        // Tabs basieren auf researchTreeLanes; wenn leer → Init aus Visual-Lanes anbieten
        DrawLaneTabs();

        EditorGUILayout.Space(6);

        // Aktive Tree-Lane bearbeiten
        if (_tree.researchTreeLanes != null && _tree.researchTreeLanes.Count > 0 &&
            _activeLane >= 0 && _activeLane < _tree.researchTreeLanes.Count)
        {
            using (var scroll = new EditorGUILayout.ScrollViewScope(_scroll))
            {
                _scroll = scroll.scrollPosition;
                using (new EditorGUILayout.VerticalScope("box"))
                {
                    EditorGUILayout.LabelField($"Tree Lane {_activeLane}: Stages & Nodes", EditorStyles.boldLabel);

                    if (_stagesLists.TryGetValue(_activeLane, out var stagesList))
                    {
                        stagesList.DoLayoutList();
                    }
                    else
                    {
                        EditorGUILayout.HelpBox("No stages list (internal). Reopen inspector.", MessageType.Info);
                    }
                }
            }
        }

        EditorGUILayout.Space(12);
        if (GUILayout.Button("Validate / Compile Tree", GUILayout.Height(28)))
        {
            RunCompile();
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawLaneTabs()
    {
        var treeLanesProp = serializedObject.FindProperty("researchTreeLanes");
        var visualLanesProp = serializedObject.FindProperty("researchLanes");

        using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
        {
            // Tabs (auch 0 Tree-Lanes möglich)
            if (treeLanesProp.arraySize > 0)
            {
                for (int i = 0; i < treeLanesProp.arraySize; i++)
                {
                    var laneProp = treeLanesProp.GetArrayElementAtIndex(i);
                    var nameProp = laneProp.FindPropertyRelative("laneName");
                    var label = string.IsNullOrWhiteSpace(nameProp.stringValue) ? $"Lane {i}" : nameProp.stringValue;

                    var style = (i == _activeLane) ? _tabStyleOn : _tabStyleOff;
                    if (GUILayout.Toggle(i == _activeLane, label, style, GUILayout.MinWidth(64)))
                        _activeLane = i;
                }
            }
            else
            {
                GUILayout.Label("No Tree Lanes defined.", EditorStyles.miniLabel);
            }

            GUILayout.FlexibleSpace();

            // Sync immer verfügbar
            using (new EditorGUI.DisabledScope(visualLanesProp.arraySize == 0))
            {
                if (GUILayout.Button(new GUIContent("Sync Tree Lanes from Visual Lanes"), EditorStyles.toolbarButton))
                {
                    // Undo & Sync
                    Undo.RecordObject(target, "Sync Tree Lanes");
                    SyncTreeLanesFromVisual(visualLanesProp, treeLanesProp);

                    // Änderungen festschreiben und Properties neu einlesen
                    serializedObject.ApplyModifiedProperties();
                    serializedObject.Update();

                    // Lists neu bauen + aktiven Tab clampen
                    BuildAllStageAndNodeLists();
                    _activeLane = Mathf.Clamp(_activeLane, 0, treeLanesProp.arraySize - 1);

                    // Diesen IMGUI-Pass sauber beenden, damit keine alten Layout-Handles verwendet werden
                    GUIUtility.ExitGUI();
                }
            }

            // +Stage nur wenn Tree-Lanes existieren
            using (new EditorGUI.DisabledScope(treeLanesProp.arraySize == 0))
            {
                if (GUILayout.Button(new GUIContent("+ Stage"), EditorStyles.toolbarButton))
                {
                    var stagesProp = treeLanesProp.GetArrayElementAtIndex(Mathf.Clamp(_activeLane, 0, treeLanesProp.arraySize - 1))
                                                  .FindPropertyRelative("stages");
                    stagesProp.arraySize++;
                    var newStage = stagesProp.GetArrayElementAtIndex(stagesProp.arraySize - 1);
                    newStage.FindPropertyRelative("nodes").arraySize = 0;
                    serializedObject.ApplyModifiedProperties();
                    BuildAllStageAndNodeLists();
                    Repaint();
                }
            }
        }
    }



    private float CalcNodesListHeight(ReorderableList nodeList, SerializedProperty nodesProp)
    {
        // ReorderableList-typische Maße:
        const float header = 18f;   // List-Header
        const float footer = 13f;   // Footer mit +/-
        float row = nodeList.elementHeight <= 0 ? EditorGUIUtility.singleLineHeight + 6 : nodeList.elementHeight;

        int count = Mathf.Max(1, nodesProp != null ? nodesProp.arraySize : 0); // mind. 1 "Empty"-Row sichtbar
        // etwas Padding oben/unten dazu
        return header + (row * count) + footer + 10f;
    }

    private float CalcStageHeight(int laneIndex, int stageIndex, ReorderableList nodeList, SerializedProperty stagesProp)
    {
        // Höhe des Stage-Headers
        float h = EditorGUIUtility.singleLineHeight + 6f;

        // Höhe der Nodes-Liste in dieser Stage
        var nodesProp = stagesProp.GetArrayElementAtIndex(stageIndex).FindPropertyRelative("nodes");
        h += CalcNodesListHeight(nodeList, nodesProp);

        // kleines Padding unten
        return h + 10f;
    }

    private void SyncTreeLanesFromVisual(SerializedProperty visualLanes, SerializedProperty treeLanes)
    {
        int vCount = visualLanes.arraySize;
        int tCount = treeLanes.arraySize;

        // Kürzen, wenn Tree mehr Lanes hat als Visual
        if (tCount > vCount)
        {
            int removed = tCount - vCount;
            for (int i = tCount - 1; i >= vCount; i--)
                treeLanes.DeleteArrayElementAtIndex(i);

            DebugManager.Log(
                $"ResearchTree Sync: {removed} Tree-Lane(s) entfernt (keine Visual-Lanes dafür vorhanden).",
                DebugManager.EDebugLevel.Dev, "Research", LogType.Warning
            );
        }

        // Auffüllen, wenn Visual mehr Lanes hat
        if (vCount > treeLanes.arraySize)
        {
            int add = vCount - treeLanes.arraySize;
            for (int i = 0; i < add; i++)
            {
                int idx = treeLanes.arraySize;
                treeLanes.arraySize++;
                var t = treeLanes.GetArrayElementAtIndex(idx);
                t.FindPropertyRelative("stages").arraySize = 0; // neu: leere Stages
            }

            DebugManager.Log(
                $"ResearchTree Sync: {add} Tree-Lane(s) hinzugefügt (aus Visual-Lanes).",
                DebugManager.EDebugLevel.Dev, "Research", LogType.Log
            );
        }

        // Namen/Farben übernehmen (Stages erhalten)
        for (int i = 0; i < treeLanes.arraySize && i < vCount; i++)
        {
            var v = visualLanes.GetArrayElementAtIndex(i);
            var t = treeLanes.GetArrayElementAtIndex(i);

            var vName = v.FindPropertyRelative("laneName").stringValue;
            var vColor = v.FindPropertyRelative("laneColor").colorValue;

            t.FindPropertyRelative("laneName").stringValue = vName;
            t.FindPropertyRelative("laneColor").colorValue = vColor;
            // Stages unverändert lassen
        }
    }


    private void RunCompile()
    {
        var compiled = ResearchTreeCompiler.Compile(_tree);

        // kleine Zusammenfassung
        int laneCount = _tree?.researchTreeLanes?.Count ?? 0;
        int stageCount = 0;
        if (_tree?.researchTreeLanes != null)
            foreach (var lane in _tree.researchTreeLanes)
                if (lane != null && lane.stages != null)
                    stageCount += lane.stages.Count;

        int nodeCount = compiled.nodesById?.Count ?? 0;
        int parentLinks = 0;
        if (compiled.parentsById != null)
            foreach (var kv in compiled.parentsById)
                parentLinks += kv.Value?.Count ?? 0;

        DebugManager.Log(
            $"ResearchTree Compile OK → Lanes={laneCount}, Stages={stageCount}, Nodes={nodeCount}, ParentLinks={parentLinks}",
            DebugManager.EDebugLevel.Dev, "Research", UnityEngine.LogType.Log
        );

        if (nodeCount == 0)
        {
            DebugManager.Log(
                "ResearchTree Hinweis: Noch keine Nodes im Tree gefunden.",
                DebugManager.EDebugLevel.Dev, "Research", UnityEngine.LogType.Warning
            );
        }
    }

    private ResearchNodeDef CreateNewNodeAsset(string suggestedDir, string laneName)
    {
        // Verzeichnis bestimmen
        string treePath = AssetDatabase.GetAssetPath(target);
        string baseDir = string.IsNullOrEmpty(suggestedDir)
            ? Path.GetDirectoryName(treePath)
            : suggestedDir;

        if (string.IsNullOrEmpty(baseDir)) baseDir = "Assets";
        if (!Directory.Exists(baseDir)) Directory.CreateDirectory(baseDir);

        // Dateiname vorschlagen
        string defaultName = "NewResearchNodeDef.asset";
        string title = "Create Research Node";
        string filter = "Research Node Asset";
        string path = EditorUtility.SaveFilePanelInProject(title, defaultName, "asset", filter, baseDir);
        if (string.IsNullOrEmpty(path))
        {
            DebugManager.Log("CreateNewNodeAsset: Abgebrochen.", DebugManager.EDebugLevel.Dev, "Research", LogType.Warning);
            return null;
        }

        // Asset erzeugen
        var node = ScriptableObject.CreateInstance<ResearchNodeDef>();
        // sinnvolle Defaults
        string fileName = Path.GetFileNameWithoutExtension(path);


        node.title = string.IsNullOrWhiteSpace(node.title) ? fileName : node.title;

        string lanePart = SanitizeIdPart(laneName);
        string filePart = SanitizeIdPart(fileName);

        string baseId = string.IsNullOrEmpty(lanePart) ? "Node_"+ filePart : (lanePart + "_" + filePart);

        var taken = CollectExistingNodeIds();
        string finalId = baseId;
        int suffix = 1;
        while (taken.Contains(finalId))
        {
            finalId = $"{baseId}_{suffix:D2}";
            suffix++;
        }
        node.id = finalId;

        // unlocks/requirements sind bereits per Default-Konstruktor initialisiert
        AssetDatabase.CreateAsset(node, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        EditorGUIUtility.PingObject(node);
        ResearchNodeEditorWindow.ShowFor(node);

        DebugManager.Log($"CreateNewNodeAsset: erstellt → {path}", DebugManager.EDebugLevel.Dev, "Research", LogType.Log);
        return node;
    }

    private static string SanitizeIdPart(string raw)
    {
        if (string.IsNullOrWhiteSpace(raw)) return "";
        var s = raw.Trim().ToLowerInvariant();
        s = Regex.Replace(s, @"\s+", "_");              // whitespace -> "_"
        s = Regex.Replace(s, @"[^a-z0-9_]+", "_");      // illegale chars -> "_"
        s = Regex.Replace(s, @"_+", "_");               // mehrfach "_" -> single
        s = s.Trim('_');                                // leading/trailing "_"
        return s;
    }

    private static HashSet<string> CollectExistingNodeIds()
    {
        var ids = new HashSet<string>(System.StringComparer.Ordinal);
        var guids = AssetDatabase.FindAssets("t:ResearchNodeDef");
        foreach (var g in guids)
        {
            var path = AssetDatabase.GUIDToAssetPath(g);
            var n = AssetDatabase.LoadAssetAtPath<ResearchNodeDef>(path);
            if (n != null && !string.IsNullOrWhiteSpace(n.id))
                ids.Add(n.id);
        }
        return ids;
    }

}
#endif
