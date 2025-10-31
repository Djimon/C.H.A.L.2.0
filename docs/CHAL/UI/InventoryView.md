# CHAL.UI.InventoryView

_Automatically generated/updated from `Assets/src/UI/InventoryView.cs`._

# InventoryView.cs Documentation

## Purpose
- Defines the `InventoryView` class for managing and displaying an inventory UI in Unity.

## Public API
- Namespace: `CHAL.UI`
- Types:
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
      - `IEnumerator BindFromTemplate()`
        - Waits for the GameManager and binds the inventory template.
      - `void OnEnable()`
        - Registers the view with the docking manager.
      - `void OnDisable()`
        - Unregisters the view from the docking manager.
      - `void OnDestroy()`
        - Unsubscribes from domain events.
      - `void Update()`
        - Unity's update method (currently empty).

## Key Behavior & Side Effects
- On enabling, registers with `UIDockingManager` and starts binding the UI from a template.
- On disabling, unregisters from `UIDockingManager`.
- Binds to an inventory domain and sets up UI elements based on the inventory definition.
- Responds to slot changes in the inventory domain and updates the UI accordingly.

## Constraints & Failure Modes
- Requires a valid `UIDocument` to function; logs an error if missing.
- Must have a grid element named "Grid" in the UXML; logs an error if not found.
- Read-only mode prevents user interactions with the inventory slots.

## Example
```csharp
InventoryView inventoryView = new InventoryView();
inventoryView.Bind(inventoryDomain, "instanceID", 4, 3);
```

## Unknowns
- The exact behavior of `IInventoryDomain` and `InvDnDProvider` is not defined in this file.
- The implementation details of `DragDropService` and how it interacts with the inventory are not provided.

