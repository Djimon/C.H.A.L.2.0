# Assets/src/UI/misc/UIDockingManager.cs

_Automatic generated/updated._

```markdown
# Purpose
- Defines the `UIDockingManager` class for managing dockable UI views in Unity.

# Public API
- Namespace: `CHAL.UI`
- Types
  - `public sealed class UIDockingManager : MonoBehaviour`
    - Public fields/properties:
      - `public static UIDockingManager Instance { get; private set; }` - Singleton instance of the manager.
      - `public int DockSpacing` - Spacing between docked views.
      - `public IReadOnlyList<UIDocument> ActiveDocs` - List of active documents.
      - `public event Action<UIDocument> OnDocAdded` - Event triggered when a document is added.
      - `public event Action<UIDocument> OnDocRemoved` - Event triggered when a document is removed.
    - Public methods:
      - `public void Register(IDockableView view)` - Registers a dockable view.
      - `public void Unregister(IDockableView view)` - Unregisters a dockable view.
      - `public void NotifyViewChanged(IDockableView view)` - Notifies that a view's properties have changed.
      - `public void QueueRelayout()` - Queues a relayout operation.
      - `public IReadOnlyList<IDockableView> GetActiveInventories()` - Retrieves active inventory views.
      - `public InventoryView? GetOtherInventory(InventoryView caller)` - Gets another inventory view, if available.

# Key Behavior & Side Effects
- Singleton pattern ensures only one instance of `UIDockingManager` exists.
- Registers and unregisters `IDockableView` instances, triggering events on document addition/removal.
- Queues relayout operations when views are registered, unregistered, or changed.
- Layouts dockable views based on their properties and panel grouping.

# Constraints & Failure Modes
- Guards against null views in registration/unregistration.
- Handles empty or null containers during layout.
- Uses `Mathf.Clamp01` to ensure width calculations stay within valid ranges.

# Example
```csharp
var dockingManager = UIDockingManager.Instance;
dockingManager.Register(myDockableView);
```

# Unknowns
- The implementation details of `IDockableView` and `InventoryView` are not provided.
- The behavior of the `OnDocAdded` and `OnDocRemoved` events is not detailed.
```
