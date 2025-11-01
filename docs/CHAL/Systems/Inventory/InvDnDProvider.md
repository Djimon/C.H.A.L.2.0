# CHAL.Systems.Inventory.InvDnDProvider

_Automatically generated/updated from `Assets/src/Systems/Inventory/InvDnDProvider.cs`._

```text
1) Purpose
- Defines a MonoBehaviour that holds an IInventoryDomain reference for a drag-and-drop domain.
- Lazily exposes a DragDropService via the Service property, creating it on first access when a domain is present.
- Rebuilds the DragDropService in the editor when domain is assigned and the service is not yet created (OnValidate).

2) Public API
- Namespace/module
  - CHAL.Systems.Inventory
- Types
  - public class InvDnDProvider : MonoBehaviour
    - Public fields/properties
      - public IInventoryDomain domain; // per Inspector/Bootstrap setzen
      - public DragDropService Service { get; } 
        - Getter returns the DragDropService instance; may create it if _service is null and domain != null
    - Public methods
      - (none)
    - Private fields
      - private DragDropService _service;
- Notes on surface
  - Service is read-only from outside; access triggers lazy initialization.
  - DragDropService(domain) is called to construct the service.

3) Key Behavior & Side Effects
- Lazy initialization
  - Accessing Service creates a new DragDropService(domain) if _service is null and domain is not null.
- Editor-time rebind
  - OnValidate ensures _service is constructed when domain is set in the editor and _service is null.
- Access pattern
  - Service getter may return null if domain is null and no prior initialization occurred.
  - No automatic rebind if domain changes at runtime after _service creation.

4) Constraints & Failure Modes
- Null handling
  - If domain is null, Service getter will not instantiate and may return null.
- Runtime domain changes
  - Changing domain after _service has been created does not automatically update the service to the new domain.
- Threading
  - All behavior occurs on Unity main thread (no explicit threading here).
- Dependencies
  - DragDropService construction relies on the IInventoryDomain provided by domain.

5) Example
```csharp
// Usage example (assumes domain is set via Inspector or at runtime)
var provider = GetComponent<CHAL.Systems.Inventory.InvDnDProvider>();
var service = provider.Service; // may instantiate DragDropService if domain != null
```

6) Unknowns
- Details of IInventoryDomain and DragDropService implementations (behavior, lifecycle, side effects).
- Whether domain changes at runtime should trigger a service rebuild (not handled beyond editor OnValidate).
```
