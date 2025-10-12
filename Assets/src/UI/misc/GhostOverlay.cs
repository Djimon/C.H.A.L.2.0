using CHAL.Systems.Inventory;
using CHAL.Systems.Items;
using CHAL.UI;
using UnityEngine;
using UnityEngine.UIElements;

namespace CHAL.UI
{
    public sealed class GhostOverlay : MonoBehaviour
    {
        public int ghostSize = 48;
        public float opacity = 0.6f;

        [SerializeField] InvDnDProvider _provider; // per Inspector
        [SerializeField] private Vector2 _offset = new Vector2(12f, 12f);
    
        VisualElement _ghost, _icon;
        Label _count;
        ItemStack? _current; 
        bool _split; 
        UIDocument _currentDoc;

        private DragDropService _svc;
        private bool _subscribed;

        void OnEnable()
        {
            CreateGhost();
            TrySubscribe(); // sofort versuchen

            if (UIDockingManager.Instance != null)
            {
                UIDockingManager.Instance.OnDocAdded += _ => EnsureParent();
                UIDockingManager.Instance.OnDocRemoved += doc => { if (_currentDoc == doc) _currentDoc = null; };
            }
        }

        void OnDisable()
        {
            TryUnsubscribe();
        }

        void TrySubscribe()
        {
            if (_subscribed) return;

            // Service kann null sein, wenn provider.domain noch nicht gesetzt wurde
            var svc = _provider?.Service;
            if (svc == null) return;

            _svc = svc;
            _svc.OnBeginDrag += HandleBegin;
            _svc.OnEndDrag += HandleEnd;
            _subscribed = true;
        }

        void TryUnsubscribe()
        {
            if (!_subscribed) return;
            if (_svc != null)
            {
                _svc.OnBeginDrag -= HandleBegin;
                _svc.OnEndDrag -= HandleEnd;
                _svc = null;
            }
            _subscribed = false;
        }

        void CreateGhost()
        {
            _ghost = new VisualElement { name = "DnD_Ghost", pickingMode = PickingMode.Ignore };
            _ghost.style.position = Position.Absolute;
            _ghost.style.visibility = Visibility.Hidden;

            _icon = new VisualElement { name = "Icon", pickingMode = PickingMode.Ignore };
            _icon.style.width = ghostSize; _icon.style.height = ghostSize; _icon.style.opacity = opacity;
            _ghost.Add(_icon);

            _count = new Label { pickingMode = PickingMode.Ignore };
            _count.style.position = Position.Absolute; _count.style.right = -4; _count.style.bottom = -4;
            _count.style.backgroundColor = new Color(0, 0, 0, opacity); _count.style.color = Color.white;
            _ghost.Add(_count);
        }

        void HandleBegin(ItemStack stack, bool split)
        {
            _current = stack; _split = split;
            EnsureParent();
            RenderContent(); 
            
            _ghost.style.visibility = Visibility.Visible;
            _count.style.visibility= Visibility.Visible;
        }
        void HandleEnd()
        {
            _current = null;
            _ghost.style.visibility = Visibility.Hidden;
            _count.style.visibility = Visibility.Hidden;
        }

        void EnsureParent()
        {
            var dm = UIDockingManager.Instance;
            if (dm == null) return;

            // Wenn wir noch keinen aktuellen Doc haben, nimm den zuletzt registrierten
            if (_currentDoc == null && dm.ActiveDocs.Count > 0)
                _currentDoc = dm.ActiveDocs[^1];

            if (_currentDoc != null && _ghost.panel == null)
                _currentDoc.rootVisualElement.Add(_ghost);
        }

        void Update()
        {
            if (!_subscribed)
                TrySubscribe();

            if (_current == null)
                return;

            // Panel unter Maus finden
            var doc = GetDocUnderMouse();
            if (doc != null && doc != _currentDoc)
            {
                _ghost.RemoveFromHierarchy();
                doc.rootVisualElement.Add(_ghost);
                _currentDoc = doc;
            }

            // Position setzen
            var panel = _currentDoc?.rootVisualElement?.panel;
            if (panel != null)
            {
                Vector2 screen = Input.mousePosition;
                screen.y = Screen.height - screen.y; // Y-Flip für UI Toolkit
                Vector2 pos = RuntimePanelUtils.ScreenToPanel(panel, screen);

                _ghost.style.left = pos.x + _offset.x;
                _ghost.style.top = pos.y + _offset.y;
                _ghost.BringToFront();
            }
        }

        UIDocument GetDocUnderMouse()
        {
            var docs = UIDockingManager.Instance.ActiveDocs;
            var screen = (Vector2)Input.mousePosition; screen.y = Screen.height - screen.y;
            for (int i = docs.Count - 1; i >= 0; i--)
            {
                var root = docs[i].rootVisualElement; var panel = root.panel;
                if (panel == null) continue;
                var p = RuntimePanelUtils.ScreenToPanel(panel, screen);
                if (root.worldBound.Contains(p)) return docs[i];
            }
            return null;
        }

        void RenderContent()
        {
            if (_current == null) return;
            if (ItemRegistry.Instance.TryGet(_current.Value.itemID, out var def) && def.icon)
                _icon.style.backgroundImage = new StyleBackground(def.icon);
            else _icon.style.backgroundImage = StyleKeyword.None;

            int shown = _split ? Mathf.Max(1, _current.Value.count / 2) : _current.Value.count;
            _count.text = shown > 1 ? shown.ToString() : "";
            _count.style.visibility = shown > 1 ? Visibility.Visible : Visibility.Hidden;
        }
    }
}