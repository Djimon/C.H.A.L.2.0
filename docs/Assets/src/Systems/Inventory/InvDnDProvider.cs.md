# Assets/src/Systems/Inventory/InvDnDProvider.cs

_Automatic generated/updated._

```markdown
# Purpose
- Defines the `InvDnDProvider` class for managing drag-and-drop functionality in an inventory system.

# Public API
- Namespace: None specified
- Types
  - public class `InvDnDProvider` : `MonoBehaviour`
    - Public fields/properties:
      - `IInventoryDomain domain`: Set via Inspector for inventory domain.
      - `DragDropService Service`: Lazy-initialized service for drag-and-drop operations.
    - Public methods:
      - `void OnValidate()`: Rebuilds the service if the domain is set and the service is null.

# Key Behavior & Side Effects
- `OnValidate` method ensures that the `DragDropService` is created when the `domain` is assigned in the editor.

# Constraints & Failure Modes
- `Service` will return null if `domain` is not set.
- `DragDropService` is only instantiated if `domain` is not null.

# Example
```csharp
public class ExampleUsage : MonoBehaviour
{
    public InvDnDProvider invDnDProvider;

    void Start()
    {
        var service = invDnDProvider.Service; // Access the DragDropService
    }
}
```

# Unknowns
- The implementation details of `DragDropService` and `IInventoryDomain` are not provided in this file.
```
