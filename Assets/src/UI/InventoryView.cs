using CHAL.Data;
using CHAL.Systems.Inventory;
using CHAL.Systems.Items;
using System;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;

namespace CHAL.UI
{

    public enum DockEdge { Left, Right, Top, Bottom }
    public enum SlotFitMode { FitWidth, FitBoth }

    public class InventoryView : MonoBehaviour, IDockableView
    {
        // ---------- Layout & UI ----------
        [Header("UXML hookup")]
        [SerializeField] private UIDocument _doc;
        [SerializeField] private string _containerElementName = "Panel";
        [SerializeField] private string _gridElementName = "Grid";

        [Header("Grid config")]
        [SerializeField] private int _cols = 4;
        [SerializeField] private int _rows = 3;
        [SerializeField] private bool _responsiveSizing = true;
        [SerializeField] private int _minSlotSize = 64;
        [SerializeField] private int _maxSlotSize = 512;
        [SerializeField] private int _slotGap = 4;
        [SerializeField] private SlotFitMode _fitMode = SlotFitMode.FitWidth;

        private int _computedSlotSize;

        [Header("Container width")]
        [Range(0.05f, 1f)][SerializeField] private float baseWidthPercent = 0.25f; // 25% des Panels
        [SerializeField] private int minWidthPx = 320;
        [SerializeField] private int maxWidthPx = 700;

        [Header("Docking (flags only; layout by DockingManager)")]
        [SerializeField] private DockEdge dockEdge = DockEdge.Left;
        [SerializeField] private int dockPriority = 0;
        [SerializeField] private bool autoDock = true;

        [Header("Behavior")]
        [SerializeField] private bool readOnly = false; // blockiert User-Interaktion

        [Header("Optional visuals")]
        public Sprite myInventoryBG;

        // ---------- Runtime ----------
        private VisualElement _outer;    // Container (gedockt)
        private VisualElement _grid;     // Slots-Grid

        private VisualElement _root;

        // --------Static Ghost --------
        static VisualElement sGhost;
        static VisualElement sGhostIcon;
        static Label sGhostCount;
        static bool sDragActive;
        static VisualElement _currentPanel;
        static readonly Vector2 _ghostOffset = new Vector2(5f, 5f);

        static readonly List<UIDocument> sDocs = new List<UIDocument>();


        // --- domain  ----
        private IInventoryDomain _domain;
        private string _instanceID;

        [SerializeField] public InvDnDProvider _invDnDProvider;
        private DragDropService _dnd;


        // Properties für Manager/QuickMove
        public string InstanceId => _instanceID;
        public bool IsVisible => _outer != null && _outer.resolvedStyle.visibility == Visibility.Visible;
        public VisualElement OuterContainer => _outer;
        public DockEdge Edge => dockEdge;
        public int DockPriority => dockPriority;
        public bool AutoDock => autoDock;
        public bool ReadOnly => readOnly;

        public float BaseWidthPercent => baseWidthPercent;

        public int MinWidthPx => minWidthPx;

        public int MaxWidthPx => maxWidthPx;

        public string StableId => InstanceId;

        public bool IsInventoryView => true;

        public bool IsItemCard => false;


        private void Awake()
        {
            if (_doc == null) _doc = GetComponent<UIDocument>();

            _root = _doc.rootVisualElement;
            CreateGhost();
        }

        private void OnEnable()
        {
            UIDockingManager.Instance?.Register(this);
            if (_doc == null) _doc = GetComponent<UIDocument>();
            if (_doc != null && !sDocs.Contains(_doc))
                sDocs.Add(_doc);
        }

        private void OnDisable()
        {
            if (_doc != null)
                sDocs.Remove(_doc);

            UIDockingManager.Instance?.Unregister(this);
        }

        private void OnDestroy()
        {
            if (_domain != null) _domain.OnSlotChanged -= OnSlotChanged;
        }

        void Update()
        {
            if (!sDragActive || _root.panel == null) return;

            

            if (sDragActive) 
            {
                if (Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.Escape))
                {
                    EndGhostDrag();
                }

                UIDocument targetDoc = GetPanelUnderMouse();

                VisualElement targetPanel = targetDoc.rootVisualElement.Q<VisualElement>("Panel");

                if (_currentPanel != targetPanel)
                {
                    ReparentGhostTo(targetDoc);
                    _currentPanel = targetPanel;
                }

                // Hole Screen-Pos (Bottom-Left) – z.B. mit Input.mousePosition
                Vector2 screenPos = Input.mousePosition;
                // Y-Flip für UI Toolkit (Top-Left)
                screenPos.y = Screen.height - screenPos.y;
                // Panel-Koordinaten (ohne PanelSettings-Scaling)
                Vector2 panelPos = RuntimePanelUtils.ScreenToPanel(_root.panel, screenPos);

                // Dann setzen:
                sGhost.style.left = panelPos.x + _ghostOffset.x;
                sGhost.style.top = panelPos.y + _ghostOffset.y;
                sGhost.BringToFront();
                sGhost.style.visibility = Visibility.Visible;
                sGhostCount.style.visibility = Visibility.Visible;
            }


        }

