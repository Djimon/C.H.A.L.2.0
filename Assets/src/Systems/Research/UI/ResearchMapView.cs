using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using CHAL.Core;   // DebugManager
using CHAL.Data;
using CHAL.UI;   // ResearchTreeDef, ResearchUIThemeDef
// nutzt deinen ResearchTreeCompiler & ResearchService
namespace CHAL.Systems.Research
{
    public sealed class ResearchMapView : MonoBehaviour
    {
        [Header("Wiring")]
        public RectTransform viewport;
        public RectTransform content;
        public Transform edgeContainer;
        public Transform nodeContainer;

        public ResearchUIThemeDef theme;
        public CodexDef treeDef;
        public CodexService service;
        private bool serviceReady = false;
        public ResearchNodeWidget nodePrefab;

        [Header("Auto-Layout (X)")]
        public int nodeSpacingX = 60;
        public int lanePaddingX = 80;

        // Pan/Zoom
        [Header("Pan/Zoom")]
        public bool enablePan = true;
        public bool enableZoom = true;

        int zoomIndex;
        float[] zoomSteps;

        [Header("HUD")]
        public ResearchHUD hud;

        // Runtime
        Dictionary<string, ResearchNodeWidget> widgets = new();
        Dictionary<string, Vector2> nodePositions = new();
        Vector2 lastMouse;
        bool dragging;

        void Awake()
        {
            
        }

        void Start()
        {

            if (!viewport || !content || !edgeContainer || !nodeContainer || !nodePrefab || theme == null || treeDef == null || service == null)
            {
                DebugManager.Log("ResearchMapView: Missing references, please wire in inspector.", DebugManager.EDebugLevel.Dev, "ResearchUI", LogType.Error);
            }

            // Zoom-Stufen aus Theme
            zoomSteps = (theme.zoomSteps != null && theme.zoomSteps.Length > 0) ? theme.zoomSteps : new float[] { 1f };
            zoomIndex = Mathf.Clamp(theme.defaultZoomIndex, 0, zoomSteps.Length - 1);
            content.localScale = Vector3.one * zoomSteps[zoomIndex];

            BuildMap();
            CenterOnActiveOrFirst();
        }

/// <summary>
/// Initializes the HUD if the service is available.
/// </summary>
        public void initHUD()
        {
            if (service == null) return;
            hud.Init(service, theme);
            serviceReady = true;
        }

        void Update()
        {
            if (serviceReady == false)
            {
                initHUD();
            }

            if (!viewport || !content) return;

            if (enablePan) HandlePan();
            if (enableZoom) HandleZoomWheel();
        }

