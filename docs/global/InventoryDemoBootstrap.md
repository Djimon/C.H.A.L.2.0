# global.InventoryDemoBootstrap

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
- Initializes an `InventoryDomain` and creates two `InventoryInstance` objects.
- Registers the inventory instances with the domain.
- Attempts to add test items to both inventory instances.
- Binds the inventory views to the respective inventory instances if they are assigned.

# Constraints & Failure Modes
- Assumes that `InventoryView` objects are assigned; no null checks for `bagAView` and `bagBView` before binding.
- The `TryAdd` method may fail silently if the item cannot be added.

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
- The behavior of `InventoryInstance.Create` and `InventoryDomain.RegisterInstance` is not defined in this file.
- The structure of `InventoryView` and `InventoryInstance.InvDef` is not detailed here.