        /// <summary>
        /// Baut UI & bindet Domain. cols/rows können hier überschrieben werden (Definition aus InvDef).
        /// </summary>
        public void Bind(IInventoryDomain domain, string instanceID, int cols, int rows)
        {
            _domain = domain;
            _instanceID = instanceID;
            _cols = Mathf.Max(1, cols);
            _rows = Mathf.Max(1, rows);

            if (!_doc) { DebugManager.Error("UIDocument fehlt."); return; }

            var root = _doc.rootVisualElement;
            _outer = root.Q<VisualElement>(_containerElementName) ?? root; // Fallback: root

            _outer.RegisterCallback<GeometryChangedEvent>(_ => RecomputeSlotMetricsAndApply());

            RecomputeSlotMetricsAndApply();

            _grid = root.Q<VisualElement>(_gridElementName);
            if (_grid == null) { DebugManager.Error("UXML braucht ein Element mit name='Grid'."); return; }

            // Container-Grundlayout (Breite in %, min/max in px)
            ApplyContainerSizing();

            // Hintergrund (optional)
            if (myInventoryBG != null)
                _grid.style.backgroundImage = new StyleBackground(myInventoryBG);

            // DnD-Service holen (shared aus Provider, sonst eigener)
            if (_invDnDProvider == null)
                _invDnDProvider = FindFirstObjectByType<InvDnDProvider>();
            if (_invDnDProvider != null)
            {
                if (_invDnDProvider.domain == null)
                    _invDnDProvider.domain = _domain;

                _dnd = _invDnDProvider.Service;
            }
            else
            {
                _dnd = new DragDropService(_domain);
            }

            // Grid bauen
            BuildGrid();

            // Domain-Events & initiales Rendern
            _domain.OnSlotChanged += OnSlotChanged;
            RenderAllNow();

        }

        private void WireSlotInteractions(VisualElement tile, int slotIndex)
        {
            // ReadOnly → keinerlei Interaktion
            if (readOnly) return;

            // LMB: kompletter Klick = Pickup ODER Drop
            tile.RegisterCallback<ClickEvent>(evt =>
            {
                if (_dnd == null) return;

                // QuickMove (Shift + leer)
                if (evt.shiftKey && !_dnd.HasFrom)
                {
                    var s = _domain.Peek(_instanceID, slotIndex);
                    if (!s.HasValue) return;

                    var other = UIDockingManager.Instance?.GetOtherInventory(this);
                    if (other == null)
                    {
                        DebugManager.Info("QuickMove abgebrochen – kein gültiges Zielinventar verfügbar.");
                        return;
                    }

                    var req = new MoveRequest
                    {
                        fromInventory = new ItemMoveObject { instanceID = _instanceID, slot = slotIndex },
                        toInventory = new ItemMoveObject { instanceID = other.InstanceId, slot = -1 },
                        moveMode = MoveMode.Move
                    };

                    if (!_domain.TryMove(req, out var res))
                        DebugManager.Info($"QuickMove fehlgeschlagen: {res.reason}");
                    else
                        DebugManager.Log($"QuickMove OK: {s.Value.itemID} → {other.InstanceId}");

                    return; // QuickMove stoppt hier
                }

                //QUick-Move must be asked first


                if (!_dnd.HasFrom)
                {
                    var s = _domain.Peek(_instanceID, slotIndex);
                    if (!s.HasValue) return; // leerer Slot
                    
                    BeginGhostDrag((ItemStack)s);

                    _dnd.BeginDrag(
                        new ItemMoveObject { instanceID = _instanceID, slot = slotIndex },
                        splitHalf: false
                    );
                }
                else
                {
                    EndGhostDrag();
                    _dnd.TryDropOn(new ItemMoveObject { instanceID = _instanceID, slot = slotIndex });
                }

            });



            // RMB: Split-Pickup (kein Auto-Drop)
            tile.RegisterCallback<MouseUpEvent>(evt =>
            {
                if (evt.button != 1) return;
                if (_dnd == null || _dnd.HasFrom) return;

                var s = _domain.Peek(_instanceID, slotIndex);
                if (!s.HasValue || s.Value.count <= 1) return;

                BeginGhostDrag((ItemStack)s, true);

                _dnd.BeginDrag(
                    new ItemMoveObject { instanceID = _instanceID, slot = slotIndex },
                    splitHalf: true
                );
            });
        }


