# CHAL.UI.ClickableObject

_Automatically generated/updated from `Assets/src/UI/misc/ClickableObject.cs`._

# Purpose
- Defines a `ClickableObject` class that represents an object that can be clicked in the game.

# Public API
- Namespace: `CHAL.UI`
- Types
  - public class `ClickableObject` : `MonoBehaviour`
    - Public fields/properties:
      - `GameObject menuUI`: Reference to the UI menu assigned in the Inspector.
    - Public methods:
      - `void OnHoverEnter()`: Handles the event when the mouse pointer enters a hover state.
      - `void OnHoverExit()`: Handles the event when the mouse pointer exits a hover state.
      - `void OnClick()`: Handles the click event for the UI.

# Key Behavior & Side Effects
- `Awake()`: Initializes the renderer and checks for the `_shimmerOn` property in the material. Logs a warning if the property is missing. Calls `SetShimmer(false)`.
- `OnClick()`: Checks if `menuUI` is not null, retrieves the `IngameUI` component, checks if the required feature is unlocked, and shows the UI if conditions are met. Calls `SetShimmer(false)`.

# Constraints & Failure Modes
- If the material does not have the `_shimmerOn` property, a warning is logged.
- `OnClick()` checks if `menuUI` is null before proceeding, preventing null reference exceptions.

# Example
```csharp
ClickableObject clickable = new ClickableObject();
clickable.OnHoverEnter(); // Activates shimmer effect
clickable.OnClick(); // Triggers UI display if conditions are met
```

# Unknowns
- None.
