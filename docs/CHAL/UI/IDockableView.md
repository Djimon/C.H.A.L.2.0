# CHAL.UI.IDockableView

_Automatically generated/updated from `Assets/src/UI/misc/IDockableView.cs`._

# Purpose
- Defines a dockable view interface for UI components.
- Provides properties for visibility, interactivity, and docking settings.

# Public API
- Namespace: CHAL.UI
- Types
  - public interface IDockableView
    - Public fields/properties
      - UIDocument doc { get; } - The outer menu container managed by the manager.
      - VisualElement OuterContainer { get; } - The outer container element.
      - bool IsVisible { get; } - Visibility controlled exclusively via UI Toolkit.
      - bool ReadOnly { get; } - Indicates interactivity status (for ActiveInventories).
      - DockEdge Edge { get; } - Docking edge setting.
      - int DockPriority { get; } - Priority for docking.
      - bool AutoDock { get; } - Indicates if auto-docking is enabled.
      - float BaseWidthPercent { get; } - Base width percentage (0..1) for left/right docking.
      - int MinWidthPx { get; } - Minimum width in pixels.
      - int MaxWidthPx { get; } - Maximum width in pixels.
      - string StableId { get; } - Identifier for logs/debugging.
      - bool IsInventoryView { get; } - True if the view is for inventories (ActiveInventories).
      - bool IsItemCard { get; } - True if the view is for an ItemCard (zIndex=10).

# Key Behavior & Side Effects
- No explicit state changes or error handling defined in the interface.

# Constraints & Failure Modes
- No specific guards, null/empty handling, or threading/async notes evident in the interface.

# Example
- Not applicable as the interface does not provide implementation examples.

# Unknowns
- No facts that cannot be determined from this file.