        // ---------- Rendering ----------
        private void BuildGrid()
        {
            // Layout des Grids: Zeilen-Container, Slots als VisualElements
            _grid.style.flexDirection = FlexDirection.Column;
            _grid.style.flexWrap = Wrap.NoWrap;
            _grid.Clear();

            int idx = 0;
            for (int r = 0; r < _rows; r++)
            {
                var row = new VisualElement();
                row.style.flexDirection = FlexDirection.Row;
                row.style.flexWrap = Wrap.NoWrap;
                row.style.marginBottom = _slotGap;

                for (int c = 0; c < _cols; c++, idx++)
                    row.Add(MakeSlot(idx));

                _grid.Add(row);
            }
        }

        private VisualElement MakeSlot(int slotIndex)
        {
            var tile = new VisualElement { name = $"slot_{slotIndex}" };

            // Sichtbare Kachel
            tile.style.width = _computedSlotSize > 0 ? _computedSlotSize : _minSlotSize;
            tile.style.height = _computedSlotSize > 0 ? _computedSlotSize : _minSlotSize;
            tile.style.marginRight = _slotGap;
            tile.style.marginBottom = 0;
            tile.style.flexDirection = FlexDirection.Column;
            tile.pickingMode = PickingMode.Position;
            tile.focusable = false;

            tile.style.backgroundColor = new Color(0.16f, 0.16f, 0.16f, 1f);
            var border = new Color(0.35f, 0.35f, 0.35f, 1f);
            tile.style.borderTopWidth = tile.style.borderRightWidth =
            tile.style.borderBottomWidth = tile.style.borderLeftWidth = 1;
            tile.style.borderTopColor = tile.style.borderRightColor =
            tile.style.borderBottomColor = tile.style.borderLeftColor = border;

            // Icon (oben ~75% Höhe)
            var icon = new Image { name = "icon" };
            icon.scaleMode = ScaleMode.ScaleToFit;
            icon.sprite = null;
            icon.tintColor = Color.gray;
            icon.pickingMode = PickingMode.Ignore;
            icon.style.width = _minSlotSize;
            icon.style.height = Mathf.RoundToInt(_minSlotSize * 0.72f);
            tile.Add(icon);

            // Label (unten)
            var label = new Label("-") { name = "label" };
            label.style.unityTextAlign = TextAnchor.MiddleCenter;
            label.style.color = Color.white;
            label.style.fontSize = Mathf.Clamp(Mathf.RoundToInt(_minSlotSize * 0.18f), 10, 16);
            label.style.flexGrow = 1;
            label.pickingMode = PickingMode.Ignore;
            tile.Add(label);

            // Interaktion (Click=LMB; RMB via MouseUp)
            WireSlotInteractions(tile, slotIndex);

            return tile;
        }

        private void OnSlotChanged(string instanceId, int slotIndex, ItemStack? newStack)
        {
            if (instanceId != _instanceID) return;
            UpdateTileVisual(slotIndex);
        }

        private void RenderAllNow()
        {
            int total = _domain.SlotCount(_instanceID);
            for (int i = 0; i < total; i++) UpdateTileVisual(i);
        }

        private void UpdateTileVisual(int slotIndex)
        {
            var tile = _grid.Q<VisualElement>($"slot_{slotIndex}");
            if (tile == null) return;

            var label = tile.Q<Label>("label") ?? tile.Q<Label>();
            var icon = tile.Q<Image>("icon") ?? tile.Q<Image>();
            if (label == null || icon == null) return;

            var s = _domain.Peek(_instanceID, slotIndex);

            if (s.HasValue)
            {
                label.text = $"×{s.Value.count}";

                Sprite sprite = null;
                string displayName = s.Value.itemID;
                if (ItemRegistry.Instance.TryGet(s.Value.itemID, out var def))
                {
                    sprite = def.icon;
                    // displayName = def.displayName ?? displayName; // später
                }

                icon.sprite = sprite;                        // Unity 6: direkt Sprite
                icon.tintColor = sprite ? Color.white : Color.gray;
                tile.tooltip = displayName;
            }
            else
            {
                label.text = "-";
                icon.sprite = null;
                icon.tintColor = Color.gray;
                tile.tooltip = "leer";
            }
        }