        // ==== Build ====

/// <summary>
/// Builds the map by cleaning up existing elements and compiling the research tree.
/// </summary>
        public void BuildMap()
        {
            // Clean
            foreach (Transform t in edgeContainer) Destroy(t.gameObject);
            foreach (Transform t in nodeContainer) Destroy(t.gameObject);
            widgets.Clear();
            nodePositions.Clear();

            // Compile Tree
            var compiled = CodexCompiler.Compile(treeDef);

            // ---- 1) Gruppieren: pro Lane -> pro Stage -> sortierte Node-IDs
            var laneStageIds = new Dictionary<int, Dictionary<int, List<string>>>();
            foreach (var kv in compiled.posById)
            {
                var id = kv.Key;
                var (lane, stage) = kv.Value;

                if (!laneStageIds.TryGetValue(lane, out var stageMap))
                {
                    stageMap = new Dictionary<int, List<string>>();
                    laneStageIds[lane] = stageMap;
                }
                if (!stageMap.TryGetValue(stage, out var list))
                {
                    list = new List<string>();
                    stageMap[stage] = list;
                }
                list.Add(id);
            }
            // stabil sortieren (nach ID) pro Stage
            foreach (var stageMap in laneStageIds.Values)
                foreach (var list in stageMap.Values)
                    list.Sort(System.StringComparer.Ordinal);

            // ---- 2) Lane-Breiten bestimmen (max parallel pro Stage)
            int laneCount = treeDef.laneBaseX != null ? treeDef.laneBaseX.Count : 0;
            var laneWidths = new int[laneCount]; // effektive Breite jeder Lane
            var laneMaxPerStage = new int[laneCount];

            for (int lane = 0; lane < laneCount; lane++)
            {
                if (!laneStageIds.TryGetValue(lane, out var stageMap))
                {
                    laneWidths[lane] = 0;
                    laneMaxPerStage[lane] = 0;
                    continue;
                }

                int maxParallel = 0;
                foreach (var kvs in stageMap)
                    maxParallel = Mathf.Max(maxParallel, kvs.Value.Count);

                laneMaxPerStage[lane] = maxParallel;

                if (maxParallel <= 1)
                {
                    laneWidths[lane] = treeDef.nodeWidth; // eine Node breit
                }
                else
                {
                    laneWidths[lane] = (maxParallel * treeDef.nodeWidth) + ((maxParallel - 1) * nodeSpacingX);
                }
            }

            // ---- 3) Lane-Center X berechnen (auf Basis laneBaseX, bei Kollisionen nach rechts schieben)
            // Center initial = laneBaseX; daraus Start/End bestimmen; bei Overlap nach rechts korrigieren
            var laneCenters = new float[laneCount];
            var laneStartX = new float[laneCount];
            var laneEndX = new float[laneCount];

            for (int lane = 0; lane < laneCount; lane++)
            {
                float desiredCenter = (lane < treeDef.laneBaseX.Count) ? treeDef.laneBaseX[lane] : (treeDef.laneBaseX.Count > 0 ? treeDef.laneBaseX[^1] + lane * 300 : 300 + lane * 300);
                float width = laneWidths[lane];
                float start = desiredCenter - width * 0.5f;
                float end = desiredCenter + width * 0.5f;

                // mit vorheriger Lane abgleichen
                if (lane > 0)
                {
                    float minStart = laneEndX[lane - 1] + lanePaddingX;
                    if (start < minStart)
                    {
                        float shift = minStart - start;
                        start += shift;
                        end += shift;
                    }
                }

                laneCenters[lane] = (start + end) * 0.5f;
                laneStartX[lane] = start;
                laneEndX[lane] = end;
            }

            // ---- 4) Node-Positionen pro Stage symmetrisch verteilen
            // y bleibt wie gehabt: topMarginY + stage * stageStepY (nach unten = -y)
            foreach (var kv in compiled.posById)
            {
                var id = kv.Key;
                var (lane, stage) = kv.Value;

                int y = treeDef.topMarginY + stage * treeDef.stageStepY;
                float centerX = laneCenters[Mathf.Clamp(lane, 0, laneCenters.Length - 1)];

                // Anzahl in dieser Stage
                int countInStage = 1;
                int indexInStage = 0;
                if (laneStageIds.TryGetValue(lane, out var stageMap) && stageMap.TryGetValue(stage, out var ids))
                {
                    countInStage = ids.Count;
                    indexInStage = ids.IndexOf(id);
                    if (indexInStage < 0) indexInStage = 0;
                }

                float x;
                if (countInStage <= 1)
                {
                    x = centerX; // nur eine Node, mittig
                }
                else
                {
                    float totalWidth = (countInStage * treeDef.nodeWidth) + ((countInStage - 1) * nodeSpacingX);
                    float startX = centerX - totalWidth * 0.5f + treeDef.nodeWidth * 0.5f; // erste Node-Mitte
                    x = startX + indexInStage * (treeDef.nodeWidth + nodeSpacingX);
                }

                nodePositions[id] = new Vector2(x, -y); // -y: UI-Y nach unten
            }

            // ---- 5) Nodes instantiieren
            foreach (var id in compiled.nodesById.Keys)
            {
                var def = compiled.nodesById[id];
                var go = Instantiate(nodePrefab.gameObject, nodeContainer);
                var rt = go.GetComponent<RectTransform>();

                // WICHTIG: gleicher Koordinatenraum wie die Edge-Graphics (Top-Left-Anchor)
                rt.anchorMin = rt.anchorMax = new Vector2(0f, 1f); // Top-Left
                rt.pivot = new Vector2(0.5f, 0.5f);            // Position = Center (passt zu unserer Berechnung)
                rt.sizeDelta = new Vector2(treeDef.nodeWidth, treeDef.nodeHeight);
                rt.anchoredPosition = nodePositions[id];           // (x, -y) -> siehe Berechnung oben

                var w = go.GetComponent<ResearchNodeWidget>();
                w.Init(this, id, def.title, null);
                widgets[id] = w;
            }

            // ---- 6) Edges zeichnen (unverändert)
            foreach (var kv in compiled.parentsById)
            {
                var child = kv.Key;
                if (!nodePositions.TryGetValue(child, out var childPos)) continue;

                var parents = kv.Value;
                foreach (var pid in parents)
                {
                    if (!nodePositions.TryGetValue(pid, out var parentPos)) continue;
                    CreateEdge(parentPos, childPos, service.IsCompleted(pid));
                }
            }

            DebugManager.Log($"ResearchMapView: built nodes={widgets.Count}", DebugManager.EDebugLevel.Dev, "ResearchUI", LogType.Log);
            RefreshAllStates();
        }

