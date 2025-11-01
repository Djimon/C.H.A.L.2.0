# CHAL.Systems.Inventory.InvDnDProvider

_Automatically generated/updated from `Assets/src/Systems/Inventory/InvDnDProvider.cs`._

1) Purpose
- Defines a Unity MonoBehaviour InvDnDProvider in the CHAL.Systems.Inventory namespace.
- Exposes a public IInventoryDomain domain reference (to be set in Inspector/bootstrap).
- Lazily creates and caches a DragDropService tied to the domain; exposes it via the Service property.
- Ensures the service can be rebuilt when the domain is updated in the editor (OnValidate).

2) Public API
- Namespace/module
  - CHAL.Systems.Inventory
- Types
  - public class InvDnDProvider : MonoBehaviour
    - Public fields/properties
      - public IInventoryDomain domain
        - Domain to use for the drag-and-drop provider (set in Inspector/bootstrap)
      - public DragDropService Service
        - Getter-only property; returns the cached _service or constructs a new DragDropService(domain) if needed
    - Public methods
      - none

3) Key Behavior & Side Effects
- Lazy initialization
  - Accessing Service will create a new DragDropService(domain) if _service is null and domain != null; then returns _service.
- Editor-time rebuild
  - OnValidate creates a new DragDropService(domain) if domain != null and _service is null.
- State/caching
  - _service is cached and reused once created.
- Domain coupling
  - Service depends on the current domain reference; changes to domain at runtime are not auto-handled outside OnValidate.

4) Constraints & Failure Modes
- Null domain handling
  - If domain is null, Service will remain null until domain is set and Service is accessed.
- Editor-only behavior
  - OnValidate runs in the editor to rebuild the service; runtime domain changes may not trigger a rebuild automatically.
- Threading/perf
  - No explicit thread-safety; service is created on-demand and cached.
- Cleanup
  - No disposal logic present for _service.

5) Example
- Not derivable from the file alone; no explicit usage example provided.

6) Unknowns
- Details of DragDropService constructor beyond accepting IInventoryDomain.
- Definition and members of IInventoryDomain.
- How and when domain is assigned in runtime versus editor bootstrap beyond the given comment.