        private void RecomputeSlotMetricsAndApply()
        {
            if (!_responsiveSizing || _outer == null || _grid == null) return;

            var r = _outer.contentRect;                // echte, gelayoutete Größe
            if (r.width <= 0f || r.height <= 0f) return;

            // 1) Fit nach Breite
            float gapsX = Mathf.Max(0, _cols - 1) * _slotGap;
            float widthForSlots = Mathf.Max(0, r.width - gapsX);
            int fromWidth = Mathf.FloorToInt(widthForSlots / Mathf.Max(1, _cols));

            int target = fromWidth;

            // 2) Option FitBoth: auch Höhe berücksichtigen (nur sinnvoll, wenn der Container eine echte Höhe hat)
            if (_fitMode == SlotFitMode.FitBoth)
            {
                float gapsY = Mathf.Max(0, _rows - 1) * _slotGap;
                float heightForSlots = Mathf.Max(0, r.height - gapsY);
                int fromHeight = Mathf.FloorToInt(heightForSlots / Mathf.Max(1, _rows));
                target = Mathf.Min(fromWidth, fromHeight);
            }

            // 3) clampen
            int maxClamp = (_maxSlotSize > 0) ? _maxSlotSize : int.MaxValue;
            int computed = Mathf.Clamp(target, _minSlotSize, maxClamp);

            if (computed == _computedSlotSize) return; // nichts zu tun
            _computedSlotSize = computed;

            ApplySlotMetrics(); // Größen an alle Tiles pushen

            UIDockingManager.Instance?.NotifyViewChanged(this);
        }

        private void ApplySlotMetrics()
        {
            // Row-Abstände
            foreach (var row in _grid.Children())
                row.style.marginBottom = _slotGap;

            // Pro Tile: Größe + Icon/Label ableiten
            for (int i = 0; i < _cols * _rows; i++)
            {
                var tile = _grid.Q<VisualElement>($"slot_{i}");
                var icon = tile?.Q<Image>("icon");
                var label = tile?.Q<Label>("label");
                if (tile == null || icon == null || label == null) continue;

                int s = _computedSlotSize;

                tile.style.width = s;
                tile.style.height = s;
                tile.style.marginRight = _slotGap;

                icon.style.width = s;
                icon.style.height = Mathf.RoundToInt(s * 0.72f);

                label.style.fontSize = Mathf.Clamp(Mathf.RoundToInt(s * 0.18f), 10, 18);
            }

        }

        private void ApplyContainerSizing()
        {
            if (_outer == null) return;

            // Prozentbreite + min/max (UI Toolkit wendet das korrekt relativ zum Parent an)
            _outer.style.width = Length.Percent(Mathf.Clamp01(baseWidthPercent) * 100f);
            _outer.style.minWidth = minWidthPx;
            _outer.style.maxWidth = maxWidthPx;

            // Außenabstand zwischen Views an einer Kante (vom DockingManager genutzt)
            _outer.style.marginLeft = _outer.style.marginRight = _outer.style.marginTop = _outer.style.marginBottom = 0;
        }

        // ------ Ghost ------
        private void CreateGhost()
        {
            // 1) Overlay im Inventory-Root anlegen
            sGhost = new VisualElement { name = "DnD_GhostOverlay" };
            sGhost.pickingMode = PickingMode.Ignore;           // Click-Through
            sGhost.style.position = Position.Absolute;
            sGhost.style.visibility = Visibility.Hidden;
            _root.Add(sGhost);

            // 2) Inhalt: Icon + Count
            sGhostIcon = new VisualElement { name = "GhostIcon" };
            sGhostIcon.pickingMode = PickingMode.Ignore;
            sGhostIcon.style.width = 48;
            sGhostIcon.style.height = 48;
            sGhostIcon.style.opacity = 0.6f;                   // halbtransparent
            sGhost.Add(sGhostIcon);

            sGhostCount = new Label();
            sGhostCount.pickingMode = PickingMode.Ignore;
            sGhostCount.style.position = Position.Absolute;
            sGhostCount.style.right = -4;
            sGhostCount.style.bottom = -4;
            sGhostCount.style.unityTextAlign = TextAnchor.MiddleCenter;
            sGhostCount.style.fontSize = 11;
            sGhostCount.style.paddingLeft = 4;
            sGhostCount.style.paddingRight = 4;
            sGhostCount.style.paddingTop = 1;
            sGhostCount.style.paddingBottom = 1;
            sGhostCount.style.backgroundColor = new Color(0, 0, 0, 0.6f);
            sGhostCount.style.color = Color.white;
            sGhost.Add(sGhostCount);

            // 3) Cursor-Follow (nur wenn Ghost aktiv)
            //_root.RegisterCallback<PointerMoveEvent>(OnPointerMove);
        }

