# Assets/src/UI/misc/ClickableObject.cs

_Automatically generated/updated from `Assets/src/UI/misc/ClickableObject.cs`._

# Purpose
- Defines a `ClickableObject` class that represents an object that can be clicked in the game.

# Public API
- Namespace: `CHAL.UI`
- Types
  - `public class ClickableObject : MonoBehaviour`
    - Public fields/properties:
      - `GameObject menuUI`: Reference to the UI menu assigned in the Inspector.
    - Public methods:
      - `void OnHoverEnter()`: Activates shimmer effect on hover.
      - `void OnHoverExit()`: Deactivates shimmer effect when hover exits.
      - `void OnClick()`: Displays the UI menu if conditions are met.

# Key Behavior & Side Effects
- `Awake()`: Initializes the renderer and material property block; checks for the `_shimmerOn` property in the material and logs a warning if absent.
- `OnHoverEnter()`: Calls `SetShimmer(true)` to enable the shimmer effect.
- `OnHoverExit()`: Calls `SetShimmer(false)` to disable the shimmer effect.
- `OnClick()`: Checks if `menuUI` is assigned and if the required feature is unlocked before showing the UI.

# Constraints & Failure Modes
- `OnClick()`: If `menuUI` is null, no action is taken.
- `Awake()`: Logs a warning if the material does not have the `_shimmerOn` property.

# Example
```csharp
// Example usage in a Unity scene
ClickableObject clickable = gameObject.AddComponent<ClickableObject>();
clickable.menuUI = someMenuUI; // Assign the UI menu in the Inspector
```

# Unknowns
- None.
