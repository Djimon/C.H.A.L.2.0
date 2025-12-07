# Assets/src/UI/InventoryView.cs

_Automatically generated/updated from `Assets/src/UI/InventoryView.cs`._

# Purpose
- Manages the inventory UI and layout in the game.
- Implements IDockableView for docking functionality.

# Public API
- Namespace: CHAL.UI
- Types
  - public class InventoryView : MonoBehaviour, IDockableView
    - Public fields/properties:
      - InventoryDef _inventoryDef
      - string inventoryID
      - Sprite myInventoryBG
      - string InstanceId
      - bool IsVisible
      - VisualElement OuterContainer
      - DockEdge Edge
      - int DockPriority
      - bool AutoDock
      - bool ReadOnly
      - float BaseWidthPercent
      - int MinWidthPx
      - int MaxWidthPx
      - string StableId
      - bool IsInventoryView
      - bool IsItemCard
      - UIDocument doc
    - Public methods:
      - void Bind(IInventoryDomain domain, string instanceID, int cols, int rows)
      - void OnEnable()
      - void Awake()
      - void OnDisable()
      - void OnDestroy()
      - void Update()
      - IEnumerator BindFromTemplate()

# Key Behavior & Side Effects
- OnEnable: Registers the view with UIDockingManager and starts binding from the template.
- OnDisable: Unregisters the view from UIDockingManager.
- OnDestroy: Unsubscribes from domain events.
- Bind: Initializes the inventory view, binds to the domain, and sets up the UI elements.
- WireSlotInteractions: Sets up interaction callbacks for inventory slots.
- BindFromTemplate: Waits for GameManager and domain readiness, then binds the inventory view.

# Constraints & Failure Modes
- Requires a valid UIDocument; logs an error if missing.
- Requires a grid element named "Grid" in the UXML; logs an error if missing.
- Read-only mode blocks user interactions with inventory slots.

# Example
```csharp
InventoryView inventoryView = new InventoryView();
inventoryView.Bind(inventoryDomain, "player_inventory", 4, 3);
```

# Unknowns
- None.
