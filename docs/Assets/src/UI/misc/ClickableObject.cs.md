# Assets/src/UI/misc/ClickableObject.cs

_Automatic generated/updated._

```markdown
## Purpose
- Defines a `ClickableObject` class that provides interactive behavior for UI elements in Unity.

## Public API
- Namespace/module: None
- Types
  - public class ClickableObject : MonoBehaviour
    - Public fields/properties
      - GameObject menuUI: Reference to the UI menu to display on click.
    - Public methods
      - void OnHoverEnter(): Activates shimmer effect on hover.
      - void OnHoverExit(): Deactivates shimmer effect on hover exit.
      - void OnClick(): Displays the menu UI if assigned and deactivates shimmer effect.

## Key Behavior & Side Effects
- `Awake()`: Initializes the renderer and material property block; checks for the `_shimmerOn` property in the material.
- `OnHoverEnter()`: Calls `SetShimmer(true)` to activate the shimmer effect.
- `OnHoverExit()`: Calls `SetShimmer(false)` to deactivate the shimmer effect.
- `OnClick()`: Displays the menu UI if `menuUI` is not null and deactivates the shimmer effect.

## Constraints & Failure Modes
- Checks if the material has the `_shimmerOn` property; logs a warning if not.
- `menuUI` must be assigned in the Inspector for `OnClick()` to function correctly.
- Assumes the presence of a `Renderer` component on the GameObject.

## Example
```csharp
ClickableObject clickable = gameObject.AddComponent<ClickableObject>();
clickable.menuUI = someMenuGameObject; // Assign the menu UI in the Inspector
```

## Unknowns
- None.
```
