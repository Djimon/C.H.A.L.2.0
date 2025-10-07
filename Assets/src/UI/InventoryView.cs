using CHAL.Systems.Inventory;
using CHAL.Systems.Items;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

public class InventoryView : MonoBehaviour
{
    public Sprite myInventoryBG;
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

        _grid.style.backgroundImage = new StyleBackground(myInventoryBG);

        _grid.style.flexDirection = FlexDirection.Column;
        _grid.style.flexWrap = Wrap.Wrap;
        _grid.Clear();

        // Layout
        int idx = 0;
        for (int r = 0; r < _rows; r++)
        {
            var row = new VisualElement();
            row.style.flexDirection = FlexDirection.Row;
            row.style.flexWrap = Wrap.NoWrap;
            row.style.marginBottom = 4;

            for (int c = 0; c < _cols; c++, idx++)
                row.Add(MakeSlot(idx));              // siehe Schritt 2

            _grid.Add(row);
        }

        //CreateGhost(root);

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

    private VisualElement MakeSlot(int slotIndex)
    {
        var tile = new VisualElement { name = $"slot_{slotIndex}" };

        // IMMER SICHTBAR
        tile.style.width = 64;
        tile.style.height = 64;
        tile.style.marginRight = 4;
        tile.style.marginBottom = 4;
        tile.style.flexDirection = FlexDirection.Column;
        tile.pickingMode = PickingMode.Position;
        tile.focusable = false;

        // sichtbarer Rahmen + dunkler Hintergrund (volle Deckkraft)
        tile.style.backgroundColor = new Color(0.16f, 0.16f, 0.16f, 1f);
        var border = new Color(0.35f, 0.35f, 0.35f, 1f);
        tile.style.borderTopWidth = tile.style.borderRightWidth =
        tile.style.borderBottomWidth = tile.style.borderLeftWidth = 1;
        tile.style.borderTopColor = tile.style.borderRightColor =
        tile.style.borderBottomColor = tile.style.borderLeftColor = border;

        // ICON (oben)
        var icon = new Image { name = "icon" };
        icon.scaleMode = ScaleMode.ScaleToFit;
        icon.image = null;                      // leer = kein Sprite
        icon.tintColor = Color.gray;            // Platzhaltergrau
        icon.style.width = 64;
        icon.style.height = 46;
        icon.pickingMode = PickingMode.Ignore;  // Events weiterreichen (falls später nötig)
        tile.Add(icon);

        // LABEL (unten)
        var label = new Label("-") { name = "label" };
        label.style.unityTextAlign = TextAnchor.MiddleCenter;
        label.style.color = Color.white;
        label.style.fontSize = 11;
        label.style.flexGrow = 1;
        label.pickingMode = PickingMode.Ignore;
        tile.Add(label);

   
        tile.RegisterCallback<ClickEvent>(evt =>
        {
            //Take
            if (!_dnd.HasFrom)
            {
                // Pickup nur wenn Slot belegt
                var s = _domain.Peek(_instanceID, slotIndex);
                if (!s.HasValue) return;

                _dnd.BeginDrag(
                    new ItemMoveObject { instanceID = _instanceID, slot = slotIndex },
                    splitHalf: false
                );
            }

            //Drop
            if (_dnd.HasFrom)
            {
                // Drop off
                _dnd.TryDropOn(new ItemMoveObject { instanceID = _instanceID, slot = slotIndex });
            }


        });


        //Split
        tile.RegisterCallback<MouseUpEvent>(evt =>
        {
            if (evt.button != 1) return;
            if (_dnd.HasFrom) return;

            if (evt.button == 1 && !_dnd.HasFrom)
            {
                // Pickup nur wenn Slot belegt
                var s = _domain.Peek(_instanceID, slotIndex);
                if (!s.HasValue) return;

                _dnd.BeginDrag(
                    new ItemMoveObject { instanceID = _instanceID, slot = slotIndex },
                    splitHalf: true
                );
            }
            evt.StopImmediatePropagation();
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
        if (label == null || icon == null) return; // Safety, sollte nicht passieren

        var s = _domain.Peek(_instanceID, slotIndex); // Domain liefert Stack (oder null)

        if (s.HasValue)
        {
            // Count
            label.text = $"×{s.Value.count}";

            // Icon + Tooltip-Name aus Registry (Fallbacks sicher)
            Sprite sprite = null; 
            string displayName = s.Value.itemID;
            if (ItemRegistry.Instance.TryGet(s.Value.itemID, out var def)) // ✔ TryGet existiert
            {
                sprite = def.icon;
                // TODO später: displayName = Localization.Get(def) – bis dahin itemId
            }

            icon.image = sprite !=null ? sprite.texture: null;               // null => bleibt grau
            icon.tintColor = sprite ? Color.white : Color.gray;
            tile.tooltip = displayName;        // Mouseover-Name
        }
        else
        {
            // leer
            label.text = "-";
            icon.image = null;
            icon.tintColor = Color.gray;
            tile.tooltip = "leer";
        }
    }

    private void RenderAllNow()
    {
        int total = _domain.SlotCount(_instanceID);
        for (int i = 0; i < total; i++) UpdateTileVisual(i);
    }
}
