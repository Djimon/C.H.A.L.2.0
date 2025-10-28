# global.InvDnDProvider

_Automatically generated/updated from `Assets/src/Systems/Inventory/InvDnDProvider.cs`._

# Purpose
- Defines the `InvDnDProvider` class for managing drag-and-drop functionality in an inventory system.

# Public API
- Namespace: `CHAL.Systems.Inventory`
- Types
  - public class `InvDnDProvider` [extends `MonoBehaviour`]
    - Public fields/properties:
      - `IInventoryDomain domain`: Set via Inspector/Bootstrap.
      - `DragDropService Service`: Lazy-initialized service for drag-and-drop operations.
    - Public methods:
      - `void OnValidate()`: Rebuilds the service if the domain is set and the service is null.

# Key Behavior & Side Effects
- `Service` property initializes `_service` with a new `DragDropService` instance if it is null and `domain` is not null.
- `OnValidate` method allows for updating the `_service` in the editor when the `domain` is set.

# Constraints & Failure Modes
- The `Service` property will return null if `domain` is not set.
- The `OnValidate` method ensures that `_service` is rebuilt only when `domain` is assigned.

# Example
```csharp
public class ExampleUsage : MonoBehaviour
{
    public InvDnDProvider invDnDProvider;

    void Start()
    {
        var service = invDnDProvider.Service; // Access the drag-and-drop service
    }
}
```

# Unknowns
- The implementation details of `DragDropService` and `IInventoryDomain` are not provided in this file.

