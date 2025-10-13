using CHAL.Core;
using CHAL.Data;
using CHAL.Systems.Inventory;
using CHAL.Systems.Items;
using System;
using System.Collections;
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
        public InventoryDef _inventoryDef;
        public string inventoryID;

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

        public UIDocument doc => _doc;


        private void OnEnable()
        {
            UIDockingManager.Instance?.Register(this);
            StartCoroutine(BindFromTemplate());
        }

        private void Awake()
        {
            if (_doc == null) _doc = GetComponent<UIDocument>();
        }


        private void OnDisable()
        {
            UIDockingManager.Instance?.Unregister(this);
        }

        private void OnDestroy()
        {
            if (_domain != null) _domain.OnSlotChanged -= OnSlotChanged;
        }

        void Update()
        {
            
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

                    _dnd.BeginDrag(
                        new ItemMoveObject { instanceID = _instanceID, slot = slotIndex },
                        splitHalf: false
                    );
                }
                else
                {
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

                _dnd.BeginDrag(
                    new ItemMoveObject { instanceID = _instanceID, slot = slotIndex },
                    splitHalf: true
                );
            });
        }


        IEnumerator BindFromTemplate()
        {
            // Warten bis GameManager + Domain bereit
            while (GameManager.Instance == null || GameManager.Instance.Inventory == null)
                yield return null;

            if (_inventoryDef == null) { Debug.LogError("InventoryView: kein Template gesetzt."); yield break; }

            // player_* Default: instanceId = "player_" + enum-name (lowercase), falls im Inspector leer
            if (string.IsNullOrEmpty(inventoryID))
            {
                inventoryID = "player_" + _inventoryDef.TypeId.ToString().ToLowerInvariant();
            }

            var gm = GameManager.Instance;
            var domain = gm != null ? gm.Inventory : null;
            if (domain == null) { yield break; }

            // Nur binden, niemals erzeugen
            if (!domain.TryGetInstance(inventoryID, out var inst))
            {
                // GameManager hat die Player_* evtl. noch nicht gebaut → später erneut versuchen
                yield break;
            }

            // Grid-Maße aus der Instanz/Def
            int cols = (inst.InvDef != null) ? inst.InvDef.cols : _inventoryDef.cols;
            int rows = (inst.InvDef != null) ? inst.InvDef.rows : _inventoryDef.rows;

            // Binden
            Bind(domain, inst.instanceID, cols, rows);
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
            var label = new Label("-") { name = "label" };  //TODO: gefährlich wegen nciht threadsave
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

    }
}