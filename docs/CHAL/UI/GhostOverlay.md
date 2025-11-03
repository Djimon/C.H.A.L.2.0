# CHAL.UI.GhostOverlay

_Automatically generated/updated from `Assets/src/UI/misc/GhostOverlay.cs`._

1) Purpose
- GhostOverlay defines a Unity MonoBehaviour that renders a draggable "ghost" with an item icon and a count when dragging items in the UI Toolkit-based UI.
- It attaches the ghost to the current UIDocument dock/window, updates its position under the mouse, and moves it between active docs as needed.
- It subscribes to a DragDropService via an InvDnDProvider to react to drag begin/end events and render the ghost accordingly.

2) Public API
- Namespace/module: CHAL.UI
- Types
  - public sealed class GhostOverlay : MonoBehaviour
    - Public fields
      - public int ghostSize
        - size (in pixels) of the ghost icon area
      - public float opacity
        - overall opacity of the ghost elements
      - [SerializeField] private Vector2 _offset
        - offset position of the ghost relative to the mouse
    - (No public methods, properties, or events are declared)

3) Key Behavior & Side Effects
- OnEnable
  - Creates the ghost visuals via CreateGhost.
  - Attempts to subscribe to drag events via TrySubscribe.
  - If UIDockingManager is available, subscribes to OnDocAdded and OnDocRemoved to manage the ghost's parent doc.
- OnDisable
  - Unsubscribes from drag events via TryUnsubscribe.
- TrySubscribe
  - If already subscribed, no-op.
  - Reads service from _provider?.Service; if null, returns.
  - Subscribes to _svc.OnBeginDrag (HandleBegin) and _svc.OnEndDrag (HandleEnd); marks as subscribed.
- TryUnsubscribe
  - If subscribed, detaches event handlers and clears _svc; marks as not subscribed.
- CreateGhost
  - Builds the UI Toolkit elements:
    - _ghost: VisualElement named "DnD_Ghost", absolute positioning, initially hidden.
    - _icon: VisualElement named "Icon" (child of _ghost), sized by ghostSize, with opacity.
    - _count: Label (child of _ghost), absolute positioned at bottom-right, background/foreground colors set with the configured opacity.
- HandleBegin(ItemStack, bool)
  - Stores current stack and split flag; ensures a parent doc; renders content; shows ghost and count.
- HandleEnd()
  - Clears current stack; hides ghost and count.
- EnsureParent()
  - Uses UIDockingManager.Instance; if no _currentDoc yet and there are active docs, selects the last one.
  - If a current doc exists and the ghost is not yet in the hierarchy, attaches _ghost to the current doc's rootVisualElement.
- Update()
  - If not subscribed, attempts to subscribe.
  - If no current item being dragged, returns.
  - Determines the doc under the mouse; if it's different from _currentDoc, moves _ghost to that doc and updates _currentDoc.
  - If a panel is available, converts the mouse position to panel coordinates and positions _ghost with _offset; brings ghost to front.
- GetDocUnderMouse()
  - Iterates active docs from top-most to bottom-most.
  - Converts the mouse screen position to panel coordinates; returns the first doc whose rootVisualElement worldBound contains the point.
- RenderContent()
  - If no current item, returns.
  - Looks up the item definition via ItemRegistry.Instance.TryGet(_current.Value.itemID, out def); if def.icon exists, sets _icon’s background to the icon; otherwise clears it.
  - Computes shown = _split ? max(1, _current.Value.count / 2) : _current.Value.count.
  - Updates _count.text to shown if greater than 1; otherwise empty.
  - Sets _count.style.visibility to Visible when shown > 1, else Hidden.

4) Constraints & Failure Modes
- _provider may be null or its Service may be null; TrySubscribe guards against this and will not subscribe until a valid service is available.
- _subscribed state prevents double-subscription; TryUnsubscribe is guarded to only detach when needed.
- UIDockingManager.Instance may be null; OnEnable guards against this when wiring doc events.
- Ghost attachment relies on _currentDoc and ActiveDocs; if there are no docs, ghost may remain unattached until a doc becomes available.
- RenderContent relies on ItemRegistry; if an item or icon is missing, icon is cleared gracefully.
- GetDocUnderMouse assumes UIDockingManager and docs with non-null rootVisualElement/panel; returns null if none found.
- Positioning uses panel-based coordinates; if panel is null, ghost remains not positioned.
- Visibility toggling heavily depends on UI Toolkit state; EnsureParent/Update keep the ghost in a valid hierarchy to avoid layout issues.
- ItemStack and item definitions are external to this file; behavior depends on their structure (e.g., count, itemID).

5) Example
- Not derivable from the file alone; no code example provided.

6) Unknowns
- Exact behavior and lifecycle of InvDnDProvider, DragDropService, UIDockingManager, and UIDocuments beyond what is used here.
- Details of ItemRegistry, ItemDef.icon, and ItemStack structure beyond usage in this file.
- Any side effects of adding/removing _ghost to rootVisualElement beyond the visible UI changes.

