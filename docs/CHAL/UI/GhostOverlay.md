# CHAL.UI.GhostOverlay

_Automatically generated/updated from `Assets/src/UI/misc/GhostOverlay.cs`._

# Purpose
- Defines the `GhostOverlay` class for displaying a draggable item overlay in the UI.

# Public API
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

# Key Behavior & Side Effects
- On enabling, creates the ghost overlay and subscribes to drag events.
- On disabling, unsubscribes from drag events.
- Updates the ghost overlay's position and visibility based on mouse position and drag state.
- Ensures the ghost overlay is added to the correct UI document when dragging starts.

# Constraints & Failure Modes
- The `InvDnDProvider` service may be null if not set, preventing subscription to drag events.
- The ghost overlay visibility is controlled by drag events; it will not display if no item is being dragged.
- Handles null checks for the current document and active documents.

# Example
```csharp
// Example usage in a Unity scene
GhostOverlay ghostOverlay = gameObject.AddComponent<GhostOverlay>();
ghostOverlay.ghostSize = 64;
ghostOverlay.opacity = 0.8f;
```

# Unknowns
- The behavior of `InvDnDProvider` and `ItemRegistry` is not defined within this file.
- The structure and contents of `ItemStack` and its properties are not detailed here.

