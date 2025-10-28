# Assets/src/UI/misc/GhostOverlay.cs

_Automatic generated/updated._

```markdown
## Purpose
- Defines the `GhostOverlay` class for managing a draggable ghost representation of items in a UI.

## Public API
- Namespace: `CHAL.UI`
- Types
  - `public sealed class GhostOverlay : MonoBehaviour`
    - Public fields/properties:
      - `public int ghostSize` - Size of the ghost icon.
      - `public float opacity` - Opacity of the ghost overlay.
    - Public methods:
      - `void OnEnable()` - Initializes the ghost and subscribes to events.
      - `void OnDisable()` - Unsubscribes from events.
      - `void Update()` - Updates the position of the ghost based on mouse position.

## Key Behavior & Side Effects
- On enabling, creates a ghost visual element and subscribes to drag events.
- On disabling, unsubscribes from drag events.
- Handles the visibility and positioning of the ghost based on drag events and mouse position.
- Updates the ghost's content based on the item being dragged.

## Constraints & Failure Modes
- Subscribes to drag events only if the service is available.
- Handles null checks for the drag service and active documents.
- Ensures the ghost is added to the correct parent document based on mouse position.

## Example
```csharp
// Example usage in a Unity scene
GhostOverlay ghostOverlay = gameObject.AddComponent<GhostOverlay>();
ghostOverlay.ghostSize = 64;
ghostOverlay.opacity = 0.8f;
```

## Unknowns
- The behavior of `InvDnDProvider` and `DragDropService` is not defined in this file.
- The structure of `ItemStack` and `ItemRegistry` is not provided in this file.
```
