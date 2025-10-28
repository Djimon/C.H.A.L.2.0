# CHAL.UI.IDockableView

_Automatically generated/updated from `Assets/src/UI/misc/IDockableView.cs`._

# Purpose
- Defines the `IDockableView` interface for UI components in the CHAL.UI namespace.

# Public API
- Namespace: `CHAL.UI`
- Types
  - `public interface IDockableView`
    - `UIDocument doc { get; }` - The outer menu container managed by the manager.
    - `VisualElement OuterContainer { get; }` - The outer visual element container.
    - `bool IsVisible { get; }` - Indicates visibility through the UI Toolkit.
    - `bool ReadOnly { get; }` - Indicates if the view is interactive (for ActiveInventories).
    - `DockEdge Edge { get; }` - The docking settings edge.
    - `int DockPriority { get; }` - The priority for docking.
    - `bool AutoDock { get; }` - Indicates if auto-docking is enabled.
    - `float BaseWidthPercent { get; }` - Base width percentage (0..1) for left/right docking.
    - `int MinWidthPx { get; }` - Minimum width in pixels.
    - `int MaxWidthPx { get; }` - Maximum width in pixels.
    - `string StableId { get; }` - Identifier for logs/debugging.
    - `bool IsInventoryView { get; }` - Indicates if the view is for inventories (ActiveInventories).
    - `bool IsItemCard { get; }` - Indicates if the view is an item card (zIndex=10).

# Key Behavior & Side Effects
- No explicit behavior or side effects are defined in this interface.

# Constraints & Failure Modes
- No specific guards, null/empty handling, or threading/async notes are provided in this interface.

# Example
```csharp
public class MyDockableView : IDockableView
{
    public UIDocument doc => /* implementation */;
    public VisualElement OuterContainer => /* implementation */;
    public bool IsVisible => /* implementation */;
    public bool ReadOnly => /* implementation */;
    public DockEdge Edge => /* implementation */;
    public int DockPriority => /* implementation */;
    public bool AutoDock => /* implementation */;
    public float BaseWidthPercent => /* implementation */;
    public int MinWidthPx => /* implementation */;
    public int MaxWidthPx => /* implementation */;
    public string StableId => /* implementation */;
    public bool IsInventoryView => /* implementation */;
    public bool IsItemCard => /* implementation */;
}
```

# Unknowns
- No unknowns are identified from this file.

