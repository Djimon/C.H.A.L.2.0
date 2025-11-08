# CHAL.UI.InGameUI

_Automatically generated/updated from `Assets/src/UI/misc/InGameUI.cs`._

# Purpose
- Defines an abstract class for in-game UI elements in Unity.
- Provides functionality to show or hide the UI and check its visibility.

# Public API
- Namespace: `CHAL.UI`
- Types
  - `public abstract class IngameUI : MonoBehaviour`
    - Public fields/properties:
      - `string requiredFeatureID`: Identifier for required features, default is "none".
      - `bool IsVisible`: Indicates if the UI element is currently visible.
    - Public methods:
      - `protected virtual void Awake()`: Initializes the UI element and hides it on start.
      - `public virtual void Show(bool show)`: Shows or hides the UI element based on the `show` parameter.

# Key Behavior & Side Effects
- The `Awake` method initializes the `root` VisualElement and sets its display style to hidden.
- The `Show` method changes the display style of the `root` VisualElement to either `Flex` or `None` based on the `show` parameter.

# Constraints & Failure Modes
- Assumes that a `UIDocument` component is attached to the same GameObject; failure to do so may result in a null reference when accessing `root`.

# Example
```csharp
public class MyIngameUI : IngameUI
{
    protected override void Awake()
    {
        base.Awake();
        // Additional initialization if needed
    }

    public void ToggleUI()
    {
        Show(!IsVisible);
    }
}
```

# Unknowns
- No information on how this class is intended to be extended or used in conjunction with other UI components.
