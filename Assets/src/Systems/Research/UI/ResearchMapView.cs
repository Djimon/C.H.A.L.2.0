using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using CHAL.Core;   // DebugManager
using CHAL.Data;   // ResearchTreeDef, ResearchUIThemeDef
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
        public ResearchTreeDef treeDef;
        public ResearchService service;
        private bool serviceReady = false;
        public ResearchNodeWidget nodePrefab;

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

        public void BuildMap()
        {
            // Clean
            foreach (Transform t in edgeContainer) Destroy(t.gameObject);
            foreach (Transform t in nodeContainer) Destroy(t.gameObject);
            widgets.Clear();
            nodePositions.Clear();

            // Compile Tree
            var compiled = ResearchTreeCompiler.Compile(treeDef);

            // Positionsberechnung pro Node
            foreach (var kv in compiled.posById)
            {
                var id = kv.Key;
                var (lane, stage) = kv.Value;
                int x = (lane >= 0 && lane < theme.laneBaseX.Count) ? theme.laneBaseX[lane] : theme.laneBaseX.Last();
                int y = theme.topMarginY + stage * theme.stageStepY;
                nodePositions[id] = new Vector2(x, -y); // -y: UI-Y nach unten
            }

            // Nodes instantiieren
            foreach (var id in compiled.nodesById.Keys)
            {
                var def = compiled.nodesById[id];
                var go = Instantiate(nodePrefab.gameObject, nodeContainer);
                var rt = go.GetComponent<RectTransform>();
                rt.anchoredPosition = nodePositions[id];
                rt.sizeDelta = new Vector2(theme.nodeWidth, theme.nodeHeight);

                var w = go.GetComponent<ResearchNodeWidget>();
                w.Init(this, id, def.title, null);
                widgets[id] = w;
            }

            // Edges zeichnen
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
            var go = new GameObject("Edge", typeof(RectTransform), typeof(ResearchEdgeGraphic));
            go.transform.SetParent(edgeContainer, false);

            var rt = go.GetComponent<RectTransform>();
            rt.anchorMin = rt.anchorMax = new Vector2(0, 1); // top-left
            rt.pivot = new Vector2(0, 1);
            rt.anchoredPosition = Vector2.zero;
            rt.sizeDelta = content.sizeDelta; // groß genug, oder (0,0) – der Graphic nutzt lokale Koords

            var g = go.GetComponent<ResearchEdgeGraphic>();
            g.color = theme.edgeColor;
            g.completedColor = theme.edgeCompletedColor;
            g.useCompletedColor = completed;
            g.thickness = theme.edgeThickness;

            g.start = from;
            g.end = to;
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
        public ResearchService serviceRef => service;
        //TODO
//        public string ActiveNodeId => service.ActiveNodeId;

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
