using CHAL.Systems.Inventory;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

public class InventoryView : MonoBehaviour
{
    [SerializeField] private Vector2 _offset;
    [SerializeField] private UIDocument _doc;

    private VisualElement _grid;
    private IInventoryDomain _domain;
    private string _instanceID;
    private int _cols, _rows;

    private VisualElement _ghost;
    private Label _ghostLabel;

    private DragDropService _dnd;

    private void Awake()
    {
        // Auto-Fallback: wenn im Inspector nichts zugewiesen ist
        if (_doc == null)
            _doc = GetComponent<UIDocument>();
    }

    public void Bind(IInventoryDomain domain, string instanceID, int cols, int rows)
    {
        _domain = domain;
        _instanceID = instanceID;
        _cols = cols;
        _rows = rows;

        _dnd = new DragDropService(_domain);

        if (!_doc) { DebugManager.Error("UIDocument fehlt."); return; }

        var root = _doc.rootVisualElement;
        root.style.position = Position.Absolute;
        root.style.left = _offset.x;
        root.style.top = _offset.y;

        _grid = root.Q<VisualElement>("Grid");
        if (_grid == null) { DebugManager.Error("UXML braucht ein Element mit name='Grid'."); return; }

        _grid.style.flexDirection = FlexDirection.Column;
        _grid.style.flexWrap = Wrap.Wrap;
        _grid.Clear();

        //Zeilencontainer erstellen
        int total = _cols * _rows;
        int idx = 0;
        for (int r = 0; r < _rows; r++)
        {
            var row = new VisualElement();
            row.style.flexDirection = FlexDirection.Row;
            //row.style.flexWrap = Wrap.NoWrap;
            for (int c = 0; c < _cols; c++, idx++)
            {
                var btn = MakeSlot(idx); // deine Button-Erzeugung (64x64 + Events)
                row.Add(btn);
            }
            _grid.Add(row);
        }

        CreateGhost(root);

        _domain.OnSlotChanged += OnSlotChanged;
        RenderAllNow();

    }

    private void CreateGhost(VisualElement root)
    {
        _ghost = new VisualElement { name = "Ghost" };
        _ghost.style.position = Position.Absolute;
        _ghost.style.width = 64; _ghost.style.height = 64;
        _ghost.style.backgroundColor = new Color(1, 1, 1, 0.1f);
        _ghost.style.borderTopWidth = _ghost.style.borderRightWidth = _ghost.style.borderBottomWidth = _ghost.style.borderLeftWidth = 1;
        _ghost.style.borderTopColor = _ghost.style.borderRightColor = _ghost.style.borderBottomColor = _ghost.style.borderLeftColor = new Color(0.3f, 0.3f, 0.3f, 0.8f);
        _ghost.style.visibility = Visibility.Hidden;

        _ghostLabel = new Label("-");
        _ghostLabel.style.unityTextAlign = TextAnchor.MiddleCenter;
        _ghostLabel.style.flexGrow = 1;
        _ghost.Add(_ghostLabel);
        root.Add(_ghost);

        root.RegisterCallback<PointerMoveEvent>(e =>
        {
            if (_dnd.HasFrom)
            {
                _ghost.style.left = e.position.x + 8;
                _ghost.style.top = e.position.y + 8;
            }
        });

        // ESC zum Abbrechen
        root.RegisterCallback<KeyDownEvent>(e =>
        {
            if (e.keyCode == KeyCode.Escape && _dnd.HasFrom)
            {
                _dnd.Cancel();
                _ghost.style.visibility = Visibility.Hidden;
            }
        });
    }

