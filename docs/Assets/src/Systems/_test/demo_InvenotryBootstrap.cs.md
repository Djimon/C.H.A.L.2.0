# Assets/src/Systems/_test/demo_InvenotryBootstrap.cs

_Automatic generated/updated._

```markdown
# Purpose
- Defines the `InventoryDemoBootstrap` class for initializing and managing inventory instances in a Unity environment.

# Public API
- Namespace/module: None specified.
- Types
  - public class `InventoryDemoBootstrap` : MonoBehaviour
    - Public fields/properties:
      - `InventoryView bagAView`: UI view for inventory A.
      - `InventoryView bagBView`: UI view for inventory B.
    - Public methods:
      - `void Awake()`: Initializes inventory domain and instances, registers them, adds test items, and binds UI views.

# Key Behavior & Side Effects
- Initializes an `InventoryDomain` and two `InventoryInstance` objects on `Awake`.
- Registers the inventory instances with the domain.
- Attempts to add predefined test items to both inventory instances.
- Binds the UI views to the respective inventory instances if they are assigned.

# Constraints & Failure Modes
- Assumes `InventoryInstance.Create` and `InventoryDomain.RegisterInstance` do not return null.
- No explicit error handling for item addition failures; relies on `TryAdd` method's internal handling.
- UI binding occurs only if `bagAView` and `bagBView` are not null.

# Example
```csharp
public class ExampleUsage : MonoBehaviour
{
    public InventoryDemoBootstrap demoBootstrap;

    void Start()
    {
        // The InventoryDemoBootstrap will initialize inventories and bind UI on Awake.
    }
}
```

# Unknowns
- The definitions and behaviors of `InventoryDomain`, `InventoryInstance`, `ItemStack`, and `InventoryView` are not provided in this file.
- The structure and contents of `InvDef` are not detailed.
```
