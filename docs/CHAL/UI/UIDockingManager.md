# CHAL.UI.UIDockingManager

_Automatically generated/updated from `Assets/src/UI/misc/UIDockingManager.cs`._

# UIDockingManager.cs

## Purpose
- Manages docking of UI views in Unity.
- Provides methods to register, unregister, and notify changes in dockable views.

## Public API
- Namespace: `CHAL.UI`
- Types:
  - `public sealed class UIDockingManager : MonoBehaviour`
    - Public fields/properties:
      - `public static UIDockingManager Instance { get; private set; }`
      - `public int DockSpacing`
      - `public IReadOnlyList<UIDocument> ActiveDocs`
      - `public event Action<UIDocument> OnDocAdded`
      - `public event Action<UIDocument> OnDocRemoved`
    - Public methods:
      - `public void Register(IDockableView view)`
      - `public void Unregister(IDockableView view)`
      - `public void NotifyViewChanged(IDockableView view)`
      - `public void QueueRelayout()`
      - `public IReadOnlyList<IDockableView> GetActiveInventories()`
      - `public InventoryView? GetOtherInventory(InventoryView caller)`

## Key Behavior & Side Effects
- Singleton pattern ensures only one instance of `UIDockingManager`.
- Registers and unregisters `IDockableView` instances, triggering events on document addition/removal.
- Queues relayout when views are registered, unregistered, or changed.
- Layouts views based on their docking edge (left/right) and visibility.

## Constraints & Failure Modes
- Guards against null views in registration/unregistration.
- Handles empty lists when querying active inventories.
- Relayout is queued to avoid multiple updates in a single frame.

## Example
```csharp
var dockingManager = UIDockingManager.Instance;
dockingManager.Register(myDockableView);
```

## Unknowns
- Specific behavior of `IDockableView` and its properties/methods.
- The impact of external factors on layout performance.

