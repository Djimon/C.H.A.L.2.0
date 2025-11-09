# Assets/src/Systems/_test/demo_InvenotryBootstrap.cs

_Automatically generated/updated from `Assets/src/Systems/_test/demo_InvenotryBootstrap.cs`._

# Purpose
- Manages the inventory system for the demo, handling multiple inventory views.

# Public API
- Namespace: None
- Types
  - public class InventoryDemoBootstrap : MonoBehaviour
    - Public fields/properties:
      - InventoryView bagAView: View for the first inventory.
      - InventoryView bagBView: View for the second inventory.
    - Public methods:
      - void Awake(): Initializes the inventory domain and instances, registers them, adds test items, and binds UI views.

# Key Behavior & Side Effects
- Initializes an `InventoryDomain` and creates two `InventoryInstance` objects (`_bagA` and `_bagB`).
- Registers the inventory instances with the domain.
- Attempts to add test items to both inventory instances.
- Binds the inventory views to the respective inventory instances if they are assigned.

# Constraints & Failure Modes
- Assumes that `InventoryInstance.Create` and `TryAdd` methods succeed without error handling.
- UI binding occurs only if `bagAView` and `bagBView` are not null.

# Example
```csharp
public class ExampleUsage : MonoBehaviour
{
    void Start()
    {
        InventoryDemoBootstrap demo = new InventoryDemoBootstrap();
        demo.Awake();
    }
}
```

# Unknowns
- The behavior of `InventoryInstance.Create` and `TryAdd` methods is not defined in this file.
- The structure and properties of `InventoryView` and `InventoryDomain` are not detailed here.

