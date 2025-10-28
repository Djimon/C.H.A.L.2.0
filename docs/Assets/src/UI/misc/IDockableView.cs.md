# Assets/src/UI/misc/IDockableView.cs

_Automatic generated/updated._

```markdown
# Purpose
- Defines the `IDockableView` interface for UI components in the CHAL.UI namespace.

# Public API
- Namespace: `CHAL.UI`
- Types
  - `public interface IDockableView`
    - Public fields/properties:
      - `UIDocument doc { get; }` - The outer menu container managed by the manager.
      - `VisualElement OuterContainer { get; }` - The outer visual element container.
      - `bool IsVisible { get; }` - Indicates visibility through UI Toolkit.
      - `bool ReadOnly { get; }` - Indicates interactivity status for ActiveInventories.
      - `DockEdge Edge { get; }` - The docking edge setting.
      - `int DockPriority { get; }` - The priority for docking.
      - `bool AutoDock { get; }` - Indicates if auto-docking is enabled.
      - `float BaseWidthPercent { get; }` - Base width percentage (0..1).
      - `int MinWidthPx { get; }` - Minimum width in pixels.
      - `int MaxWidthPx { get; }` - Maximum width in pixels.
      - `string StableId { get; }` - Identifier for logs/debugging.
      - `bool IsInventoryView { get; }` - Indicates if it is an inventory view.
      - `bool IsItemCard { get; }` - Indicates if it is an item card (zIndex=10).

# Key Behavior & Side Effects
- No explicit behavior or side effects are defined in this interface.

# Constraints & Failure Modes
- No specific guards, null/empty handling, threading/async notes, or performance hints are evident.

# Example
- No minimal example is derivable from the interface.

# Unknowns
- No facts that cannot be determined from this file.
```
