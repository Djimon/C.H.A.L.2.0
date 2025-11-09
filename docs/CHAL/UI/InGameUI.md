# Assets/src/UI/misc/InGameUI.cs

_Automatically generated/updated from `Assets/src/UI/misc/InGameUI.cs`._

# Purpose
- Defines an abstract class for in-game UI elements in Unity.
- Provides functionality to show or hide UI elements based on a boolean flag.

# Public API
- Namespace: `CHAL.UI`
- Types
  - `public abstract class IngameUI : MonoBehaviour`
    - Public fields/properties:
      - `public string requiredFeatureID`: Identifier for required features.
      - `public bool IsVisible`: Indicates if the UI element is currently visible.
    - Public methods:
      - `protected virtual void Awake()`: Initializes the root visual element and hides it.
      - `public virtual void Show(bool show)`: Shows or hides the UI element based on the `show` parameter.

# Key Behavior & Side Effects
- The `Awake` method initializes the `root` visual element and sets its display style to hidden.
- The `Show` method changes the display style of the `root` visual element to either `Flex` or `None` based on the `show` parameter.

# Constraints & Failure Modes
- Assumes a `UIDocument` component is attached to the same GameObject; failure to do so may result in a null reference when accessing `root`.

# Example
```csharp
public class MyIngameUI : IngameUI
{
    protected override void Awake()
    {
        base.Awake();
        // Additional initialization
    }

    public void ToggleUI()
    {
        Show(!IsVisible);
    }
}
```

# Unknowns
- None.
