# CHAL.UI.InventoryView

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
      - void OnDisable()
      - void OnDestroy()
      - void Update()

# Key Behavior & Side Effects
- OnEnable: Registers the view with UIDockingManager and starts binding from template.
- OnDisable: Unregisters the view from UIDockingManager.
- OnDestroy: Unsubscribes from domain slot change events.
- Bind: Initializes the inventory view, binds to the domain, and sets up the UI elements.
- QuickMove functionality allows items to be moved between inventories using mouse interactions.

# Constraints & Failure Modes
- Requires a valid UIDocument; errors are logged if it is missing.
- Requires a grid element named "Grid" in the UXML; errors are logged if it is missing.
- Read-only mode blocks user interactions with the inventory slots.

# Example
```csharp
var inventoryView = new InventoryView();
inventoryView.Bind(inventoryDomain, "player_inventory", 4, 3);
```

# Unknowns
- The behavior of the DragDropService and its integration with the inventory system is not detailed in this file.
- The specifics of the IInventoryDomain interface and its methods are not defined in this file.

