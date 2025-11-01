# CHAL.UI.IDockableView

_Automatically generated/updated from `Assets/src/UI/misc/IDockableView.cs`._

Purpose
- Defines the public interface IDockableView in the CHAL.UI namespace.
- Specifies the contract for dockable views, exposing UI Toolkit elements and layout/docking metadata.
- Declares identity and view-type flags used by managers/consumers (StableId, IsInventoryView, IsItemCard).

Public API
- Namespace/module: CHAL.UI
- Types
  - public interface IDockableView
    - UIDocument doc { get; }
      - The required UIDocument container the manager positions
    - VisualElement OuterContainer { get; }
      - Outer container VisualElement
    - bool IsVisible { get; }
      - Visibility controlled exclusively via UI Toolkit
    - bool ReadOnly { get; }
      - Interactivity flag (true = read-only)
    - DockEdge Edge { get; }
      - Dock edge setting
    - int DockPriority { get; }
      - Docking priority
    - bool AutoDock { get; }
      - Whether to auto-dock
    - float BaseWidthPercent { get; }
      - Base width as a percentage of available space (0..1)
    - int MinWidthPx { get; }
      - Minimum width in pixels
    - int MaxWidthPx { get; }
      - Maximum width in pixels
    - string StableId { get; }
      - Stable identifier for logs/debug
    - bool IsInventoryView { get; }
      - True for inventory views (ActiveInventories)
    - bool IsItemCard { get; }
      - True for item cards (zIndex = 10)

Key Behavior & Side Effects
- None defined (interface only; no implementation or runtime behavior).

Constraints & Failure Modes
- None explicit in this file (no guards, threading, or async details).

Example
- Not provided (no derivable minimal example from this interface alone).

Unknowns
- DockEdge, UIDocument, VisualElement types are referenced but not defined in this file.
- Exact semantics of how IsVisible interacts with a concrete implementation or how BaseWidthPercent is clamped/used.
- Expected interactions between doc and OuterContainer in implementation.
- Any default values or lifecycle behavior are not specified here.
