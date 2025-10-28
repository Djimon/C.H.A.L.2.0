# global.InGameUI

_Automatically generated/updated from `Assets/src/UI/misc/InGameUI.cs`._

# Purpose
- Defines an abstract class for in-game UI management.
- Provides methods to show/hide the UI and check its visibility.

# Public API
- Namespace/module: None specified.
- Types
  - protected abstract class IngameUI : MonoBehaviour
    - Public fields/properties:
      - root: VisualElement representing the root of the UI.
    - Public methods:
      - virtual void Awake() 
        - Initializes the root VisualElement and hides it.
      - virtual void Show(bool show) 
        - Shows or hides the UI based on the boolean parameter.
      - bool IsVisible 
        - Returns true if the UI is currently visible.

# Key Behavior & Side Effects
- Awake: Initializes the UI and sets it to be hidden initially.
- Show: Changes the display style of the UI based on the input parameter.

# Constraints & Failure Modes
- Assumes a UIDocument component is attached to the same GameObject.
- No explicit error handling for missing components.

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