        public void EndGhostDrag()
        {
            sDragActive = false;
            if (sGhost != null)
            {
                sGhost.style.visibility = Visibility.Hidden;
                sGhostCount.style.visibility = Visibility.Hidden;
            }
                
        }

        public void BeginGhostDrag(ItemStack s, bool isSplit = false)
        {

            ItemRegistry.Instance.TryGet(s.itemID, out var def);
            Sprite icon = def.icon;
            int stackCount = s.count;

            EnsureGhostExists();            // lazy-create
            EnsureGhostInSomePanel();       // initial anhängen (irgendein aktives Panel)

            if (icon != null)
                sGhostIcon.style.backgroundImage = new StyleBackground(icon);
            else
                sGhostIcon.style.backgroundImage = StyleKeyword.None;

            if (stackCount > 1)
            {
                sGhostCount.text = isSplit? Mathf.Max(1, Mathf.FloorToInt(stackCount / 2)).ToString() : stackCount.ToString();
                sGhostCount.style.visibility = Visibility.Visible;
            }
            else
            {
                sGhostCount.text = "";
                sGhostCount.style.visibility = Visibility.Hidden;
            }

            sGhost.BringToFront();
            sGhost.style.visibility = Visibility.Visible;
            sDragActive = true;
        }

        static void EnsureGhostExists()
        {
            if (sGhost != null) return;

            sGhost = new VisualElement { name = "DnD_GhostOverlay_Global" };
            sGhost.pickingMode = PickingMode.Ignore;
            sGhost.style.position = Position.Absolute;
            sGhost.style.visibility = Visibility.Hidden;

            sGhostIcon = new VisualElement { name = "GhostIcon" };
            sGhostIcon.pickingMode = PickingMode.Ignore;
            sGhostIcon.style.width = 48;
            sGhostIcon.style.height = 48;
            sGhostIcon.style.opacity = 0.6f;
            sGhost.Add(sGhostIcon);

            sGhostCount = new Label();
            sGhostCount.pickingMode = PickingMode.Ignore;
            sGhostCount.style.position = Position.Absolute;
            sGhostCount.style.right = -4;
            sGhostCount.style.bottom = -4;
            sGhostCount.style.fontSize = 11;
            sGhostCount.style.backgroundColor = new Color(0, 0, 0, 0.6f);
            sGhostCount.style.color = Color.white;
            sGhost.Add(sGhostCount);
        }

        static void EnsureGhostInSomePanel()
        {
            // Hänge initial in das erste verfügbare Panel
            for (int i = sDocs.Count - 1; i >= 0; i--)
            {
                var doc = sDocs[i];
                if (doc != null && doc.rootVisualElement != null && doc.rootVisualElement.panel != null)
                {
                    ReparentGhostTo(doc);
                    _currentPanel = doc.rootVisualElement.Q<VisualElement>("Panel");
                    return;
                }
            }
            // kein Panel -> bleibt detached; Update blendet es dann aus
        }

        static void ReparentGhostTo(UIDocument doc)
        {
            if (doc == null || doc.rootVisualElement == null) return;

            // vorsorglich ablösen
            sGhost.RemoveFromHierarchy();
            // an das Ziel-Panel hängen (Root)
            doc.rootVisualElement.Add(sGhost);
            // nach vorn holen
            sGhost.BringToFront();
        }

        static UIDocument GetPanelUnderMouse()
        {
            Vector2 screen = Input.mousePosition;
            screen.y = Screen.height - screen.y;

            // Wir prüfen jedes Panel: Screen->Panel und Contains(worldBound)
            // Reihenfolge: letzte registrierte zuerst (typisch "oben")
            for (int i = sDocs.Count - 1; i >= 0; i--)
            {
                var doc = sDocs[i];
                if (doc == null) continue;

                var root = doc.rootVisualElement;
                var panel = root?.panel;
                if (panel == null) continue;

                Vector2 panelPos = RuntimePanelUtils.ScreenToPanel(panel, screen);
                if (root.worldBound.Contains(panelPos))
                    return doc;
            }
            return null;
        }

    }
}