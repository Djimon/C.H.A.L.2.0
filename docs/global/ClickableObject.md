# global.ClickableObject

_Automatically generated/updated from `Assets/src/UI/misc/ClickableObject.cs`._

# Purpose
- Defines a `ClickableObject` class that manages hover and click interactions for a GameObject in Unity.

# Public API
- Namespace/module: None
- Types
  - public class ClickableObject : MonoBehaviour
    - Public fields/properties:
      - GameObject menuUI: Reference to the UI menu to display on click.
    - Public methods:
      - void OnHoverEnter(): Activates shimmer effect on hover.
      - void OnHoverExit(): Deactivates shimmer effect on hover exit.
      - void OnClick(): Displays the menu UI if assigned and deactivates shimmer effect.

# Key Behavior & Side Effects
- `Awake()`: Initializes the renderer and checks for the "_shimmerOn" property in the material. Logs a warning if the property is missing.
- `OnHoverEnter()`: Calls `SetShimmer(true)` to activate the shimmer effect.
- `OnHoverExit()`: Calls `SetShimmer(false)` to deactivate the shimmer effect.
- `OnClick()`: Displays the menu UI if `menuUI` is not null and deactivates the shimmer effect.

# Constraints & Failure Modes
- Checks if the material has the "_shimmerOn" property; logs a warning if not.
- `OnClick()` checks for null `menuUI` and `IngameUI` component before attempting to show the UI.

# Example
```csharp
ClickableObject clickable = gameObject.AddComponent<ClickableObject>();
clickable.menuUI = someMenuGameObject; // Assign the menu UI GameObject
```

# Unknowns
- None.