    private VisualElement MakeSlot(int i)
    {
        var tile = new VisualElement { name = $"slot_{i}"};
        // simple Größe
        tile.style.width = 64; tile.style.height = 64; tile.style.marginRight = 4; tile.style.marginBottom = 4;
        tile.focusable = true;
        tile.pickingMode = PickingMode.Position;
        int slotIndex = i;
        var label = new Label("-");

        var icon = new Image { name = "icon" };
        icon.scaleMode = ScaleMode.ScaleToFit;
        icon.image = null;                       // starten ohne Sprite
        icon.tintColor = Color.gray;             // Fallback: eintönig grau
        icon.style.width = 64; icon.style.height = 46;                  // oben Icon
        icon.pickingMode = PickingMode.Ignore;   // Events an das Tile durchlassen
        tile.Add(icon);


        label.style.unityTextAlign = TextAnchor.MiddleCenter;
        label.style.flexGrow = 1;
        label.pickingMode = PickingMode.Ignore; // <<<<<< WICHTIG
        tile.Add(label);

        // LMB: Drag starten & auf Ziel droppen
        tile.RegisterCallback<PointerDownEvent>(evt =>
        {
            if (evt.button != 0) return;

            if (evt.button == 0)
            {
                var from = new ItemMoveObject { instanceID = _instanceID, slot = slotIndex };
                _dnd.BeginDrag(from,splitHalf:false);

                var s = _domain.Peek(_instanceID, slotIndex);

                if (!s.HasValue) return;

                if (s.HasValue && s.Value.count > 1)
                    _ghostLabel.text = $"{s.Value.itemID}\n×{System.Math.Max(1, s.Value.count)}";
                else
                    _ghostLabel.text = $"{s.Value.itemID ?? "-"}\n×1";

                _ghost.style.visibility = Visibility.Visible;
                evt.StopImmediatePropagation();
            }
        });
        //Drag benden: ablegen
        tile.RegisterCallback<PointerUpEvent>(evt =>
        {
            if (evt.button != 0 || !_dnd.HasFrom) return;

            if (evt.button == 0 && _dnd.HasFrom)
            {
                // Drop hier ablegen
                _dnd.TryDropOn(new ItemMoveObject { instanceID = _instanceID, slot = slotIndex });
                _ghost.style.visibility = Visibility.Hidden;
                evt.StopPropagation();
            }
        });

        // RMB: halbieren
        tile.RegisterCallback<PointerDownEvent>(evt =>
        {
            if (evt.button != 1) return;

            if (evt.button == 1)
            {
                var from = new ItemMoveObject { instanceID = _instanceID, slot = slotIndex };
                _dnd.BeginDrag(from, splitHalf: true);

                var s = _domain.Peek(_instanceID, slotIndex);

                if (!s.HasValue) return;

                if (s.HasValue && s.Value.count > 1)
                    _ghostLabel.text = $"{s.Value.itemID}\n×{System.Math.Max(1, s.Value.count / 2)}";
                else
                    _ghostLabel.text = $"{s.Value.itemID ?? "-"}\n×1";

                _ghost.style.visibility = Visibility.Visible;
                evt.StopImmediatePropagation();
            }
        });

        return tile;
    }

    private void OnDestroy()
    {
        if (_domain != null) _domain.OnSlotChanged -= OnSlotChanged;
    }

    private void OnSlotChanged(string instanceId, int slotIndex, ItemStack? newStack)
    {
        if (instanceId != _instanceID) return;
        UpdateTileVisual(slotIndex);
    }
    private void UpdateTileVisual(int slotIndex)
    {
        var tile = _grid.Q<VisualElement>($"slot_{slotIndex}");
        if (tile == null) return;

        var label = tile.Q<Label>("label") ?? tile.Q<Label>();
        var icon = tile.Q<Image>("icon") ?? tile.Q<Image>();

        if (icon == null)
        {
            icon = new Image { name = "icon", scaleMode = ScaleMode.ScaleToFit, pickingMode = PickingMode.Ignore };
            icon.style.width = 64; icon.style.height = 46;
            tile.Insert(0, icon);
        }
        if (label == null)
        {
            label = new Label("-") { name = "label" };
            label.style.unityTextAlign = TextAnchor.MiddleCenter;
            label.style.fontSize = 11;
            label.style.flexGrow = 1;
            label.pickingMode = PickingMode.Ignore;
            tile.Add(label);
        }

        var s = _domain.Peek(_instanceID, slotIndex);

        if (s.HasValue)
        {
            label.text = $"{s.Value.itemID}\n×{s.Value.count}";
            // icon.image = <Sprite aus Registry>; // später
            icon.tintColor = Color.white;  // oder neutral
        }
        else
        {
            label.text = "-";
            icon.image = null;             // fallback
            icon.tintColor = Color.gray;   // einstöckiges Grau
        }
    }

    private void RenderAllNow()
    {
        int total = _domain.SlotCount(_instanceID);
        for (int i = 0; i < total; i++) UpdateTileVisual(i);
    }
}
