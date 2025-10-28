# Assets/src/UI/misc/InGameUI.cs

_Automatic generated/updated._

```markdown
# Purpose
- Defines an abstract class for in-game UI management.
- Provides methods to show/hide the UI and check its visibility.

# Public API
- Namespace/module: None specified.
- Types
  - protected abstract class IngameUI : MonoBehaviour
    - Public fields/properties:
      - VisualElement root: The root visual element of the UI.
    - Public methods:
      - virtual void Awake(): Initializes the root visual element and hides it.
      - virtual void Show(bool show): Shows or hides the UI based on the boolean parameter.
      - bool IsVisible: Gets whether the UI is currently visible.

# Key Behavior & Side Effects
- `Awake`: Initializes the `root` and sets its display style to `None`, making it hidden at start.
- `Show`: Changes the display style of `root` to either `Flex` or `None` based on the `show` parameter.
- `IsVisible`: Checks if the UI is currently displayed.

# Constraints & Failure Modes
- Assumes a `UIDocument` component is attached to the same GameObject; failure to do so may result in a null reference.
- The `root` element's visibility is controlled by the `DisplayStyle` enum.

# Example
```csharp
public class MyIngameUI : IngameUI
{
    void Start()
    {
        Show(true); // Show the UI when the game starts
    }
}
```

# Unknowns
- No information on how this class is intended to be extended or used in a broader context.
```
