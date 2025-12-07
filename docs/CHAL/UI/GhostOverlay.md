# Assets/src/UI/misc/GhostOverlay.cs

_Automatically generated/updated from `Assets/src/UI/misc/GhostOverlay.cs`._

# Purpose
- Defines the `GhostOverlay` class for displaying a draggable item overlay in the UI.

# Public API
- Namespace: `CHAL.UI`
- Types
  - public sealed class `GhostOverlay` : `MonoBehaviour`
    - Public fields/properties:
      - `int ghostSize`: Size of the ghost overlay.
      - `float opacity`: Opacity of the ghost overlay.
      - `Vector2 _offset`: Offset position of the ghost overlay.
    - Public methods:
      - `void OnEnable()`: Initializes the ghost overlay, subscribes to events, and sets up document management.
      - `void OnDisable()`: Unsubscribes from events.
      - `void Update()`: Updates the position of the ghost overlay based on mouse position and manages document changes.

# Key Behavior & Side Effects
- On enabling, creates the ghost overlay, subscribes to drag events, and manages document addition/removal.
- On disabling, unsubscribes from drag events.
- Handles the beginning and end of drag operations, showing or hiding the ghost overlay accordingly.
- Updates the ghost overlay's position based on the mouse's current location and the active UI document.

# Constraints & Failure Modes
- The `InvDnDProvider` service may be null if not set, preventing subscription to drag events.
- The ghost overlay visibility is dependent on the drag state and the current item being dragged.
- The overlay is only added to the active document if it exists.

# Example
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

# Unknowns
- The behavior of `InvDnDProvider` and `DragDropService` is not defined within this file.
- The structure and contents of `ItemStack` and `ItemRegistry` are not detailed in this file.
