# CHAL.UI.InventoryView

_Automatically generated/updated from `Assets/src/UI/InventoryView.cs`._

```text
1) Purpose
- UI view for displaying and interacting with an inventory grid in CHAL.UI.
- Binds to an inventory domain, builds a slot grid, and renders slot contents.
- Integrates with drag-and-drop (DnD) and docking systems; handles responsive slot sizing and per-slot visuals.

2) Public API
- Namespace/module
  - CHAL.UI

- Types
  - public class InventoryView : MonoBehaviour, IDockableView
    - Public fields
      - public InventoryDef _inventoryDef; // Template/definition for the inventory to render
      - public string inventoryID; // Optional instance identifier; used by template binding when empty
      - public InvDnDProvider _invDnDProvider; // DnD provider (serialized)
    - Public properties
      - public string InstanceId => _instanceID // runtime instance identifier
      - public bool IsVisible => _outer != null && _outer.resolvedStyle.visibility == Visibility.Visible
      - public VisualElement OuterContainer => _outer // top-level container (post-docking)
      - public DockEdge Edge => dockEdge
      - public int DockPriority => dockPriority
      - public bool AutoDock => autoDock
      - public bool ReadOnly => readOnly
      - public float BaseWidthPercent => baseWidthPercent
      - public int MinWidthPx => minWidthPx
      - public int MaxWidthPx => maxWidthPx
      - public string StableId => InstanceId
      - public bool IsInventoryView => true
      - public bool IsItemCard => false
      - public UIDocument doc => _doc

    - Public methods
      - public void Bind(IInventoryDomain domain, string instanceID, int cols, int rows)
        - Binds the view to a domain instance with layout (cols/rows) overrides and initializes UI, grid, DnD, and event wiring.

3) Key Behavior & Side Effects
- OnEnable
  - UIDockingManager.Instance?.Register(this)
  - StartCoroutine(BindFromTemplate())
- Awake
  - If _doc is null, set to GetComponent<UIDocument>()
- Bind(domain, instanceID, cols, rows)
  - Stores domain and instanceID; clamps cols/rows to >= 1
  - Validates _doc; logs error and returns if missing
  - Determines _outer (container) from template root or fallback to root
  - Subscribes to GeometryChangedEvent to trigger RecomputeSlotMetricsAndApply
  - Recomputes slot metrics and applies visuals
  - Resolves _grid VisualElement; logs error if not found and returns
  - Applies container sizing; applies optional background
  - Resolves or creates a Drag-and-Drop service (_dnd) using _invDnDProvider or new DragDropService(_domain)
  - Builds grid (slots)
  - Subscribes to _domain.OnSlotChanged; renders initial state via RenderAllNow
- BindFromTemplate
  - Waits for GameManager.Instance and GameManager.Inventory to be non-null
  - Uses _inventoryDef; derives inventoryID if empty (player_<type> lowercase)
  - Gets domain from GameManager.Inventory; tries to fetch a named instance; if not present, yields (tries again later)
  - Reads cols/rows from inst.InvDef or from _inventoryDef
  - Calls Bind(domain, inst.instanceID, cols, rows)
- WireSlotInteractions
  - If ReadOnly, no interactions
  - Left-click: handles QuickMove when Shift held and a valid target inventory exists; otherwise initiates or completes drag/drop with DnD service
  - Right-click (MouseUp with button 1): splits a stack for drag (splitHalf = true) if there is a stack with count > 1
- OnSlotChanged
  - If the change is for this instance, triggers UpdateTileVisual for the affected slot
- RenderAllNow
  - Iterates through all slots (domain.SlotCount) and calls UpdateTileVisual
- UpdateTileVisual
  - Locates the slot tile, its label, and icon
  - If a slot has a stack: shows count, resolves sprite from ItemRegistry, sets icon tint, and updates tooltip
  - If empty: shows placeholder label, clears icon, sets gray tint, tooltip to "leer"
- RecomputeSlotMetricsAndApply
  - If responsive sizing enabled and container grid exists, computes slot size:
    - Width-based sizing with gaps, optional FitBoth mode
    - Clamps to min/max slot sizes
  - Applies new metrics and notifies UIDockingManager about the view change
- ApplySlotMetrics
  - Updates per-row margins
  - Updates each slot's size, icon size (scaled to slot), and label font size
- ApplyContainerSizing
  - Sets container width as percent; enforces min/max width in px
  - Resets margins to zero (dock layout)
- OnDestroy
  - Unregisters from domain events if present
- Update
  - Currently no implementation
- Additionally, the view relies on:
  - IInventoryDomain for slot data and events
  - ItemRegistry for item sprites
  - InvDnDProvider / DragDropService for DnD behavior
  - UIDockingManager for docking notifications

4) Constraints & Failure Modes
- Null checks and guarded access
  - Logs error if UIDocument is missing during Bind; returns early
  - Logs error if grid element is missing in UXML; returns
  - OnSlotChanged guards against non-matching instance IDs
- Coroutine-based binding
  - BindFromTemplate yields until GameManager and Inventory domain exist
  - If domain instance not found in GameManager, binding exits and may retry later
- Threading
  - All operations occur on Unity main thread; no explicit async concurrency beyond coroutines
- Performance
  - Recomputes and applies metrics on GeometryChangedEvent and container resize; contains loops over all slots
- State integrity
  - If _domain becomes null or OnSlotChanged fires after destruction, checks guard handling
  - DnD service resolves based on available provider or domain; may be null if domain is missing
- Visual correctness
  - Uses responsive sizing with min/max slot sizes; may rely on correct parent layout for accurate sizing

5) Example
- Not derivable from the file in a self-contained, minimal usage snippet without external setup; omitted.

6) Unknowns
- Exact definitions and members of:
  - IInventoryDomain, ItemStack, MoveRequest, MoveMode
  - InvDnDProvider, DragDropService, UIDockingManager behavior
  - ItemRegistry and how icons are defined (Sprite/icon retrieval)
  - SlotFitMode enum values and their expected semantics
  - DebugManager, its log levels, and how errors are surfaced
- Specific runtime behavior of the docking/layout system in edge cases (e.g., dynamic template changes, multiple inventories)
- Any side effects of dynamically changing _inventoryDef or inventoryID at runtime
```
