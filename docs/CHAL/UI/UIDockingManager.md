# CHAL.UI.UIDockingManager

_Automatically generated/updated from `Assets/src/UI/misc/UIDockingManager.cs`._

# Purpose
- Manages the docking of UI views in a Unity application.

# Public API
- Namespace: `CHAL.UI`
- Types
  - `public sealed class UIDockingManager : MonoBehaviour`
    - Public fields/properties:
      - `public static UIDockingManager Instance { get; private set; }` - Singleton instance of the docking manager.
      - `public int DockSpacing` - Spacing between docked views.
      - `public IReadOnlyList<UIDocument> ActiveDocs` - List of active documents.
      - `public event Action<UIDocument> OnDocAdded` - Event triggered when a document is added.
      - `public event Action<UIDocument> OnDocRemoved` - Event triggered when a document is removed.
    - Public methods:
      - `public void Register(IDockableView view)` - Registers a dockable view.
      - `public void Unregister(IDockableView view)` - Unregisters a dockable view.
      - `public void NotifyViewChanged(IDockableView view)` - Notifies that a view's properties have changed.
      - `public void QueueRelayout()` - Marks the layout for relayout.
      - `public IReadOnlyList<IDockableView> GetActiveInventories()` - Retrieves visible, interactive inventories.
      - `public InventoryView? GetOtherInventory(InventoryView caller)` - Gets another inventory view, if available.

# Key Behavior & Side Effects
- Ensures only one instance of `UIDockingManager` exists; disables if a second instance is created.
- Queues a relayout when views are registered, unregistered, or changed.
- Layouts dockable views based on their properties and visibility, grouping them by their parent panel.

# Constraints & Failure Modes
- Handles null views gracefully in registration and unregistration.
- Uses `Mathf.Clamp01` to ensure width calculations remain within valid bounds.
- Assumes that `OuterContainer` is not null when performing layout operations.

# Example
```csharp
var dockingManager = UIDockingManager.Instance;
dockingManager.Register(myDockableView);
```

# Unknowns
- None.

