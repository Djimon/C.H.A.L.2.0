# global.InventoryDemoBootstrap

_Automatically generated/updated from `Assets/src/Systems/_test/demo_InvenotryBootstrap.cs`._

# Purpose
- Defines the `InventoryDemoBootstrap` class for initializing and managing inventory instances in a Unity environment.

# Public API
- Namespace: None specified
- Types
  - public class InventoryDemoBootstrap : MonoBehaviour
    - Public fields/properties:
      - `InventoryView bagAView`: UI view for inventory bag A.
      - `InventoryView bagBView`: UI view for inventory bag B.
    - Public methods:
      - `void Awake()`: Initializes inventory domain and instances, registers them, adds test items, and binds UI views.

# Key Behavior & Side Effects
- Initializes an `InventoryDomain` and two `InventoryInstance` objects (`_bagA` and `_bagB`) on Awake.
- Registers the inventory instances with the domain.
- Attempts to add predefined test items to both inventory bags.
- Binds the UI views to the respective inventory instances if they are assigned.

# Constraints & Failure Modes
- Assumes `InventoryInstance.Create` and `TryAdd` methods handle their own error states.
- UI binding occurs only if `bagAView` and `bagBView` are not null.

# Example
```csharp
public class ExampleUsage : MonoBehaviour
{
    public InventoryDemoBootstrap demoBootstrap;

    void Start()
    {
        // The InventoryDemoBootstrap will automatically initialize on Awake.
    }
}
```

# Unknowns
- The definitions and behaviors of `InventoryDomain`, `InventoryInstance`, and `ItemStack` are not provided in this file.
- The structure and properties of `InvDef` are not detailed in this file.

