# Assets/src/UI/InventoryView.cs

_Automatic generated/updated._

```markdown
# Purpose
- Defines the `InventoryView` class for managing and displaying an inventory UI in Unity.

# Public API
- Namespace: `CHAL.UI`
- Types
  - `public class InventoryView : MonoBehaviour, IDockableView`
    - Public fields/properties:
      - `InventoryDef _inventoryDef`
      - `string inventoryID`
      - `string InstanceId`
      - `bool IsVisible`
      - `VisualElement OuterContainer`
      - `DockEdge Edge`
      - `int DockPriority`
      - `bool AutoDock`
      - `bool ReadOnly`
      - `float BaseWidthPercent`
      - `int MinWidthPx`
      - `int MaxWidthPx`
      - `string StableId`
      - `bool IsInventoryView`
      - `bool IsItemCard`
      - `UIDocument doc`
    - Public methods:
      - `void Bind(IInventoryDomain domain, string instanceID, int cols, int rows)`
        - Binds the inventory domain and initializes the UI.
      - `void OnEnable()`
        - Registers the view with the docking manager.
      - `void OnDisable()`
        - Unregisters the view from the docking manager.
      - `void OnDestroy()`
        - Unsubscribes from domain events.
      - `IEnumerator BindFromTemplate()`
        - Binds the inventory view from a template after ensuring dependencies are ready.

# Key Behavior & Side Effects
- Initializes UI components and binds to the inventory domain on enable.
- Updates the visual representation of inventory slots when items change.
- Handles user interactions for item dragging and dropping.
- Responds to changes in the inventory domain by updating the UI.

# Constraints & Failure Modes
- Requires a valid `UIDocument` to function; logs an error if missing.
- Handles null checks for UI elements and inventory domain interactions.
- Read-only mode prevents user interactions with the inventory slots.

# Example
```csharp
InventoryView inventoryView = new InventoryView();
inventoryView.Bind(inventoryDomain, "instanceID", 4, 3);
```

# Unknowns
- The behavior of `IInventoryDomain` and its methods cannot be determined from this file.
- The structure of `InventoryDef` and `ItemStack` is not defined in this file.
```
