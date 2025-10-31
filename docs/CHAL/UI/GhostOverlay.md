# CHAL.UI.GhostOverlay

_Automatically generated/updated from `Assets/src/UI/misc/GhostOverlay.cs`._

# GhostOverlay.cs

## Purpose
- Defines a UI component for displaying a draggable ghost overlay during item drag-and-drop operations.

## Public API
- Namespace: `CHAL.UI`
- Types
  - `public sealed class GhostOverlay : MonoBehaviour`
    - Public fields/properties:
      - `public int ghostSize` - Size of the ghost overlay.
      - `public float opacity` - Opacity of the ghost overlay.
    - Public methods:
      - `void OnEnable()` - Initializes the ghost overlay and subscribes to events.
      - `void OnDisable()` - Unsubscribes from events.
      - `void Update()` - Updates the position of the ghost overlay based on mouse position.

## Key Behavior & Side Effects
- On enabling, creates the ghost overlay and subscribes to drag-and-drop events.
- On disabling, unsubscribes from events.
- Handles visibility of the ghost overlay based on drag state.
- Updates the position of the ghost overlay to follow the mouse cursor.
- Ensures the ghost overlay is added to the correct UI document.

## Constraints & Failure Modes
- The drag-and-drop service may be null if the provider's domain is not set.
- The ghost overlay will not be displayed if there is no current item being dragged.
- The ghost overlay's parent document is determined based on the active documents in the docking manager.

## Example
```csharp
// Example of using GhostOverlay in a Unity scene
public class ExampleUsage : MonoBehaviour
{
    public GhostOverlay ghostOverlay;

    void Start()
    {
        // Configure ghost overlay properties if needed
        ghostOverlay.ghostSize = 64;
        ghostOverlay.opacity = 0.8f;
    }
}
```

## Unknowns
- The behavior of `InvDnDProvider` and `DragDropService` is not defined in this file.
- The structure and contents of `ItemStack` and `ItemRegistry` are not detailed in this file.