        void CreateEdge(Vector2 from, Vector2 to, bool completed)
        {
            // Lokale Bounding-Box ermitteln (Top-Left Raum)
            float minX = Mathf.Min(from.x, to.x);
            float maxX = Mathf.Max(from.x, to.x);
            float minY = Mathf.Min(from.y, to.y);
            float maxY = Mathf.Max(from.y, to.y);

            // Größe darf nie 0 sein, sonst Cull/Mask-Probleme
            float w = Mathf.Max(1f, maxX - minX);
            float h = Mathf.Max(1f, maxY - minY);

            // Edge-Objekt anlegen
            var go = new GameObject("Edge", typeof(RectTransform), typeof(ResearchEdgeGraphic));
            go.transform.SetParent(edgeContainer, false);

            var rt = go.GetComponent<RectTransform>();
            rt.anchorMin = rt.anchorMax = new Vector2(0f, 1f); // Top-Left
            rt.pivot = new Vector2(0f, 1f);               // lokale (0,0) = Top-Left dieses Edge-Rechtecks
            rt.anchoredPosition = new Vector2(minX, minY);    // an die Bounding-Box setzen
            rt.sizeDelta = new Vector2(w, h);          // exakt so groß wie gebraucht

            // Graphic konfigurieren – Start/End in lokale Koords dieses Rects
            var g = go.GetComponent<ResearchEdgeGraphic>();
            g.raycastTarget = false;                     // Edge blockt keine Klicks
            g.color = theme.edgeColor;
            g.completedColor = theme.edgeCompletedColor;
            g.useCompletedColor = completed;
            g.thickness = theme.edgeThickness;

            // Punkte relativ zur lokalen Top-Left dieses Edge-Rechtecks
            g.start = new Vector2(from.x - minX, from.y - minY);
            g.end = new Vector2(to.x - minX, to.y - minY);

            g.SetAllDirty();
        }

        void RefreshAllStates()
        {
            foreach (var w in widgets.Values) w.ApplyState();
        }

        // ==== Interaction ====

        void HandlePan()
        {
            // Pan nur, wenn Maus über Viewport
            if (Input.GetMouseButtonDown(0) && RectTransformUtility.RectangleContainsScreenPoint(viewport, Input.mousePosition))
            {
                // Wenn der Klick auf dem HUD (UITK) liegt: NICHT schließen, NICHT pannen
                if (hud != null && hud.IsPointerOverUI(Input.mousePosition)) return;
                // Klick liegt in der Map -> Details schließen und Pan starten
                if (hud != null) hud.HideDetails();
                dragging = true;
                lastMouse = (Vector2)Input.mousePosition;
            }
            if (Input.GetMouseButton(0) && dragging)
            {
                Vector2 m = Input.mousePosition;
                Vector2 delta = m - lastMouse;
                lastMouse = m;

                content.anchoredPosition += delta; // 1:1 Screen-Pan fühlt sich bei Zoom gut an
            }
            if (Input.GetMouseButtonUp(0)) dragging = false;
        }

        void HandleZoomWheel()
        {
            float scroll = Input.mouseScrollDelta.y;
            if (Mathf.Abs(scroll) < 0.01f) return;

            int dir = scroll > 0 ? +1 : -1;
            SetZoomIndex(zoomIndex + dir, (Vector2)Input.mousePosition);
        }

/// <summary>
/// Sets the zoom index and adjusts the content scale based on the specified screen pivot.
/// </summary>
/// <param name="newIndex">The new zoom index to set.</param>
/// <param name="screenPivot">The screen point used to maintain the content position during zoom.</param>
        public void SetZoomIndex(int newIndex, Vector2 screenPivot)
        {
            newIndex = Mathf.Clamp(newIndex, 0, zoomSteps.Length - 1);
            if (newIndex == zoomIndex) return;

            // Weltpunkt (im Content) unter dem Mauszeiger ermitteln
            Vector2 localPointOld;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(content, screenPivot, null, out localPointOld);

            zoomIndex = newIndex;
            float newScale = zoomSteps[zoomIndex];
            content.localScale = Vector3.one * newScale;

            // Nach dem Skalieren den Content so verschieben, dass der Mauspunkt "stehen bleibt"
            Vector2 localPointNew;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(content, screenPivot, null, out localPointNew);
            Vector2 localDelta = localPointNew - localPointOld;
            content.anchoredPosition += localDelta * newScale; // Korrektur

            DebugManager.Log($"ResearchMapView: zoom={newScale:0.00}", DebugManager.EDebugLevel.Dev, "ResearchUI", LogType.Log);
        }

        // ==== Public API ====

        public ResearchUIThemeDef Theme => theme;
        public CodexService serviceRef => service;
        //TODO
//        public string ActiveNodeId => service.ActiveNodeId;

/// <summary>
/// Handles the event when a node is clicked.
/// Updates the visual state of widgets and shows details if the node is available.
/// </summary>
/// <param name="nodeId">The identifier of the clicked node.</param>
        public void OnNodeClicked(string nodeId)
        {
            // Auswahl visualisieren
            foreach (var kv in widgets)
                kv.Value.ApplyState(isSelected: kv.Key == nodeId);

            // (HUD später) – hier nur als Beispiel: aktive Forschung setzen, wenn erlaubt
            if (service.IsNodeAvailable(nodeId) && !service.IsCompleted(nodeId))
            {
                if (hud) hud.ShowDetails(nodeId);
                RefreshAllStates();
            }

            
        }

/// <summary>
/// Centers the content on the active node or the first widget if no active node is found.
/// </summary>
        public void CenterOnActiveOrFirst()
        {
            string id = service.GetActiveNodeId();
            if (string.IsNullOrEmpty(id) && widgets.Count > 0)
                id = widgets.Keys.First();

            if (!string.IsNullOrEmpty(id) && nodePositions.TryGetValue(id, out var pos))
            {
                Vector2 vpCenter = viewport.rect.center;
                content.anchoredPosition = vpCenter - pos * content.localScale.x;
            }
        }
    }
}
