# CHAL.UI.UIDockingManager

_Automatically generated/updated from `Assets/src/UI/misc/UIDockingManager.cs`._

1) Purpose
- Implements a singleton MonoBehaviour that manages docking layout for views implementing IDockableView.
- Registers/unregisters dockable views, tracks related UIDocuments, and triggers layout passes.
- Computes and applies left/right docking layouts per panel, with support for inventory querying.

2) Public API
- Namespace/module
  - CHAL.UI

- Types
  - public sealed class UIDockingManager : MonoBehaviour
    - Public fields/properties
      - public static UIDockingManager Instance { get; private set; }
      - public int DockSpacing = 8;
      - public IReadOnlyList<UIDocument> ActiveDocs => _docs;
    - Public events
      - public event Action<UIDocument> OnDocAdded;
      - public event Action<UIDocument> OnDocRemoved;
    - Public methods
      - public void Register(IDockableView view)
      - public void Unregister(IDockableView view)
      - public void NotifyViewChanged(IDockableView view)
      - public void QueueRelayout()
      - public IReadOnlyList<IDockableView> GetActiveInventories()
      - public InventoryView? GetOtherInventory(InventoryView caller)

3) Key Behavior & Side Effects
- Awake
  - Enforces singleton; logs error and disables itself if another instance exists.
- Update
  - If _relayoutQueued is true, clears flag and calls Relayout().
- Register(IDockableView)
  - Adds to _views if non-null and not already present.
  - If view.doc exists and not yet tracked, adds to _docs and fires OnDocAdded(view.doc).
  - Ensures Absolute position for view.OuterContainer.
  - Queues a relayout.
- Unregister(IDockableView)
  - Removes from _views.
  - If view.doc exists and is removed from _docs, fires OnDocRemoved(view.doc).
  - Queues a relayout.
- NotifyViewChanged(IDockableView)
  - Queues a relayout when a view’s properties/visibility change.
- Relayout()
  - Groups views by panel (outerContainer.panel) for visible, non-null views.
  - For each panel with a valid width, partitions into left and right AutoDock groups ordered by DockPriority.
  - Calls LayoutLeft for left-docked views and LayoutRight for right-docked views.
- LayoutLeft/LayoutRight
  - For AutoDock items, computes width via ComputeWidthPx and anchors to left or right edge, resetting opposite edge to Auto.
  - Applies spacing between consecutive items via DockSpacing.
- ComputeWidthPx
  - Converts base width percent to pixels given panelWidth; clamps with min/max pixel constraints; guarantees minimum width of at least 1 px.
- GetActiveInventories
  - Returns visible, non-readonly inventory views, sorted by DockPriority.
- GetOtherInventory
  - Returns the single other inventory (of type InventoryView) excluding the caller; returns null if none or ambiguous.

4) Constraints & Failure Modes
- Singleton guard
  - If an existing Instance is present, logs error and disables this component.
- Panel width guard
  - Only layouts panels with a valid (non-zero) width; otherwise skips.
- Null checks
  - Many operations guard against nulls (views, OuterContainer, panel, etc.).
- Positioning
  - Ensures Absolute positioning for containers that are laid out.
- Public API assumptions
  - Relies on external interfaces/types: IDockableView, InventoryView, UIDocument, etc. behavior is defined elsewhere.
- Threading/async
  - All UI operations occur on Unity main thread; no explicit threading support.

5) Example
- Minimal usage example:
```csharp
// Assuming you have a view implementing IDockableView in scope
public class ExampleUsage : MonoBehaviour
{
    [SerializeField] private UIDockingManager _manager;
    [SerializeField] private IDockableView _dockableView;

    void Start()
    {
        // Obtain manager instance (scene must contain UIDockingManager)
        var manager = UIDockingManager.Instance;
        if (manager != null && _dockableView != null)
        {
            manager.Register(_dockableView);
        }
    }
}
```

6) Unknowns
- Details of IDockableView, InventoryView, UIDocument, and panel/VisualElement schemas are defined elsewhere.
- Exact meanings of properties used in filtering (IsVisible, IsInventoryView, ReadOnly, Edge, DockPriority, AutoDock, BaseWidthPercent, MinWidthPx, MaxWidthPx, OuterContainer) beyond their names.
- Behavior of UIDocument instances and the semantics of OnDocAdded/OnDocRemoved beyond event hookup.
- How panels and their VisualTree/width interact in contexts outside this file.

