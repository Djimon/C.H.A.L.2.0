# Assets/src/UI/misc/UIDockingManager.cs

_Automatically generated/updated from `Assets/src/UI/misc/UIDockingManager.cs`._

# Purpose
- Manages the docking and layout of dockable views in the UI.

# Public API
- Namespace: `CHAL.UI`
- Types
  - `public sealed class UIDockingManager : MonoBehaviour`
    - Public fields/properties:
      - `public static UIDockingManager Instance { get; private set; }` - Singleton instance of the docking manager.
      - `public int DockSpacing` - Spacing between docked views.
      - `public IReadOnlyList<UIDocument> ActiveDocs` - List of currently active documents.
      - `public event Action<UIDocument> OnDocAdded` - Event triggered when a document is added.
      - `public event Action<UIDocument> OnDocRemoved` - Event triggered when a document is removed.
    - Public methods:
      - `public void Register(IDockableView view)` - Registers a dockable view.
      - `public void Unregister(IDockableView view)` - Unregisters a dockable view.
      - `public void NotifyViewChanged(IDockableView view)` - Notifies that a view's properties or visibility have changed.
      - `public void QueueRelayout()` - Marks the layout for relayout.
      - `public IReadOnlyList<IDockableView> GetActiveInventories()` - Retrieves active, interactive inventories.
      - `public InventoryView GetOtherInventory(InventoryView caller)` - Gets another inventory view, if available.

# Key Behavior & Side Effects
- Ensures only one instance of `UIDockingManager` exists; disables itself if another instance is found.
- Queues a relayout when views are registered, unregistered, or changed.
- Layouts dockable views based on their properties and visibility, grouping them by their associated panels.

# Constraints & Failure Modes
- Null checks are performed on views before processing.
- Views are only added if they are not already registered.
- Layout calculations depend on the panel's width; if the width is zero or negative, layout adjustments are skipped.

# Example
```csharp
var dockingManager = UIDockingManager.Instance;
dockingManager.Register(myDockableView);
```

# Unknowns
- None.
